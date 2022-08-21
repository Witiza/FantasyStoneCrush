using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using T so it can be used with all types of events
public class EventActiveSwitch : MonoBehaviour
{
    public GameEndEventBus bus;
    public bool InitialState = false;
    private void Awake()
    {
        bus.Event += BusEvent;
        gameObject.SetActive(InitialState);
    }

    private void BusEvent(GameEndInfo val)
    {
        gameObject.SetActive(!InitialState);
    }

    private void OnDestroy()
    {
        bus.Event -= BusEvent;
    }
}
