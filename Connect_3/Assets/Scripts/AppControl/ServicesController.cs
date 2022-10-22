using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using TMPro;
using UnityEngine.AddressableAssets;


public class ServicesController : MonoBehaviour
{
    public PlayerProgressionService progression;
    public VolumeOptionsSO volumeOptions;
    [SerializeField] StringEventBus _loadEvent;
    public bool UseSavegame;
    public string DevelopmentID;
    public string ProducionID;
    [SerializeField] bool _devBuild = true;
    [SerializeField] SlicedFilledImage _loadingBar;
    [SerializeField] TMP_Text _loadingText;
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
        DisplayLoadingStatus(0.1f, "Initializing");
        await Initialize(ID);

        GameLoginService gameLoginService = new GameLoginService();
        RemoteGameConfigService remoteGameConfigService = new RemoteGameConfigService();
        GameConfigService gameConfigService = new GameConfigService();
        PlayerProgressionService playerProgressionService = progression;
        GameAnalyticsService gameAnalyticsService = new GameAnalyticsService();
        GameAdsService gameAdsService = new GameAdsService("4928653", "Rewarded_Android");
        GameIAPService gameIAPService = new GameIAPService();
        GameSaveService gameSaveService = new GameSaveService();
        GameLevelsService gameLevelService = new GameLevelsService();

        ServiceLocator.AddService<GameLoginService>(gameLoginService);
        ServiceLocator.AddService<RemoteGameConfigService>(remoteGameConfigService);
        ServiceLocator.AddService<GameConfigService>(gameConfigService);
        ServiceLocator.AddService<PlayerProgressionService>(playerProgressionService);
        ServiceLocator.AddService<GameAnalyticsService>(gameAnalyticsService);
        ServiceLocator.AddService<GameAdsService>(gameAdsService);
        ServiceLocator.AddService<GameIAPService>(gameIAPService);
        ServiceLocator.AddService<GameSaveService>(gameSaveService);
        ServiceLocator.AddService<GameLevelsService>(gameLevelService);

        DisplayLoadingStatus(0.2f, "Logging In");
        await Task.WhenAny(gameLoginService.Initialize(),Task.Delay(5000));
        bool logged = gameLoginService.Initialized;
        DisplayLoadingStatus(0.3f, "Fetching Remote Config");
        await remoteGameConfigService.Initialize();
        gameConfigService.Initialize();
        playerProgressionService.Initialize();
        DisplayLoadingStatus(0.4f, "Fetching Cloud Save");
        await gameSaveService.Initialize();
        if (logged)
        {
            DisplayLoadingStatus(0.5f, "Initializing Analytics");
            await gameAnalyticsService.Initialize();
            DisplayLoadingStatus(0.6f, "Initializing Ads");
            await gameAdsService.Initialize();
            DisplayLoadingStatus(0.7f, "Initializing In App Purchases");
            await gameIAPService.Initialize(new Dictionary<string, string>
            {
                ["Test1"] = "com.witizagames.fantasystonecrush.test1"
            });
        }
        DisplayLoadingStatus(0.8f, "Fetching New Levels");
        await gameLevelService.Initialize();
        DisplayLoadingStatus(1f, "");

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

    void DisplayLoadingStatus(float amount, string text)
    {
        _loadingBar.fillAmount = amount;
        _loadingText.text = text;
    }
    void OnDestroy()
    {
        _cancellationTaskSource.SetResult(true);
        volumeOptions.SaveOptions();
        ServiceLocator.GetService<GameSaveService>().SaveGame();
        ServiceLocator.GetService<GameLevelsService>().SaveLevels();
    }
}
