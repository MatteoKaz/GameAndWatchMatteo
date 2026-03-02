using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{

  public int score;
    
    public event Action ONBonus;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;


    void Update()
    {
        
    }

       IEnumerator AddPoint()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            yield return new WaitForSeconds(10f);
            score += 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bonus>() != null)
        {
            score += 25;
            var actor = collision.gameObject;
            _audioEventDispatcher.PlayAudio(AudioType.Win);
            Destroy(actor);
            ONBonus?.Invoke();

        }
    }

    private void Start()
    {
        StartTime();

    }
    private void StartTime()
    {
        
        StartCoroutine(AddPoint());
    }
}
