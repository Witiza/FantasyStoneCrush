using UnityEngine;

[CreateAssetMenu]
public class VolumeOptionsSO : ScriptableObject
{
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SFXVolume { get; set; }

    public void SaveOptions()
    {
        SaveSystem.SaveOptions(this);
    }
    public void LoadOptions()
    {
        VolumeOptionsSO tmp = SaveSystem.LoadOptions();
        MasterVolume = tmp.MasterVolume;
        MusicVolume = tmp.MusicVolume;
        SFXVolume = tmp.SFXVolume;
    }
}
