using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeroInventory : ScriptableObject
{
    List<InventoryItem> items;

    public void ApplyItems(Multipliers target)
    {
        foreach (InventoryItem item in items)
        {
            item.ApplyMultipliers(target);
        }
    }
}