using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public int waveNumber;
    public int waveValue;         // valeur en points de la wave
    public int maxMoves;          // nombre de coups possible
    public int numEnemies;
    public List<GameObject> enemyTypes;  // prefabs d'ennemis
    public int numBonuses;
}

public class WaveManager : MonoBehaviour
{
    public int currentWave = 1;
    public WaveData currentWaveData;
    public SpawnerEnemy spawner;

    private void Start()
    {
        StartWave(currentWave);
    }

    public void StartWave(int wave)
    {
        currentWave = wave;
        currentWaveData = GenerateWaveData(wave);
        spawner.SpawnWave(currentWaveData);
    }

    private WaveData GenerateWaveData(int wave)
    {
        // Création automatique de la wave selon la progression
        WaveData data = new WaveData();
        data.waveNumber = wave;
        data.waveValue = 10 + wave * 5;        // exemple : augmente avec la wave
        data.maxMoves = 5 + wave;              // nombre de coups possibles
        data.numEnemies = 2 + wave / 2;        // plus d’ennemis progressivement
        data.numBonuses = 1 + wave / 3;        // bonus qui fait grossir
        data.enemyTypes = new List<GameObject>();
        // tu peux assigner ici des types selon la wave
        return data;
    }

    public void NextWave()
    {
        StartWave(currentWave + 1);  // passe à la wave suivante automatiquement
    }
}
