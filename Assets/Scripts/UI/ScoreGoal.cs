using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class ScoreGoal : MonoBehaviour
{
    [SerializeField] TMP_Text ScoreTexte;
    [SerializeField] WaveManager WM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        ScoreTexte.text = $" {WM.currentWaveData.waveValue.ToString()} ";
    }
}
