using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    [SerializeField] public PlayerMovement pm;
    [SerializeField] public AIManger aim;
    [SerializeField] public GridManager gm;

    public event Action playerTurn;
    public event Action EnemyTurn;

    void OnEnable()
    {
        pm.TurnDone += SwitchTurnToEnemy;
        aim.TurnDoneEnemy += SwitchTurnToPlayer;
        gm.FinishInitialize += SwitchTurnToPlayer;

    }
    
    void OnDisable()
    {
        pm.TurnDone -= SwitchTurnToEnemy;
        aim.TurnDoneEnemy -= SwitchTurnToPlayer;
        gm.FinishInitialize -= SwitchTurnToPlayer;
    }

    public void SwitchTurnToPlayer()
    {
        playerTurn?.Invoke();
        Debug.Log("TurnPlayer");
    }

    public void SwitchTurnToEnemy()
    {
        EnemyTurn?.Invoke();
    }


}
