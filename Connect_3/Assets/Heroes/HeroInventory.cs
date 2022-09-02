using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes/HeroInventory")]
public class HeroInventory : ScriptableObject
{
    List<ItemModel> items;

    public void ApplyItems(Multipliers target)
    {
        foreach (ItemModel item in items)
        {
            item.ApplyMultipliers(target);
        }
    }
}
