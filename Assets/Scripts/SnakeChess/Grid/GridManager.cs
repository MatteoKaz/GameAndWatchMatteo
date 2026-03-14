using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    
    [SerializeField] public int width = 8;
    [SerializeField] public int height = 8;
    [SerializeField] float cellSize = 2f; // taille d’une case dans le monde
    [SerializeField] Vector3 startPosition = new Vector3(6.02f, 3f, 0f);
    [SerializeField] float spacing = 0.2f;  // espace supplémentaire entre cellules
    [SerializeField] Sprite White;
    [SerializeField] Sprite Black;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    Vector2Int[,] gridPositions;
    public Cell[,] allCells;
    public event Action FinishInitialize;
    public bool FinishInvoke = false;

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
       
    }

    IEnumerator GenerateGrid()
    {
        gridPositions = new Vector2Int[width, height];
        ClearGrid();

        Debug.Log("Generation");
        for (int y = 0; y < height; y++)
        {
                for (int x = 0; x < width; x++)
                {
                    Vector3 worldPos = startPosition + new Vector3(
                    x  * (cellSize + spacing),
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

                    yield return new WaitForSeconds(0.03f);

                }
        }
        yield return new WaitForSeconds(0.75f);
        FinishInvoke = true;
        FinishInitialize?.Invoke();
    }
    public void InvokeThings()
    {
        FinishInvoke = false;
        StartCoroutine(GenerateGrid());
    }


    public void ClearGrid()
    {
        // Détruire tous les GameObjects des cellules
        if (allCells != null)
        {
            for (int y = 0; y < allCells.GetLength(1); y++)
            {
                for (int x = 0; x < allCells.GetLength(0); x++)
                {
                    if (allCells[x, y] != null)
                    {
                        Destroy(allCells[x, y].gameObject);
                        allCells[x, y] = null;
                    }
                }
            }
        }

        // Vider le tableau de positions
        gridPositions = new Vector2Int[width, height];

        // Vider la liste de toutes les cellules si tu en as une
        
    }
}
