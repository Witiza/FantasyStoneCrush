using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultLevelProvider : ILevelProvider
{
    public bool loaded { get { return levels != null; }}
    public int priority { get; set; } = 0;
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
    public void SaveLevels(List<BoardConfig> levels)
    {
        return;
    }
}
