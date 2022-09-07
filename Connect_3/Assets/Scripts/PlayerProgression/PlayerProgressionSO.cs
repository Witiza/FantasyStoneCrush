using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu]
public class PlayerProgressionSO  : ScriptableObject
{
    public int CurrentLevel;
    public int MaxLevelUnlocked;
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


    public void AddCoins(int coins)
    {
        Coins += coins;
        CoinsChange.NotifyEvent(coins);
    }

    public void AddGems(int gems)
    {
        Gems += gems;
        GemsChange.NotifyEvent(gems);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(new SaveGameJsonWrapper(this));
    }
    public void LoadGame()
    {
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
    }
}
