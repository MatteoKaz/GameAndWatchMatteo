using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerScoreSnake : MonoBehaviour
{

    public int score;
    public float multiplicator = 1f;
    public float multiplierBaseValue = 1f;
    public float multiplierValue = 2f;
    public float multiplierValueBase = 2f;


    public float multiplicatorEnchainement = 1f;
    public float multiplicatorEnchainementBaseValue = 1f;

    public float multiplierValueEnchainementValue = 2f;
    public float BasemultiplierValueEnchainementAdd = 2f;



    public int bonusValue = 0;

    public event Action ONBonus;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    [SerializeField] private PlayerEat pe;
    [SerializeField] SnakeBody sb;
    [SerializeField] ChangeMovement cm;
    public int PointReceive = 0;

    [SerializeField] TMP_Text UIPoint;
    [SerializeField] TMP_Text UImultiplier;
    [SerializeField] TMP_Text UIScore;

    public event Action ShakeCam;
    public void OnEnable()
    {
        sb.GrownUp += AddMult;
        sb.GrownDown += MinusMult;
    }



    public void AddPoint(int Score)
    {
        
        PointReceive += Mathf.RoundToInt (Score * multiplicatorEnchainement);
        PointReceive += bonusValue;
        //_audioEventDispatcher.PlayAudio(AudioType.Win);

        ONBonus?.Invoke();
    }

    private void Start()
    {
        multiplicator =  1.00f;

    }
    private void StartTime()
    {


    }
    public void AddMult()
    {
        multiplicator += multiplierValue;
    }
    public void MinusMult()
    {
        multiplicator = Mathf.Clamp(multiplicator - multiplierValue, multiplicatorEnchainementBaseValue, multiplicator) ;
    }

    public void CalculateScore()
    {
        StartCoroutine(AnimEnd());
    }


    public IEnumerator AnimEnd()
    {
        int pointToShow = 0;
        score += Mathf.RoundToInt(PointReceive * multiplicator);
        yield return new WaitForSeconds(0.25f);

        yield return new WaitForSeconds(0.75f);

        // Point Ajoute multiplication demain de ces scores
        for (int i = 0; i < pe.indexPlaceToDie; i++)
        {
            pe.animatorsDeadpiece[i].SetTrigger("Piece");
            yield return new WaitForSeconds(0.18f);
            pe.deathPlace[i].sprite = null;
            pe.TMP_Texts[i].color = Color.white;
            pe.animatorsDeadpiece[i].SetTrigger("Point");
            yield return new WaitForSeconds(0.15f);
            if (int.TryParse(pe.TMP_Texts[i].text, out int number))
            {
                pointToShow += number; // 42
            }
                else
            {
                Debug.Log("Conversion impossible");
            }
            UIPoint.text = $"{pointToShow}";
            //animPoint
            yield return new WaitForSeconds(0.5f);
            ShakeCam?.Invoke();

            yield return new WaitForSeconds(0.35f);

        }
        yield return new WaitForSeconds(0.35f);


        //Multiplication


        pointToShow = 0;
        UIPoint.text = $"{pointToShow}";
        //Score Final
        
        for (int i = 0; i <= score; i++)
        {
            UIScore.text = $"{i} ";
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);
        for (int i = score; i == 0; i--)
        {
            UIScore.text = $"{i} ";
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(2f);
        
    }
    public void ResetValue()
    {
        cm.movementChange = cm.baseMoveChange;
        multiplicator = multiplierBaseValue;
        PointReceive = 0;
        score = 0;
        multiplierValue = multiplierValueBase;
        multiplicatorEnchainement = multiplicatorEnchainementBaseValue;
        multiplierValueEnchainementValue = BasemultiplierValueEnchainementAdd;

    }
}
