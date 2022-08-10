using System;
using UnityEngine;

[CreateAssetMenu]
public class Booster : ScriptableObject
{
    public event Action<bool> BoosterEvent;
    public int amount;
    public void NotifyEvent()
    {
        if(amount >0)
        {
            amount--;
            BoosterEvent?.Invoke(true);

        }
        else
        {
            Debug.Log("Refilling booster for debugging purposes");
            amount = 10;
            BoosterEvent?.Invoke(false);
        }
    }
}


