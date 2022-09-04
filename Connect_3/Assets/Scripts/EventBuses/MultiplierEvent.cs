using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/MultiplierEvent")]
public class MultiplierEvent : ScriptableObject
{
    public event Action<Multipliers> Event;
    public void NotifyEvent(Multipliers multiplers)=>Event?.Invoke(multiplers);
}
