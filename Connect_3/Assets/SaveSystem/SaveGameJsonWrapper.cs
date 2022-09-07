using System.Collections.Generic;
public class SaveGameJsonWrapper
{
    public SaveGameJsonWrapper()
    { }
    public SaveGameJsonWrapper(PlayerProgressionSO progression)
    {
        CurrentLevel = progression.CurrentLevel;
        MaxLevelUnlocked = progression.MaxLevelUnlocked;
        Coins = progression.Coins;
        Gems = progression.Gems;
        playerInventory = progression.inventory.items;
        warriorInventory = progression.warriorInventory.items;
        rogueInventory = progression.rogueInventory.items;
        archerInventory = progression.archerInventory.items;
        mageInventory = progression.mageInventory.items;
        TileBoosterAmount = progression.TileBooster.amount;
        TurnBoosterAmount = progression.TurnBooster.amount;
        ManaBoosterAmount = progression.ManaBooster.amount;
    }
    public int CurrentLevel;
    public int MaxLevelUnlocked;
    public int Coins;
    public int Gems;
    public List<ItemModel> playerInventory;
    public List<ItemModel> warriorInventory;
    public List<ItemModel> rogueInventory;
    public List<ItemModel> archerInventory;
    public List<ItemModel> mageInventory;
    public int TileBoosterAmount;
    public int TurnBoosterAmount;
    public int ManaBoosterAmount;
}


  