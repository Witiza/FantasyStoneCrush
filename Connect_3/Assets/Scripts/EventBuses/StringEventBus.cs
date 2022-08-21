using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/StringEvent")]
public class StringEventBus : ScriptableObject
{
    public event Action<string> Event;
    public void NotifyEvent(string val) => Event?.Invoke(val);
}

