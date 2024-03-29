using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModel
{
    public string name;
    public string icon = "amulet";
    public Color iconColor = Color.white;

    public Multipliers ItemMultipliers;

    public void ApplyMultipliers(Multipliers target)
    {
        target.CritChance += ItemMultipliers.CritChance;
        target.CriticalMultiplier += ItemMultipliers.CriticalMultiplier;
        target.NormalMultiplier += ItemMultipliers.NormalMultiplier;
        target.ManaGainMultiplier += ItemMultipliers.ManaGainMultiplier;
    }
}
