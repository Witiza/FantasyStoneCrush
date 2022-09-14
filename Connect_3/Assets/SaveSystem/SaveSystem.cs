using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO: Find a way to use the same function for options and progression
public static partial class SaveSystem
{
    private static string ProgressionPath = Application.persistentDataPath + "/savegame.json";
    private static string InventoryPath = Application.persistentDataPath + "/inventory.json";
    private static string WarriorInventoryPath = Application.persistentDataPath + "warrior.json";
    private static string RogueInventoryPath = Application.persistentDataPath + "/rogue.json";
    private static string ArcherInventoryPath = Application.persistentDataPath + "/archer.json";
    private static string MageInventoryPath = Application.persistentDataPath + "/mage.json";

    public static void SaveGame(SaveGameJsonWrapper progression)
    {
        string to_save = JsonUtility.ToJson(progression);
        Save(ProgressionPath, to_save);
    }

    //-------------------------Loading--------------------------
    public static SaveGameJsonWrapper LoadGame(out bool success)
    {
        success = false;
        SaveGameJsonWrapper data = new SaveGameJsonWrapper();
        string to_load = Load(ProgressionPath);
        if (to_load != "")
        {
            data = JsonUtility.FromJson<SaveGameJsonWrapper>(to_load);
            success = true;
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

  