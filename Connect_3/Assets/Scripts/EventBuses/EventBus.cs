using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/Event")]
public class EventBus : ScriptableObject
{
    public event Action Event;
    public void NotifyEvent() => Event?.Invoke();
}

