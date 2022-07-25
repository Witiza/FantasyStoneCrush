using System;
using UnityEngine;

[CreateAssetMenu]
public class EventBus : ScriptableObject
{
    public event Action Event;
    public void NotifyEvent() => Event?.Invoke();
}
