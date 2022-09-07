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