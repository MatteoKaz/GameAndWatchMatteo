using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SaveData saveData;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        saveData = SaveManager.Load<SaveData>();
    }

    public void SaveGame()
    {
        SaveManager.Save(saveData);
    }

    void OnApplicationQuit()
    {
        SaveManager.Save(saveData);
    }
}