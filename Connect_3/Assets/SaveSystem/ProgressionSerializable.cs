[System.Serializable]
public class ProgressionSerializable
{
    public ProgressionSerializable() { }
    public ProgressionSerializable(int level, int maxLevel, int coins)
    {
        CurrentLevel = level;
        MaxLevelUnlocked = maxLevel;
        Coins = coins;
    }
   public int CurrentLevel;
   public int MaxLevelUnlocked;
   public int Coins;
}