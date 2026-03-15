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

    [SerializeField] Animator animatorPoint;
    [SerializeField] Animator animatorMult;

    public event Action AnimEnded;
    public event Action ShakeCam;
    public event Action LittleShakeCam;
    public event Action MicroShakeCam;
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
        float multToShow = multiplierBaseValue;
        score += Mathf.RoundToInt(PointReceive * multiplicator);
        yield return new WaitForSeconds(0.25f);

        yield return new WaitForSeconds(0.75f);

        // Point Ajoute multiplication demain de ces scores
        for (int i = 0; i < pe.indexPlaceToDie; i++)
        {
           
            
            pe.animatorsDeadpiece[i].SetTrigger("Piece");
            
            yield return new WaitForSeconds(0.4f);
            LittleShakeCam?.Invoke();
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
            animatorPoint.SetTrigger("WonPoint");
            yield return new WaitForSeconds(0.1f);
            UIPoint.text = $"{pointToShow}";
            //animPoint
            ShakeCam?.Invoke();
            
            yield return new WaitForSeconds(0.5f);
            pe.TMP_Texts[i].color = Color.clear;
            
            yield return new WaitForSeconds(0.1f);
            

        }
        yield return new WaitForSeconds(0.35f);


        //Multiplication
        for (int i = 1; i < sb.segments.Count ; i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (i == 1)
            {
                Animator anim = sb.segments[i].GetComponent<Animator>();


                anim.SetTrigger("CalculateMult");
                yield return new WaitForSeconds(0.05f);
                LittleShakeCam?.Invoke();
                yield return new WaitForSeconds(0.2f);
                
                animatorMult.SetTrigger("MultAnim");
                yield return new WaitForSeconds(0.1f);
                UImultiplier.text = $"{multToShow}";
                yield return new WaitForSeconds(0.1f);
                ShakeCam?.Invoke();
                yield return new WaitForSeconds(0.3f);

            }
            else
            {
                Animator anim = sb.segments[i].GetComponent<Animator>();

                anim.SetTrigger("CalculateMult");
                
                yield return new WaitForSeconds(0.2f);
                LittleShakeCam?.Invoke();
                yield return new WaitForSeconds(0.2f);
                
                animatorMult.SetTrigger("MultAnim");
                yield return new WaitForSeconds(0.1f);
                UImultiplier.text = $"{multToShow += multiplierValueBase}";
                yield return new WaitForSeconds(0.2f);
                ShakeCam?.Invoke();
                yield return new WaitForSeconds(0.3f);



            }
            yield return new WaitForSeconds(0.35f); 
        }
        yield return new WaitForSeconds(1f);
        pointToShow = 0;
        UIPoint.text = $"{pointToShow}";
        multToShow = 0;
        UImultiplier.text = $"{multToShow}";
        //Score Final

        int displayScore = 0;

        while (displayScore < score)
        {
            int step = Mathf.Clamp(displayScore / 100, 1, 300);
            displayScore += step;

            if (displayScore > score) displayScore = score; 

            UIScore.text = displayScore.ToString();
            if (displayScore % 25 <= step)
                MicroShakeCam?.Invoke();

            yield return null;
        }

        UIScore.text = score.ToString();
        displayScore = score;
        yield return new WaitForSeconds(3f);
        while (displayScore > 0)
        {
            int step = Mathf.Clamp(displayScore / 100, 1, 500);
            displayScore -= step;

            UIScore.text = displayScore.ToString();
            if (displayScore % 50 <= step)
                MicroShakeCam?.Invoke();

            yield return null;
        }
     
        yield return new WaitForSeconds(0.25f);
        pe.indexPlaceToDie = 0;

        AnimEnded?.Invoke();
    }
    public void ResetValue()
    {
        pe.movetoLooseMult = pe.BasemovetoLooseMult;
        cm.movementChange = cm.baseMoveChange;
        multiplicator = multiplierBaseValue;
        PointReceive = 0;
        score = 0;
        multiplierValue = multiplierValueBase;
        multiplicatorEnchainement = multiplicatorEnchainementBaseValue;
        multiplierValueEnchainementValue = BasemultiplierValueEnchainementAdd;

    }
}
