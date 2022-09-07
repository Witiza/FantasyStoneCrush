using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public PlayerProgressionSO progression;
    public VolumeOptionsSO volumeOptions;
    public bool UseSavegame;
    void Awake()
    {
        if (UseSavegame)
        {
            progression.LoadGame();
            volumeOptions.LoadOptions();
        }
    }

    void OnDestroy()
    {
        if (UseSavegame)
        {
            progression.SaveGame();
            volumeOptions.SaveOptions();
        }
    }
}
