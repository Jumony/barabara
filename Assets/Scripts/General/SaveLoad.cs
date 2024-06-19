using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class SaveLoad
{

    public static void Save(PlayerProgression playerProgression)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerProgression.bara");

        bf.Serialize(file, playerProgression);
        file.Close();
    }

    public static PlayerProgression Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerProgression.bara"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerProgression.bara", FileMode.Open);
            PlayerProgression playerProgression = (PlayerProgression)bf.Deserialize(file);
            file.Close();
            return playerProgression;
        }

        // Creates a new PlayerProgression object if one does not already exist
        else
        {
            return new PlayerProgression();
        }
    }

    public static void ResetSave()
    {
        string path = Application.persistentDataPath + "/playerProgression.bara";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file reset");
        }
    }
}
