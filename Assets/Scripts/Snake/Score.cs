using System;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int playerScore;
    
    public String Name;
    [SerializeField] public TMP_InputField inputField;
    public void AddScore(int score)
    {
        playerScore += score;
        

    }
    public int GetScore() => playerScore;
    void OnTextChanged(string value)
    {
        Name = value;
    }
    public void EndScore()
    {
        GameManager.Instance.saveData.topScoresSnake.AddScore(Name, playerScore);


        if (playerScore > GameManager.Instance.saveData.highScoreSnake)
            GameManager.Instance.saveData.highScoreSnake = playerScore;

        SaveManager.Save(GameManager.Instance.saveData);
    }
    private void Start()
    {
        inputField.onValueChanged.AddListener(OnTextChanged);
    }
}
