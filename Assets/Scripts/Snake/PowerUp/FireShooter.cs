


using UnityEngine;

public class FireShooter : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AiManager2 aiManager;
    [SerializeField] private PlayerMovement2 playerMovement;
    [SerializeField] private PlayerEat2 playerEat;
    [SerializeField] private float shootCooldown = 0.4f; // délai minimum entre deux tirs

    private float lastShootTime = -999f;

    private void OnEnable()
    {
        playerMovement.OnMove += OnPlayerMove;
    }

    private void OnDisable()
    {
        playerMovement.OnMove -= OnPlayerMove;
    }

    private void OnPlayerMove(Vector2Int tailCoord)
    {
        if (playerEat.state != PlayerEat2.PlayerState.Fire) return;

        // Cooldown entre les tirs
        if (Time.time - lastShootTime < shootCooldown) return;
        lastShootTime = Time.time;

        Vector2Int headCoord = playerMovement.coordPlayer;
        Vector3 spawnPos = gridManager.allCells[headCoord.x, headCoord.y].transform.position;

        GameObject go = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        FireBall fb = go.GetComponent<FireBall>();
        fb.Init(headCoord, playerMovement.currentDirection, gridManager, aiManager);
    }
}