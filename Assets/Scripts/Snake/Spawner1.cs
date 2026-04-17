using System.Collections.Generic;
using UnityEngine;

public class Spawner1 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SnakeTimeManger timeManager;
    [SerializeField] private SnakeBody2 snake;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private List<GameObject> BonusPrefab;

    [Header("Rules")]
    [SerializeField] private int dangerRadius = 2;

    private Dictionary<Vector2Int, GameObject> spawnedObjects = new Dictionary<Vector2Int, GameObject>();

    void OnEnable()
    {
        timeManager.OnTick += TrySpawn;
        timeManager.OnBonusTick += TrySpawnBonus;
    }

    void OnDisable()
    {
        timeManager.OnTick -= TrySpawn;
        timeManager.OnBonusTick -= TrySpawnBonus;
    }

    void TrySpawn()
    {
        if (gridManager == null || snake == null) return;

        Cell best = GetBestCell();
        if (best == null) return;

        Vector2Int coord = best.coord;

        GameObject obj = Instantiate(prefabToSpawn, best.transform.position, Quaternion.identity);

        RegisterSpawn(coord, obj);
    }
    void TrySpawnBonus()
    {
        if (gridManager == null || snake == null) return;

        Cell best = GetBestCell();
        if (best == null) return;

        Vector2Int coord = best.coord;
        int i = Random.Range(0,3);  
        GameObject obj = Instantiate(BonusPrefab[i], best.transform.position, Quaternion.identity);

        RegisterSpawn(coord, obj);
    }

    Cell GetBestCell()
    {
        Cell[,] grid = gridManager.allCells;

        Cell best = null;
        float bestScore = float.MinValue;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Cell cell = grid[x, y];
                if (cell == null) continue;

                if (!IsFree(cell)) continue;

                float score = Evaluate(cell);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = cell;
                }
            }
        }

        return best;
    }

    bool IsFree(Cell cell)
    {
        Vector2Int c = cell.coord;

        if (spawnedObjects.ContainsKey(c))
            return false;

        return true;
    }

    float Evaluate(Cell cell)
    {
        float score = 0f;

        float dist = GetMinSnakeDistance(cell.coord);
        score += dist * 10f;

        if (dist <= dangerRadius)
            score -= 1000f;

        score -= GetSnakeDensityPenalty(cell.coord) * 200f;

        return score;
    }

    float GetMinSnakeDistance(Vector2Int pos)
    {
        float min = float.MaxValue;

        foreach (var c in snake.snakeCoords)
        {
            float d = Vector2Int.Distance(pos, c);
            if (d < min) min = d;
        }

        return min;
    }

    float GetSnakeDensityPenalty(Vector2Int pos)
    {
        int count = 0;

        foreach (var c in snake.snakeCoords)
        {
            if (Vector2Int.Distance(pos, c) <= 1.5f)
                count++;
        }

        return count;
    }

    // ---------------- MEMORY ----------------

    public void RegisterSpawn(Vector2Int coord, GameObject obj)
    {
        spawnedObjects[coord] = obj;
    }

    public GameObject GetObjectAt(Vector2Int coord)
    {
        if (spawnedObjects.TryGetValue(coord, out GameObject obj))
            return obj;

        return null;
    }

    public void RemoveSpawn(Vector2Int coord)
    {
        if (spawnedObjects.ContainsKey(coord))
        {
            Destroy(spawnedObjects[coord]);
            spawnedObjects.Remove(coord);
        }
    }
}