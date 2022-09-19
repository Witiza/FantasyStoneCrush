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
    public int gemsAddedByAd { get; private set; }
    public int movesAddedByAd { get; private set; }
    public int movesAddedByBooster { get; private set; }
    public int tilesAddedByBooster { get; private set; }


    public int costTurnBooster { get; private set; }
    public int costManaBooster { get; private set; }
    public int costTileBooster { get; private set; }

    public int costNormalChest { get; private set; }
    public int costBigChest { get; private set; }
    public int costGemsChest { get; private set; }
    public int bigChestItemAmount { get; private set; }


    public void Clear()
    { }

    public void Initialize()
    {
        RemoteGameConfigService config = ServiceLocator.GetService<RemoteGameConfigService>();
        initialGems = config.Get("InitialGems", 10);
        initialCoins = config.Get("InitialCoins", 200);
        initialTurnBooster = config.Get("InitialTurnBooster", 3);
        initialManaBooster = config.Get("InitialManaBooster", 3);
        initialTileBooster = config.Get("InitialTileBooster", 3);
        coinsWonMultiplier = config.Get("CoinsWonMultiplier", 5);
        coinsWonMultiplierLowLevel = config.Get("CoinsWonMultiplierLowLevel", 5);
        gemsAddedByAd = config.Get("GemsAddedByAd", 5);
        movesAddedByAd = config.Get("MovesAddedByAd", 5);
        movesAddedByBooster = config.Get("MovesAddedByBooster", 5);
        tilesAddedByBooster = config.Get("TilesAddedByBooster", 5);
        costTurnBooster = config.Get("TurnBoosterCost", 3);
        costManaBooster = config.Get("ManaBoosterCost", 3);
        costTileBooster = config.Get("TileBoosterCost", 3);
        costNormalChest = config.Get("NormalChestCost", 200);
        costBigChest = config.Get("BigChestCost", 400);
        costGemsChest = config.Get("GemsChestCost", 500);
        bigChestItemAmount = config.Get("BigChestItemAmount", 3);
    }

    public int GetShopCost(int id)
    {
        int ret = 0;
        switch (id)
        {
            case 1:
                ret = costTurnBooster;
                break;
            case 2:
                ret = costManaBooster;
                break;
            case 3:
                ret = costTileBooster;
                break;
            case 4:
                ret = costNormalChest;
                break;
            case 5:
                ret = costBigChest;
                break;
            case 6:
                ret = costGemsChest;
                break;
            default:
                ret = 0;
                break;
        }
        return ret;
    }
}
