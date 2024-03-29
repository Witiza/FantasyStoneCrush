using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


[RequireComponent(typeof(IGameplayInput))]
public class BoardView : MonoBehaviour
{
    public static List<VisualTile> tiles = new List<VisualTile>();
    public GameObject[] TilePrefabs;
    public GameObject[] ParticlePrefabs;
    public BoardController Board;
    [SerializeField]
    public PlayerProgressionService PlayerProgression;
    IGameplayInput _gameplayInput;
    int _availableMoves;
    int _currentScore = 0;
    BoardConfig _config;
    VisualSelector _visualSelector;
    public TMP_Text MovesText;
    public TMP_Text ScoreText;
    public GameEndEventBus GameWon;
    public GameEndEventBus GameLost;
    public EventBus MovesAdded;
    public Booster TileBooster;
    public Booster TurnBooster;
    GameAnalyticsService _analyticsService;
    GameConfigService _gameConfigService;
    GameSaveService _progressionProviderService;
    private void Awake()
    {
        DOTween.Init().SetCapacity(400, 400);
        tiles.Clear();
        BoardEvents.TileCreated += BoardEventsTileCreated;
        BoardEvents.TileChanged += BoardEventsTileChanged;
        BoardEvents.TileMoved += BoardEventsTileMoved;
        BoardEvents.TileSwapped += BoardEventsTileSwapped;
        BoardEvents.TileDestroyed += BoardEventsTileDestroyed;
        BoardEvents.SpecialTileDestroyed += BoardEventsSpecialTileDestroyed;
        TileBooster.BoosterEvent += TileBoosterEvent;
        TurnBooster.BoosterEvent += TurnBoosterEvent;
        MovesAdded.Event += MovesAddedEvent;
        _gameplayInput = gameObject.GetComponent<IGameplayInput>();
        _gameplayInput.StartTouch += InputStartTouch; ;
        _gameplayInput.SwapTouch += InputSwapTouch; ;
        _gameplayInput.EndTouch += InputEndTouch;
        _config = PlayerProgression.GetCurrentLevelBoard();
        Board = new BoardController(_config.board.GridSize.x, _config.board.GridSize.y, _config.board);
        _visualSelector = new VisualSelector();
        _availableMoves = _config.AvailableMoves;
        _analyticsService = ServiceLocator.GetService<GameAnalyticsService>();
        _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        _progressionProviderService = ServiceLocator.GetService<GameSaveService>();
        _analyticsService.SendEvent("LevelStarted", new Dictionary<string, object> { ["CurrentLevel"] = PlayerProgression.CurrentLevel });
        InitializeTexts();
       // HandleTutorial();
    }

    void HandleTutorial()
    {
        if(_config.tutorial != null)
        {
            Instantiate(_config.tutorial, MovesText.transform.parent);
        }
    }

    private void MovesAddedEvent()
    {
        _availableMoves += _gameConfigService.movesAddedByAd;
        UpdateMoves();
    }

    private void TurnBoosterEvent(bool success)
    {
        if (success)
        {
            _availableMoves += _gameConfigService.movesAddedByBooster;
            UpdateMoves();
        }
    }

    private void TileBoosterEvent(bool success)
    {
        if (success)
        {
            for (int i = 0; i < _gameConfigService.tilesAddedByBooster; i++)
            {
                Vector2Int tmp;
                do
                {
                    tmp = new Vector2Int(Random.Range(0, _config.BoardWidth), Random.Range(0, _config.BoardHeight));
                } while (!Board.ChangeTile(tmp, (TileType)Random.Range(6, 9)));
            }
        }
    }

    private void InputEndTouch()
    {
        if (Board.EndTouch())
        {
            Board.ProcessBoard();
            UpdateMoves();
            StartCoroutine(_gameplayInput.CoroutineTurnWait());
        }
    }

    private void InputSwapTouch(Direction dir)
    {
        if (Board.SwapAction(dir))
        {
            Board.ProcessBoard();
            UpdateMoves();
            StartCoroutine(_gameplayInput.CoroutineTurnWait());
        }
    }

    private void InputStartTouch(Vector2 pos)
    {
        Vector2Int coords = DetermineClosestTile(pos);
        if (coords.x != -1)
        {
            Board.SelectTile(coords);
        }
    }

    void InitializeTexts()
    {
        MovesText.text = $"{_availableMoves}";
        ScoreText.text = $"{_currentScore}/{_config.ScoreOrBoxesNeeded}";
    }
    void UpdateMoves()
    {
        _availableMoves--;
        MovesText.text = $"{_availableMoves}";
        if (_availableMoves == 0)
        {
            GameLost.NotifyEvent(BuildGameEndInfo(false));
        }
    }

    void UpdateScore()
    {
        _currentScore++;
        ScoreText.text = $"{_currentScore}/{_config.ScoreOrBoxesNeeded}";
        if (_currentScore >= _config.ScoreOrBoxesNeeded)
        {
            GameWon.NotifyEvent(BuildGameEndInfo(true));
        }
    }

