using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
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

    public List<BoardConfig> getAsList()
    {
        List<BoardConfig> ret = new List<BoardConfig>();
        foreach(BoardConfigSaveWrapper level in levels)
        {
            ret.Add(level.getBoardConfig());
        }
        return ret;
    }
}


