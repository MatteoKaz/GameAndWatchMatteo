using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static PlayerEat2;

public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustomSnake m_inputPlayerManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] public SnakeBody2 snakeBody;
    [SerializeField] private AiManager2 aiManager;
    [SerializeField] private PlayerEat2 pe;
    public Vector2Int coordPlayer;

    public Vector2Int currentDirection = Vector2Int.right;
    private Vector2Int nextDirection = Vector2Int.right;

    private bool hasBufferedInput = false;

    [SerializeField] private float moveInterval = 0.2f;
    private float timer = 0f;
    public bool isPlaying = false;
    public bool Ghost = false;
    public event Action<Vector2Int> OnMove;
   
    private void OnEnable()
    {
        gridManager.FinishInitialize += PlacePlayer;
        gridManager.FinishInitialize += StartGame;
        m_inputPlayerManager.OnMoveUp += () => SetDirection(Vector2Int.up);
        m_inputPlayerManager.OnMoveDown += () => SetDirection(Vector2Int.down);
        m_inputPlayerManager.OnMoveLeft += () => SetDirection(Vector2Int.left);
        m_inputPlayerManager.OnMoveRight += () => SetDirection(Vector2Int.right);
    }

    private void OnDisable()
    {
        gridManager.FinishInitialize -= PlacePlayer;
    }

    private void Update()
    {
        
    }

    private IEnumerator GameLoop()
    {
        while (isPlaying)
        {
            currentDirection = nextDirection;
            hasBufferedInput = false;

            Vector2Int targetCoord = WrapPosition(coordPlayer + currentDirection);
            Cell targetCell = gridManager.GetCell(targetCoord);
            Vector2Int tailCoord = snakeBody.snakeCoords[snakeBody.snakeCoords.Count - 1];
            if (targetCell == null) { GameOver(); yield break; }
            if(snakeBody.snakeCoords.Count > 1 &&  
            snakeBody.snakeCoords
                .Take(snakeBody.snakeCoords.Count - 1)
            .Contains(targetCoord))
            {
                if(Ghost == false)
                {
                    // D'abord on bouge VERS la case fatale
                    coordPlayer = targetCoord;
                    snakeBody.StartCoroutine(snakeBody.MoveSnakeTo(targetCoord));

                    // On attend la FIN COMPLÈTE de l'animation
                    yield return new WaitForSeconds(snakeBody.moveDuration * 0.95f);
                    yield return new WaitUntil(() => snakeBody.MoveFinish);

                    GameOver();
                    yield break;
                }
                
            }
            coordPlayer = targetCoord;
            OnMove?.Invoke(tailCoord);
            // Lance l'animation sans l'attendre complètement
            snakeBody.StartCoroutine(snakeBody.MoveSnakeTo(targetCoord));
           
                
            // Attend 95% de la durée relance avant la fin de l'animation
            yield return new WaitForSeconds(snakeBody.moveDuration * 0.95f);
           
            // Attend que MoveFinish soit true (les 5% restants + fin propre)
            yield return new WaitUntil(() => snakeBody.MoveFinish);
           
        }
    }

    private Vector2Int WrapPosition(Vector2Int pos)
    {
        pos.x = (pos.x + gridManager.width) % gridManager.width;
        pos.y = (pos.y + gridManager.height) % gridManager.height;
        return pos;
    }
    private void SetDirection(Vector2Int dir)
    {
        if (dir == -currentDirection) return;

        if (!hasBufferedInput)
        {
            nextDirection = dir;
            hasBufferedInput = true;
        }
    }

   

    public void StartGame()
    {
        isPlaying = true;
        StartCoroutine(GameLoop());
       
    }

    public void GameOver()
    {
        isPlaying = false;
        Debug.Log("Game Over");
    }

    public void PlacePlayer()
    {
        Vector2Int center = new Vector2Int(
            gridManager.width / 2,
            gridManager.height / 2
        );

        coordPlayer = center;

        snakeBody.CreateSnake();

        snakeBody.StartCoroutine(
            snakeBody.FirstMoveSnakeTo(coordPlayer)
        );
    }

    private Vector3 GetWrappedDelta(Vector3 from, Vector3 to)
{
    Vector3 delta = to - from;

    float width = gridManager.width * gridManager.cellSize;
    float height = gridManager.height * gridManager.cellSize;

    if (delta.x > width / 2f) delta.x -= width;
    else if (delta.x < -width / 2f) delta.x += width;

    if (delta.y > height / 2f) delta.y -= height;
    else if (delta.y < -height / 2f) delta.y += height;

    return delta;
}
}