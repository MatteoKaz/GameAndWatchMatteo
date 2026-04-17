
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner2 : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AiManager2 aiManager;

    [Header("Prefabs des pièces")]
    [SerializeField] private List<GameObject> enemyPrefabs; // glisse tes prefabs ici

    [Header("Spawn initial")]
    [SerializeField] private int initialSpawnCount = 3;

    [Header("Spawn périodique")]
    [SerializeField] private float spawnInterval = 30f;

    [Header("Cap maximum d'ennemis")]
    [SerializeField] private int maxEnemies = 10;


    public event Action FinishSpawn;

    private void OnEnable()
    {
        gridManager.FinishInitialize += OnGridReady;
    }

    private void OnDisable()
    {
        gridManager.FinishInitialize -= OnGridReady;
    }

    
    private void OnGridReady()
    {
        // Spawn initial
        for (int i = 0; i < initialSpawnCount; i++)
            SpawnOneEnemy();

        FinishSpawn?.Invoke();
        // Démarre le timer périodique
        StartCoroutine(PeriodicSpawn());
    }

    
    private IEnumerator PeriodicSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (aiManager.enemies.Count < maxEnemies)
                SpawnOneEnemy();
            else
                Debug.Log("Cap d'ennemis atteint, pas de spawn.");
        }
    }

    
    private void SpawnOneEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("Aucun prefab d'ennemi assigné !");
            return;
        }

        // Choisir un prefab au hasard dans la liste
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        Enemy2 em = go.GetComponent<Enemy2>();
        if (em == null)
        {
            Debug.LogWarning("Le prefab n'a pas de composant EnemyMovement !");
            Destroy(go);
            return;
        }

        // Injecter les références
        em.gridManager = gridManager;
        em.aim = aiManager;
        em.snakeBody = aiManager.snakeBody;

        // Assigner un type de pièce aléatoire
        

        // Placer sur la grille + calculer la valeur
        em.PlaceEnemy();

        // Enregistrer dans AIManger
        aiManager.enemies.Add(em);

        Debug.Log($"Spawné : {em.currentMoveType} | Total ennemis : {aiManager.enemies.Count}");
    }

    
    public void ResetSpawner()
    {
        StopAllCoroutines();
        aiManager.ClearEnemies();
    }
}
