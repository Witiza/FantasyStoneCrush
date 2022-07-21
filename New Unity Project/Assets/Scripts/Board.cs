using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(IGameplayInput))]
public class Board : MonoBehaviour
{
    public GameObject tile_prefab;
    BoardPosition selected_tile;
    public static BoardPosition[,] board = new BoardPosition[9,9];
    public float tile_size = 0.5f;
    IGameplayInput gameplay_input;

    void Start()
    {
        UpdateBoardPos();
    }

    void Update()
    {
        for(int i = 0;i<9;i++)
        {
            for(int j = 0;j<9;j++)
            {
                if (board[i, j].dirty && board[i,j].target_tile != null )
                {
                    NotifyNeighbours(board[i, j]);
                }
            }
        }
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j].target_tile == null)
                {
                    if (!GetHighestValid(board[i, j]))
                    {
                        GameObject tile = Instantiate(tile_prefab);
                        board[i, j].target_tile = tile.GetComponent<Tile>();
                        MoveVisualTile(board[i, j]);
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
            if (selected_tile.target_tile != null)
            {
                selected_tile.target_tile.GetComponent<SpriteRenderer>().color = Color.red;
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
        //for (int i = 0; i < 9; i++)
        //{
        //    for (int j = 0; j < 9; j++)
        //    {
        //        if ((board[i,j].Position-pos).magnitude < (closest.Position-pos).magnitude)
        //        {
        //            closest = board[i,j];
        //        }
        //    }
        //}
        //board[i, j].Position = new Vector3(transform.position.x + i * tile_size, transform.position.y + j * tile_size, 0);
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
                board[i, j] = new BaseTile();
                board[i, j].board_position = new Vector2(i, j);
                board[i, j].reference = board;
                GameObject tile = Instantiate(tile_prefab);
                board[i, j].target_tile = tile.GetComponent<Tile>();
                MoveVisualTile(board[i, j]);
                
            }
        }
    }

    //This funciton should tell something tot he tile script, so it does de tween shit.
    void MoveVisualTile(BoardPosition tile)
    {
       tile.target_tile.transform.position = new Vector3(transform.position.x + tile.board_position.x * tile_size, transform.position.y + tile.board_position.y* tile_size, 0);
    }
    bool GetHighestValid(BoardPosition deleted)
    {
        bool ret = false;
        BoardPosition tmp = deleted;

        deleted.target_tile = null;

        while(tmp.board_position.y<8&&deleted.target_tile==null)
        {
            tmp = board[(int)tmp.board_position.x, (int)tmp.board_position.y + 1];
            if(tmp.target_tile!=null)
            {
                deleted.target_tile = tmp.target_tile;
                MoveVisualTile(deleted);
                deleted.dirty = true;
                tmp.target_tile = null;
                ret = true;
            }
        }
        return ret;
    }

    bool CanSwap(BoardPosition tile)
    {
        bool ret = false;
        List<BoardPosition> horizontal_neighbours;
        List<BoardPosition> vertical_neighbours;

        GetNeighbours(tile, out horizontal_neighbours, out vertical_neighbours);
        if(horizontal_neighbours.Count>=2||vertical_neighbours.Count>=2)
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
                    neighbour.DestroyTile();

                    Destroy(neighbour.target_tile.gameObject);
                    neighbour.target_tile = null;
                }
            }
            if (vertical_count >= 2)
            {
                foreach (BoardPosition neighbour in vertical_neighbours)
                {
                    neighbour.DestroyTile();
                    Destroy(neighbour.target_tile.gameObject);
                    neighbour.target_tile = null;
                }
            }
            tile.DestroyTile();

        }
        else
        {
            //NO MATCHES?
        }
        tile.dirty = false;
    }

    //This should be in its own script or someting
    void SpecialTileGeneration(BoardPosition tile, List<BoardPosition> vertical, List<BoardPosition> horizontal)
    {
        if(vertical.Count + horizontal.Count > 2)
        {
            //generate bomb
        }
        else if(horizontal.Count > 2)
        {
            //horizontal missile
        }
        else if(vertical.Count > 2)
        {
            //vertical missile
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
        if (other.target_tile != null)
        {
            
            Tile tmp = tile.target_tile;
            tile.target_tile = other.target_tile;
            other.target_tile = tmp;
            if (CanSwap(tile) || CanSwap(other))
            {
                MoveVisualTile(other);
                MoveVisualTile(tile);
                tile.dirty = true;
                other.dirty = true;
                ret = true;
            }
            else
            {
                //We undo the swap, this should be in another function
                tmp = tile.target_tile;
                tile.target_tile = other.target_tile;
                other.target_tile = tmp;
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
            selected_tile.target_tile.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            selected_tile = null;
        }
    }

    private void InputEndTouch()
    {
        if (selected_tile != null)
        {
            selected_tile.target_tile.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void InputStartTouch(Vector2 pos)
    {
        SelectTile(pos);
    }
}