    GameEndInfo BuildGameEndInfo(bool won)
    {
        GameEndInfo gameEndInfo = new GameEndInfo(_availableMoves,_currentScore,PlayerProgression.CurrentLevel,PlayerProgression.MaxLevelUnlocked,won);
        return gameEndInfo;
    }

    public void OnDestroy()
    {
        _progressionProviderService.SaveGame();
        BoardEvents.TileCreated -= BoardEventsTileCreated;
        BoardEvents.TileChanged -= BoardEventsTileChanged;
        BoardEvents.TileMoved -= BoardEventsTileMoved;
        BoardEvents.TileSwapped -= BoardEventsTileSwapped;
        BoardEvents.TileDestroyed -= BoardEventsTileDestroyed;
        TileBooster.BoosterEvent -= TileBoosterEvent;
        TurnBooster.BoosterEvent -= TurnBoosterEvent;


        _gameplayInput.StartTouch -= InputStartTouch;
        _gameplayInput.SwapTouch -= InputSwapTouch;
        _gameplayInput.EndTouch -= InputEndTouch;
    }

    private void BoardEventsTileSwapped(Vector2 pos1, Vector2 pos2)
    {
        VisualTile tile = GetTileAtPos(pos1);
        VisualTile other = GetTileAtPos(pos2);
        tile.SetBoardPosition(pos2,MovementType.SWAP);
        other.SetBoardPosition(pos1,MovementType.SWAP);
    }

    private void BoardEventsTileDestroyed(Vector2 obj, TileType type)
    {
        VisualTile tile = GetTileAtPos(obj);
        tiles.Remove(tile);
        tile.DestroyVisualTile(true);
        if(_config.Objective == GameObjectives.BOXES&&(int)type==9)
        {
            UpdateScore();
        }
        else if(_config.Objective == GameObjectives.SCORE&&type >0&&(int)type<=5)
        {
            UpdateScore();
        }
    }

    private void BoardEventsTileMoved(Vector2 origin, Vector2 destination)
    {
        VisualTile tile = GetTileAtPos(origin);
        tile.SetBoardPosition(destination, MovementType.DOWNWARDS);
    }
    private void BoardEventsTileChanged(Vector2 pos, TileType type)
    {
        VisualTile tile = GetTileAtPos(pos);
        tiles.Remove(tile);
        tile.DestroyVisualTile(false);

        tile = Instantiate(TilePrefabs[(int)type],gameObject.transform).GetComponent<VisualTile>();
        tile.InitializeTile(pos, _config.TileSize,MovementType.CHANGE);
        tiles.Add(tile);
    }

    private void BoardEventsTileCreated(Vector2 pos, TileType type)
    {
        VisualTile tile = Instantiate(TilePrefabs[(int)type],gameObject.transform).GetComponent<VisualTile>();
        tile.InitializeTile( pos,_config.TileSize);
        tiles.Add(tile);
    }

    private void BoardEventsSpecialTileDestroyed(Vector2Int pos, SpecialTileCombination type)
    { 
        GameObject tmp = Instantiate(ParticlePrefabs[(int)type], gameObject.transform);
        tmp.transform.position = GetTileAtPos(pos).transform.position;
    }

    Vector2Int DetermineClosestTile(Vector2 screen_pos)
    {
        Vector2Int ret = new Vector2Int(-1,-1);
        Vector3 pos = Camera.main.ScreenToWorldPoint(screen_pos);
        Vector3 tmp = pos - transform.position;
        pos.z = 0;
        float x = (tmp.x / (_config.TileSize));
        float y = (tmp.y / (_config.TileSize));
        int rounded_x = (int)Mathf.Round(x);
        int rounded_y = (int)Mathf.Round(y);
        if (rounded_x >= 0 && rounded_x < _config.BoardWidth && rounded_y >= 0 && rounded_y < _config.BoardHeight)
        {
            ret = new Vector2Int(rounded_x, rounded_y);
        }
        return ret;
    }

    public static VisualTile GetTileAtPos(Vector2 pos)
    {
        VisualTile tile = tiles.Find(e => e.BoardPos == pos);
        if(tile == null)
        {
            Debug.Log($"COULDNT FIND TILE AT {pos.x},{pos.y}");
        }
        return tile;
    }

    //private void OnDrawGizmos()
    //{
    //    BoardConfig config = PlayerProgression.levels[PlayerProgression.CurrentLevel];
    //    float width =  config.TileSize * config.BoardWidth;
    //    float height = config.TileSize * config.BoardHeight;
    //    Vector3 center = transform.position;
    //    center.x += (width / 2)-config.TileSize/2;
    //    center.y += (height / 2)-config.TileSize/2;
    //    Color color = Color.black;
    //    color.a = 0.5f;
    //    Gizmos.color = color;
    //    Gizmos.DrawCube(center, new Vector3(width, height, 1));
    //}
}
