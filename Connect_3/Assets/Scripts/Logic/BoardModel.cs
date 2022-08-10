using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Array2DEditor;
public class BoardModel
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    BoardPosition[,] _board;
    public BoardModel(int width, int height, Array2DInt board= null)
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
                _board[i, j].GameBoard = this;
                GenerateTileType(_board[i, j]);
                BoardEvents.NotifyCreated(_board[i, j].BoardPos, (int)_board[i, j].Type);
            }
        }
    }

    void GenerateExistingBoard(Array2DInt board)
    {
        for(int i = 0;i<Width;i++)
        {
            for(int j=0;j<Height;j++)
            {
                _board[i, j] = new BoardPosition();
                _board[i, j].BoardPos = new Vector2Int(i, j);
                _board[i, j].GameBoard = this;
                if (board[i,j] ==-1)
                {
                    GenerateTileType(_board[i, j]);
                }
                else
                {
                    _board[i, j].Type = (TileType)board[i, j];
                }
                BoardEvents.NotifyCreated(_board[i, j].BoardPos, (int)_board[i, j].Type);
            }
        }
    }

    void GenerateTileType(BoardPosition tile)
    {
        do
        {
            tile.Type = (TileType)Random.Range(1, 6);
        } while (tile.SameTypeNeighbours());
    }

    //Very ugly function, find a better way to do this (maybe cheat and generate a board again?
    public void ShuffleBoard()
    {
        Debug.Log("Shuffling Board");
        for(int i = 0;i<Width;i++)
        {
            for(int j =0;j<Height;j++)
            {
                if (_board[i,j].IsBaseTile())
                {
                    GenerateTileType(_board[i, j]);
                    BoardEvents.NotifyChanged(new Vector2(i, j), (int)_board[i, j].Type);
                }
            }
        }
        
        #region Complex Shuffle Fuckery
        //List<ModelTile> tiles = new List<ModelTile>();
        //for(int i = 0;i<Width;i++)
        //{
        //    for(int j=0;j<Height;j++)
        //    {
        //        tiles.Add(new ModelTile(new Vector2Int(i,j),_board[i, j].Type));
        //        _board[i, j].Type = TileType.NULL;
        //    }
        //}
        //for(int i = 0;i<Width;i++)
        //{
        //    for(int j = 0;j<Height;j++)
        //    {
        //        foreach(ModelTile tile in tiles)
        //        {
        //            _board[i, j].Type = tile.TileType;
        //            if (!_board[i,j].SameTypeNeighbours())
        //            {
        //                BoardEvents.NotifyMoved(tile.BoardPosition, new Vector2Int(i, j));
        //                tiles.Remove(tile);
        //                break;
        //            }
        //        }
        //    }
        //}
        ////Leftover tiles, wont check for matches;
        //if(tiles.Count >0)
        //{
        //    ModelTile tmp;
        //    for (int i = 0; i < Width; i++)
        //    {
        //        for (int j = 0; j < Height; j++)
        //        {
        //            if (_board[i,j].Type == TileType.NULL)
        //            {
        //                tmp = tiles.Last();
        //                _board[i, j].Type = tmp.TileType;
        //                BoardEvents.NotifyMoved(tmp .BoardPosition, new Vector2Int(i, j));
        //                tiles.Remove(tiles.Last());
        //            }
        //        }
        //    }
        //}
        #endregion
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
