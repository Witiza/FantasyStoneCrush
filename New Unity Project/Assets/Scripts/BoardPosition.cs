using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BoardPosition
{
    public Vector3 Position;
    public Vector2 board_position;
    public Color Color;
    public Tile target_tile;
    public BoardPosition[,] reference;

    public bool CheckType(BoardPosition other)
    {
        return other.target_tile.tile_type == target_tile.tile_type;
    }
    public int CheckRight(int count,List<BoardPosition> neighbours)
    {
        if (board_position.x < 8)
        {
            BoardPosition other = reference[(int)board_position.x + 1, (int)board_position.y];
            if (other.target_tile != null && CheckType(other))
            {
                neighbours.Add(other);
                count+=other.CheckRight(count,neighbours);
                return ++count;
            }
        }
        return count;
    }

        public int CheckLeft(int count, List<BoardPosition> neighbours)
        {
            if (board_position.x >0)
            {
                BoardPosition other = reference[(int)board_position.x - 1, (int)board_position.y];
                if (other.target_tile != null && CheckType(other))
                {
                neighbours.Add(other);
                count += other.CheckLeft(count,neighbours);
                    return ++count;
                }
            }
           return count;
        }

    public int CheckUp(int count, List<BoardPosition> neighbours)
    {
        if (board_position.y < 8)
        {
            BoardPosition other = reference[(int)board_position.x, (int)board_position.y+1];
            if (other.target_tile != null && CheckType(other)  )
            {
                neighbours.Add(other);
                count += other.CheckUp(count,neighbours);
                return ++count;
            }
        }
        return count;
    }

    public int CheckDown(int count, List<BoardPosition> neighbours)
    {
        if (board_position.y > 0)
        {
            BoardPosition other = reference[(int)board_position.x, (int)board_position.y-1];
            if (other.target_tile != null && CheckType(other)  )
            {
                neighbours.Add(other);
                count += other.CheckLeft(count,neighbours);
                return ++count;
            }
        }
        return count;
    }
}