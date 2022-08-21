using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/IntEvent")]
public class IntEventBus : ScriptableObject
{
    public event Action<int> Event;
    public void NotifyEvent(int val) => Event?.Invoke(val);
}

