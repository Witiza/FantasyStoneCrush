using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActiveSwitch : MonoBehaviour
{
    public EventBus bus;
    public bool InitialState = false;
    private void Awake()
    {
        bus.Event += BusEvent;
        gameObject.SetActive(InitialState);
    }

    private void BusEvent()
    {
        Debug.Log("Event called called");
        gameObject.SetActive(!InitialState);
    }

    private void OnDestroy()
    {
        bus.Event -= BusEvent;
    }
}
