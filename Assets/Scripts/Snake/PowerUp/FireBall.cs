

using System.Collections;
using System.Drawing;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Vector2Int currentCoord;
    private Vector2Int direction;
    private GridManager gridManager;
    private AiManager2 aiManager;
    [SerializeField] GameObject point;
    [SerializeField] private float travelDuration = 0.06f; // durée du lerp entre deux cases

    public void Init(Vector2Int startCoord, Vector2Int dir, GridManager gm, AiManager2 aim)
    {
        currentCoord = startCoord;
        direction = dir;
        gridManager = gm;
        aiManager = aim;
        StartCoroutine(Travel());
    }

    private IEnumerator Travel()
    {
        while (true)
        {
            Vector2Int next = currentCoord + direction;

            if (!InsideGrid(next))
            {
                Destroy(gameObject);
                yield break;
            }

            // Lerp visuel vers la case suivante
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.allCells[next.x, next.y].transform.position;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / travelDuration;
                transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(t));
                yield return null;
            }

            transform.position = endPos;
            currentCoord = next;

            // Vérifier collision après le lerp
            Enemy2 hit = GetEnemyAt(next);
            if (hit != null)
            {
                if (hit.isBroken)
                {
                    point.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{hit.Value}";
                    Instantiate(point, hit.transform.position, Quaternion.identity);
                    point.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{hit.Value}";
                    hit.ClearChosenMove();
                    aiManager.RemoveEnemy(hit);
                    Destroy(hit.gameObject);
                }
                else
                {
                    hit.SetBroken(true);
                }

                Destroy(gameObject);
                yield break;
            }
        }
    }

    private Enemy2 GetEnemyAt(Vector2Int coord)
    {
        foreach (Enemy2 e in aiManager.enemies)
            if (e != null && e.coordEnemy == coord)
                return e;
        return null;
    }

    private bool InsideGrid(Vector2Int p) =>
        p.x >= 0 && p.y >= 0 && p.x < gridManager.width && p.y < gridManager.height;
}