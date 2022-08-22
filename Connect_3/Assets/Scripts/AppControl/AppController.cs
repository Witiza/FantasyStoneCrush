using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public PlayerProgressionSO progression;
    public VolumeOptionsSO volumeOptions;

    void Awake()
    {
        progression.LoadGame();
        volumeOptions.LoadOptions();
    }

    void OnDestroy()
    {
        progression.SaveGame();
        volumeOptions.SaveOptions();
    }
}
