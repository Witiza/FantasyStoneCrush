using System;
using UnityEngine;

[CreateAssetMenu]
public class Booster : ScriptableObject
{
    public event Action<bool> BoosterEvent;
    public int amount;
    public int cost = 20;
   
    public void NotifyEvent()
    {
        if(amount >0)
        {
            amount--;
            BoosterEvent?.Invoke(true);
        }
        else
        {
            BoosterEvent?.Invoke(false);
        }
    }
}


