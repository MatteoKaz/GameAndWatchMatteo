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
public class ScoreEntry
{
    public string name;
    public int score;

    public ScoreEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[System.Serializable]
public class TopScores
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();

    public void AddScore(string name, int score)
    {
        entries.Add(new ScoreEntry(name, score));
        entries.Sort((a, b) => b.score.CompareTo(a.score)); // tri décroissant
        if (entries.Count > 10)
            entries.RemoveAt(10); 
    }
}

