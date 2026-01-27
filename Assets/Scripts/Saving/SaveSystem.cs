using System;
using System.IO;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public PlayerState playerState;
}

public class SaveSystem
{
    private static SaveData _saveData;

    private static string GetFileName()
    {
        string saveFile = Application.persistentDataPath + "/savedata.save";
        return saveFile;
    }

    public static void Save()
    {
        SaveRuntimeDataToObject();

        var serializedData = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(GetFileName(), serializedData);
    }

    public static void Load()
    {
        var savedContent = File.ReadAllText(GetFileName());
        _saveData = JsonUtility.FromJson<SaveData>(savedContent);

        WriteSaveToRuntimeData();
    }

    private static void SaveRuntimeDataToObject()
    {
        Player.Instance.Save(ref _saveData.playerState);
    }

    private static void WriteSaveToRuntimeData()
    {
        Player.Instance.Load(_saveData.playerState);
    }
}