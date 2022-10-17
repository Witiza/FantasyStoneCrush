using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteLevelProvider:ILevelProvider
{
    public bool loaded { get { return levels.Count>0; } }
    public int priority { get; set; } = 0;
    List<BoardConfig> levels = new List<BoardConfig>();

    public async Task<bool> Initialize()
    {
        RemoteGameConfigService config = ServiceLocator.GetService<RemoteGameConfigService>();
        int level_amount = config.Get("LevelAmount", 0);
        string name;
        BoardConfigSaveWrapper tmp;

        for (int i = 0;i<level_amount;i++)
        {
            name = i != 0 ? "Level" + i.ToString() : "Tutorial";
            tmp = config.Get<BoardConfigSaveWrapper>(name);
            if (tmp != null)
            {
                levels.Add(tmp.getBoardConfig());
            }
            else
            {
                Debug.LogWarning("MISSING LEVEL FROM CONFIG");
            }
        }
        return true;
    }

    void AddLevel(string name)
    { 
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
        return;
    }
}
