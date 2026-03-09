using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustomSnake m_inputPlayerManager;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    public Vector2Int coordPlayer;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SnakeBody snakeBody;


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
   
    bool IsValidMove(Vector2Int target)
    {
        int dx = target.x - coordPlayer.x;
        int dy = target.y - coordPlayer.y;

        int adx = Mathf.Abs(dx);
        int ady = Mathf.Abs(dy);

        switch (currentMoveType)
        {
            case MoveType.Fou:
                return adx == ady && adx != 0;

            case MoveType.Roi:
                return adx <= 1 && ady <= 1 && (adx != 0 || ady != 0);

            case MoveType.Tour:
                return (dx == 0 && dy != 0) || (dx != 0 && dy == 0);

            case MoveType.Cavalier:
                return (adx == 2 && ady == 1) || (adx == 1 && ady == 2);

            case MoveType.Dame:
                return (adx == ady) || (dx == 0 && dy != 0) || (dx != 0 && dy == 0);
        }

        return false;
    }
    public void TryMove(Cell newCell)
    {
        if (IsValidMove(newCell.coord))
        {
            if (snakeBody.MoveFinish == true)
            {
                snakeBody.MoveFinish = false;
                coordPlayer = newCell.coord;
                transform.position = newCell.transform.position;
                snakeBody.StartCoroutine(snakeBody.MoveSnakeTo(newCell.coord));

                snakeBody.MoveSnakeTo(coordPlayer);
            }
            
        }
    }

    public void PlacePlayer()
    {
        int x = Random.Range(0, 7);
        int y = Random.Range(0, 7);
        coordPlayer = new Vector2Int(x, y);
        snakeBody.CreateSnake();
        snakeBody.StartCoroutine(snakeBody.MoveSnakeTo(coordPlayer));
        snakeBody.MoveSnakeTo(coordPlayer);
    }

    public void Update()
    {
        foreach (Cell cell in gridManager.allCells)
        {
            if (cell == null) continue;

            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();

            if (IsValidMove(cell.coord))
                sr.color = Color.yellow;
            else
                sr.color = cell.cellColor;
        }
    }

  

}
