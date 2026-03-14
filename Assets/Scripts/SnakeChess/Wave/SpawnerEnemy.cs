using UnityEngine;
using System.Collections.Generic;

public class SpawnerEnemy : MonoBehaviour
{
    public Transform enemyParent;
    [SerializeField] public GridManager gridManager;
    [SerializeField] public AIManger aim;

    public void SpawnWave(WaveData waveData)
    {
        ClearEnemies();

        foreach (EnemySpawnData enemyData in waveData.enemies)
        {
            for (int i = 0; i < enemyData.count; i++)
            {
                GameObject enemyObj = Instantiate(enemyData.enemyPrefab, enemyParent);

                EnemyMovement em = enemyObj.GetComponent<EnemyMovement>();

                // transfert des références (inchangé)
                aim.enemies.Add(em);
                em.aim = aim;
                em.gridManager = gridManager;
                em.snakeBody = aim.snakeBody;
               
                em.pe = aim.pe;
            }
        }
        //aim.PlaceEnemyAim();

        // bonus possible ici avec waveData.numBonuses
    }

    private void ClearEnemies()
    {
        aim.enemies.Clear();
        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }

        // nettoyer la liste d'ennemis dans l'AI
        
    }
}