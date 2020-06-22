using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveManager : EditorWindow
{
    public static string directory = "/SaveData/";
    public static string fileName = "MyData.txt";

    public static void Save(DataPlayer dataPlay)
    {
        string dir = Application.persistentDataPath + directory;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(dataPlay);
        File.WriteAllText(dir + fileName, json);
    }

    public static bool CheckData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        if (File.Exists(fullPath))
            return true;
        else
        {
            return false;
        }
    }

    public static DataPlayer Load()
    {
        DataPlayer dataplayer = new DataPlayer();
        string fullPath = Application.persistentDataPath + directory + fileName;
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            Debug.Log(json);
            dataplayer = JsonUtility.FromJson<DataPlayer>(json);
        }
        else
        {
            Debug.Log("Save File does not exits");
        }

        return dataplayer;
    }
    [MenuItem("Save/Del")]
    public static void Del()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log("Del data");
        }
    }
    [MenuItem("Save/DelAll")]
    public static void DelAll()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            Debug.Log("Del data");
        }
        PlayerPrefs.DeleteAll();
    }
}