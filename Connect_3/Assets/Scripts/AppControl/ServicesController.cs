using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;


public class ServicesController : MonoBehaviour
{
    public PlayerProgressionService progression;
    public VolumeOptionsSO volumeOptions;
    [SerializeField] StringEventBus _loadEvent;
    public bool UseSavegame;
    public string DevelopmentID;
    public string ProducionID;
    [SerializeField] bool _devBuild = true;

    private TaskCompletionSource<bool> _cancellationTaskSource;

    public void Awake()
    {

        _cancellationTaskSource = new();      
        LoadServicesCancellable().ContinueWith(task =>
                    Debug.LogException(task.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
    }
    private async Task LoadServicesCancellable()
    {
        await Task.WhenAny(LoadServices(), _cancellationTaskSource.Task);
    }

    private async Task LoadServices()
    {
        string ID = _devBuild ? DevelopmentID : ProducionID;
        Debug.Log(ID);
        await Initialize(ID);

        GameLoginService gameLoginService = new GameLoginService();
        RemoteGameConfigService remoteGameConfigService = new RemoteGameConfigService();
        GameConfigService gameConfigService = new GameConfigService();
        PlayerProgressionService playerProgressionService = progression;
        GameAnalyticsService gameAnalyticsService = new GameAnalyticsService();
        GameAdsService gameAdsService = new GameAdsService("4928653", "Rewarded_Android");

        ServiceLocator.AddService<GameLoginService>(gameLoginService);
        ServiceLocator.AddService<RemoteGameConfigService>(remoteGameConfigService);
        ServiceLocator.AddService<GameConfigService>(gameConfigService);
        ServiceLocator.AddService<PlayerProgressionService>(playerProgressionService);
        ServiceLocator.AddService<GameAnalyticsService>(gameAnalyticsService);
        ServiceLocator.AddService<GameAdsService>(gameAdsService);

        await gameLoginService.Initialize();
        await remoteGameConfigService.Initialize();
        gameConfigService.Initialize();
        playerProgressionService.Initialize();
        await gameAnalyticsService.Initialize();
        await gameAdsService.Initialize();

        _loadEvent.NotifyEvent("MainMenu");
    }
    public async Task Initialize(string environmentID)
    {
        InitializationOptions options = new InitializationOptions();
        if (!string.IsNullOrEmpty(environmentID))
        {
            options.SetEnvironmentName(environmentID);
        }

        await UnityServices.InitializeAsync(options);
    }

    void OnDestroy()
    {
        _cancellationTaskSource.SetResult(true);

        if (UseSavegame)
        {
            progression.SaveGame();
            volumeOptions.SaveOptions();
        }
    }
}
