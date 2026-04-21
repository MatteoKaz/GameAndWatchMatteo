using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int highScoreSnake = 0;
    public int highScoreGameAndWatch = 0;
    public TopScores topScoresSnake = new TopScores();
    public TopScores topScoresGameAndWatch = new TopScores();
}

[System.Serializable]
public class TopScores
{
    public List<int> scores = new List<int>();

    public void AddScore(int score)
    {
        scores.Add(score);
        scores.Sort((a, b) => b.CompareTo(a)); // tri décroissant
        if (scores.Count > 10)
            scores.RemoveAt(10); // garde seulement le top 10
    }
}

