using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsDisplay : MonoBehaviour
{
    public PlayerProgressionService progression;
    public string DefaultText;
    public TMP_Text text;
    public IntEventBus CoinsEvent;
    // Start is called before the first frame update
    void Awake()
    {
        CoinsEvent.Event += CoinsChanged;
        text.text = DefaultText + progression.Coins.ToString();
    }

    private void CoinsChanged(int obj)
    {
        text.text = DefaultText + progression.Coins.ToString();
    }

    // Update is called once per frame
    void OnDestroy()
    {
        CoinsEvent.Event -= CoinsChanged;
    }
}
