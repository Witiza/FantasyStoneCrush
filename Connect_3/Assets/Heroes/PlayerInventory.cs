using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    public List<ItemModel> items = new List<ItemModel>();

    public void AddItem(ItemModel item)
    {
        if(!items.Contains(item))
        {
            items.Add(item);
        }
        else
        {
            Debug.LogWarning("Adding an already existing item to the players inventory");
        }
    }
}