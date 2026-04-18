using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.InferenceEngine.Tokenization.PostProcessors.Templating;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class SnakeBody2 : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private GameObject HeadsegmentPrefab;
    [SerializeField] private Sprite headSprite;
    [SerializeField] private Sprite bodySprite;
    [SerializeField] private Sprite tailSprite;
    [SerializeField] public PlayerMovement2 playerMovement;
    [SerializeField] private PlayerEat2 pe;

    public bool MoveFinish = true;

    public event Action GrownUp;
    public event Action GrownDown;

    public List<Vector2Int> snakeCoords = new List<Vector2Int>();
    public List<GameObject> segments = new List<GameObject>();

    public int startSize = 3;
    public float moveDuration = 0.19f; // Légèrement < moveInterval (0.2f) pour éviter tout gap
    public int GrowValue = 1;
    public int sizemax = 5;

    public void DestroySnake()
    {
        snakeCoords.Clear();
        foreach (var seg in segments)
            Destroy(seg);
        segments.Clear();
    }

    public void CreateSnake()
    {
        Vector2Int start = playerMovement.coordPlayer;
        start.x = Mathf.Clamp(start.x, startSize - 1, gridManager.width - 1);
        start.y = Mathf.Clamp(start.y, 0, gridManager.height - 1);

        snakeCoords.Clear();
        foreach (var seg in segments) Destroy(seg);
        segments.Clear();

        for (int i = 0; i < startSize; i++)
        {
            Vector2Int pos = start + new Vector2Int(-i, 0);
            pos.x = Mathf.Clamp(pos.x, 0, gridManager.width - 1);

            GameObject seg = Instantiate(i == 0 ? HeadsegmentPrefab : segmentPrefab);
            seg.transform.position = gridManager.allCells[pos.x, pos.y].transform.position;

            SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
            if (i == 0) sr.sprite = headSprite;
            else if (i == startSize - 1) sr.sprite = tailSprite;
            else sr.sprite = bodySprite;

            snakeCoords.Add(pos);
            segments.Add(seg);
        }

        GrownUp?.Invoke();
        UpdateRotations();
    }

    public IEnumerator MoveSnakeTo(Vector2Int target)
    {
        MoveFinish = false;
        target = gridManager.Wrap(target);

        // Capturer la taille au début — elle ne doit pas changer pendant l'animation
        int count = segments.Count;

        List<Vector2Int> newCoords = new List<Vector2Int> { target };
        for (int i = 0; i < count - 1; i++)
            newCoords.Add(snakeCoords[i]);

        Vector3[] startPositions = new Vector3[count];
        Vector3[] endPositions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            Vector2Int from = snakeCoords[i];
            Vector2Int to = newCoords[i];

            from.x = Mathf.Clamp(from.x, 0, gridManager.width - 1);
            from.y = Mathf.Clamp(from.y, 0, gridManager.height - 1);
            to.x = Mathf.Clamp(to.x, 0, gridManager.width - 1);
            to.y = Mathf.Clamp(to.y, 0, gridManager.height - 1);

            startPositions[i] = gridManager.GetWorldPosWrapped(from, from);
            endPositions[i] = gridManager.GetWorldPosWrapped(from, to);
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            t = Mathf.Min(t, 1f);

            // Utiliser count capturé + vérifier que segments est toujours valide
            int safeCount = Mathf.Min(count, segments.Count);
            for (int i = 0; i < safeCount; i++)
            {
                if (segments[i] != null)
                    segments[i].transform.position =
                        GetWrappedLerp(startPositions[i], endPositions[i], t);
            }

            yield return null;
        }

        snakeCoords = newCoords;
        UpdateRotations();
        MoveFinish = true;
        pe.CheckEat();
    }
    private Vector3 GetWrappedLerp(Vector3 from, Vector3 to, float t)
    {
        float width = gridManager.width * gridManager.cellSize;
        float height = gridManager.height * gridManager.cellSize;

        Vector3 delta = to - from;

        if (delta.x > width / 2f) delta.x -= width;
        else if (delta.x < -width / 2f) delta.x += width;

        if (delta.y > height / 2f) delta.y -= height;
        else if (delta.y < -height / 2f) delta.y += height;

        return from + delta * t;
    }
    public IEnumerator FirstMoveSnakeTo(Vector2Int target)
    {
        MoveFinish = false;

        List<Vector2Int> path = GetLinearPath(snakeCoords[0], target);

        foreach (Vector2Int next in path)
        {
            List<Vector2Int> newCoords = new List<Vector2Int> { next };
            for (int i = 0; i < segments.Count - 1; i++)
                newCoords.Add(snakeCoords[i]);

            Vector3[] startPositions = new Vector3[segments.Count];
            Vector3[] endPositions = new Vector3[segments.Count];

            for (int i = 0; i < segments.Count; i++)
            {
                startPositions[i] = segments[i].transform.position;
                Vector2Int coord = newCoords[i];
                coord.x = Mathf.Clamp(coord.x, 0, gridManager.width - 1);
                coord.y = Mathf.Clamp(coord.y, 0, gridManager.height - 1);
                endPositions[i] = gridManager.allCells[coord.x, coord.y].transform.position;
            }

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / moveDuration;
                t = Mathf.Min(t, 1f);
                for (int i = 0; i < segments.Count; i++)
                    segments[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], t);
                yield return null;
            }

            snakeCoords = newCoords;
        }

        MoveFinish = true;
    }

    public void Grow()
    {
        for (int i = 0; i < GrowValue; i++)
        {
            if (segments.Count >= sizemax) break;

            Vector2Int tail = snakeCoords[snakeCoords.Count - 1];
            Vector2Int dir = Vector2Int.zero;

            if (snakeCoords.Count > 1)
                dir = tail - snakeCoords[snakeCoords.Count - 2];

            Vector2Int newTailPos = tail + dir;
            newTailPos.x = Mathf.Clamp(newTailPos.x, 0, gridManager.width - 1);
            newTailPos.y = Mathf.Clamp(newTailPos.y, 0, gridManager.height - 1);

            if (snakeCoords.Contains(newTailPos))
            {
                Vector2Int[] checks = { tail + Vector2Int.up, tail + Vector2Int.down,
                                        tail + Vector2Int.left, tail + Vector2Int.right };
                foreach (var pos in checks)
                {
                    if (!snakeCoords.Contains(pos) &&
                        pos.x >= 0 && pos.x < gridManager.width &&
                        pos.y >= 0 && pos.y < gridManager.height)
                    {
                        newTailPos = pos;
                        break;
                    }
                }
            }

            snakeCoords.Add(newTailPos);
            GameObject seg = Instantiate(segmentPrefab);
            seg.transform.position = gridManager.allCells[newTailPos.x, newTailPos.y].transform.position;
            segments.Add(seg);

            RefreshSprites();
            GrownUp?.Invoke();
        }
    }

    
        public void RemoveSegmentAt(Vector2Int targetPos)
    {
        playerMovement.isPlaying = false;
        StartCoroutine(RemoveSegmentAfterMove(targetPos));
    }

    private IEnumerator RemoveSegmentAfterMove(Vector2Int targetPos)
    {
        // Attendre la fin de l'animation en cours
        yield return new WaitUntil(() => MoveFinish);

        int index = snakeCoords.IndexOf(targetPos);
        if (index == -1) { StartCoroutine(Relaunch()); yield break; }

        if (snakeCoords.Count <= 2)
        {
            if (index == 1)
            {
                Destroy(segments[index]);
                segments.RemoveAt(index);
                snakeCoords.RemoveAt(index);
                segments[0].GetComponent<SpriteRenderer>().sprite = headSprite;
            }
            UpdateRotations();
            StartCoroutine(Relaunch());
            yield break;
        }

        if (index == 0) { StartCoroutine(Relaunch()); yield break; }

        Destroy(segments[index]);
        segments.RemoveAt(index);
        snakeCoords.RemoveAt(index);

        RefreshSprites();
        GrownDown?.Invoke();
        UpdateRotations();
        StartCoroutine(Relaunch());
    }

    IEnumerator Relaunch()
    {
        yield return new WaitForSeconds(0.1f);
        playerMovement.StartGame();
    }
    private List<Vector2Int> GetLinearPath(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int dir = to - from;
        int stepX = dir.x == 0 ? 0 : (dir.x > 0 ? 1 : -1);
        int stepY = dir.y == 0 ? 0 : (dir.y > 0 ? 1 : -1);

        Vector2Int current = from;
        int safety = 100;
        while (current != to && safety-- > 0)
        {
            current += new Vector2Int(stepX, stepY);
            path.Add(current);
        }
        return path;
    }

    private void RefreshSprites()
    {
        if (segments.Count == 0) return;
        segments[0].GetComponent<SpriteRenderer>().sprite = headSprite;
        if (segments.Count > 1)
            segments[segments.Count - 1].GetComponent<SpriteRenderer>().sprite = tailSprite;
        for (int i = 1; i < segments.Count - 1; i++)
            segments[i].GetComponent<SpriteRenderer>().sprite = bodySprite;
    }

    private void UpdateRotations()
    {
        // Sécurité : les deux listes doivent être de même taille
        int count = Mathf.Min(segments.Count, snakeCoords.Count);

        for (int i = 0; i < count; i++)
        {
            if (segments[i] == null) continue;

            Vector2Int dir;
            if (i == 0)
                dir = count > 1 ? snakeCoords[0] - snakeCoords[1] : Vector2Int.right;
            else
                dir = snakeCoords[i] - snakeCoords[i - 1];

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            segments[i].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Grow>() != null)
        {
            Grow();
            Destroy(collision.gameObject);
        }
    }
}