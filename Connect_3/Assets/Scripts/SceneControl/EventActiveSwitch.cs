using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using T so it can be used with all types of events
public class EventActiveSwitch : MonoBehaviour
{
    public GameEndEventBus bus;
    public bool InitialState = false;
    bool switched = false;
    private void Awake()
    {
        Debug.Log("Awoken");
        bus.Event += BusEvent;
        gameObject.SetActive(InitialState);
    }

    //TODO: Look where game end is triggered various times so I can get rid of switched
    private void BusEvent(GameEndInfo val)
    {
        if (!switched)
        {
            gameObject.SetActive(!InitialState);
            switched = true;
        }
    }

    private void OnDestroy()
    {
        bus.Event -= BusEvent;
    }
}
