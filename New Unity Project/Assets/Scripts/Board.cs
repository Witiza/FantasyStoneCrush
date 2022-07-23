using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//BoardPosition should have a variable type, and emmit events saying the type of tyle and the coordinates. They should also send events to move xy tile to xy position
[RequireComponent(typeof(IGameplayInput))]
public class Board : MonoBehaviour
{
    BoardPosition selected_tile;
    public static BoardPosition[,] board = new BoardPosition[9,9];
    public static float tile_size = 0.5f;
    IGameplayInput gameplay_input;

    private void Awake()
    {

    }
    void Start()
    {
        UpdateBoardPos();
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j].IsValid())
                {
                    if (board[i, j].to_destroy)
                    {
                        TileRemover.DestroyTile(board[i, j]);
                        board[i, j].to_destroy = false;
                    }
                }
            }
        }
        for (int i = 0;i<9;i++)
        {
            for(int j = 0;j<9;j++)
            {
                if (board[i, j].dirty && board[i,j].IsValid())
                {
                    //This should call the board position funciton and handle it internally
                    if (board[i, j].IsBaseTile())
                    {
                        NotifyNeighbours(board[i, j]);
                    }
                    else if (board[i,j].IsSpecialTile())
                    {
                       // Debug.Log("EIN?");
                        //TileRemover.DestroyTile(board[i, j]);
                    }
                }
            }
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (!board[i, j].IsValid())
                {
                    if (!GetHighestValid(board[i, j]))
                    {
                        board[i, j].type = (TileType)Random.Range(1, 6);
                        BoardEvents.NotifyCreated(board[i, j].board_position, (int)board[i, j].type);
                    }
                }
            }
        }

    }

    void SelectTile(Vector2 screen_pos)
    {
        selected_tile = DetermineClosestTile(screen_pos);
        if (selected_tile != null)
        {
            if (selected_tile.IsValid())
            {
                BoardEvents.NotifySelected(selected_tile.board_position);
                if(selected_tile.IsSpecialTile())
                {
                    TileRemover.DestroyTile(selected_tile);
                    Debug.Log("selected special");
                }
            }
            else
            {
                selected_tile = null;
            }
        }
    }
    //Find better way to do this
    BoardPosition DetermineClosestTile(Vector2 screen_pos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(screen_pos);
        Vector3 tmp = pos - transform.position;
        pos.z = 0;
        BoardPosition closest = null;
        float x = (tmp.x / (tile_size));
        float y = (tmp.y / (tile_size));
        int rounded_x = (int)Mathf.Round(x);
        int rounded_y = (int)Mathf.Round(y);
        if(rounded_x >=0&&rounded_x < 9 && rounded_y >=0&&rounded_y<9)
        {
            closest = board[rounded_x, rounded_y];
        }
        return closest;
    }
    void UpdateBoardPos()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                board[i, j] = new BoardPosition();
                board[i, j].board_position = new Vector2(i, j);
                board[i, j].reference = board;
                board[i, j].type = (TileType)Random.Range(1, 6);
                BoardEvents.NotifyCreated(board[i, j].board_position, (int)board[i, j].type);
            }
        }
    }

    //This funciton should tell something tot he tile script, so it does de tween shit.
    //void MoveVisualTile(BoardPosition tile)
    //{
    //   tile.target_tile.transform.position = new Vector3(transform.position.x + tile.board_position.x * tile_size, transform.position.y + tile.board_position.y* tile_size, 0);
    //}
    bool GetHighestValid(BoardPosition deleted)
    {
        bool ret = false;
        BoardPosition tmp = deleted;

        deleted.type = TileType.NULL;

        while(tmp.board_position.y<8&&!deleted.IsValid())
        {
            tmp = board[(int)tmp.board_position.x, (int)tmp.board_position.y + 1];
            if(tmp.IsValid())
            {
                deleted.type = tmp.type;
                BoardEvents.NotifyMoved(tmp.board_position, deleted.board_position);
                deleted.dirty = true;
                tmp.type = TileType.NULL;
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
    //Needs to be sepparated into getting neighbours and doing things to them.
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
        else
        {
            //NO MATCHES?
        }
        tile.dirty = false;
    }

    public static void DestroyRow(int row)
    {
        if(CoordinateInsideBoard(row))
        {
            for(int i = 0;i<9;i++)
            {
                board[row, i].to_destroy = true;
            }
        }
    }

    public static void DestroyColumn(int column)
    {
        if (CoordinateInsideBoard(column))
        {
            for (int i = 0; i < 9; i++)
            {
                board[i, column].to_destroy = true;
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
                    //Added to destroy otherwise two bombs will keep exploding each other
                    //TileRemover.DestroyTile(board[x, y]);
                    Debug.Log($"Destroying tile at {x},{y}");
                    board[x, y].to_destroy = true;
                }
            }
        }
    }
    static bool CoordinatesInsideBoard(int x, int y)
    {
        return CoordinateInsideBoard(x) && CoordinateInsideBoard(y);
    }

    static bool CoordinateInsideBoard(int coord)
    {
        return coord >= 0 && coord < 9;
    }
    //This should be in its own script or someting
    void SpecialTileGeneration(BoardPosition tile, List<BoardPosition> vertical, List<BoardPosition> horizontal)
    {
        if(vertical.Count + horizontal.Count > 2)
        {
            //generate bomb
            tile.type = TileType.BOMB;
            BoardEvents.NotifyCreated(tile.board_position, (int)TileType.BOMB);
        }
        else if(horizontal.Count > 2)
        {
            //horizontal missile
            tile.type = TileType.HORIZONTAL_ROCKET;

            BoardEvents.NotifyCreated(tile.board_position, (int)TileType.HORIZONTAL_ROCKET);
        }
        else if(vertical.Count > 2)
        {
            //vertical missile
            tile.type = TileType.VERTICAL_ROCKET;
            BoardEvents.NotifyCreated(tile.board_position, (int)TileType.VERTICAL_ROCKET);
        }
    }
    void SwapAction(Direction dir)
    {
        bool swapped = false;
        switch (dir)
        {
            case Direction.RIGHT:
                swapped = SwapRight();
                break;
            case Direction.LEFT:
                swapped = SwapLeft();
                break;
            case Direction.UP:
                swapped = SwapTop();
                break;
            case Direction.DOWN:
                swapped = SwapBottom();
                break;
            default:
                break;
        }
    }

    bool SwapTop()
    {
        bool ret = false;
        if(selected_tile.board_position.y <8)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x, (int)selected_tile.board_position.y + 1];
            ret = SwapVisualTile(selected_tile, other);
        }
        return ret;
    }

    bool SwapBottom()
    {
        bool ret = false;
        if (selected_tile.board_position.y > 0)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x, (int)selected_tile.board_position.y - 1];
            ret = SwapVisualTile(selected_tile, other);
        }
        return ret;
    }

    bool SwapRight()
    {
        bool ret=false;
        if (selected_tile.board_position.x < 8)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x+1, (int)selected_tile.board_position.y];
            ret = SwapVisualTile(selected_tile, other);
        }
        return ret;
    }

    bool SwapLeft()
    {
        bool ret = false;
        if (selected_tile.board_position.x > 0)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x - 1, (int)selected_tile.board_position.y];
            ret = SwapVisualTile(selected_tile, other);
            }
        return ret;
    }

    //Needs some refactoring. visual position and visual color should change somewhere else.
    bool SwapVisualTile(BoardPosition tile, BoardPosition other)
    {
        bool ret = false;
        if (other.IsValid())
        {
            TileType tmp = tile.type;
            tile.type = other.type;
            other.type = tmp;
            if (CanSwap(tile) || CanSwap(other))
            {
                BoardEvents.NotifySwap(tile.board_position, other.board_position);
                tile.dirty = true;
                other.dirty = true;
                ret = true;
            }
            else
            {
                //We undo the swap, this should be in another function
                tmp = tile.type;
                tile.type = other.type;
                other.type = tmp;
            }

        }
        return ret;
    }

    private void OnEnable()
    {
        gameplay_input = gameObject.GetComponent<IGameplayInput>();
        gameplay_input.StartTouch += InputStartTouch;
        gameplay_input.Swap += InputSwap;
        gameplay_input.EndTouch += InputEndTouch;
    }
    private void OnDisable()
    {
        gameplay_input.StartTouch -= InputStartTouch;
        gameplay_input.Swap -= InputSwap;
        gameplay_input.EndTouch -= InputEndTouch;
    }
    private void InputSwap(Direction dir)
    {
        if (selected_tile != null)
        {
            SwapAction(dir);
            BoardEvents.NotifyUnselected(selected_tile.board_position);
            selected_tile = null;
        }
    }

    private void InputEndTouch()
    {
        if (selected_tile != null)
        {
            BoardEvents.NotifyUnselected(selected_tile.board_position);
        }
    }

    private void InputStartTouch(Vector2 pos)
    {
        SelectTile(pos);
    }
}
