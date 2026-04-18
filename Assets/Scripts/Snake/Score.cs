using UnityEngine;

public class Score : MonoBehaviour
{
    public int playerScore;
    

    public void AddScore(int score)
    {
        playerScore += score;
    }
    public int GetScore() => playerScore;
}
