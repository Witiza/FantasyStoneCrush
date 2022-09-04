using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/ItemEvent")]
public class EquipmentEvent : ScriptableObject
{
    public event Action<ItemModel,bool> Event;
    public void NotifyEvent(ItemModel item, bool equipping) => Event?.Invoke(item,equipping);
}