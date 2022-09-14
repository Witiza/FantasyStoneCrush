using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Threading.Tasks;


[CreateAssetMenu]
public class PlayerProgressionService  : ScriptableObject , IService
{
    public int CurrentLevel = 0;
    public int MaxLevelUnlocked = 0;
    public int Coins;
    public int Gems;
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
        CurrentLevel = 0;
        MaxLevelUnlocked = 0;
        Coins = 0;
        Gems = 0;
        TileBooster.amount = 0;
        TurnBooster.amount = 0;
        ManaBooster.amount = 0;
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
    public  async Task Initialize()
    {
        LoadGame();
    }
}