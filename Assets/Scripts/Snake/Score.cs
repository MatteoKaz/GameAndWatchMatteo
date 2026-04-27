using System;
using TMPro;

using UnityEngine;

public class Score : MonoBehaviour
{
    public int playerScore;
    [SerializeField] SnakeBody2 playerMovement;
    
    public String Name;
    [SerializeField] public TMP_InputField inputField;
    public void AddScore(int score)
    {
        playerScore += score;
        playerMovement.moveDuration -= 0.005f;
        playerMovement.moveDuration = Mathf.Clamp(playerMovement.moveDuration, 0.12f, playerMovement.moveDuration);

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
