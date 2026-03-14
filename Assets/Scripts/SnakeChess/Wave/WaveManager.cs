using UnityEngine;
using System;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public DataWave dataWave;   // référence au ScriptableObject

    public int currentWave = 1;
    public WaveData currentWaveData;

    public SpawnerEnemy spawner;

    public event Action NewWave;
    [SerializeField] public AIManger aim;

    private void Start()
    {
        StartWave(currentWave);
    }

    public void StartWave(int wave)
    {
        
        if (dataWave == null)
        {
            Debug.LogError("DataWave non assigné");
            return;
        }

        if (wave <= 0 || wave > dataWave.waves.Count)
        {
            Debug.LogWarning("Wave inexistante : " + wave);
            return;
        }

        currentWave = wave;

        currentWaveData = dataWave.waves[wave - 1];

        NewWave?.Invoke();

        spawner.SpawnWave(currentWaveData);
    }

    public void NextWave()
    {
        Debug.Log("NouvelleWave");
        int nextWave = currentWave + 1;

        if (nextWave > dataWave.waves.Count)
        {
            Debug.Log("Plus de waves définies");
            return;
        }

        StartWave(nextWave);
    }
}