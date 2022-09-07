using UnityEngine;

[CreateAssetMenu]
public class VolumeOptionsSO : ScriptableObject
{
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SFXVolume { get; set; }

    public void SaveOptions()
    {
        SaveSystem.SaveOptions(new VolumeOptionsSerializable(MasterVolume,MusicVolume,SFXVolume));
    }
    public void LoadOptions()
    {
        VolumeOptionsSerializable tmp = SaveSystem.LoadOptions();
        if (tmp != null)
        {
            MasterVolume = tmp.MasterVolume;
            MusicVolume = tmp.MusicVolume;
            SFXVolume = tmp.SFXVolume;
        }
    }
}
