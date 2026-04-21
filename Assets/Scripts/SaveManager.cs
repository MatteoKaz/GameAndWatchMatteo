using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static readonly string SavePath =
        Application.persistentDataPath + "/save.json";

    public static void Save<T>(T data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[Save] Sauvegardé : {SavePath}");
    }

    public static T Load<T>() where T : new()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[Save] Aucune save trouvée, création par défaut.");
            return new T();
        }
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<T>(json);
    }

    public static void Delete()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}