using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;


public class GameSaveService:IService
{
    public bool Initialized { get; set; }
    List<IProgressionProvider> progressionProviders = new List<IProgressionProvider>();
    PlayerProgressionService playerProgressionService;
    public async Task<bool> Initialize()
    {
        playerProgressionService = ServiceLocator.GetService<PlayerProgressionService>();
        progressionProviders.Add(new FileProgressionProvider());
        progressionProviders.Add(new RemoteUnityProgressionProvider());
        for(int i = 0;i<progressionProviders.Count;i++)
        {
            await progressionProviders[i].Initialize();
        }
        Initialized = true;
        LoadGame();
        return true;
    }
    public void Clear()
    {

    }

    public void SaveGame()
    {
        for(int i = 0;i<progressionProviders.Count;i++)
        {
            progressionProviders[i].Save();
        }
    }
    public void LoadGame()
    {
        List<SaveGameJsonWrapper> saves = new List<SaveGameJsonWrapper>();
        SaveGameJsonWrapper tmp;
        for (int i = 0;i<progressionProviders.Count;i++)
        {
            tmp = progressionProviders[i].Load();
            if(tmp!= null)
                saves.Add(tmp);
        }
         tmp = saves[0];
        for(int i = 1;i<progressionProviders.Count;i++)
        {
            tmp = SaveGameJsonWrapper.GetHighest(tmp, saves[i]);
        }
        Debug.Log("LOADING");
        playerProgressionService.LoadGame(tmp);
    }
}
