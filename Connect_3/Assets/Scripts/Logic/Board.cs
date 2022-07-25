using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IGameplayInput))]
public class Board : MonoBehaviour
{
    BoardPosition _selectedTile;

    public static BoardPosition[,] GameBoard;
    public static float _tileSize = 0.5f;


    public static int BoardWidth = 12;
    public static int BoardHeight = 12;

    IGameplayInput _gameplayInput;

    private void Awake()
    {
        GameBoard = new BoardPosition[BoardWidth, BoardHeight];
    }
    void Start()
    {
        GenerateBoard();
    }

    void Update()
    {
        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                if (GameBoard[i, j].IsValid() && GameBoard[i, j].ToDestroy)
                {
                    TileRemover.DestroyTile(GameBoard[i, j]);
                    GameBoard[i, j].ToDestroy = false;
                }
            }
        }
        for (int i = 0;i< BoardWidth; i++)
        {
            for(int j = 0;j< BoardHeight; j++)
            {
                if (GameBoard[i, j].Dirty && GameBoard[i,j].IsValid())
                {
                    if (GameBoard[i, j].IsBaseTile())
                    {
                        NotifyNeighbours(GameBoard[i, j]);
                    }
                }
            }
        }

        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                if (!GameBoard[i, j].IsValid())
                {
                    if (!GetHighestValid(GameBoard[i, j]))
                    {
                        GameBoard[i, j]._type = (TileType)Random.Range(1, 6);
                        GameBoard[i, j].Dirty = true;
                        BoardEvents.NotifyCreated(GameBoard[i, j].BoardPos, (int)GameBoard[i, j]._type);
                    }
                }
            }
        }

    }

    void SelectTile(Vector2 screen_pos)
    {
        _selectedTile = DetermineClosestTile(screen_pos);
        if (_selectedTile != null)
        {
            if (_selectedTile.IsValid())
            {
                BoardEvents.NotifySelected(_selectedTile.BoardPos);
                if(_selectedTile.IsSpecialTile())
                {
                    TileRemover.DestroyTile(_selectedTile);
                }
            }
            else
            {
                _selectedTile = null;
            }
        }
    }
    BoardPosition DetermineClosestTile(Vector2 screen_pos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(screen_pos);
        Vector3 tmp = pos - transform.position;
        pos.z = 0;
        BoardPosition closest = null;
        float x = (tmp.x / (_tileSize));
        float y = (tmp.y / (_tileSize));
        int rounded_x = (int)Mathf.Round(x);
        int rounded_y = (int)Mathf.Round(y);
        if(rounded_x >=0&&rounded_x < BoardWidth && rounded_y >=0&&rounded_y< BoardHeight)
        {
            closest = GameBoard[rounded_x, rounded_y];
        }
        return closest;
    }
    void GenerateBoard()
    {
        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                GameBoard[i, j] = new BoardPosition();
                GameBoard[i, j].BoardPos = new Vector2(i, j);
                GameBoard[i, j]._type = (TileType)Random.Range(1, 6);
                BoardEvents.NotifyCreated(GameBoard[i, j].BoardPos, (int)GameBoard[i, j]._type);
            }
        }
    }

    bool GetHighestValid(BoardPosition deleted)
    {
        bool ret = false;
        BoardPosition tmp = deleted;

        deleted._type = TileType.NULL;

        while(tmp.BoardPos.y<BoardHeight-1&&!deleted.IsValid())
        {
            tmp = GameBoard[(int)tmp.BoardPos.x, (int)tmp.BoardPos.y + 1];
            if(tmp.IsValid())
            {
                deleted._type = tmp._type;
                BoardEvents.NotifyMoved(tmp.BoardPos, deleted.BoardPos);
                deleted.Dirty = true;
                tmp._type = TileType.NULL;
                ret = true;
            }
        }
        return ret;
    }

    bool CanSwap(BoardPosition tile)
    {
        bool ret = false;
        if (tile.IsBaseTile())
        {
            List<BoardPosition> horizontal_neighbours;
            List<BoardPosition> vertical_neighbours;

            GetNeighbours(tile, out horizontal_neighbours, out vertical_neighbours);
            if (horizontal_neighbours.Count >= 2 || vertical_neighbours.Count >= 2)
            {
                ret = true;
            }
        }
        else if(tile.IsSpecialTile())
        {
            ret = true;
        }
        return ret;
    }
    void GetNeighbours(BoardPosition tile, out List<BoardPosition> horizontal, out List<BoardPosition> vertical)
    {
        horizontal = new List<BoardPosition>();
        vertical = new List<BoardPosition>();

        tile.CheckRight(horizontal);
        tile.CheckLeft(horizontal);
        tile.CheckUp(vertical);
        tile.CheckDown(vertical);
    }
    void NotifyNeighbours(BoardPosition tile)
    {
        List<BoardPosition> horizontal_neighbours;
        List<BoardPosition> vertical_neighbours;

        GetNeighbours(tile, out horizontal_neighbours, out vertical_neighbours);
        int horizontal_count = horizontal_neighbours.Count;
        int vertical_count = vertical_neighbours.Count;
        if (horizontal_count >= 2 || vertical_count >= 2)
        {
            if (horizontal_count >= 2)
            {
                foreach (BoardPosition neighbour in horizontal_neighbours)
                {
                    TileRemover.DestroyTile(neighbour);
                }
            }
            if (vertical_count >= 2)
            {
                foreach (BoardPosition neighbour in vertical_neighbours)
                {
                    TileRemover.DestroyTile(neighbour);
                }
            }
            TileRemover.DestroyTile(tile);
            SpecialTileGeneration(tile, vertical_neighbours, horizontal_neighbours);
        }
        tile.Dirty = false;
    }

    public static void DestroyRow(int row)
    {
        if(CoordinateInsideX(row))
        {
            for(int i = 0;i<BoardWidth;i++)
            {
                GameBoard[row, i].ToDestroy = true;
            }
        }
    }

    public static void DestroyColumn(int column)
    {
        if (CoordinateInsideY(column))
        {
            for (int i = 0; i < BoardHeight; i++)
            {
                GameBoard[i, column].ToDestroy = true;
            }
        }
    }

    public static void DestroyArea(int size,Vector2 pos)
    {
        int x;
        int y;
        for(int i = -size;i<=size;i++)
        {
            for(int j = -size; j<=size;j++)
            {
                x = (int)pos.x + i;
                y = (int)pos.y + j;
                if(CoordinatesInsideBoard(x,y))
                {
                    GameBoard[x, y].ToDestroy = true;
                }
            }
        }
    }
    static bool CoordinatesInsideBoard(int x, int y)
    {
        return CoordinateInsideX(x) && CoordinateInsideY(y);
    }

    public static bool CoordinateInsideY(int coord)
    {
        return coord >= 0 && coord < BoardHeight;
    }
    public static bool CoordinateInsideX(int coord)
    {
        return coord >= 0 && coord < BoardWidth;
    }
    void SpecialTileGeneration(BoardPosition tile, List<BoardPosition> vertical, List<BoardPosition> horizontal)
    {
        if(vertical.Count >=2 && horizontal.Count >= 2)
        {
            tile._type = TileType.BOMB;
            BoardEvents.NotifyCreated(tile.BoardPos, (int)TileType.BOMB);
        }
        else if(horizontal.Count > 2)
        {
            tile._type = TileType.HORIZONTAL_ROCKET;
            BoardEvents.NotifyCreated(tile.BoardPos, (int)TileType.HORIZONTAL_ROCKET);
        }
        else if(vertical.Count > 2)
        {
            tile._type = TileType.VERTICAL_ROCKET;
            BoardEvents.NotifyCreated(tile.BoardPos, (int)TileType.VERTICAL_ROCKET);
        }
    }
    void SwapAction(Direction dir)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                SwapRight();
                break;
            case Direction.LEFT:
                SwapLeft();
                break;
            case Direction.UP:
                SwapTop();
                break;
            case Direction.DOWN:
                SwapBottom();
                break;
            default:
                break;
        }
    }

    bool SwapTop()
    {
        bool ret = false;
        if( CoordinateInsideY((int)_selectedTile.BoardPos.y+1))
        {
            BoardPosition other = GameBoard[(int)_selectedTile.BoardPos.x, (int)_selectedTile.BoardPos.y + 1];
            ret = SwapTiles(_selectedTile, other);
        }
        return ret;
    }
    bool SwapBottom()
    {
        bool ret = false;
        if (CoordinateInsideY((int)_selectedTile.BoardPos.y-1))
        {
                BoardPosition other = GameBoard[(int)_selectedTile.BoardPos.x, (int)_selectedTile.BoardPos.y - 1];
            ret = SwapTiles(_selectedTile, other);
        }
        return ret;
    }
    bool SwapRight()
    {
        bool ret=false;
        if (CoordinateInsideX((int)_selectedTile.BoardPos.x+1))
        {
            BoardPosition other = GameBoard[(int)_selectedTile.BoardPos.x+1, (int)_selectedTile.BoardPos.y];
            ret = SwapTiles(_selectedTile, other);
        }
        return ret;
    }

    bool SwapLeft()
    {
        bool ret = false;
        if (CoordinateInsideX((int)_selectedTile.BoardPos.x-1))
        {
            BoardPosition other = GameBoard[(int)_selectedTile.BoardPos.x - 1, (int)_selectedTile.BoardPos.y];
            ret = SwapTiles(_selectedTile, other);
            }
        return ret;
    }

    bool SwapTiles(BoardPosition tile, BoardPosition other)
    {
        bool ret = false;
        if (other.IsValid())
        {
            TileType tmp = tile._type;
            tile._type = other._type;
            other._type = tmp;
            if (CanSwap(tile) || CanSwap(other))
            {
                BoardEvents.NotifySwap(tile.BoardPos, other.BoardPos);
                tile.Dirty = true;
                other.Dirty = true;
                ret = true;
            }
            else
            {
                tmp = tile._type;
                tile._type = other._type;
                other._type = tmp;
            }

        }
        return ret;
    }

    private void OnEnable()
    {
        _gameplayInput = gameObject.GetComponent<IGameplayInput>();
        _gameplayInput.StartTouch += InputStartTouch;
        _gameplayInput.SwapTouch += InputSwap;
        _gameplayInput.EndTouch += InputEndTouch;
    }
    private void OnDisable()
    {
        _gameplayInput.StartTouch -= InputStartTouch;
        _gameplayInput.SwapTouch -= InputSwap;
        _gameplayInput.EndTouch -= InputEndTouch;
    }
    private void InputSwap(Direction dir)
    {
        if (_selectedTile != null)
        {
            SwapAction(dir);
            BoardEvents.NotifyUnselected(_selectedTile.BoardPos);
            _selectedTile = null;
        }
    }

    private void InputEndTouch()
    {
        if (_selectedTile != null)
        {
            BoardEvents.NotifyUnselected(_selectedTile.BoardPos);
        }
    }

    private void InputStartTouch(Vector2 pos)
    {
        SelectTile(pos);
    }
}
