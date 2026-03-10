using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;


public class AIManger : MonoBehaviour
{
    public List<EnemyMovement> enemies = new List<EnemyMovement>();
    [SerializeField] public TurnManager tm;
    public event Action TurnDoneEnemy;
    public EnemyMovement enemy;
    private int count = 0 ;
    private bool TurnEnemy= false;
    [SerializeField] public GridManager gm;

    void OnEnable()
    {
        tm.EnemyTurn +=MoveEnemy;
        tm.playerTurn += LaunchEnemy;
        gm.FinishInitialize += InitializeStart;
    }

    void LaunchEnemy()
    {
        if (TurnEnemy == true)
        {
            TurnEnemy = false;
            if (enemies.Count == 0)
            return;
             int index = UnityEngine.Random.Range(0, enemies.Count);
             enemy = enemies[index];
             enemy.TryMove();
             enemy.SetSpriteColor(Color.red);
             
             if (count != 0)
            {
            //enemy.TryMove();
            }
            else
            {
                 count ++;
            }
         
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.25f);
        if (enemy != null)
        {
            enemy.StartCoroutine(enemy.MoveEnemy());

        }
        else
        {
            LaunchEnemy();
            yield return new WaitForSeconds(0.5f);
            if (enemy != null)
            {
                enemy.StartCoroutine(enemy.MoveEnemy());

            }
        }
        
    }
    void MoveEnemy()
    {
        
            TurnEnemy = true;
            StartCoroutine(Wait());
       
        
    }

    public void SetEndTurn()
    {
        TurnDoneEnemy?.Invoke();
    }


    public void InitializeStart()
    {
        StartCoroutine(Initialize());
    }
    public IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.25f);

        TurnEnemy = true;
        LaunchEnemy();
           
        

    }
}
