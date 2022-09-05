[System.Serializable]
public class Multipliers
{
    public Multipliers(Multipliers other)
    {
        CritChance = other.CritChance;
        NormalMultiplier = other.NormalMultiplier;
        CriticalMultiplier = other.CriticalMultiplier;
        ManaGainMultiplier = other.ManaGainMultiplier;
    }
    
    public Multipliers()
    {
        CritChance = 0;
        NormalMultiplier = 0;
        CriticalMultiplier = 0;
        ManaGainMultiplier = 0;
    }
    public int CritChance = 10;
    public float NormalMultiplier = 4;
    public float CriticalMultiplier = 8;
    public float ManaGainMultiplier = 5;
}