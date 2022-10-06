using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultLevelProvider : ILevelProvider
{
    public bool loaded { get { return levels != null; }}
    string path = "Levels/DefaultLevels";
    List<BoardConfig> levels;

    public async Task<bool> Initialize()
    {
        levels = Resources.Load<LevelList>(path).levels;
        await Task.Yield();
        return true;
    }
    public List<BoardConfig> GetLevels()
    {
        return levels;
    }
    public void SetLevels()
    {
        return;
    }
    public void SaveLevels()
    {
        return;
    }
}

public class SavedLevelProvider:ILevelProvider
{
    public bool loaded { get { return levels != null; } }
    List<BoardConfig> levels;

    public async Task<bool> Initialize()
    {
        LevelListSaveWrapper levels_loaded = SaveSystem.LoadClass<LevelListSaveWrapper>(SaveSystem.LevelsPath);
        await Task.Yield();
        return true;
    }
    public List<BoardConfig> GetLevels()
    {
        return levels;
    }
    public void SetLevels()
    {
        return;
    }
    public void SaveLevels()
    {
        LevelListSaveWrapper level_list = new LevelListSaveWrapper(levels);
        SaveSystem.SaveClass(level_list, SaveSystem.LevelsPath);
        return;
    }
}
