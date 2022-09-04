using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EventBuses/HeroSelected")]
public class HeroEventBus : ScriptableObject
{
    public event Action<HeroModel> Event;
    public void NotifyEvent(HeroModel hero) => Event?.Invoke(hero);
}