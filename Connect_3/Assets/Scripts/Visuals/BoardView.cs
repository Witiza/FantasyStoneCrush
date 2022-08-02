using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IGameplayInput))]
public class BoardView : MonoBehaviour
{
    public static List<VisualTile> tiles = new List<VisualTile>();
    public GameObject tile_prefab;
    BoardController _board;
    VisualSelector _visualSelector;
    [SerializeField]
    public BoardConfig Config;
    IGameplayInput _gameplayInput;

    private void Awake()  
    {
        tiles.Clear();
        BoardEvents.TileCreated += BoardEvents_TileCreated;
        BoardEvents.TileMoved += BoardEvents_TileMoved;
        BoardEvents.TileSwapped += BoardEvents_TileSwapped;
        BoardEvents.TileDestroyed += BoardEvents_TileDestroyed;
        _gameplayInput = gameObject.GetComponent<IGameplayInput>();
        _gameplayInput.StartTouch += InputStartTouch; ;
        _gameplayInput.SwapTouch += InputSwapTouch; ;
        _gameplayInput.EndTouch += InputEndTouch; 
        _board = new BoardController(Config.BoardWidth, Config.BoardHeight);
        _visualSelector = new VisualSelector();
    }

    private void InputEndTouch()
    {
        if (_board.EndTouch())
        {
            _board.ProcessBoard();
        }
    }

    private void InputSwapTouch(Direction dir)
    {
        if (_board.SwapAction(dir))
        {
            _board.ProcessBoard();
        }
    }

    private void InputStartTouch(Vector2 pos)
    {
        Vector2Int coords = DetermineClosestTile(pos);
        if (coords.x != -1)
        {
            _board.SelectTile(coords);
        }
    }

    public void OnDestroy()
    {
        BoardEvents.TileCreated -= BoardEvents_TileCreated;
        BoardEvents.TileMoved -= BoardEvents_TileMoved;
        BoardEvents.TileSwapped -= BoardEvents_TileSwapped;
        BoardEvents.TileDestroyed -= BoardEvents_TileDestroyed;
        _gameplayInput.StartTouch -= InputStartTouch;
        _gameplayInput.SwapTouch -= InputSwapTouch;
        _gameplayInput.EndTouch -= InputEndTouch;
    }

    private void BoardEvents_TileSwapped(Vector2 pos1, Vector2 pos2)
    {
        VisualTile tile = GetTileAtPos(pos1);
        VisualTile other = GetTileAtPos(pos2);
        tile.SetBoardPosition(pos2);
        other.SetBoardPosition(pos1);
    }

    private void BoardEvents_TileDestroyed(Vector2 obj,int type)
    {
        VisualTile tile = GetTileAtPos(obj);
        tiles.Remove(tile);
        Destroy(tile.gameObject);
    }

    private void BoardEvents_TileMoved(Vector2 origin, Vector2 destination)
    {
        VisualTile tile = GetTileAtPos(origin);
        tile.SetBoardPosition(destination);
    }

    private void BoardEvents_TileCreated(Vector2 pos, int type)
    {
        VisualTile tile = Object.Instantiate(tile_prefab).GetComponent<VisualTile>();

        //Initialization should be methods in a viewtilehandler or sumthing
        tile.InitializeTile((TileType)type, pos,Config.TileSize);
        tiles.Add(tile);
    }

    Vector2Int DetermineClosestTile(Vector2 screen_pos)
    {
        Vector2Int ret = new Vector2Int(-1,-1);
        Vector3 pos = Camera.main.ScreenToWorldPoint(screen_pos);
        Vector3 tmp = pos - transform.position;
        pos.z = 0;
        float x = (tmp.x / (Config.TileSize));
        float y = (tmp.y / (Config.TileSize));
        int rounded_x = (int)Mathf.Round(x);
        int rounded_y = (int)Mathf.Round(y);
        if (rounded_x >= 0 && rounded_x < Config.BoardWidth && rounded_y >= 0 && rounded_y < Config.BoardHeight)
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
}
