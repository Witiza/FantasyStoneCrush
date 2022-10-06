using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO: Find a way to use the same function for options and progression
public static partial class SaveSystem
{
    public static string ProgressionPath = Application.persistentDataPath + "/savegame.json";
    public static string LevelsPath = Application.persistentDataPath + "/levels.json";

    public static void SaveClass<T>(T save_class,string path)
    {
        string to_save = JsonUtility.ToJson(save_class);
        Save(path, to_save);
    }

    public static T LoadClass<T>(string path)
    {
        T data = default(T);
        string to_load = Load(path);
        if (to_load != "")
        {
            data = JsonUtility.FromJson<T>(to_load);
        }
        return data;
    }

    public static void SaveOptions(VolumeOptionsSO data)
    {
        PlayerPrefs.SetFloat("MasterVolume",data.MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume",data.MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", data.SFXVolume);
    }

    public static VolumeOptionsSO LoadOptions()
    {
        VolumeOptionsSO data =ScriptableObject.CreateInstance<VolumeOptionsSO>();
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

  