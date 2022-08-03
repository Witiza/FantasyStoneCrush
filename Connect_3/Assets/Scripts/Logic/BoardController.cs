using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoardController
{
    BoardPosition _selectedTile;
    BoardModel _board;
    public BoardController(int width, int height,List<BoardPosition> board = null)
    {
        _board = new BoardModel(width, height, board);
        CheckForMatches();
    }

    public void ProcessBoard()
    {
        int changed = -1;
        int iterations = 0;
        while (changed != 0&&iterations<100)
        {
            iterations++;
            changed = 0;
            changed += ProcessTilesDestroyedBySpecials();
            changed += ProcessMatches();
            changed += ProcessTileMovement();
            changed += CheckForMatches();
        }
        Debug.Log($"Iterations done on the board: {iterations}");
    }

    int ProcessTilesDestroyedBySpecials()
    {
        int ret = 0;
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (_board[i, j].IsValid() && _board[i, j].ToDestroy)
                {
                    DestroyTile(_board[i, j]);
                    _board[i, j].ToDestroy = false;
                    ret = 1;
                }
            }
        }
        return ret;
    }

    int ProcessMatches()
    {
        int ret = 0;
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (_board[i, j].Dirty && _board[i, j].IsValid())
                {
                    if (_board[i, j].IsBaseTile())
                    {
                        NotifyNeighbours(_board[i, j]);
                        ret = 1;
                    }
                }
            }
        }
        return ret;
    }

    int ProcessTileMovement()
    {
        int ret = 0;
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                if (!_board[i, j].IsValid())
                {
                    if (!GetHighestValid(_board[i, j]))
                    {
                        _board[i, j].Type = (TileType)Random.Range(1, 6);
                        _board[i, j].Dirty = true;
                        BoardEvents.NotifyCreated(_board[i, j].BoardPos, (int)_board[i, j].Type);
                        ret = 1;
                    }
                }
            }
        }
        return ret;
    }
    public void SelectTile(Vector2Int coords)
    {
        _selectedTile = _board[coords];
        if (_selectedTile != null)
        {
            if (_selectedTile.IsValid())
            {
                BoardEvents.NotifySelected(_selectedTile.BoardPos);
            }
            else
            {
                _selectedTile = null;
            }
        }
    }

    bool GetHighestValid(BoardPosition deleted)
    {
        bool ret = false;
        BoardPosition tmp = deleted;

        deleted.Type = TileType.NULL;
        while(tmp.BoardPos.y<_board.Height-1&&!deleted.IsValid())
        {
            tmp = _board[(int)tmp.BoardPos.x, (int)tmp.BoardPos.y + 1];
            if (tmp.IsBox())
            {
                ret = true;
                break;
            }
            if(tmp.IsValid())
            {
                deleted.Type = tmp.Type;
                BoardEvents.NotifyMoved(tmp.BoardPos, deleted.BoardPos);
                deleted.Dirty = true;
                tmp.Type = TileType.NULL;
                ret = true;
                break;
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
                    DestroyTile(neighbour);
                }
            }
            if (vertical_count >= 2)
            {
                foreach (BoardPosition neighbour in vertical_neighbours)
                {
                    DestroyTile(neighbour);
                }
            }
            DestroyTile(tile);
            SpecialTileGeneration(tile, vertical_neighbours, horizontal_neighbours);
        }
        tile.Dirty = false;
    }

    public void DestroyRow(int row)
    {
        if(_board.CoordinateInsideX(row))
        {
            for(int i = 0;i<_board.Width;i++)
            {
                _board[row, i].ToDestroy = true;
            }
        }
    }

    public void DestroyColumn(int column)
    {
        if (_board.CoordinateInsideY(column))
        {
            for (int i = 0; i < _board.Height; i++)
            {
                _board[i, column].ToDestroy = true;
            }
        }
    }

    public void DestroyArea(int size,Vector2 pos)
    {
        int x;
        int y;
        for(int i = -size;i<=size;i++)
        {
            for(int j = -size; j<=size;j++)
            {
                x = (int)pos.x + i;
                y = (int)pos.y + j;
                if(_board.CoordinatesInsideBoard(x,y))
                {
                    _board[x, y].ToDestroy = true;
                }
            }
        }
    }

    void SpecialTileGeneration(BoardPosition tile, List<BoardPosition> vertical, List<BoardPosition> horizontal)
    {
        if(vertical.Count >=2 && horizontal.Count >= 2)
        {
            tile.Type = TileType.BOMB;
            BoardEvents.NotifyCreated(tile.BoardPos, (int)TileType.BOMB);
        }
        else if(horizontal.Count > 2)
        {
            tile.Type = TileType.HORIZONTAL_ROCKET;
            BoardEvents.NotifyCreated(tile.BoardPos, (int)TileType.HORIZONTAL_ROCKET);
        }
        else if(vertical.Count > 2)
        {
            tile.Type = TileType.VERTICAL_ROCKET;
            BoardEvents.NotifyCreated(tile.BoardPos, (int)TileType.VERTICAL_ROCKET);
        }
    }

    public  void DestroyTile(BoardPosition tile)
    {
        switch (tile.Type)
        {
            case TileType.NULL:
                return;
                break;
            case TileType.SHIELD:
                break;
            case TileType.DAGGER:
                break;
            case TileType.ARROW:
                break;
            case TileType.WAND:
                break;
            case TileType.CHALICE:
                break;
            case TileType.BOMB:
                DestroyArea(1, tile.BoardPos);
                break;
            case TileType.HORIZONTAL_ROCKET:
                DestroyRow(tile.BoardPos.x);
                break;
            case TileType.VERTICAL_ROCKET:
                DestroyColumn(tile.BoardPos.y);
                break;
            default:
                break;
        }
        BoardEvents.NotifyDestroyed(tile.BoardPos, (int)tile.Type);
        tile.Type = TileType.NULL;
    }

    //This function should be better coded
    void CombineSpecialTiles(BoardPosition tile, BoardPosition other)
    {
        Vector2Int pos = tile.BoardPos;
        if(tile.Type == TileType.BOMB)
        {
            if(other.Type == TileType.BOMB)
            {
                DestroyArea(3, pos);
            }
            else if(other.Type == TileType.VERTICAL_ROCKET||other.Type==TileType.HORIZONTAL_ROCKET)
            {
                DestroyRow(pos.x - 1);
                DestroyRow(pos.x);
                DestroyRow(pos.x + 1);
                DestroyColumn(pos.y - 1);
                DestroyColumn(pos.y);
                DestroyColumn(pos.y + 1);
            }
        }
        else if(tile.Type == TileType.VERTICAL_ROCKET||tile.Type ==TileType.HORIZONTAL_ROCKET)
        {
            if (other.Type == TileType.BOMB)
            {
                DestroyRow(pos.x - 1);
                DestroyRow(pos.x);
                DestroyRow(pos.x + 1);
                DestroyColumn(pos.y - 1);
                DestroyColumn(pos.y);
                DestroyColumn(pos.y + 1);
            }
            else if( other.Type == TileType.VERTICAL_ROCKET||tile.Type==TileType.HORIZONTAL_ROCKET)
            {
                DestroyRow(pos.x);
                DestroyColumn(pos.y);
            }
        }
    }
    public bool SwapAction(Direction dir)
    {
        bool ret = false;
        if (_selectedTile != null)
        {
            switch (dir)
            {
                case Direction.RIGHT:
                    ret = SwapRight();
                    break;
                case Direction.LEFT:
                    ret = SwapLeft();
                    break;
                case Direction.UP:
                    ret = SwapTop();
                    break;
                case Direction.DOWN:
                    ret = SwapBottom();
                    break;
                default:
                    break;
            }
            BoardEvents.NotifyUnselected(_selectedTile.BoardPos);
            _selectedTile = null;
        }
        return ret;
    }

    bool SwapTop()
    {
        bool ret = false;
        if(_board.CoordinateInsideY((int)_selectedTile.BoardPos.y+1))
        {
            BoardPosition other = _board[(int)_selectedTile.BoardPos.x, (int)_selectedTile.BoardPos.y + 1];
            ret = SwapTiles(_selectedTile, other);
        }
        return ret;
    }
    bool SwapBottom()
    {
        bool ret = false;
        if (_board.CoordinateInsideY((int)_selectedTile.BoardPos.y-1))
        {
                BoardPosition other = _board[(int)_selectedTile.BoardPos.x, (int)_selectedTile.BoardPos.y - 1];
            ret = SwapTiles(_selectedTile, other);
        }
        return ret;
    }
    bool SwapRight()
    {
        bool ret=false;
        if (_board.CoordinateInsideX((int)_selectedTile.BoardPos.x+1))
        {
            BoardPosition other = _board[(int)_selectedTile.BoardPos.x+1, (int)_selectedTile.BoardPos.y];
            ret = SwapTiles(_selectedTile, other);
        }
        return ret;
    }

    bool SwapLeft()
    {
        bool ret = false;
        if (_board.CoordinateInsideX((int)_selectedTile.BoardPos.x-1))
        {
            BoardPosition other = _board[(int)_selectedTile.BoardPos.x - 1, (int)_selectedTile.BoardPos.y];
            ret = SwapTiles(_selectedTile, other);
            }
        return ret;
    }

    bool SwapTiles(BoardPosition tile, BoardPosition other)
    {
        bool ret = false;
        if (other.IsValid())
        {
            if (tile.IsBaseTile())
            {
                TileType tmp = tile.Type;
                tile.Type = other.Type;
                other.Type = tmp;
                if (CanSwap(tile) || CanSwap(other))
                {
                    BoardEvents.NotifySwap(tile.BoardPos, other.BoardPos);
                    tile.Dirty = true;
                    other.Dirty = true;
                    ret = true;
                }
                else
                {
                    tmp = tile.Type;
                    tile.Type = other.Type;
                    other.Type = tmp;
                }

            }
            else if(tile.IsSpecialTile())
            {
                if(other.IsBaseTile())
                {
                    TileType tmp = tile.Type;
                    tile.Type = other.Type;
                    other.Type = tmp;
                    tile.Dirty = true;
                    other.Dirty = true;
                    BoardEvents.NotifySwap(tile.BoardPos, other.BoardPos);
                    DestroyTile(other);
                    ret = true;
                }
                else if(other.IsSpecialTile())
                {
                    BoardEvents.NotifySwap(tile.BoardPos, other.BoardPos);
                    CombineSpecialTiles(tile,other);
                    ret = true;
                }
            }
        }
        return ret;
    }

    int CheckForMatches()
    {
        bool match_found = false;
        for (int i = 0;i<_board.Width-1&!match_found;i++)
        {
            for(int j = 0;j<_board.Height-1&!match_found;j++)
            {
                if (TrySwap(_board[i, j], _board[i, j + 1]) || TrySwap(_board[i, j], _board[i+1,j]))
                {
                    match_found = true;
                    break;
                }
            }
            if (match_found) break;
        }
        if(!match_found)
        {
            _board.ShuffleBoard();
        }
        return match_found==true?0:1;
    }
    //This function and the swap one should be the same
    bool TrySwap(BoardPosition tile, BoardPosition other)
    {
        bool ret = false;
        if (other.IsValid())
        {
            TileType tmp = tile.Type;
            tile.Type = other.Type;
            other.Type = tmp;
            if (CanSwap(tile) || CanSwap(other))
            {
                ret = true;
            }
            tmp = tile.Type;
            tile.Type = other.Type;
            other.Type = tmp;
        }
        return ret;
    }

    public bool EndTouch()
    {
        bool ret = false;
        if (_selectedTile != null )
        {
            if (_selectedTile.IsSpecialTile())
            {
                DestroyTile(_selectedTile);
                ret = true;
            }
            else
            {
                BoardEvents.NotifyUnselected(_selectedTile.BoardPos);
            }
            
        }
        return ret;
    }
}
