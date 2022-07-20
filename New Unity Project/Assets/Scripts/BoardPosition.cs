using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BoardPosition
{
    public Vector2 board_position;
    public Tile target_tile;
    public BoardPosition[,] reference;
    public bool dirty = true;

    public bool CheckType(BoardPosition other)
    {
        return other.target_tile.tile_type == target_tile.tile_type;
    }
    public bool CheckRight(List<BoardPosition> neighbours)
    {
        if (board_position.x < 8)
        {
            BoardPosition other = reference[(int)board_position.x + 1, (int)board_position.y];
            if (other.target_tile != null && CheckType(other))
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
            if (board_position.x >0)
            {
                BoardPosition other = reference[(int)board_position.x - 1, (int)board_position.y];
                if (other.target_tile != null && CheckType(other))
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
        if (board_position.y < 8)
        {
            BoardPosition other = reference[(int)board_position.x, (int)board_position.y+1];
            if (other.target_tile != null && CheckType(other))
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
        if (board_position.y > 0)
        {
            BoardPosition other = reference[(int)board_position.x, (int)board_position.y-1];
            if (other.target_tile != null && CheckType(other)  )
            {
                neighbours.Add(other);
                other.CheckDown(neighbours);
                return true;
            }
        }
        return false;
    }
}