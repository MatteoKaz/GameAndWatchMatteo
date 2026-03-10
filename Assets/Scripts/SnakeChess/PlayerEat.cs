using UnityEngine;

public class PlayerEat : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] public AIManger aim;
    [SerializeField] private PlayerMovement pm;

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

                Destroy(em.gameObject);
            }
        }
    }
}
