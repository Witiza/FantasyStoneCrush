using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/GameEndEvent")]
public class GameEndEventBus : ScriptableObject
{
    public event Action<GameEndInfo> Event;
    public void NotifyEvent(GameEndInfo val) => Event?.Invoke(val);
}

