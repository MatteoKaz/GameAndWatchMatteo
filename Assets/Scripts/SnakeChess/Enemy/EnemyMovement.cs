using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;


public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public GridManager gridManager;
    [SerializeField] public SnakeBody snakeBody;
    [SerializeField] public AIManger aim;
    [SerializeField] public Sprite sprite;
    [SerializeField] public PlayerEat pe;
    public Vector2Int coordEnemy;
    public Vector2Int CurrentcoordEnemy;
    public Vector3 futurmove;
    public int Value;

    [SerializeField] private float moveDuration = 0.3f;
    public event Action CutPlayer;

    public enum MoveType
    {
        Roi,
        Fou,
        Tour,
        Cavalier,
        Dame
    }

    public MoveType currentMoveType;

    private void OnEnable()
    {
        //aim.tm.pm.SpawnedSnake += PlaceEnemy;
    }

    private void OnDisable()
    {
        //aim.tm.pm.SpawnedSnake -= PlaceEnemy;
    }

    public void SetSpriteColor(Color color)
    {
       SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = color;   
    }

    public void SetValue()
    {
        switch (currentMoveType)
        {
            case MoveType.Roi:
                Value = 10;
                break;
            case MoveType.Cavalier:
                Value = 30; 
                break;
            case MoveType.Tour:
                Value = 50;
                break;
            case MoveType.Fou:
                Value = 30;
                break;
            case MoveType.Dame:
                Value = 90;
                break;
        }
    }

    public void PlaceEnemy()
    {
        Vector2Int spawnPos;
        int maxTries = 100;
        int tries = 0;
        SetValue();
        do
        {
            int x = UnityEngine.Random.Range(0, gridManager.width);
            int y = UnityEngine.Random.Range(0, gridManager.height);
            spawnPos = new Vector2Int(x, y);
            tries++;
        }
        while ((IsEnemy(spawnPos) || IsSnake(spawnPos)) && tries < maxTries);

        if (tries >= maxTries)
        {
            Debug.LogWarning("Impossible de trouver une case libre pour l'ennemi !");
            return;
        }

        coordEnemy = spawnPos;
        CurrentcoordEnemy = spawnPos;
        transform.position = gridManager.allCells[spawnPos.x, spawnPos.y].transform.position;

    }

    public IEnumerator MoveEnemy()
    {
        Vector3 startPos = transform.position;
        
        float t = 0f;

        Cell cellscript = gridManager.allCells[coordEnemy.x, coordEnemy.y];

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, futurmove, t);
            yield return null;
        }
        
        CurrentcoordEnemy = coordEnemy;
        Debug.Log("value " + CurrentcoordEnemy);
        transform.position = futurmove;
        pe.CutOrKillPlayer(CurrentcoordEnemy);
        cellscript.ColorCase(Color.white);
        SetSpriteColor(Color.white);
        aim.SetEndTurn();

    }

    public void TryMove()
    {
     
        List<Vector2Int> moves = GetPossibleMoves();

        if (moves.Count == 0)
            return;

        Vector2Int target = ChooseAction(moves);

        coordEnemy = target;
        futurmove = gridManager.allCells[target.x, target.y].transform.position;
        Cell cellscript = gridManager.allCells[target.x, target.y];
        cellscript.ColorCase(Color.red);
        
    }

    public List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> candidates = new List<Vector2Int>();

        // Générer toutes les positions possibles selon le type de pièce
        switch (currentMoveType)
        {
            case MoveType.Roi:
                AddKingMoves(candidates);
                break;
            case MoveType.Cavalier:
                AddKnightMoves(candidates);
                break;
            case MoveType.Tour:
                AddLineMoves(candidates, new Vector2Int[]
                {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
                });
                break;
            case MoveType.Fou:
                AddLineMoves(candidates, new Vector2Int[]
                {
                new Vector2Int(1,1), new Vector2Int(1,-1), new Vector2Int(-1,1), new Vector2Int(-1,-1)
                });
                break;
            case MoveType.Dame:
                AddLineMoves(candidates, new Vector2Int[]
                {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
                new Vector2Int(1,1), new Vector2Int(1,-1), new Vector2Int(-1,1), new Vector2Int(-1,-1)
                });
                break;
        }

        // Filtrer par coups légaux
        return candidates.Where(IsLegalMove).ToList();
    }

    bool IsLegalMove(Vector2Int target)
    {
        int dx = target.x - coordEnemy.x;
        int dy = target.y - coordEnemy.y;
        int adx = Mathf.Abs(dx);
        int ady = Mathf.Abs(dy);

        switch (currentMoveType)
        {
            case MoveType.Roi:
                return adx <= 1 && ady <= 1 && (adx != 0 || ady != 0);
            case MoveType.Tour:
                return ((dx == 0 && dy != 0) || (dx != 0 && dy == 0)) && !IsPathBlocked(target);
            case MoveType.Fou:
                return adx == ady && adx != 0 && !IsPathBlocked(target);
            case MoveType.Dame:
                return ((adx == ady) || (dx == 0 && dy != 0) || (dx != 0 && dy == 0)) && !IsPathBlocked(target);
            case MoveType.Cavalier:
                return (adx == 2 && ady == 1) || (adx == 1 && ady == 2);
        }

        return false;
    }

    void AddKingMoves(List<Vector2Int> moves)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector2Int pos = coordEnemy + new Vector2Int(x, y);
                if (!InsideGrid(pos)) continue;
                if (IsEnemy(pos)) continue;
                moves.Add(pos); // le roi peut attaquer serpent ou vide, on gère l’event plus tard
            }
        }
    }

    void AddKnightMoves(List<Vector2Int> moves)
    {
        Vector2Int[] offsets =
        {
            new Vector2Int(2,1), new Vector2Int(2,-1), new Vector2Int(-2,1), new Vector2Int(-2,-1),
            new Vector2Int(1,2), new Vector2Int(-1,2), new Vector2Int(1,-2), new Vector2Int(-1,-2)
        };

        foreach (var o in offsets)
        {
            Vector2Int pos = coordEnemy + o;
            if (!InsideGrid(pos)) continue;
            if (IsEnemy(pos)) continue;
            moves.Add(pos);
        }
    }

    void AddLineMoves(List<Vector2Int> moves, Vector2Int[] directions)
    {
        foreach (Vector2Int dir in directions)
        {
            Vector2Int pos = coordEnemy;
            while (true)
            {
                pos += dir;
                if (!InsideGrid(pos)) break;
                if (IsEnemy(pos)) break;
                if (IsSnake(pos))
                {
                    moves.Add(pos);
                   
                    //CutPlayer?.Invoke();// case serpent pour couper/attaquer
                    break;
                }
                moves.Add(pos);
            }
        }
    }

    bool IsPathBlocked(Vector2Int target)
    {
        Vector2Int dir = new Vector2Int(
            target.x == coordEnemy.x ? 0 : (target.x > coordEnemy.x ? 1 : -1),
            target.y == coordEnemy.y ? 0 : (target.y > coordEnemy.y ? 1 : -1)
        );

        Vector2Int pos = coordEnemy + dir;
        while (pos != target)
        {
            if (IsEnemy(pos)) return true;
            if (IsSnake(pos)) return true; // stop avant le serpent
            pos += dir;
        }

        return false;
    }

    bool InsideGrid(Vector2Int pos) => pos.x >= 0 && pos.y >= 0 && pos.x < gridManager.width && pos.y < gridManager.height;

    bool IsEnemy(Vector2Int pos) => aim.enemies.Any(e => e != this && e.coordEnemy == pos);

    bool IsSnake(Vector2Int pos) => snakeBody.snakeCoords.Contains(pos);

    // --- Choix du mouvement ---
    Vector2Int ChooseAction(List<Vector2Int> moves)
    {
        if (moves.Count == 0) return coordEnemy;

        int r = UnityEngine.Random.Range(0,10);
        switch (r)
        {
            case 0: return ApproachSnake(moves);
            case 1: return CutSnake(moves);
            case 2: return ApproachSnake(moves);
           
            case 3: return ApproachSnake(moves);
            case 4: return ApproachSnake(moves);
            default: return RandomMove(moves);
        }
    }


    Vector2Int ChaseHead(List<Vector2Int> moves)
    {
        Vector2Int head = snakeBody.snakeCoords[0];
        Vector2Int bestMove = moves[0];
        float bestDist = float.MaxValue;
        foreach (Vector2Int m in moves)
        {
            float d = Vector2Int.Distance(m, head);
            if (d < bestDist) { bestDist = d; bestMove = m; }
        }
        return bestMove;
    }

    Vector2Int CutSnake(List<Vector2Int> moves)
    {
        Vector2Int head = snakeBody.snakeCoords[0];
        Vector2Int target = snakeBody.snakeCoords[snakeBody.snakeCoords.Count / 2];

        Vector2Int bestMove = moves[0];
        float bestDist = float.MaxValue;

        foreach (Vector2Int m in moves)
        {
            if (m == head) continue;

            float d = Vector2Int.Distance(m, target);

            if (d < bestDist)
            {
                bestDist = d;
                bestMove = m;
            }
        }

        return bestMove;
    }

    Vector2Int ApproachSnake(List<Vector2Int> moves)
    {
        Vector2Int head = snakeBody.snakeCoords[0];

        Vector2Int bestMove = moves[0];
        float bestDist = float.MaxValue;

        foreach (Vector2Int m in moves)
        {
            if (m == head) continue;

            foreach (Vector2Int s in snakeBody.snakeCoords)
            {
                float d = Vector2Int.Distance(m, s);

                if (d < bestDist)
                {
                    bestDist = d;
                    bestMove = m;
                }
            }
        }

        return bestMove;
    }
    Vector2Int RandomMove(List<Vector2Int> moves)
    {
        Vector2Int head = snakeBody.snakeCoords[0];

        List<Vector2Int> safeMoves = new List<Vector2Int>();

        foreach (Vector2Int m in moves)
        {
            if (m != head) // évite la tête
            {
                safeMoves.Add(m);
            }
        }

        if (safeMoves.Count > 0)
            return safeMoves[UnityEngine.Random.Range(0, safeMoves.Count)];

        // fallback si aucun safe move
        return moves[UnityEngine.Random.Range(0, moves.Count)];
    }

}