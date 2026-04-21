using UnityEditor.Overlays;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int playerScore;
   

    public void AddScore(int score)
    {
        playerScore += score;
        

    }
    public int GetScore() => playerScore;

    public void EndScore()
    {
        GameManager.Instance.saveData.topScoresSnake.AddScore(playerScore);


        if (playerScore > GameManager.Instance.saveData.highScoreSnake)
            GameManager.Instance.saveData.highScoreSnake = playerScore;

        SaveManager.Save(GameManager.Instance.saveData);
    }
}
