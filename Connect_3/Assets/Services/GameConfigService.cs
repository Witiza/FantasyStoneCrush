using System.Threading.Tasks;
using Unity.Services.RemoteConfig;


public class GameConfigService : IService
{
    public int initialCoins { get; private set; }
    public int initialGems { get; private set; }
    public int initialTurnBooster { get; private set; }
    public int initialManaBooster { get; private set; }
    public int initialTileBooster { get; private set; }

    public float coinsWonMultiplier { get; private set; }
    public float coinsWonMultiplierLowLevel { get;private set; }

    public void Clear()
    { }

    public void Initialize()
    {
        IsInitialized = true;
        RemoteGameConfigService config = ServiceLocator.GetService<RemoteGameConfigService>();
        initialGems = config.Get("InitialGems", 10);
        initialCoins = config.Get("InitialCoins", 200);
        initialTurnBooster = config.Get("InitialTurnBooster", 3);
        initialManaBooster = config.Get("InitialManaBooster", 3);
        initialTileBooster = config.Get("InitialTileBooster", 3);
        coinsWonMultiplier = config.Get("CoinsWonMultiplier", 5);
        coinsWonMultiplierLowLevel = config.Get("CoinsWonMultiplierLowLevel", 5);
    }
}
