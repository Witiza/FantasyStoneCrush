using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Board : MonoBehaviour
{
    public GameObject tile_prefab;
    BoardPosition selected_tile;
    Vector2 initial_touch_position;
    public float minimum_magnitude = 1;
    BoardPosition[,] board = new BoardPosition[9,9];
    public float tile_size = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        UpdateBoardPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BoardPosition clicked = DetermineClosestTile(Input.mousePosition);
            Destroy(clicked.target_tile.gameObject);
            NotifyDisapear(clicked);
            
        }
        if (Input.touchCount >0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    SelectTile(touch.position);
                    initial_touch_position = touch.position;
                    break;
                case TouchPhase.Moved:
                    if(selected_tile != null)
                    { 
                    Vector2 diff = touch.position - initial_touch_position;
                        if ((diff).magnitude > minimum_magnitude)
                        {
                            SwapAction(diff);
                            selected_tile = null;
                        }
                    }
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    if (selected_tile != null)
                    {
                        selected_tile.target_tile.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    break;
                case TouchPhase.Canceled:
                    if (selected_tile != null)
                    {
                        selected_tile.target_tile.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void SelectTile(Vector2 screen_pos)
    {
        selected_tile = DetermineClosestTile(screen_pos);
        if (selected_tile.target_tile != null)
        {
            selected_tile.target_tile.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            selected_tile = null;
        }
    }
    BoardPosition DetermineClosestTile(Vector2 screen_pos)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(screen_pos);
        pos.z = 0;
        BoardPosition closest = board[0, 0];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if ((board[i,j].Position-pos).magnitude < (closest.Position-pos).magnitude)
                {
                    closest = board[i,j];
                }
            }
        }
        return closest;
    }
    //Debugging purposes
    void UpdateBoardPos()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                board[i, j] = new BoardPosition();
                board[i, j].Position = new Vector3(transform.position.x + i * tile_size, transform.position.y + j * tile_size, 0);
                board[i, j].Color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                board[i, j].board_position = new Vector2(i, j);
                board[i, j].reference = board;
                GameObject tile = Instantiate(tile_prefab);
                tile.transform.position = board[i, j].Position;
                board[i, j].target_tile = tile.GetComponent<Tile>();

            }
        }
    }

    void NotifyDisapear(BoardPosition deleted)
    {
        if(deleted.board_position.y !=8)
        {
            BoardPosition to_move = board[(int)deleted.board_position.x, (int)deleted.board_position.y + 1];
            if (to_move.target_tile != null)
            {
                deleted.target_tile = to_move.target_tile;
                deleted.target_tile.transform.position = deleted.Position;
                NotifyDisapear(to_move);
            }
            else
            {
                deleted.target_tile = null;
            }
        }
        else
        {
            deleted.target_tile = null;
        }
    }

    //Shit name
    //EL PROBLEMA ES QUE BUSCAN LAS X PRIMERAS, QUE AL MOVERLAS DEJAN OTRO HUECO DEL MISMO TAMAÑO. TENDRIAN QUE CAER TODAS LAS QUE ESTAN ARRIBA. QUIZAS ES BUENA IDEA HACER UNA LSITA CON TODAS LAS QUE HAY ARRIBA Y MOVERLAS X ESPACIOS.
    void DestroyAndMoveVertically(List<BoardPosition> tiles)
    {

        tiles = tiles.OrderBy(x => x.board_position.y).ToList();
        foreach (BoardPosition tile in tiles)
        {
            Destroy(tile.target_tile.gameObject);
            tile.target_tile = null;
        }
        foreach (BoardPosition tile in tiles)
        {
            GetHighestValid(tile);
        }
    }

    void GetHighestValid(BoardPosition tile)
    {
        BoardPosition tmp = tile;

        tile.target_tile = null;

        while(tmp.board_position.y<8&&tile.target_tile==null)
        {
            tmp = board[(int)tmp.board_position.x, (int)tmp.board_position.y + 1];
            if(tmp.target_tile!=null)
            {
                tile.target_tile = tmp.target_tile;
                tile.target_tile.transform.position = tile.Position;
                tmp.target_tile = null;
            }
        }
    }

    void NotifyNeighbours(BoardPosition tile)
    {
        int x = (int)tile.board_position.x;
        int y = (int)tile.board_position.y;

        //BoardPosition right = x < 8 ? board[x + 1, y] : null;
        //BoardPosition left = x > 0 ? board[x - 1, y] : null;
        //BoardPosition top = y < 8 ? board[x, y + 1] : null;
        //BoardPosition bottom = y > 0 ? board[x, y - 1] : null;
        List<BoardPosition> neighbours = new List<BoardPosition>();

        int count = 0;
        count +=tile.CheckRight(count,neighbours);
        int count2 = 0;
        count2 += tile.CheckLeft(count2, neighbours);
        if(count+count2 >=2)
        {
            foreach(BoardPosition neighbour in neighbours)
            {
                if (neighbour != tile)
                {
                    Destroy(neighbour.target_tile.gameObject);
                    NotifyDisapear(neighbour);
                }
            }
        }
        neighbours.Clear();
        int count3 = 0;
        count3 += tile.CheckUp(count3, neighbours);
        int count4 = 0;
        count4 += tile.CheckDown(count4, neighbours);
        if(count3+count4>=2)
        {
            neighbours.Add(tile);
            DestroyAndMoveVertically(neighbours);
        }
        else if(count + count2 >= 2)
        {
            Destroy(tile.target_tile.gameObject);
            NotifyDisapear(tile);
        }

        //Debug.Log($"Checking Right, the tile {tile.target_tile.tile_type} found {count} matches");
        //Debug.Log($"Checking Left, the tile {tile.target_tile.tile_type} found {count2} matches");
        //Debug.Log($"Checking Up, the tile {tile.target_tile.tile_type} found {count} matches");
        //Debug.Log($"Checking Down, the tile {tile.target_tile.tile_type} found {count2} matches");
    }

    void SwapAction(Vector2 diff)
    {
        BoardPosition other = null;
        if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
        {
            if (diff.y > 0)
            {
                other = SwapTop();
            }
            else
            {
                other = SwapBottom();
            }
        }
        else
        {
            if (diff.x > 0)
            {
                other = SwapRight();
            }
            else
            {
                other = SwapLeft();
            }
        }
        //This means that the swap has been done
        if(other != null)
        {
            NotifyNeighbours(other);
            NotifyNeighbours(selected_tile);
        }
    }
    BoardPosition SwapTop()
    {
        BoardPosition ret = null;
        if(selected_tile.board_position.y <8)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x, (int)selected_tile.board_position.y + 1];
            if (other.target_tile != null)
            {
                Tile tmp = selected_tile.target_tile;
                selected_tile.target_tile = other.target_tile;
                selected_tile.target_tile.gameObject.transform.position = selected_tile.Position;
                other.target_tile = tmp;
                other.target_tile.gameObject.transform.position = other.Position;
                tmp.GetComponent<SpriteRenderer>().color = Color.white;
                ret = other;
            }
        }
        return ret;
    }

    BoardPosition SwapBottom()
    {
        BoardPosition ret = null;
        if (selected_tile.board_position.y > 0)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x, (int)selected_tile.board_position.y - 1];
            if (other.target_tile != null)
            {
                Tile tmp = selected_tile.target_tile;
                selected_tile.target_tile = other.target_tile;
                selected_tile.target_tile.gameObject.transform.position = selected_tile.Position;
                other.target_tile = tmp;
                other.target_tile.gameObject.transform.position = other.Position;
                tmp.GetComponent<SpriteRenderer>().color = Color.white;
                ret = other;
            }
        }
        return ret;
    }

    BoardPosition SwapRight()
    {
        BoardPosition ret=null;
        if (selected_tile.board_position.x < 8)
        {

            BoardPosition other = board[(int)selected_tile.board_position.x+1, (int)selected_tile.board_position.y];
            if (other.target_tile != null)
            {
                Tile tmp = selected_tile.target_tile;
                selected_tile.target_tile = other.target_tile;
                selected_tile.target_tile.gameObject.transform.position = selected_tile.Position;
                other.target_tile = tmp;
                other.target_tile.gameObject.transform.position = other.Position;
                tmp.GetComponent<SpriteRenderer>().color = Color.white;
                ret = other;
            }
        }
        return ret;
    }

    BoardPosition SwapLeft()
    {
        BoardPosition ret = null;
        if (selected_tile.board_position.x > 0)
        {
            BoardPosition other = board[(int)selected_tile.board_position.x - 1, (int)selected_tile.board_position.y];
            if (other.target_tile != null)
            {
                Tile tmp = selected_tile.target_tile;
                selected_tile.target_tile = other.target_tile;
                selected_tile.target_tile.gameObject.transform.position = selected_tile.Position;
                other.target_tile = tmp;
                other.target_tile.gameObject.transform.position = other.Position;
                tmp.GetComponent<SpriteRenderer>().color = Color.white;
                ret = other;
            }
        }
        return ret;
    }
}
