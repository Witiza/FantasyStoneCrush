using UnityEngine;

[CreateAssetMenu]
public class VolumeOptionsSO : ScriptableObject
{
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SFXVolume { get; set; }
}