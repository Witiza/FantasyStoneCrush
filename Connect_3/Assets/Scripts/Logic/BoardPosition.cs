using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NULL,
    SHIELD,
    DAGGER,
    ARROW,
    WAND,
    CHALICE,
    BOMB,
    VERTICAL_ROCKET,
    HORIZONTAL_ROCKET,
    BOX,
    GRUNT
}
public class BoardPosition 
{
    public Vector2Int BoardPos;
    public bool Dirty = true;
    public TileType Type;
    public bool ToDestroy;
    public BoardModel GameBoard;
    
    public BoardPosition()
    {
        Dirty = true;
        ToDestroy = false;
    }
    public BoardPosition(BoardPosition other)
    {
        Dirty = other.Dirty;
        Type = other.Type;
        ToDestroy = other.ToDestroy;
        GameBoard = other.GameBoard;
        BoardPos = other.BoardPos;
    }
    public bool IsValid()
    {
        return Type != TileType.NULL;
    }
    public bool IsBaseTile()
    {
        return Type > 0 && (int)Type < 6;
    }
    public bool IsSpecialTile()
    {
        return (int)Type >= 6&&(int)Type<9;
    }
    public bool IsBox()
    {
        return (int)Type == 9;
    }
    public bool IsEnemy()
    {
        return (int)Type == 10;
    }
    public bool CheckType(BoardPosition other)
    {
        return other.Type == Type;
    }
    public bool CheckRight(List<BoardPosition> neighbours)
    {
        if (GameBoard.CoordinateInsideX((int)BoardPos.x+1))
        {
            BoardPosition other = GameBoard[(int)BoardPos.x + 1, (int)BoardPos.y];
            if (other.IsValid() && CheckType(other))
            {
                neighbours.Add(other);
                other.CheckRight(neighbours);
                return true;
            }
        }
        return false; 
    }

        public bool CheckLeft(List<BoardPosition> neighbours)
        {
            if (GameBoard.CoordinateInsideX((int)BoardPos.x-1))
            {
                BoardPosition other = GameBoard[(int)BoardPos.x - 1, (int)BoardPos.y];
                if (other.IsValid() && CheckType(other))
                {
                neighbours.Add(other);
                other.CheckLeft(neighbours);
                    return true;
                }
            }
           return false;
        }

    public bool CheckUp(List<BoardPosition> neighbours)
    {
        if (GameBoard.CoordinateInsideY((int)BoardPos.y+1))
        {
            BoardPosition other = GameBoard[(int)BoardPos.x, (int)BoardPos.y+1];
            if (other.IsValid() && CheckType(other))
            {
                neighbours.Add(other);
                other.CheckUp(neighbours);
                return true;
            }
        }
        return false;
    }

    public bool CheckDown(List<BoardPosition> neighbours)
    {
        if (GameBoard.CoordinateInsideY((int)BoardPos.y-1))
        {
            BoardPosition other = GameBoard[(int)BoardPos.x, (int)BoardPos.y-1];
            if (other.IsValid() && CheckType(other)  )
            {
                neighbours.Add(other);
                other.CheckDown(neighbours);
                return true;
            }
        }
        return false;
    }

    public bool SameTypeNeighbours()
    {
        bool ret = false;
        BoardPosition other;
        if (IsBaseTile())
        {
            if (GameBoard.CoordinateInsideY(BoardPos.y - 1))
            {
                other = GameBoard[BoardPos.x, BoardPos.y - 1];
                if (other.IsValid() && CheckType(other))
                {
                    ret = true;
                }
            }

            if (GameBoard.CoordinateInsideX(BoardPos.x - 1))
            {
                other = GameBoard[BoardPos.x - 1, BoardPos.y];
                if (other.IsValid() && CheckType(other))
                {
                    ret = true;
                }
            }
        }
        
        return ret;
    }
}