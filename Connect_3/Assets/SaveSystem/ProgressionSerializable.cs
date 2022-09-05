[System.Serializable]
public class ProgressionSerializable
{
    public ProgressionSerializable() { }
    public ProgressionSerializable(int level, int maxLevel, int coins, int gems)
    {
        CurrentLevel = level;
        MaxLevelUnlocked = maxLevel;
        Coins = coins;
        Gems = gems;
    }
   public int CurrentLevel;
   public int MaxLevelUnlocked;
   public int Coins;
   public int Gems;
}