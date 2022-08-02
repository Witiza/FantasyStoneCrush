using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardModel
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    BoardPosition[,] _board;
    public BoardModel(int width, int height, List<BoardPosition> board = null)
    {
        Width = width;
        Height = height;
        _board = new BoardPosition[Width, Height];
        if (board == null)
        {
            GenerateNewBoard();
        }
        else
        {
            GenerateExistingBoard(board);
        }
    }

    void GenerateNewBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                _board[i, j] = new BoardPosition();
                _board[i, j].BoardPos = new Vector2Int(i, j);
                _board[i, j].Type = (TileType)Random.Range(1, 6);
                _board[i, j].GameBoard = this;  
                BoardEvents.NotifyCreated(_board[i, j].BoardPos, (int)_board[i, j].Type);
            }
        }
    }

    void GenerateExistingBoard(List<BoardPosition> board)
    {
        foreach(BoardPosition item in board)
        {
            int x = item.BoardPos.x;
            int y = item.BoardPos.y;
            if (_board[x,y] == null)
            {
                _board[x, y] = new BoardPosition(item);
            }
            else
            {
                Debug.LogError("Using an already existing board with repeated members");
            }
        }
    }

     public bool CoordinatesInsideBoard(int x, int y)
    {
        return CoordinateInsideX(x) && CoordinateInsideY(y);
    }

    public  bool CoordinateInsideY(int coord)
    {
        return coord >= 0 && coord < Height;
    }
    public  bool CoordinateInsideX(int coord)
    {
        return coord >= 0 && coord < Width;
    }

    //:)
    public BoardPosition this[int x, int y]
    {
        get => _board[x, y];
    }

    public BoardPosition this[Vector2Int pos]
    {
        get => _board[pos.x, pos.y];
    }
    public BoardPosition GetCell(int x, int y) => this[x, y];


    public BoardPosition GetCell(Vector2Int pos) => _board[pos.x, pos.y];
}
