﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public class RemoteUnityProgressionProvider:IProgressionProvider
{
    PlayerProgressionService playerProgressionService;
    string _dataJson= "not found";
    public async Task<bool> Initialize()
    {
        playerProgressionService = ServiceLocator.GetService<PlayerProgressionService>();
        Application.focusChanged += OnApplicationFocusChanged;
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync();
        foreach (var keyValuePair in savedData)
        {
            Debug.Log("Key: " + keyValuePair.Key + " Value: " + keyValuePair.Value);
        }

        savedData.TryGetValue("data", out _dataJson);
        Debug.Log("Loaded  " + _dataJson + " for user " + AuthenticationService.Instance.PlayerId);
        return true;
    }
    private  async void OnApplicationFocusChanged(bool hasFocus)
    {
        if (!hasFocus)
        {
            try
            {
                await CloudSaveService.Instance.Data.ForceSaveAsync(new Dictionary<string, object> { { "data", _dataJson } });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Debug.Log("Saved  " + _dataJson + " for user " + AuthenticationService.Instance.PlayerId);
        }
    }
    public SaveGameJsonWrapper Load()
    {
        return JsonUtility.FromJson<SaveGameJsonWrapper>(_dataJson);
    }
    public void Save()
    {
        _dataJson = JsonUtility.ToJson(new SaveGameJsonWrapper(playerProgressionService));
#if UNITY_EDITOR
        OnApplicationFocusChanged(false);
#endif
    }
}
