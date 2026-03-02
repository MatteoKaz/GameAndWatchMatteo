using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDatas", menuName = "Scriptable Objects/PlayerDatas")]
public class SO_PlayerDatas : ScriptableObject
{
    public string Name;
    public int Score;
    public int Level;

    private SaveController saveSystem;

    public void LoadDatas()
    {
        CheckSaveSystem();

        PlayerDatas datas = saveSystem.Load();
        Name = datas.Name;
        Score = datas.Score;
        Level = datas.Level;
    }

    public void SaveDatas()
    {
        CheckSaveSystem();
        PlayerDatas datas = new PlayerDatas();
        datas.Name = Name;
        datas.Score = Score;
        datas.Level = Level;
        saveSystem.Save(datas);
    }

    private void CheckSaveSystem()
    {
        if (saveSystem == null)
        {
            saveSystem = new SaveController();
        }
    }
}
