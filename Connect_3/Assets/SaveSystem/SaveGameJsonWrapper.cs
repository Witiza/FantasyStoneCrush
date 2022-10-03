using System.Collections.Generic;
public class SaveGameJsonWrapper
{
    public SaveGameJsonWrapper()
    {
        GameConfigService config = ServiceLocator.GetService<GameConfigService>();
        Coins = config.initialCoins;
        Gems = config.initialGems;
        TileBoosterAmount = config.initialTileBooster;
        TileBoosterAmount = config.initialTurnBooster;
        ManaBoosterAmount = config.initialManaBooster;
        playerInventory = new List<ItemModel>();
        warriorInventory = new List<ItemModel>();
        rogueInventory = new List<ItemModel>();
        archerInventory = new List<ItemModel>();
        mageInventory = new List<ItemModel>();
}
    public SaveGameJsonWrapper(PlayerProgressionService progression)
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
    public int CurrentLevel = 0;
    public int MaxLevelUnlocked=0;
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

    public static SaveGameJsonWrapper GetHighest(SaveGameJsonWrapper a,SaveGameJsonWrapper b)
    {
        SaveGameJsonWrapper ret = a;
        if(a.CurrentLevel < b.CurrentLevel)
        {
            ret = b;
        }
        else if(a.CurrentLevel == b.CurrentLevel)
        {
            int intA = a.GetItemAmount();
            int intB = b.GetItemAmount();
            if(intA < intB)
            {
                ret = b;
            }
            else if(intA == intB)
            {
               intA = a.GetBoosterAmount();
               intB = b.GetBoosterAmount();
                if(intA<intB)
                {
                    ret = b;
                }
                else if(intA==intB)
                {
                    intA = a.GetResourceAmount();
                    intB = b.GetResourceAmount();
                    if(intA<intB)
                    {
                        ret = b;
                    }
                }
            }
        }
        return ret;
    }

    public int GetItemAmount()
    {
        return playerInventory.Count+warriorInventory.Count+archerInventory.Count+rogueInventory.Count+mageInventory.Count;
    }

    public int GetBoosterAmount()
    {
        return TileBoosterAmount + TurnBoosterAmount + ManaBoosterAmount;
    }

    public int GetResourceAmount()
    {
        return Coins + Gems * 10;
    }
}


  