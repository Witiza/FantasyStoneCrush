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
    public int CritChance = 10;
    public float NormalMultiplier = 4;
    public float CriticalMultiplier = 8;
    public float ManaGainMultiplier = 5;
}