using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.MVVM;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] public GridManager gridManager;
    [SerializeField] public SnakeBody2 snakeBody;
    [SerializeField] public AiManager2 aim;
    private Vector2Int? chosenMove = null;
    public Vector2Int coordEnemy;
    public int Value;
    public bool isFrozen = false;
    private GameObject iceInstance;
    [SerializeField] private float moveDuration = 0.3f;
    public bool isBroken = false;
    // Branche cet event sur ton système de collision (couper le serpent, game over, etc.)
    public event Action<Vector2Int> OnHitSnake;
    public Color color = Color.white; 
    public enum MoveType { Roi, Fou, Tour, Cavalier, Dame }
    public MoveType currentMoveType;

    
    [Range(0f, 1f)]
    [SerializeField] private float chaseChance = 0.5f; // 0 = 100% random, 1 = 100% chase

    public void SetSpriteColor(Color color) =>
        GetComponent<SpriteRenderer>().color = color;


    public void SetBroken(bool broken)
    {
        isBroken = broken;

        // Visuel : teinte orange pour indiquer l'état brisé
        SetSpriteColor(broken ? new Color(1f, 0.5f, 0f) : Color.white);
        color = (broken ? new Color(1f, 0.5f, 0f) : Color.white);
    }
    public void SetValue()
    {
        Value = currentMoveType switch
        {
            MoveType.Roi => 10,
            MoveType.Cavalier => 30,
            MoveType.Tour => 50,
            MoveType.Fou => 30,
            MoveType.Dame => 90,
            _ => 0
        };
    }

   
    public void PlaceEnemy()
    {
        SetValue();
        Vector2Int spawnPos;
        int tries = 0;

        do
        {
            spawnPos = new Vector2Int(
                UnityEngine.Random.Range(0, gridManager.width),
                UnityEngine.Random.Range(0, gridManager.height));
            tries++;
        }
        while ((IsEnemy(spawnPos) || IsSnake(spawnPos)) && tries < 100);

        if (tries >= 100) { Debug.LogWarning("Pas de case libre pour l'ennemi !"); return; }

        coordEnemy = spawnPos;
        transform.position = gridManager.allCells[spawnPos.x, spawnPos.y].transform.position;
    }

   
    public void Tick()
    {
        if (isFrozen) return;
        if (NoLegalMove()) return;

        TryMove();
        StartCoroutine(MoveEnemy());
    }

    public bool NoLegalMove() => GetPossibleMoves().Count == 0;

    
    void TryMove()
    {
        if (isFrozen) return;
        List<Vector2Int> moves = GetPossibleMoves();
        if (moves.Count == 0) return;

        Vector2Int target = ChooseAction(moves);
        bool hitsSnake = IsSnake(target);

        coordEnemy = target;

        Cell cell = gridManager.allCells[target.x, target.y];
        cell.ColorCase(Color.red);

        // On retient si la destination est sur le serpent pour le callback
        if (hitsSnake)
            OnHitSnake?.Invoke(target);
    }

  
    public void ChooseNextMove()
    {
        List<Vector2Int> moves = GetPossibleMoves();
        if (moves.Count == 0) return;

        chosenMove = ChooseAction(moves);
        foreach (Cell cel in gridManager.allCells)
        {
            cel.ColorCase(Color.white);
        }
        // Case destination en rouge immédiatement
        Cell cell = gridManager.allCells[chosenMove.Value.x, chosenMove.Value.y];
        cell.ColorCase(Color.red);
    }

    // Vérifie que le coup est encore légal au moment d'exécuter
    public bool IsChosenMoveStillValid()
    {
        if (chosenMove == null) return false;
        return IsLegalMove(chosenMove.Value);
    }

    public void ClearChosenMove()
    {
        // Remettre la case en blanc si le coup est annulé
        if (chosenMove != null)
        {
            gridManager.allCells[chosenMove.Value.x, chosenMove.Value.y].ColorCase(Color.white);
            chosenMove = null;
        }
    }

    // Appelé après le délai si le coup est encore valide
    public void ExecuteMove()
    {
        
        if (chosenMove == null) return;

        bool hitsSnake = IsSnake(chosenMove.Value);
        coordEnemy = chosenMove.Value;
        chosenMove = null;

        if (hitsSnake)
            OnHitSnake?.Invoke(coordEnemy);

        StartCoroutine(MoveEnemy());
    }

    public IEnumerator MoveEnemy()
    {
        
        Vector3 startPos = transform.position;
        Vector3 endPos = gridManager.allCells[coordEnemy.x, coordEnemy.y].transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        if (snakeBody.snakeCoords.Contains(coordEnemy))
        {
            
            if (coordEnemy == snakeBody.snakeCoords[0])
            {
                if (snakeBody.playerMovement.Ghost == false)
                aim.playerMovement.GameOver();
            }
            else
            {
                 if (snakeBody.playerMovement.Ghost == false)
                    snakeBody.RemoveSegmentAt(coordEnemy);
            }
        }


        transform.position = endPos;
        gridManager.allCells[coordEnemy.x, coordEnemy.y].ColorCase(Color.white);
       
        SetSpriteColor(color);
        if (aim.fireTrail.IsBurning(coordEnemy))
        {
            yield return new WaitForSeconds(0.2f);
            aim.score.AddScore(Value);
            ClearChosenMove();
            aim.RemoveEnemy(this);
            Destroy(gameObject);
        }
        

    }
    public List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> candidates = new();

        switch (currentMoveType)
        {
            case MoveType.Roi: AddKingMoves(candidates); break;
            case MoveType.Cavalier: AddKnightMoves(candidates); break;
            case MoveType.Tour:
                AddLineMoves(candidates, new[] {
                    Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right });
                break;
            case MoveType.Fou:
                AddLineMoves(candidates, new[] {
                    new Vector2Int(1,1), new Vector2Int(1,-1),
                    new Vector2Int(-1,1), new Vector2Int(-1,-1) });
                break;
            case MoveType.Dame:
                AddLineMoves(candidates, new[] {
                    Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
                    new Vector2Int(1,1), new Vector2Int(1,-1),
                    new Vector2Int(-1,1), new Vector2Int(-1,-1) });
                break;
        }

        return candidates.Where(IsLegalMove).ToList();
    }

    bool IsLegalMove(Vector2Int target)
    {
        int dx = target.x - coordEnemy.x, dy = target.y - coordEnemy.y;
        int adx = Mathf.Abs(dx), ady = Mathf.Abs(dy);

        return currentMoveType switch
        {
            MoveType.Roi => adx <= 1 && ady <= 1 && (adx != 0 || ady != 0),
            MoveType.Tour => ((dx == 0 && dy != 0) || (dx != 0 && dy == 0)) && !IsPathBlocked(target),
            MoveType.Fou => adx == ady && adx != 0 && !IsPathBlocked(target),
            MoveType.Dame => ((adx == ady) || (dx == 0 && dy != 0) || (dx != 0 && dy == 0)) && !IsPathBlocked(target),
            MoveType.Cavalier => (adx == 2 && ady == 1) || (adx == 1 && ady == 2),
            _ => false
        };
    }

    void AddKingMoves(List<Vector2Int> moves)
    {
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector2Int pos = coordEnemy + new Vector2Int(x, y);
                if (InsideGrid(pos) && !IsEnemy(pos)) moves.Add(pos);
            }
    }

    void AddKnightMoves(List<Vector2Int> moves)
    {
        foreach (var o in new Vector2Int[] {
            new(2,1), new(2,-1), new(-2,1), new(-2,-1),
            new(1,2), new(-1,2), new(1,-2), new(-1,-2) })
        {
            Vector2Int pos = coordEnemy + o;
            if (InsideGrid(pos) && !IsEnemy(pos)) moves.Add(pos);
        }
    }

    void AddLineMoves(List<Vector2Int> moves, Vector2Int[] directions)
    {
        foreach (var dir in directions)
        {
            Vector2Int pos = coordEnemy;
            while (true)
            {
                pos += dir;
                if (!InsideGrid(pos)) break;
                if (IsEnemy(pos)) break;
                moves.Add(pos);
                if (IsSnake(pos)) break; // peut capturer mais pas traverser
            }
        }
    }

    bool IsPathBlocked(Vector2Int target)
    {
        Vector2Int dir = new(
            target.x == coordEnemy.x ? 0 : (target.x > coordEnemy.x ? 1 : -1),
            target.y == coordEnemy.y ? 0 : (target.y > coordEnemy.y ? 1 : -1));

        Vector2Int pos = coordEnemy + dir;
        while (pos != target)
        {
            if (IsEnemy(pos)) return true;
            pos += dir;
        }
        return false;
    }

    
    Vector2Int ChooseAction(List<Vector2Int> moves)
    {
        return UnityEngine.Random.value < chaseChance
            ? ApproachSnake(moves)
            : RandomMove(moves);
    }

    Vector2Int ApproachSnake(List<Vector2Int> moves)
    {
        Vector2Int best = moves[0];
        float bestDist = float.MaxValue;

        foreach (var m in moves)
            foreach (var s in snakeBody.snakeCoords)
            {
                float d = Vector2Int.Distance(m, s);
                if (d < bestDist) { bestDist = d; best = m; }
            }

        return best;
    }
    public void Freeze(GameObject icePrefab)
    {
        isFrozen = true;

        // Spawner le prefab de glace sur l'ennemi
        iceInstance = Instantiate(icePrefab, transform.position, Quaternion.identity);
        iceInstance.transform.SetParent(transform); // suit l'ennemi
    }

    public void Unfreeze()
    {
        isFrozen = false;

        if (iceInstance != null)
        {
            Destroy(iceInstance);
            iceInstance = null;
        }
    }

    Vector2Int RandomMove(List<Vector2Int> moves) =>
        moves[UnityEngine.Random.Range(0, moves.Count)];

   
    bool InsideGrid(Vector2Int p) =>
        p.x >= 0 && p.y >= 0 && p.x < gridManager.width && p.y < gridManager.height;

    bool IsEnemy(Vector2Int p) => aim.enemies.Any(e => e != this && e.coordEnemy == p);
    bool IsSnake(Vector2Int p) => snakeBody.snakeCoords.Contains(p);
}