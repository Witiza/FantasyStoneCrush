using System;
using UnityEngine;

[CreateAssetMenu]
public class StringEventBus : ScriptableObject
{
    public event Action<string> Event;
    public void NotifyEvent(string str) => Event?.Invoke(str);
}

