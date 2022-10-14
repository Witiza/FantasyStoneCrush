using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum ResourceType
{
    COINS,
    GEMS
}
public class ResourceItem
{
    public ResourceType type;
    public int amount;
}
[CreateAssetMenu]
public class PlayerProgressionService  : ScriptableObject , IService
{
    public bool Initialized{ get{  return _initialized; } }
    private bool _initialized = false;
    public int CurrentLevel = 0;
    public int MaxLevelUnlocked = 0;
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public List<ResourceItem> resources;
    public PlayerInventory inventory;
    public HeroInventory warriorInventory;
    public HeroInventory rogueInventory;
    public HeroInventory archerInventory;
    public HeroInventory mageInventory;
    public Booster TileBooster;
    public Booster TurnBooster;
    public Booster ManaBooster;
    public IntEventBus CoinsChange;
    public IntEventBus GemsChange;

    public void UpdateResource(ResourceType type, int amount)
    {
        var resource = resources.Find(r => r.type == type);
        if(resource == null) { resource = new ResourceItem { type=type, amount=0 }; resources.Add(resource); }
        resource.amount += amount;
    }
    public BoardConfig GetCurrentLevelBoard()
    {
        return ServiceLocator.GetService<GameLevelsService>().levels[CurrentLevel];
    }
    public void ResetProgression()
    {
        GameConfigService config = ServiceLocator.GetService<GameConfigService>();

        CurrentLevel = 0;
        MaxLevelUnlocked = 0;
        Coins = config.initialCoins;
        Gems = config.initialGems;
        TileBooster.amount = config.initialTileBooster;
        TurnBooster.amount = config.initialTurnBooster;
        ManaBooster.amount = config.initialManaBooster;
        inventory.items.Clear();
        warriorInventory.items.Clear();
        rogueInventory.items.Clear();
        archerInventory.items.Clear();
        mageInventory.items.Clear();
        ServiceLocator.GetService<GameSaveService>().SaveGame();
    }

    public void ModifyCoins(int coins)
    {
        Coins += coins;
        CoinsChange.NotifyEvent(coins);
    }

    public void ModifyGems(int gems)
    {
        Gems += gems;
        GemsChange.NotifyEvent(gems);
    }

    public void SetCoins(int amount)
    {
        Coins = amount;
    }

    public void SetGems(int amount)
    {
        Gems = amount;
    }

    public void ModifyTileBooster(int amount)
    {
        TileBooster.amount += amount;
    }

    public void ModifyManaBooster(int amount)
    {
        ManaBooster.amount += amount;
    }

    public void ModifyTurnBooster(int amount)
    {
        TurnBooster.amount += amount;
    }

    public void LoadGame(SaveGameJsonWrapper save)
    {
    CurrentLevel = save.CurrentLevel;
    MaxLevelUnlocked = save.MaxLevelUnlocked;
    Coins = save.Coins;
    Gems = save.Gems;
    inventory.items = save.playerInventory;
    warriorInventory.items = save.warriorInventory;
    rogueInventory.items = save.rogueInventory;
    archerInventory.items = save.archerInventory;
    mageInventory.items = save.mageInventory;
    TileBooster.amount = save.TileBoosterAmount;
    TurnBooster.amount = save.TurnBoosterAmount;
    ManaBooster.amount = save.ManaBoosterAmount;
    }

    public void Clear()
    {

    }
    public void Initialize()
    {
        _initialized = true;
    }
}
