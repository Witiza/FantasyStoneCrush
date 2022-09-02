using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heroes/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    public int test;
    public List<ItemModel> items;
}