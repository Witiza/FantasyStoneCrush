using UnityEngine;

[CreateAssetMenu]
public class HeroModel : ScriptableObject
{
    public int id;
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

    public bool HasItem(ItemModel item)
    {
        return Inventory.HasItem(item);
    }

    public int NormalStrength { get => Mathf.RoundToInt(BaseNormalStrength*_finalMultipliers.NormalMultiplier); }
    public int CriticalStrength { get => Mathf.RoundToInt(BaseCriticalStrength*_finalMultipliers.CriticalMultiplier); }
    public int ManaGain { get => Mathf.RoundToInt(BaseManaGain * _finalMultipliers.ManaGainMultiplier); }
    public float CritChance { get=>_finalMultipliers.CritChance; } 
}
