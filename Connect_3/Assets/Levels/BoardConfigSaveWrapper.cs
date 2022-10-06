using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;

public class BoardConfigSaveWrapper
{
    public float TileSize = 0.5f;
    public GameObjectives Objective;
    public int AvailableMoves = 20;
    public int ScoreOrBoxesNeeded = 20;
    public Array2DInt board = new Array2DInt();
    public GameObject tutorial;
    public string _tutorial;

    public BoardConfigSaveWrapper(BoardConfig config)
    {
        TileSize = config.TileSize;
        Objective = config.Objective;
        AvailableMoves = config.AvailableMoves;
        ScoreOrBoxesNeeded = config.ScoreOrBoxesNeeded;
        board = config.board;
        _tutorial = config._tutorial;
    }

    public BoardConfig getBoardConfig()
    {
        BoardConfig ret = new BoardConfig();
        ret.TileSize =                      TileSize;
        ret.Objective =                    Objective;
        ret.AvailableMoves =           AvailableMoves;
        ret.ScoreOrBoxesNeeded = ScoreOrBoxesNeeded;
        ret.board =                         board;
        ret._tutorial =                     _tutorial;
        return ret;
    }
}

public class LevelListSaveWrapper
{
    public List<BoardConfigSaveWrapper> levels = new List<BoardConfigSaveWrapper>();

    public LevelListSaveWrapper(LevelList list)
    {
        FromList(list.levels);
    }
    public LevelListSaveWrapper(List<BoardConfig> list)
    {
        FromList(list);
    }

    void FromList(List<BoardConfig> list)
    {
        levels = new List<BoardConfigSaveWrapper>();
        foreach (BoardConfig level in list)
        {
            levels.Add(new BoardConfigSaveWrapper(level));
        }
    }
}


