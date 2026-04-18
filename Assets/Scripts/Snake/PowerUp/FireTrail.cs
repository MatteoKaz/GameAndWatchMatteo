using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private AiManager2 aiManager;
    [SerializeField] private PlayerMovement2 playerMovement;
    [SerializeField] private PlayerEat2 playerEat;
    [SerializeField] private float fireDuration = 4f;

    // Coord -> GameObject du feu posé
    private Dictionary<Vector2Int, GameObject> activeFires = new();
    private bool isActive = false;

    private void OnEnable()
    {
        playerMovement.OnMove += OnPlayerMove;
    }

    private void OnDisable()
    {
        playerMovement.OnMove -= OnPlayerMove;
    }

 
    public void SetActive(bool active)
    {
        isActive = active;

        if (!active)
            ClearAllFires();
    }

  
    private void OnPlayerMove(Vector2Int coord)
    {
        if (!isActive) return;
        if (activeFires.ContainsKey(coord)) return; // feu déjà là

        // Spawner le feu sur la case
        Vector3 worldPos = gridManager.allCells[coord.x, coord.y].transform.position;
        GameObject fire = Instantiate(firePrefab, worldPos, Quaternion.identity);
        activeFires[coord] = fire;

        // Le détruire après fireDuration
        StartCoroutine(DestroyFireAfter(coord, fireDuration));
    }

    public bool IsBurning(Vector2Int coord) => activeFires.ContainsKey(coord);

  
    private IEnumerator DestroyFireAfter(Vector2Int coord, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveFire(coord);
    }

    private void RemoveFire(Vector2Int coord)
    {
        if (activeFires.TryGetValue(coord, out GameObject fire))
        {
            if (fire != null) Destroy(fire);
            activeFires.Remove(coord);
        }
    }

    public void ClearAllFires()
    {
        foreach (var fire in activeFires.Values)
            if (fire != null) Destroy(fire);
        activeFires.Clear();
    }
}