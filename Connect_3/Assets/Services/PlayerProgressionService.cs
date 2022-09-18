using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CreateAssetMenu]
public class PlayerProgressionService  : ScriptableObject , IService
{
    public int CurrentLevel = 0;
    public int MaxLevelUnlocked = 0;
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public PlayerInventory inventory;
    public HeroInventory warriorInventory;
    public HeroInventory rogueInventory;
    public HeroInventory archerInventory;
    public HeroInventory mageInventory;
    public List<BoardConfig> levels;
    public Booster TileBooster;
    public Booster TurnBooster;
    public Booster ManaBooster;
    public IntEventBus CoinsChange;
    public IntEventBus GemsChange;


    public BoardConfig GetCurrentLevelBoard()
    {
        return levels[CurrentLevel];
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

    public void SaveGame()
    {
        SaveSystem.SaveGame(new SaveGameJsonWrapper(this));
    }
    public void LoadGame()
    {
        GameConfigService config = ServiceLocator.GetService<GameConfigService>();
        bool success;
        SaveGameJsonWrapper tmp = SaveSystem.LoadGame(out success);
        if (success)
        {
            CurrentLevel = tmp.CurrentLevel;
            MaxLevelUnlocked = tmp.MaxLevelUnlocked;
            Coins = tmp.Coins;
            Gems = tmp.Gems;
            inventory.items = tmp.playerInventory;
            warriorInventory.items = tmp.warriorInventory;
            rogueInventory.items = tmp.rogueInventory;
            archerInventory.items = tmp.archerInventory;
            mageInventory.items = tmp.mageInventory;
            TileBooster.amount = tmp.TileBoosterAmount;
            TurnBooster.amount = tmp.TurnBoosterAmount;
            ManaBooster.amount = tmp.ManaBoosterAmount;
        }
        else
        {
            Coins = config.initialCoins;
            Gems = config.initialGems;
            TileBooster.amount = config.initialTileBooster;
            TurnBooster.amount = config.initialTurnBooster;
            ManaBooster.amount = config.initialManaBooster;
        }
    }

    public void Clear()
    {

    }
    public void Initialize()
    {
        LoadGame();
    }
}
