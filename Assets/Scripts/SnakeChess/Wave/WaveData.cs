using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int count;
}

[System.Serializable]
public class WaveData
{
    public int waveNumber;
    public int waveValue;
    public int maxMoves;
    public List<EnemySpawnData> enemies;
    public int numBonuses;

    public int GetTotalEnemies()
    {
        int total = 0;
        foreach (var e in enemies)
            total += e.count;
        return total;
    }
}

