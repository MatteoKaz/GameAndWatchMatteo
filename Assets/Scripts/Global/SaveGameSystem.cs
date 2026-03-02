using UnityEngine;

public class SaveGameSystem : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas playerData;

    public void LoadGame()
    {
        playerData.LoadDatas();
    }

    public void SaveGame()
    {
        playerData.SaveDatas();
    }

}
