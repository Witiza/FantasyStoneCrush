using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO: Find a way to use the same function for options and progression
public static class SaveSystem
{
    private static string ProgressionPath = Application.persistentDataPath + "/savegame.json";
    public static void SaveGame(ProgressionSerializable progression)
    {
        string to_save = JsonUtility.ToJson(progression);
        Save(ProgressionPath, to_save);
    }

    public static ProgressionSerializable LoadGame()
    {
        ProgressionSerializable data=new ProgressionSerializable();
        string to_load = Load(ProgressionPath);
        if (to_load != "")
        {
            data = JsonUtility.FromJson<ProgressionSerializable>(to_load);
        }
        return data;
    }

    public static void SaveOptions(VolumeOptionsSerializable data)
    {
        PlayerPrefs.SetFloat("MasterVolume",data.MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume",data.MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", data.SFXVolume);
    }

    public static VolumeOptionsSerializable LoadOptions()
    {
        VolumeOptionsSerializable data=new VolumeOptionsSerializable();
        data.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        data.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        data.SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        return data;
    }

    private static void Save(string path, string file)
    {
        File.WriteAllText(path, file);
    }

    private static string Load(string path)
    {
        string ret = "";
        if(File.Exists(path))
        {
            ret = File.ReadAllText(path);
        }
        return ret;
    }
}

  