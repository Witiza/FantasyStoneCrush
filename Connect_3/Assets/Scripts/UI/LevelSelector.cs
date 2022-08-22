using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    Scrollbar bar;
    
    public PlayerProgressionSO Progression;
    public StringEventBus PlayLevel;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponentInChildren<Scrollbar>();
        bar.value = (float)Progression.CurrentLevel / (float)bar.numberOfSteps;
        bar.SetValueWithoutNotify(bar.value);
    }

    //https://forum.unity.com/threads/scrollbar-steps-c.653977/
    public void OnSliderChanged(float value)
    {
        int currentStep = Mathf.RoundToInt(value / (1f / (float)bar.numberOfSteps));
        if (value >= 0.5) currentStep -= 1;
        Progression.CurrentLevel = currentStep;
    }
}
