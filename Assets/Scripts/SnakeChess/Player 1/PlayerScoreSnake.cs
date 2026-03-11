using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScoreSnake : MonoBehaviour
{

    public int score;
    public float multiplicator = 1f;

    public event Action ONBonus;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    [SerializeField] private PlayerEat pe;
    [SerializeField] SnakeBody sb;
    public void OnEnable()
    {
        sb.GrownUp += AddMult;
        sb.GrownDown += MinusMult;
    }



    public void AddPoint(int Score)
    {

        score += Mathf.RoundToInt (Score * multiplicator);

        //_audioEventDispatcher.PlayAudio(AudioType.Win);

        ONBonus?.Invoke();
    }

    private void Start()
    {
        multiplicator =  1.75f;

    }
    private void StartTime()
    {


    }
    public void AddMult()
    {
        multiplicator += 0.25f;
    }
    public void MinusMult()
    {
        multiplicator -= 0.25f;
    }
}
