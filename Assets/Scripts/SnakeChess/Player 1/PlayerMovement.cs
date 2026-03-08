using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustomSnake m_inputPlayerManager;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    public Vector2Int coordPlayer;
    [SerializeField] private GridManager gridManager;

    public enum MoveType
    {
        Roi,      // déplacement d'une case dans toutes les directions
        Fou,      // diagonales
        Tour,     // lignes et colonnes
        Cavalier, // mouvement en L
        Dame      // toutes les directions
    }
    public MoveType currentMoveType;
    private void Start()
    {
       currentMoveType = MoveType.Fou;
    } 

    private void OnEnable()
    {
        m_inputPlayerManager.OnMove += TryMove;
        gridManager.FinishInitialize += PlacePlayer;
       
    }

    private void OnDisable()
    {
        m_inputPlayerManager.OnMove -= TryMove;
        gridManager.FinishInitialize -= PlacePlayer;
    }

    public void TryMove(Cell newCell)
    {
        Vector2Int CellCoord = newCell.coord;
        if (currentMoveType == MoveType.Fou)
        {
            Debug.Log("Le fou se déplace en diagonale");
            int dx = CellCoord.x - coordPlayer.x;
            int dy = CellCoord.y - coordPlayer.y;
            if (Mathf.Abs(dx) == Mathf.Abs(dy) && dx != 0)
            {
                coordPlayer = newCell.coord;
                transform.position = newCell.transform.position;
            }


        }
        if (currentMoveType == MoveType.Roi)
        {
            int dx = Mathf.Abs(CellCoord.x - coordPlayer.x);
            int dy = Mathf.Abs(CellCoord.y - coordPlayer.y);

            if (dx <= 1 && dy <= 1 && (dx != 0 || dy != 0))
            {
                coordPlayer = newCell.coord;
                transform.position = newCell.transform.position;
            }

        }
        if (currentMoveType == MoveType.Tour)
        {
            int dx = CellCoord.x - coordPlayer.x;
            int dy = CellCoord.y - coordPlayer.y;

            // La cellule est valide si elle est sur la même ligne ou la même colonne
            if ((dx == 0 && dy != 0) || (dx != 0 && dy == 0))
            {
                coordPlayer = newCell.coord;
                transform.position = newCell.transform.position;
            }
        }
        if (currentMoveType == MoveType.Cavalier)
        {
            int dx = Mathf.Abs(CellCoord.x - coordPlayer.x);
            int dy = Mathf.Abs(CellCoord.y - coordPlayer.y);

            // La cellule est valide si elle est sur la même ligne ou la même colonne
            if ((dx == 2 && dy == 1) || (dx == 1 && dy == 2))
            {
                coordPlayer = newCell.coord;
                transform.position = newCell.transform.position;
            }
        }
        if (currentMoveType == MoveType.Dame)
        {
            int dx = Mathf.Abs(CellCoord.x - coordPlayer.x);
            int dy = Mathf.Abs(CellCoord.y - coordPlayer.y);
            if (Mathf.Abs(dx) == Mathf.Abs(dy) || (dx == 0 && dy != 0) || (dx != 0 && dy == 0))
            {
                coordPlayer = newCell.coord;
                transform.position = newCell.transform.position;
            }
            Debug.Log("Le fou se déplace en diagonale");
        }
        //coordPlayer = newCell.coord;
        //transform.position = newCell.transform.position;
    }

    public void PlacePlayer()
    {
        int x = Random.Range(0, 7);
        int y = Random.Range(0, 7);
        coordPlayer = new Vector2Int(x, y);
        transform.position = gridManager.allCells[x, y].transform.position;
    }

    public void Update()
    {
        foreach (Cell cell in gridManager.allCells)
        {
            if (cell != null)
            {
                Vector2Int CellCoord = cell.coord;
                if (currentMoveType == MoveType.Fou)
                {
                    
                    int dx = CellCoord.x - coordPlayer.x;
                    int dy = CellCoord.y - coordPlayer.y;
                    if (Mathf.Abs(dx) == Mathf.Abs(dy) && dx != 0)
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        if (sr != null)
                            sr.color = Color.yellow;

                    }
                    else
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        sr.color = cell.cellColor;
                    }
                }

                if (currentMoveType == MoveType.Roi)
                {
                    int dx = Mathf.Abs(CellCoord.x - coordPlayer.x);
                    int dy = Mathf.Abs(CellCoord.y - coordPlayer.y);

                    if (dx <= 1 && dy <= 1 && (dx != 0 || dy != 0))
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        if (sr != null)
                            sr.color = Color.yellow;

                    }
                    else
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        sr.color = cell.cellColor;
                    }

                }
                if (currentMoveType == MoveType.Tour)
                {
                    int dx = CellCoord.x - coordPlayer.x;
                    int dy = CellCoord.y - coordPlayer.y;

                    // La cellule est valide si elle est sur la même ligne ou la même colonne
                    if ((dx == 0 && dy != 0) || (dx != 0 && dy == 0))
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        if (sr != null)
                            sr.color = Color.yellow;

                    }
                    else
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        sr.color = cell.cellColor;
                    }
                }
                if (currentMoveType == MoveType.Cavalier)
                {
                    int dx = Mathf.Abs(CellCoord.x - coordPlayer.x);
                    int dy = Mathf.Abs(CellCoord.y - coordPlayer.y);
                    if ((dx == 2 && dy == 1) ||  (dx == 1 && dy == 2))
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        if (sr != null)
                            sr.color = Color.yellow;
                   }
                    else
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        sr.color = cell.cellColor;
                    }


                }
                if (currentMoveType == MoveType.Dame)
                {
                    int dx = Mathf.Abs(CellCoord.x - coordPlayer.x);
                    int dy = Mathf.Abs(CellCoord.y - coordPlayer.y);
                    if (Mathf.Abs(dx) == Mathf.Abs(dy) || (dx == 0 && dy != 0) || (dx != 0 && dy == 0))
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        if (sr != null)
                            sr.color = Color.yellow;
                    }
                    else
                    {
                        SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                        sr.color = cell.cellColor;
                    }
                }

            }
        }
    }
  
  
}
