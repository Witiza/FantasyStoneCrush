using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderSetter : MonoBehaviour
{
    public VolumeOptionsSO volume;
    public Slider Master;
    public Slider Music;
    public Slider SFX;

    private void Start()
    {
        Master.value = volume.MasterVolume;
        Music.value = volume.MusicVolume;
        SFX.value = volume.SFXVolume;
    }
}

