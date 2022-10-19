using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.Networking;

public class RemoteLevelProvider:ILevelProvider
{
    public bool loaded { get { return levels.Count>0; } }
    public int priority { get; set; } = 0;
    List<BoardConfig> levels = new List<BoardConfig>();
    string defaultPath = "Levels/GameLevels";

    public async Task<bool> Initialize()
    {
        RemoteGameConfigService config = ServiceLocator.GetService<RemoteGameConfigService>();
        levels = (await Addressables.LoadAssetAsync<LevelList>(config.Get("LevelsPath",defaultPath)).Task).levels;

        #region RemoteConfigAttempt
        //This works but the devs should add each level manually, which sucks.

        //int level_amount = config.Get("LevelAmount", 0);
        //string name;
        //BoardConfigSaveWrapper tmp;

        //for (int i = 0;i<level_amount;i++)
        //{
        //    name = i != 0 ? "Level" + i.ToString() : "Tutorial";
        //    tmp = config.Get<BoardConfigSaveWrapper>(name);
        //    if (tmp != null)
        //    {
        //        levels.Add(tmp.getBoardConfig());
        //    }
        //    else
        //    {
        //        Debug.LogWarning("MISSING LEVEL FROM CONFIG");
        //    }
        //}
        #endregion
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
        return;
    }
}
