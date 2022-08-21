using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/SimpleEvent")]
public class SimpleEventBus : ScriptableObject
{
    public event Action Event;
    public void NotifyEvent() => Event?.Invoke();
}

