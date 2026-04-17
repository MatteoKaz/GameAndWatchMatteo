using TMPro;
using UnityEngine;

public class uiScore : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreTexte;
    [SerializeField] Score Score;

    private void Update()
    {
        ScoreTexte.text = $"Score: {Score.playerScore}";
    }
}
