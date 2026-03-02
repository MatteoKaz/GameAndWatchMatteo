using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _timeStepDuration = 1.5f;
    [SerializeField] private float _TimeToReverse = 25f;
    public event Action OnTimePassed;
    public event Action OnTimeReverse;
    [SerializeField] private InputPlayerManagerCustom m_inputPlayerManager;
    private bool Move = false;
    private bool Stop= false;
   IEnumerator SpendingTime()
    {
        while (true)
        {
            
                yield return new WaitForSeconds(_timeStepDuration);
            if (Stop != true)
            {

                OnTimePassed?.Invoke();
            }
           
           
        }

        
           
        
    }

    IEnumerator SpendingTimeReverse()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(_TimeToReverse);
             OnTimeReverse?.Invoke();
            Stop = true;
            yield return new WaitForSeconds(1f);
            Stop = false;
        }
    }


    IEnumerator TimerLooseTime()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Move == true)
            {
                yield return new WaitForSeconds(1.5f);
                Move = false;
            }
            else
            {
                _timeStepDuration = Mathf.Clamp(_timeStepDuration - 0.15f, 0.5f, 1.5f);
            }
            
        }
    }
    

    private void Start()
    {
        StartTime();

    }
    private void StartTime()
    {
        StartCoroutine(SpendingTime());
        StartCoroutine(SpendingTimeReverse());
        StartCoroutine(TimerLooseTime());
    }

    public void StopTime()
    {
        StopAllCoroutines();

    }

    private void OnEnable()
    {
        m_inputPlayerManager.OnMoveLeft += MoveToPrevPosition;
        m_inputPlayerManager.OnMoveRight += MoveToNextPosition;
    }

    private void OnDisable()
    {
        m_inputPlayerManager.OnMoveLeft -= MoveToPrevPosition;
        m_inputPlayerManager.OnMoveRight -= MoveToNextPosition;
    }

    void MoveToPrevPosition()
    {
        _timeStepDuration = Mathf.Clamp(_timeStepDuration + 0.05f, 0.5f, 1.5f);
        Move = true;
    }
    void MoveToNextPosition()
    {
        _timeStepDuration = Mathf.Clamp(_timeStepDuration + 0.05f, 0.5f, 1.5f);
        Move = true;
    }
}
