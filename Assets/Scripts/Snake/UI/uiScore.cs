using TMPro;
using UnityEngine;

public class uiScore : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreTexte;
    [SerializeField] Score Score;
    [SerializeField] TMP_Text HighScoreTexte;
    private void Update()
    {
        ScoreTexte.text = $"Score: {Score.playerScore}";
        if (HighScoreTexte != null )
        {
            if (GameManager.Instance.saveData.highScoreSnake != 0)
                HighScoreTexte.text = $"Record : {GameManager.Instance.saveData.highScoreSnake}";
        }
        
    }
}
