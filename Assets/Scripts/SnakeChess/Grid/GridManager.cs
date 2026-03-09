using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    
    [SerializeField] public int width = 8;
    [SerializeField] public int height = 8;
    [SerializeField] float cellSize = 2f; // taille d’une case dans le monde
    [SerializeField] Vector3 startPosition = new Vector3(2f, 3f, 0f);
    [SerializeField] float spacing = 0.2f;  // espace supplémentaire entre cellules
    [SerializeField] Sprite White;
    [SerializeField] Sprite Black;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    Vector2Int[,] gridPositions;
    public Cell[,] allCells;
    public event Action FinishInitialize;

    void Awake()
    {
        allCells = new Cell[width, height];
    }
    void Start()
    {
        StartCoroutine(GenerateGrid());
    }
   

    void CellClicked(Vector2Int coord)
    {
        // logique pour déplacer le serpent, vérifier validité, score, etc.
        Debug.Log("Cell clicked: " + coord);
    }

    IEnumerator GenerateGrid()
    {
        gridPositions = new Vector2Int[width, height];

        
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 worldPos = startPosition + new Vector3(
                    x * (cellSize + spacing),
                    y * (cellSize + spacing),
                    0
                );

                    GameObject cell = Instantiate(cellPrefab, worldPos, Quaternion.identity);
                    cell.name = $"Cell_{x}_{y}";

                    Cell cellscript = cell.GetComponent<Cell>();
                    cellscript.Init(x, y, () => CellClicked(cellscript.coord));

                    gridPositions[x, y] = new Vector2Int(x, y);
                    allCells[x, y] = cellscript;

                    SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite = ((x + y) % 2 == 0) ? Black : White;
                        cellscript.sprite = sr.sprite;
                    }

                    // délai entre chaque case
                    yield return new WaitForSeconds(0.03f);
                }
            }
        

        FinishInitialize?.Invoke();
    }
}
