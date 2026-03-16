using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AIManger : MonoBehaviour
{
    public List<EnemyMovement> enemies = new List<EnemyMovement>();
    [SerializeField] public GridManager gridManager;
    [SerializeField] public SnakeBody snakeBody;
    [SerializeField] public PlayerEat pe;
    [SerializeField] public TurnManager tm;
    public event Action TurnDoneEnemy;
    public EnemyMovement enemy;
    private int count = 0 ;
    private bool TurnEnemy= false;
    [SerializeField] public GridManager gm;
    [SerializeField] private WaveEnd waveEnd;
    [SerializeField] public PlayerMovement playerMovement;
    private bool chosenEnemyDead = false;
    private bool enemyIsMoving = false;
    private bool beginWave = false;
    void OnEnable()
    {
        tm.EnemyTurn +=MoveEnemy;
        tm.playerTurn += LaunchEnemy;
        gm.FinishInitialize += InitializeStart;
        tm.pm.SpawnedSnake += PlaceEnemyAim;

    }


    public void PlaceEnemyAim()
    {
        StartCoroutine(PlaceEnemyAimCoroutine());

    }
    void LaunchEnemy()
    {
        if (TurnEnemy ==true)
        {

            TurnEnemy = false;
            if (enemies.Count == 0) 
                return;
            EnemyMovement chosen = null;
            // Mélanger la liste pour éviter les boucles infinies
            List<EnemyMovement> shuffled = enemies.OrderBy(x => UnityEngine.Random.value).ToList();



            foreach (var e in shuffled)
            {
                if (!e.NoLegalMove()) // si e peut bouger
                {
                    chosen = e;
                    break;
                }
            }
            enemy = chosen;
            if (enemy == null)
            {
                Debug.Log("Aucun ennemi ne peut bouger !");
                return;
            }

            if (chosenEnemyDead == false)
                //enemy.ColorCase();




            enemy.SetSpriteColor(Color.red);

            if (count != 0)
            {
                //enemy.TryMove();
            }
            else
            {
                count++;
            }


        }




    }
    public IEnumerator PlaceEnemyAimCoroutine()
    {
        if (enemies.Count == 0)
            yield break; // quitte la coroutine si pas d'ennemis

        // Attendre que la grille soit complètement générée
        while (gridManager.FinishInvoke == false)
        {
            yield return null; // attend la frame suivante
        }

        // Placer les ennemis
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].PlaceEnemy();
        }

        // Lancer la wave
        waveEnd.BeginWave();
    }

    public IEnumerator Wait()
    {

        
        yield return new WaitForSeconds(0.5f);
        if (enemy != null)
        {
            enemy.TryMove();
            enemy.StartCoroutine(enemy.MoveEnemy());
            chosenEnemyDead = false;
        }
        else
        {
            if (beginWave == false)
            {
                chosenEnemyDead = true;
            }
            

            LaunchEnemy();
            chosenEnemyDead = false;
            yield return new WaitForSeconds(0.5f);
            if (enemy != null)
            {
                TurnEnemy = true;
                enemy.TryMove();
                enemy.StartCoroutine(enemy.MoveEnemy());
                chosenEnemyDead = false;
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
        beginWave = true;
        LaunchEnemy();
        beginWave = false;


    }

    public void ClearEnemies()
    {
        // Vérifie si la liste existe et contient des ennemis
        if (enemies == null || enemies.Count == 0)
            return;

        // Parcours tous les ennemis et détruit leur GameObject
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i].gameObject);
            }
        }

        // Vide la liste pour supprimer toutes les références
        enemies.Clear();
    }

   
}
