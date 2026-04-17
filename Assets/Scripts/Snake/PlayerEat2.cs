using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEat2 : MonoBehaviour
{
    [SerializeField] private PlayerMovement2 playerMovement;
    [SerializeField] private Spawner1 spawner;
    [SerializeField] private AiManager2 aiManager;
    [SerializeField] Score score;
    private float TimeBonus = 6f;

    private Coroutine Timer ;
    public event Action<GameObject> OnEatObject;
    public enum PlayerState
    {
        normal,
        Fire,
        Ice,
        Coin
    }
    public PlayerState state;
    public void CheckEat()
    {
        Vector2Int playerPos = playerMovement.coordPlayer;
        foreach (Enemy2 e in aiManager.enemies)
        {
            if (e == null) continue;
            if (e.coordEnemy == playerMovement.coordPlayer)
            {
                if (state == PlayerState.normal)
                {
                    playerMovement.GameOver(); break;
                }
                
                if(state == PlayerState.Fire)
                {
                    aiManager.RemoveEnemy(e);
                    score.AddScore(e.Value);
                    Destroy(e.gameObject);
                    break; 
                }
            }

        }
        
        GameObject obj = spawner.GetObjectAt(playerPos);
        if (obj == null) return;

        EatData data = obj.GetComponent<EatData>();
        if (data == null) return;

        OnEat(data);

        Destroy(obj);
    }

    public void OnEat(EatData data)
    {
        switch (data.type)
        {
            case EatData.EatType.Normal:
                playerMovement.snakeBody.Grow();
                score.AddScore(data.value);
                break;
            case EatData.EatType.Fire:
                state = PlayerState.Fire;
                foreach (GameObject gm in playerMovement.snakeBody.segments)
                {
                    Animator animator= gm.GetComponent<Animator>();
                    animator.SetTrigger("Combo");
                }
                
                if (Timer!= null)
                    StopCoroutine(Timer);
                Timer = StartCoroutine(TimeBeforeStopBonus());
                break;



        }

    }
    IEnumerator TimeBeforeStopBonus()
    {
        yield return new WaitForSeconds(TimeBonus-3f);
        foreach (GameObject gm in playerMovement.snakeBody.segments)
        {
            Animator animator = gm.GetComponent<Animator>();
            animator.SetTrigger("ComboEnd");
        }
        yield return new WaitForSeconds(3f);
        foreach (GameObject gm in playerMovement.snakeBody.segments)
        {
            Animator animator = gm.GetComponent<Animator>();
            animator.SetTrigger("BackToIdle");
        }
        state = PlayerState.normal; 
    }
}