using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes/HeroInventory")]
public class HeroInventory : ScriptableObject
{
    public List<ItemModel> items = new List<ItemModel>();

    public void ApplyItems(Multipliers target)
    {
        foreach (ItemModel item in items)
        {
            item.ApplyMultipliers(target);
        }
    }


    public void EquipItem(ItemModel item)
    {
        items.Add(item);
        if(items.Count > 4)
        {
            Debug.LogError("Added an item over the item limit, this shouldnt happen");
        }
    }
    public void UnequipItem(ItemModel item)
    {
        if (!items.Remove(item))
        {
            Debug.LogError("Could not remove item");
        }
    }

    public bool HasItem(ItemModel item)
    {
        return items.Contains(item);
    }
}
