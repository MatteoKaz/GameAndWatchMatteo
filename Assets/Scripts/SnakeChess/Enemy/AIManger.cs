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


    void OnEnable()
    {
        tm.EnemyTurn +=MoveEnemy;
        tm.playerTurn += LaunchEnemy;
    }

    void LaunchEnemy()
    {
        if (enemies.Count == 0)
            return;
        int index = UnityEngine.Random.Range(0, enemies.Count);
        enemy = enemies[index];
        if (count != 0)
        {
            enemy.TryMove();
        }
        else
        {
            count ++;
        }


    }
    void MoveEnemy()
    {
        enemy.StartCoroutine(enemy.MoveEnemy());
        TurnDoneEnemy?.Invoke();
    }
}
