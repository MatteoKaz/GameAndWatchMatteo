using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class MultiplierUiSnake : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreTexte;
    [SerializeField] PlayerScoreSnake ScorePlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //ScoreTexte.text = $"{ScorePlayer.PointReceive.ToString()} X {ScorePlayer.multiplicator.ToString()} ";
    }
}
