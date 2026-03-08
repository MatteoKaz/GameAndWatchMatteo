using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject cellPrefab;
    
    [SerializeField] int width = 8;
    [SerializeField] int height = 8;
    [SerializeField] float cellSize = 2f; // taille d’une case dans le monde
    [SerializeField] Vector3 startPosition = new Vector3(2f, 3f, 0f);
    [SerializeField] float spacing = 0.2f;  // espace supplémentaire entre cellules

    Vector2Int[,] gridPositions;
    public Cell[,] allCells;
    public event Action FinishInitialize;

    void Awake()
    {
        allCells = new Cell[width, height];
    }
    void Start()
    {
        gridPositions = new Vector2Int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
                    sr.color = ((x + y) % 2 == 0) ? Color.white : Color.black;
                    cellscript.cellColor = sr.color;

                }





            }
        }
        FinishInitialize?.Invoke();
    }
   

    void CellClicked(Vector2Int coord)
    {
        // logique pour déplacer le serpent, vérifier validité, score, etc.
        Debug.Log("Cell clicked: " + coord);
    }

   
}
