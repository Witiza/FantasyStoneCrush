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
    HORIZONTAL_ROCKET
}
public  class BoardPosition 
{
    public Vector2 BoardPos;
    public bool Dirty  { get; set; }
    public TileType _type;
    public bool ToDestroy  { get; set; }

    public BoardPosition()
    {
        Dirty = true;
        ToDestroy = false;
    }
    public bool IsValid()
    {
        return _type != TileType.NULL;
    }
    public bool IsBaseTile()
    {
        return _type > 0 && (int)_type < 6;
    }
    public bool IsSpecialTile()
    {
        return (int)_type >= 6;
    }
    public bool CheckType(BoardPosition other)
    {
        return other._type == _type;
    }
    public bool CheckRight(List<BoardPosition> neighbours)
    {
        if (Board.CoordinateInsideX((int)BoardPos.x+1))
        {
            BoardPosition other = Board.GameBoard[(int)BoardPos.x + 1, (int)BoardPos.y];
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
            if (Board.CoordinateInsideX((int)BoardPos.x-1))
            {
                BoardPosition other = Board.GameBoard[(int)BoardPos.x - 1, (int)BoardPos.y];
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
        if (Board.CoordinateInsideY((int)BoardPos.y+1))
        {
            BoardPosition other = Board.GameBoard[(int)BoardPos.x, (int)BoardPos.y+1];
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
        if (Board.CoordinateInsideY((int)BoardPos.y-1))
        {
            BoardPosition other = Board.GameBoard[(int)BoardPos.x, (int)BoardPos.y-1];
            if (other.IsValid() && CheckType(other)  )
            {
                neighbours.Add(other);
                other.CheckDown(neighbours);
                return true;
            }
        }
        return false;
    }
}