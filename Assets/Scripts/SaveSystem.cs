using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{

    private static string _filePath = "/player.milk";

    public static void SaveGame(CreatureMood mood)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + _filePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(mood);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + _filePath;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }
}
