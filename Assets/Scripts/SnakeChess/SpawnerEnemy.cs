using UnityEngine;
using System.Collections.Generic;

public class SpawnerEnemy : MonoBehaviour
{
    public Transform enemyParent; // parent pour organiser les ennemis
    public GridManager gridManager;
    [SerializeField] private AIManger aim;

    public void SpawnWave(WaveData waveData)
    {
        ClearEnemies(); // supprimer les anciens ennemis si nécessaire

        for (int i = 0; i < waveData.numEnemies; i++)
        {
            // Choisir un prefab aléatoire parmi les types disponibles
            GameObject prefab = waveData.enemyTypes[Random.Range(0, waveData.enemyTypes.Count)];

    

            // Instancier l’ennemi
            GameObject enemyObj = Instantiate(prefab, enemyParent);

            // Ajouter la référence à la liste des ennemis gérée dans ton système
            
            EnemyMovement em = enemyObj.GetComponent<EnemyMovement>();
            aim.enemies.Add(em);
            em.gridManager = aim.gridManager;
            em.snakeBody = aim.snakeBody;
            em.aim = aim;
            em.pe = aim.pe;


            // tu peux stocker les ennemis dans une liste globale si besoin
            
        }

        // bonus à placer de la même façon si nécessaire
    }

    private void ClearEnemies()
    {
        // détruit tous les ennemis précédents
        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }
    }
}