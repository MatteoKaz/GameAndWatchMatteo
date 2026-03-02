using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNamePanel : MonoBehaviour
{
    [SerializeField] private SO_PlayerDatas _playerDatas;
    [SerializeField] private TMP_InputField playerInputField;

    public void LoadDatasInPanel()
    {
        playerInputField.text = _playerDatas.Name;
    }

    public void SaveDatasInSO()
    {
        _playerDatas.Name = playerInputField.text;
    }
}
