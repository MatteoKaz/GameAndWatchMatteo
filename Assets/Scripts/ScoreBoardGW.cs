using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardGW : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowContainer;
    private List<GameObject> _spawnedRows = new List<GameObject>();

    

    public void Open(TopScores topScores)
    {
        ClearRows();

        for (int i = 0; i < topScores.entries.Count; i++)
        {
            ScoreEntry entry = topScores.entries[i];

            GameObject row = Instantiate(rowPrefab, rowContainer);
            _spawnedRows.Add(row);

            // Récupère le script de la row et l'initialise
            row.GetComponent<RowsScript>().Init(i + 1, entry.name, entry.score);
        }
    }

   public void OpenFunction()
    {
        Open(GameManager.Instance.saveData.topScoresGameAndWatch);
    }
    public void OpenFunctionSnake()
    {
        Open(GameManager.Instance.saveData.topScoresSnake);
    }
    public void Close()
    {
        ClearRows();
    }

    private void ClearRows()
    {
        foreach (GameObject row in _spawnedRows)
            Destroy(row);

        _spawnedRows.Clear();
    }
}
