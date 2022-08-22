using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO: Find a way to use the same function for options and progression
public static class SaveSystem
{
    public static void SaveGame(ProgressionSerializable progression)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savegame.qinc";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, progression);
        stream.Close();
    }

    public static ProgressionSerializable LoadGame()
    {
        ProgressionSerializable data=null;
        string path = Application.persistentDataPath + "/savegame.qinc";
        Debug.Log("Loading game from " + path);
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            data = formatter.Deserialize(stream) as ProgressionSerializable;
            stream.Close();
        }
        else
        {
            Debug.Log("Creating new savegame");
        }
        return data;
    }

    public static void SaveOptions(VolumeOptionsSerializable data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.qinc";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static VolumeOptionsSerializable LoadOptions()
    {
        VolumeOptionsSerializable data=null;
        string path = Application.persistentDataPath + "/options.qinc";
        Debug.Log("Loading options from " + path);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            data = formatter.Deserialize(stream) as VolumeOptionsSerializable;
            stream.Close();
        }
        else
        {
            Debug.Log("Creating new options");
        }
        return data;
    }
}

  