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

    async void  Awake()
    {
        _cancellationTaskSource = new TaskCompletionSource<bool>();
        
        await LoadServicesCancellable().ContinueWith(task =>
                    Debug.LogException(task.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
        //Where do I put this
        volumeOptions.LoadOptions();
        _loadEvent.NotifyEvent("MainMenu");
    }
    private async Task LoadServicesCancellable()
    {
        await Task.WhenAny(LoadServices(), _cancellationTaskSource.Task);
    }

    private async Task LoadServices()
    {
        await Initialize(_devBuild ? DevelopmentID : ProducionID).ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);

        GameLoginService gameLoginService = new GameLoginService();
        RemoteGameConfigService remoteGameConfigService = new RemoteGameConfigService();
        GameConfigService gameConfigService = new GameConfigService();
        PlayerProgressionService playerProgressionService = progression;

        ServiceLocator.AddService<GameLoginService>(gameLoginService);
        ServiceLocator.AddService<RemoteGameConfigService>(remoteGameConfigService);
        ServiceLocator.AddService<GameConfigService>(gameConfigService);
        ServiceLocator.AddService<PlayerProgressionService>(playerProgressionService);

        await gameLoginService.Initialize().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        await remoteGameConfigService.Initialize().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        await gameConfigService.Initialize().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        if (UseSavegame)
        {
            await playerProgressionService.Initialize().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }
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
