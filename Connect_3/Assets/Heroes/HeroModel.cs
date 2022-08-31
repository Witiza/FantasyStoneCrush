using UnityEngine;

[CreateAssetMenu]
public class HeroModel : ScriptableObject
{

    public HeroInventory Inventory;
    public int BaseNormalStrength;
    public int BaseCriticalStrength;
    public int BaseManaGain;
    public int MaxMana;
    public Multipliers BaseMultipliers;

    Multipliers _finalMultipliers;


    public void GenerateFinalStats()
    {
        _finalMultipliers = new Multipliers(BaseMultipliers);
        Inventory.ApplyItems(_finalMultipliers);
    }

    public int NormalStrength { get => Mathf.RoundToInt(BaseNormalStrength*_finalMultipliers.NormalMultiplier); }
    public int CriticalStrength { get => Mathf.RoundToInt(BaseCriticalStrength*_finalMultipliers.CriticalMultiplier); }
    public int ManaGain { get => Mathf.RoundToInt(BaseManaGain * _finalMultipliers.ManaGainMultiplier); }
    public float CritChance { get=>_finalMultipliers.CritChance; } 
}

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