using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataWave", menuName = "Scriptable Objects/DataWave")]
public class DataWave : ScriptableObject


{
    public List<WaveData> waves;
}
