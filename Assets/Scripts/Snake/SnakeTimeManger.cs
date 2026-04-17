using System;
using System.Collections;
using UnityEngine;

public class SnakeTimeManger : MonoBehaviour
{
    [SerializeField] private float tickInterval = 3f;
    [SerializeField] private float BonusTick = 8f;
    public event Action OnTick;
    public event Action OnBonusTick;

    private bool running = false;

    void Start()
    {
        StartCoroutine(TimeLoop());
        StartCoroutine(GreatBonusTimer());
    }

    IEnumerator TimeLoop()
    {
        running = true;

        while (running)
        {
            yield return new WaitForSeconds(tickInterval);
            OnTick?.Invoke();
        }
    }
    IEnumerator GreatBonusTimer()
    {
        running = true;
        while(running)
        {
            yield return new WaitForSeconds(BonusTick);
            OnBonusTick?.Invoke();
        }
    }

    public void Stop()
    {
        running = false;
    }

    public void SetSpeed(float newInterval)
    {
        tickInterval = Mathf.Max(0.05f, newInterval);
    }
}