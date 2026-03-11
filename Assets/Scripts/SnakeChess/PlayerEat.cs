using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEat : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] public AIManger aim;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] public PlayerScoreSnake playerscore;
    [SerializeField] private SnakeBody snakeBody;

    public event Action Eat;
    void OnEnable()
    {

    }

    public void PlayerKill()
    {
        for (int i = aim.enemies.Count - 1; i >= 0; i--)
        {
            if (aim.enemies[i].CurrentcoordEnemy == pm.coordPlayer)
            {
                pm.currentMoveType = (PlayerMovement.MoveType)aim.enemies[i].currentMoveType;

                // Launch effect et point ici

                EnemyMovement em = aim.enemies[i];
                aim.enemies.RemoveAt(i);

                playerscore.AddPoint(em.Value);
                Eat?.Invoke();
                Destroy(em.gameObject);
            }
        }
    }

    public void CutOrKillPlayer(Vector2Int pos)
    {
        for (int i = 0; i < snakeBody.snakeCoords.Count; i++)
            if(pos == snakeBody.snakeCoords[i])
            {
                snakeBody.RemoveSegmentAt(pos);
            }

       
    }

}
