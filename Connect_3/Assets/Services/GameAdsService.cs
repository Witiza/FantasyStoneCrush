﻿using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameAdsService : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener, IService
{
    public bool Initialized { get { return _initializationStatus == TaskStatus.RanToCompletion; }}

    private string _adsGameId;
    private string _adUnitId;

    private bool _isAdLoaded = false;

    TaskStatus _initializationStatus = TaskStatus.Created;
    TaskStatus _showAdStatus = TaskStatus.Created;
    public bool IsAdReady => _isAdLoaded;

    public GameAdsService(string adsGameId, string adUnitId)
    {
        _adsGameId = adsGameId;
        _adUnitId = adUnitId;
    }

    public async Task Initialize(bool testMode = false)
    {
        _initializationStatus = TaskStatus.Running;
        Advertisement.Initialize(_adsGameId, testMode, this);
        while(_initializationStatus == TaskStatus.Running)
        {
            await Task.Delay(100);
        }
    }

    private async void Retry()
    {
        await Task.Delay(10000);
        await Initialize();
    }
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        _initializationStatus = TaskStatus.RanToCompletion;
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        _initializationStatus = TaskStatus.Faulted;
        if (error == UnityAdsInitializationError.UNKNOWN)
        {
            Retry();
        }
    }

    public void LoadAd()
    {
        _isAdLoaded = false;
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        _isAdLoaded = true;
        Debug.Log("Ad Loaded: " + adUnitId);
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
    }

    public async Task<bool> ShowAd()
    {
        if(_showAdStatus == TaskStatus.Running)
        {
            Debug.LogError("Can't show Ad because there is one already showing");
            return false;
        }
        else if (_initializationStatus != TaskStatus.RanToCompletion)
        {
            Debug.LogError("Can't show Ad because the Ads service hasnt been initialized");
            return false;
        }
        else if(!IsAdReady)
        {
            Debug.LogError("Can't show Ad because there is not an Ad ready");
        }
        _showAdStatus = TaskStatus.Running;
        Advertisement.Show(_adUnitId, this);

#if UNITY_EDITOR
        await Task.Delay(2000);
        OnUnityAdsShowComplete(_adUnitId, UnityAdsShowCompletionState.COMPLETED);
#endif
        while (_showAdStatus == TaskStatus.Running)
        {
            await Task.Delay(100);
        }

        return _showAdStatus == TaskStatus.RanToCompletion;
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Unity Ads Rewarded Ad:" + showCompletionState.ToString());
        Advertisement.Load(_adUnitId, this);
        _showAdStatus = showCompletionState == UnityAdsShowCompletionState.COMPLETED ? TaskStatus.RanToCompletion : TaskStatus.Faulted;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Advertisement.Load(_adUnitId, this);
        _showAdStatus = TaskStatus.Faulted;
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("Started watching an ad");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("User clicked in the ad");
    }
}