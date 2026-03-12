using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class NumberOfCou : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreTexte;
    [SerializeField] PlayerScoreSnake ScorePlayer;
    [SerializeField] PlayerMovement pm;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        ScoreTexte.text = $" {pm.NumberOfMoves.ToString()}";
    }
}
