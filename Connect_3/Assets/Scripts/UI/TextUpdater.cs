using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    public string DefaultText;
    public TMP_Text text;
    public IntEventBus bus;
    private void Awake()
    {
        bus.Event += BusEvent;
    }

    private void BusEvent(int number)
    {
        text.text = DefaultText + number.ToString();
    }

    private void OnDestroy()
    {
        bus.Event -= BusEvent;
    }

}
