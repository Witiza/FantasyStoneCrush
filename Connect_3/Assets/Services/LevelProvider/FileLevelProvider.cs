using System.Collections.Generic;
using System.Threading.Tasks;

public class FileLevelProvider:ILevelProvider
{
    public bool loaded { get { return levels != null; } }
    public int priority { get; set; } = 0;
    List<BoardConfig> levels;

    public async Task<bool> Initialize()
    {
        LevelListSaveWrapper levels_loaded = SaveSystem.LoadClass<LevelListSaveWrapper>(SaveSystem.LevelsPath);
        if(levels_loaded !=null && levels_loaded.levels!=null&&levels_loaded.levels.Count>0)
        {
            levels = levels_loaded.getAsList();
        }
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
    public void SaveLevels(List<BoardConfig> to_save)
    {
        LevelListSaveWrapper level_list = new LevelListSaveWrapper(to_save);
        SaveSystem.SaveClass(level_list, SaveSystem.LevelsPath);
        return;
    }
}
