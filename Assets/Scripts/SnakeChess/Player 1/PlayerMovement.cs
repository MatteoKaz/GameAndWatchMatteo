using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustomSnake m_inputPlayerManager;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    public Vector2Int coordPlayer;
    [SerializeField] private GridManager gridManager;
    [SerializeField] public SnakeBody snakeBody;
    [SerializeField] public TurnManager tm;
    [SerializeField] public AIManger aim;
    public bool playerTurn = false;
    [SerializeField] public WaveManager wm;
    public int NumberOfMoves = 8;

    public event Action CheckCombo;
    public event Action TurnDone;
    public event Action SpawnedSnake;

    public enum MoveType
    {
        Roi,      // déplacement d'une case dans toutes les directions
        Fou,      // diagonales
        Tour,     // lignes et colonnes
        Cavalier, // mouvement en L
        Dame      // toutes les directions
    }
    public MoveType currentMoveType;
    private void Start()
    {
       currentMoveType = MoveType.Fou;
    } 

    private void OnEnable()
    {
        m_inputPlayerManager.OnMove += TryMove;
        gridManager.FinishInitialize += PlacePlayer;
        tm.playerTurn += TurnPlayer;
        wm.NewWave += SetNewWave;


    }

    public void Spawned()
    {
        SpawnedSnake?.Invoke();
    }


    public void SetNewWave()
    {
        NumberOfMoves = wm.currentWaveData.maxMoves;
        if (wm.currentWave != 1)
        {
            
        }
        
    }



    private void OnDisable()
    {
        m_inputPlayerManager.OnMove -= TryMove;
        gridManager.FinishInitialize -= PlacePlayer;
        tm.playerTurn -= TurnPlayer;
    }
   


    public void TurnPlayer()
    {
    
        playerTurn = true;
        Debug.Log("value " + playerTurn);
        ColorCell();
    }


    public void EndTurn()
    {
        TurnDone?.Invoke();
    }



    bool IsValidMove(Vector2Int target)
    {
        int dx = target.x - coordPlayer.x;
        int dy = target.y - coordPlayer.y;

        int adx = Mathf.Abs(dx);
        int ady = Mathf.Abs(dy);

        switch (currentMoveType)
        {
            case MoveType.Fou:
                return (adx == ady && adx != 0) && !IsPathBlocked(target);

            case MoveType.Roi:
                return adx <= 1 && ady <= 1 && (adx != 0 || ady != 0);

            case MoveType.Tour:
                 return ((dx == 0 && dy != 0) || (dx != 0 && dy == 0)) && !IsPathBlocked(target);

            case MoveType.Cavalier:
                return (adx == 2 && ady == 1) || (adx == 1 && ady == 2);

            case MoveType.Dame:
                return (
                    adx == ady ||
                    dx == 0 ||
                    dy == 0
                ) && !IsPathBlocked(target);
        }

        return false;
    }
    public void TryMove(Cell newCell)
    {
        if (playerTurn == true)
        {
            if (IsValidMove(newCell.coord))
            {
                bool occupied = snakeBody.snakeCoords.Contains(newCell.coord);

                if (snakeBody.MoveFinish == true && occupied == false && NumberOfMoves != 0)
                {
                    snakeBody.MoveFinish = false;
                    coordPlayer = newCell.coord;
                    transform.position = newCell.transform.position;
                    
                    playerTurn = false;
                    snakeBody.StartCoroutine(snakeBody.MoveSnakeTo(newCell.coord));
                    NumberOfMoves -= 1;
                    




                }

            }
        }
        
       
    }







    public void PlacePlayer()
    {
        int x = UnityEngine.Random.Range(0, 7);
        int y = UnityEngine.Random.Range(0, 7);
        coordPlayer = new Vector2Int(x, y);
        snakeBody.CreateSnake();
        snakeBody.StartCoroutine(snakeBody.FirstMoveSnakeTo(coordPlayer));
        
       
    }

    public void ColorCell()
    {
        foreach (Cell cell in gridManager.allCells)
        {
            bool occupied = snakeBody.snakeCoords.Contains(cell.coord);
            bool validMove = IsValidMove(cell.coord);
            if (cell == null) continue;

            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();

            if (IsValidMove(cell.coord))
                sr.color = Color.yellow;
            else
            {
                sr.sprite = cell.sprite;
                sr.color = Color.white;
            }
               

            if (occupied && validMove)
            {
               


                sr.color = Color.white;
            }
           
        }
    }

    public void UpdateGridHighlights()
    {
        // Parcours toutes les cellules de la grille
        foreach (Cell cell in gridManager.allCells)
        {
            // La case est interdite si :
            // - elle est déjà occupée par une partie du serpent
            // - ou le mouvement vers elle n'est pas valide
            bool occupied = snakeBody.snakeCoords.Contains(cell.coord);
            bool validMove = IsValidMove(cell.coord);

           

          
        }

                
           
        
    }
    public void SetGray(bool value)
    {

        foreach (Cell cell in gridManager.allCells)
        {
            if (cell == null) continue;

            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();


            sr.color = value ? Color.gray : sr.color = cell.cellColor;
        }
    }

    bool IsPathBlocked(Vector2Int target)
    {
        int dx = target.x - coordPlayer.x;
        int dy = target.y - coordPlayer.y;

        int stepX = dx == 0 ? 0 : dx / Mathf.Abs(dx);
        int stepY = dy == 0 ? 0 : dy / Mathf.Abs(dy);

        Vector2Int pos = coordPlayer + new Vector2Int(stepX, stepY);

        while (pos != target)
        {
            if (snakeBody.snakeCoords.Contains(pos))
                return true; // un segment bloque le chemin

            for (int i = aim.enemies.Count - 1; i >= 0; i--)
            {
                if (aim.enemies[i].CurrentcoordEnemy == pos)
                    return true;
            }
                pos += new Vector2Int(stepX, stepY);
        }

        return false;
    }



}
