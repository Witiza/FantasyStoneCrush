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
[System.Serializable]
public class VolumeOptionsSerializable
{
    public VolumeOptionsSerializable() { }
    public VolumeOptionsSerializable(float master, float music, float sfx)
    {
        MasterVolume = master;
        MusicVolume = music;
        SFXVolume = sfx;
    }
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
}