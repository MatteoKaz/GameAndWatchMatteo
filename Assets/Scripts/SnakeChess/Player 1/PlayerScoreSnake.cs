using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScoreSnake : MonoBehaviour
{

    public int score;
    public float multiplicator = 1f;
    public float multiplierBaseValue = 1f;
    public float multiplierValue = 2f;
    public float multiplierValueBase = 2f;


    public float multiplicatorEnchainement = 1f;
    public float multiplicatorEnchainementBaseValue = 1f;

    public float multiplierValueEnchainementValue = 4f;
    public float BasemultiplierValueEnchainementAdd = 4f;



    public int bonusValue = 0;

    public event Action ONBonus;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    [SerializeField] private PlayerEat pe;
    [SerializeField] SnakeBody sb;
    [SerializeField] ChangeMovement cm;
    public int PointReceive = 0;
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
        score += Mathf.RoundToInt(PointReceive * multiplicator);
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
