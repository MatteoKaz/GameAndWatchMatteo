using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeBody : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private Sprite headSprite;        
    [SerializeField] private Sprite bodySprite;
    [SerializeField] private Sprite tailSprite;
    [SerializeField] private PlayerMovement playerMovement;
    public bool MoveFinish = true;
    [SerializeField] private PlayerEat pe;

    public event Action GrownUp;
    public event Action GrownDown;
    public List<Vector2Int> snakeCoords = new List<Vector2Int>();
    public List<GameObject> segments = new List<GameObject>();

    public int startSize = 3;
    public float moveDuration = 0.2f;
    

    private void OnEnable()
    {
       // gridManager.FinishInitialize += CreateSnake;
    }

    private void OnDisable()
    {
       // gridManager.FinishInitialize -= CreateSnake;
    }
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
        foreach (var seg in segments)
            Destroy(seg);
        segments.Clear();

        for (int i = 0; i < startSize; i++)
        {
            Vector2Int pos = start + new Vector2Int(-i, 0);
            pos.x = Mathf.Clamp(pos.x, 0, gridManager.width - 1);

            GameObject seg = Instantiate(segmentPrefab);
            seg.transform.position = gridManager.allCells[pos.x, pos.y].transform.position;

            SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
            if (i == 0)
                sr.sprite = headSprite;
            else if (i + 1 == startSize)
                sr.sprite = tailSprite;
            else
                sr.sprite = bodySprite;

            snakeCoords.Add(pos);
            segments.Add(seg);
            
        }

        UpdateRotations();
        playerMovement.Spawned();
    }

    /// <summary>
    /// Déplace le serpent vers la case cible en Lerp, sécurisé.
    /// </summary>
    public IEnumerator MoveSnakeTo(Vector2Int target)
    {
        // Génère le chemin case par case (inclut Cavalier en L)
        List<Vector2Int> path = GetPathToTarget(snakeCoords[0], target, playerMovement.currentMoveType);

        foreach (Vector2Int next in path)
        {
            // Crée la nouvelle configuration du serpent
            List<Vector2Int> newCoords = new List<Vector2Int> { next };
            for (int i = 0; i < segments.Count - 1; i++)
            {
                newCoords.Add(snakeCoords[i]); // décale les positions existantes
            }

            // Stock positions actuelles et futures
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

            // Lerp pour mouvement fluide
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / moveDuration;
                for (int i = 0; i < segments.Count; i++)
                    segments[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], t);

                UpdateRotations();
                yield return null;
            }

            // Fixe la position finale et met à jour snakeCoords
            for (int i = 0; i < segments.Count; i++)
                segments[i].transform.position = endPositions[i];

            snakeCoords = newCoords;
            UpdateRotations();
        }
        MoveFinish = true;
        playerMovement.coordPlayer = snakeCoords[0];
        pe.PlayerKill();



        playerMovement.EndTurn();
    }

    /// <summary>
    /// Retourne le chemin case par case vers la cible, avec Cavalier correct en L.
    /// </summary>
    private List<Vector2Int> GetPathToTarget(Vector2Int head, Vector2Int target, PlayerMovement.MoveType moveType)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        if (moveType == PlayerMovement.MoveType.Cavalier)
        {
            int dx = target.x - head.x;
            int dy = target.y - head.y;

            if (Mathf.Abs(dx) == 2 && Mathf.Abs(dy) == 1)
            {
                int stepX = dx > 0 ? 1 : -1;
                int stepY = dy > 0 ? 1 : -1;

                // Décomposer l'axe X de 2 en deux étapes
                Vector2Int first = head + new Vector2Int(stepX, 0);
                Vector2Int second = first + new Vector2Int(stepX, 0);

                // Puis faire l'axe Y
                Vector2Int third = second + new Vector2Int(0, stepY);

                path.Add(first);
                path.Add(second);
                path.Add(third);
            }
            else if (Mathf.Abs(dx) == 1 && Mathf.Abs(dy) == 2)
            {
                int stepX = dx > 0 ? 1 : -1;
                int stepY = dy > 0 ? 1 : -1;

                // Décomposer l'axe Y de 2 en deux étapes
                Vector2Int first = head + new Vector2Int(0, stepY);
                Vector2Int second = first + new Vector2Int(0, stepY);

                // Puis faire l'axe X
                Vector2Int third = second + new Vector2Int(stepX, 0);

                path.Add(first);
                path.Add(second);
                path.Add(third);
            }
            else
            {
                path.Add(target); // fallback si mouvement impossible
            }

            return path;
        }

        // Roi/Fou/Tour/Dame : déplacement case par case
        Vector2Int dir = target - head;
        int stepX1 = dir.x == 0 ? 0 : (dir.x > 0 ? 1 : -1);
        int stepY1 = dir.y == 0 ? 0 : (dir.y > 0 ? 1 : -1);

        Vector2Int current = head;
        int maxSteps = 100;

        while (current != target && maxSteps-- > 0)
        {
            current += new Vector2Int(stepX1, stepY1);
            path.Add(current);
        }

        return path;
    }

    public void Grow()
    {
        Vector2Int tail = snakeCoords[snakeCoords.Count - 1];

        snakeCoords.Add(tail);

        GameObject seg = Instantiate(segmentPrefab);
        seg.transform.position = gridManager.allCells[tail.x, tail.y].transform.position;
        segments.Add(seg); 
        
        GrownUp?.Invoke();
    }

    private void UpdateRotations()
    {
        for (int i = 0; i < segments.Count; i++)
        {
           
            if (i + 1 == segments.Count )
            {
                if(segments.Count == 1)
                {
                    Vector2Int dir = Vector2Int.right;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    segments[i].transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                else
                {
                    Vector2Int dir = snakeCoords[i] - snakeCoords[i - 1];
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    segments[i].transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                    
            }
            else
            {
                Vector2Int dir = snakeCoords[i] - snakeCoords[i + 1];
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                segments[i].transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

    }





    public IEnumerator FirstMoveSnakeTo(Vector2Int target)
    {
        // Génère le chemin case par case (inclut Cavalier en L)
        List<Vector2Int> path = GetPathToTarget(snakeCoords[0], target, playerMovement.currentMoveType);

        foreach (Vector2Int next in path)
        {
            // Crée la nouvelle configuration du serpent
            List<Vector2Int> newCoords = new List<Vector2Int> { next };
            for (int i = 0; i < segments.Count - 1; i++)
            {
                newCoords.Add(snakeCoords[i]); // décale les positions existantes
            }

            // Stock positions actuelles et futures
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

            // Lerp pour mouvement fluide
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / moveDuration;
                for (int i = 0; i < segments.Count; i++)
                    segments[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], t);

                UpdateRotations();
                yield return null;
            }

            // Fixe la position finale et met à jour snakeCoords
            for (int i = 0; i < segments.Count; i++)
                segments[i].transform.position = endPositions[i];

            snakeCoords = newCoords;
            UpdateRotations();
        }
    }

    /// <summary>
    /// Supprime un segment du serpent correspondant à la position donnée.
    /// Gère correctement les cas queue, corps, et serpent minimal (2 segments).
    /// </summary>
    public void RemoveSegmentAt(Vector2Int targetPos)
    {
        int index = snakeCoords.IndexOf(targetPos);

        if (index == -1) return; // la position n'est pas dans le serpent

        // Si le serpent est minimal (2 segments : tête + queue)
        if (snakeCoords.Count <= 2)
        {
            // Option : supprimer la queue uniquement
            if (index == 1)
            {
                Destroy(segments[index]);
                segments.RemoveAt(index);
                snakeCoords.RemoveAt(index);

                // Le serpent est réduit à une seule tête, on garde son sprite
                segments[0].GetComponent<SpriteRenderer>().sprite = headSprite;
                Debug.Log("Dead");
            }
            else
            {
                // Tentative de supprimer la tête : déclenche fin de partie
                Debug.Log("Le joueur est mort : suppression de la tête !");
                // Ici tu peux appeler une fonction GameOver()
            }

            UpdateRotations();
            return;
        }

        // Cas général : corps ou queue
        if (index == 0)
        {
            // Suppression de la tête non autorisée
            Debug.Log("Suppression de la tête non supportée ici.");
            return;
        }

        if (index == snakeCoords.Count - 1)
        {
            // Supprime la queue
            Vector2Int tailPos = snakeCoords[snakeCoords.Count - 2];

            Destroy(segments[index]);
            segments.RemoveAt(index);
            snakeCoords.RemoveAt(index);

            // Repositionne la nouvelle queue
            SpriteRenderer sr = segments[segments.Count - 1].GetComponent<SpriteRenderer>();
            sr.sprite = tailSprite;
            segments[segments.Count - 1].transform.position = gridManager.allCells[tailPos.x, tailPos.y].transform.position;
        }
        else
        {
            // Supprime un segment du corps
            Destroy(segments[index]);
            segments.RemoveAt(index);
            snakeCoords.RemoveAt(index);
        }

        GrownDown?.Invoke();
        // Mise à jour des sprites tête et queue
        segments[0].GetComponent<SpriteRenderer>().sprite = headSprite;
        segments[segments.Count - 1].GetComponent<SpriteRenderer>().sprite = tailSprite;

        UpdateRotations();
    }
}