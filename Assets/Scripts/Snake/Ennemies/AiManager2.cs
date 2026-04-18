using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AiManager2 : MonoBehaviour
{
    [Header("Ennemis")]
    public List<Enemy2> enemies = new();

    [Header("Références")]
    [SerializeField] public GridManager gridManager;
    [SerializeField] public SnakeBody2 snakeBody;
    [SerializeField] public PlayerMovement2 playerMovement;
    [SerializeField] private WaveEnd waveEnd;
    [SerializeField] private EnemySpawner2 enemySpawner;
    [SerializeField] public FireTrail fireTrail;
    [SerializeField] public Score score;
    [Header("Timer")]
    [SerializeField] private float tickInterval = 3f; // secondes entre chaque coup

    private int currentEnemyIndex = 0;

    
    void Start()
    {
        // Si tu n'utilises pas l'event SpawnedSnake, tu peux appeler PlaceAndStart() directement ici
    }
    private void OnEnable()
    {
        enemySpawner.FinishSpawn += PlaceAndStart;
    }
    private void OnDisable()
    {
        enemySpawner.FinishSpawn -= PlaceAndStart;
    }
    // Appelle cette méthode quand le snake est prêt (depuis ton SpawnedSnake event, ou Start)
    public void PlaceAndStart()
    {
        StartCoroutine(PlaceEnemiesAndBegin());
    }

    IEnumerator PlaceEnemiesAndBegin()
    {
        // Attendre que la grille soit prête
        while (!gridManager.FinishInvoke)
            yield return null;

        foreach (var e in enemies)
            e.PlaceEnemy();

        //waveEnd.BeginWave();

        // Démarrer le timer
        InvokeRepeating(nameof(Tick), tickInterval, tickInterval);
    }


    void Tick()
    {
        if (enemies.Count == 0) return;

        int tries = 0;
        while (tries < enemies.Count)
        {
            Enemy2 e = enemies[currentEnemyIndex];
            currentEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
            tries++;

            if (!e.NoLegalMove() && e.isFrozen == false)
            {
                // Choix immédiat + coloration
                e.ChooseNextMove();

                e.SetSpriteColor(Color.red);
                
                // Mouvement après le délai
                StartCoroutine(DelayedMove(e));
                return;
            }
        }
        Debug.Log("Aucun ennemi ne peut bouger.");
    }

    IEnumerator DelayedMove(Enemy2 e)
    {
        yield return new WaitForSeconds(tickInterval);

        // Si l'ennemi a été détruit ou supprimé entre temps
        if (e == null || !enemies.Contains(e))
            yield break;
        if (e.isFrozen)
        {
            e.SetSpriteColor(Color.white);
            e.ClearChosenMove();
            yield break;
        }
        // Si le coup choisi n'est plus valide, on ne fait rien
        // (le prochain Tick refera un choix)
        if (!e.IsChosenMoveStillValid())
        {
            e.SetSpriteColor(Color.white);
            
            e.ClearChosenMove();
            yield break;
        }

        e.ExecuteMove();
        

        
      


    }

    public void ClearEnemies()
    {
        CancelInvoke(nameof(Tick));

        for (int i = enemies.Count - 1; i >= 0; i--)
            if (enemies[i] != null)
                Destroy(enemies[i].gameObject);

        enemies.Clear();
        currentEnemyIndex = 0;
    }

    public void RemoveEnemy(Enemy2 e)
    {
        enemies.Remove(e);
       
        if (currentEnemyIndex >= enemies.Count)
            currentEnemyIndex = 0;
    }
}
