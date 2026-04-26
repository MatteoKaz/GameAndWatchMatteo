using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

public class PlayerEat2 : MonoBehaviour
{
    [SerializeField] private PlayerMovement2 playerMovement;
    [SerializeField] private Spawner1 spawner;
    [SerializeField] private AiManager2 aiManager;
    [SerializeField] Score score;
    [SerializeField] private FireTrail fireTrail;
    [SerializeField] private IceEffect iceEffect;
    private float TimeBonusFire = 10f;
    private float TimeBonusIce =  15f;
    private float TimeBonusGhost = 10f;
    private Coroutine Timer ;
    private Coroutine TimerIce;
    private Coroutine TimerGhost;
    public event Action<GameObject> OnEatObject;
    [SerializeField] GameObject point;

    private List<GhostMarker> ghostMarkers = new List<GhostMarker>();
    [SerializeField] GameObject ghostMarkerPrefab;
    public enum PlayerState
    {
        normal,
        Fire,
        Ice,
        Ghost
    }
    public PlayerState state;
    public void CheckEat()
    {
        Vector2Int playerPos = playerMovement.coordPlayer;
        foreach (Enemy2 e in aiManager.enemies)
        {
            if (e == null) continue;
            if (e.coordEnemy == playerMovement.coordPlayer)
            {
               

                if( e.isFrozen)
                 {
                    playerMovement.isPlaying = false;
                    point.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{e.Value}";
                    Instantiate(point, e.transform.position, Quaternion.identity);
                    point.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{e.Value}";
                    e.ClearChosenMove();
                     aiManager.RemoveEnemy(e);
                     score.AddScore(e.Value);
                     Destroy(e.gameObject);
                    StartCoroutine(Relaunch());
                    break; 
                 }
                if ( e.isBroken)
                {

                    e.ClearChosenMove();
                    point.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{e.Value}";
                    Instantiate(point, e.transform.position, Quaternion.identity);
                    
                    aiManager.RemoveEnemy(e);
                    score.AddScore(e.Value);
                    Destroy(e.gameObject);
                    return;
                }
                if ((state == PlayerState.normal || state == PlayerState.Fire) && !e.isFrozen)
                {
                    playerMovement.GameOver();
                    return;
                }

            }

        }
        
        GameObject obj = spawner.GetObjectAt(playerPos);
        if (obj == null) return;

        EatData data = obj.GetComponent<EatData>();
        if (data == null) return;

        OnEat(data);

        Destroy(obj);
    }
    IEnumerator Relaunch()
    {
        yield return new WaitForSeconds(0.05f);
        playerMovement.StartGame();
    }
    public void OnEat(EatData data)
    {
        switch (data.type)
        {
            case EatData.EatType.Normal:
                playerMovement.snakeBody.Grow();
                point.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{data.value}";
                Instantiate(point, data.transform.position, Quaternion.identity);
                
                score.AddScore(data.value);
                break;
            case EatData.EatType.Fire:
                state = PlayerState.Fire;

                TimeBonusFire = data.duration;

                //fireTrail.SetActive(true);

                foreach (GameObject gm in playerMovement.snakeBody.segments)
                {
                    Animator animator= gm.GetComponent<Animator>();
                    animator.SetTrigger("Combo");
                }
                
                if (Timer!= null)
                    StopCoroutine(Timer);
                Timer = StartCoroutine(TimeBeforeStopBonus());
                break;
            case EatData.EatType.Ice:
                
                state = PlayerState.Ice;
                TimeBonusIce = data.duration;
                iceEffect.SetActive(true);
                if (TimerIce != null) StopCoroutine(TimerIce);
                TimerIce = StartCoroutine(TimeBeforeStopBonusIce());
                break;
            case EatData.EatType.Ghost:
                state = PlayerState.Ghost;
                TimeBonusGhost = data.duration;
                playerMovement.Ghost = true;
                // effet visuel transparence sur les segments
                foreach (GameObject seg in playerMovement.snakeBody.segments)
                {
                    SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
                    Color c = sr.color;
                    c.a = 0.4f;
                    sr.color = c;
                }

                if (TimerGhost != null) StopCoroutine(TimerGhost);
                TimerGhost = StartCoroutine(TimeAfterStopBonusGhost());
                StartCoroutine(GhostOverlapCheck());
                break;



        }

    }
    IEnumerator TimeBeforeStopBonus()
    {
        yield return new WaitForSeconds(TimeBonusFire-3f);
        if (state == PlayerState.Fire)
        {
            foreach (GameObject gm in playerMovement.snakeBody.segments)
            {
                Animator animator = gm.GetComponent<Animator>();
                animator.SetTrigger("ComboEnd");
            }
            yield return new WaitForSeconds(3f);
            foreach (GameObject gm in playerMovement.snakeBody.segments)
            {
                Animator animator = gm.GetComponent<Animator>();
                animator.SetTrigger("BackToIdle");
            }

        }
        state = PlayerState.normal;
       //fireTrail.SetActive(false);
         
    }
    IEnumerator TimeBeforeStopBonusIce()
    {
        yield return new WaitForSeconds(TimeBonusIce);
        
            iceEffect.SetActive(false);
            state = PlayerState.normal;
        
    }
    IEnumerator TimeAfterStopBonusGhost()
    {
        yield return new WaitForSeconds(TimeBonusGhost - 2.5f);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 2.5f;

            foreach (GameObject seg in playerMovement.snakeBody.segments)
            {
                SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = Mathf.Clamp(Mathf.PingPong(t, 1f), 0.5f, 1f);
                sr.color = c;
            }
            yield return null;
        }

        foreach (GameObject seg in playerMovement.snakeBody.segments)
        {
            SpriteRenderer sr = seg.GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }

        foreach (Enemy2 e in aiManager.enemies)
        {
            if (e == null) continue;
            if (playerMovement.snakeBody.snakeCoords.Contains(e.coordEnemy))
            {
                playerMovement.isPlaying = false;
                e.ClearChosenMove();
                aiManager.RemoveEnemy(e);
                score.AddScore(e.Value);
                SpawnPoint(e.Value, e.transform.position);
                Destroy(e.gameObject);
                StartCoroutine(Relaunch());
                break;
            }
        }

        playerMovement.Ghost = false;
        state = PlayerState.normal;
    }

 

    private IEnumerator GhostOverlapCheck()
    {
        while (state == PlayerState.Ghost)
        {
            RefreshGhostMarkers();
            yield return new WaitForSeconds(0.15f);
        }
        ClearGhostMarkers();
    }

    private void RefreshGhostMarkers()
    {
        // Supprimer les markers dont l'ennemi n'overlap plus OU est déjà détruit
        foreach (GhostMarker m in ghostMarkers.ToList())
        {
            if (m == null || m.linkedEnemy == null ||
                !playerMovement.snakeBody.snakeCoords.Contains(m.linkedEnemy.coordEnemy))
            {
                if (m != null) Destroy(m.gameObject);
                ghostMarkers.Remove(m);
            }
        }

        // Spawner uniquement si pas déjà marqué ET ennemi encore vivant
        foreach (Enemy2 e in aiManager.enemies.ToList())
        {
            if (e == null) continue;
            if (!playerMovement.snakeBody.snakeCoords.Contains(e.coordEnemy)) continue;

            // Vérification stricte par référence — évite les doublons
            bool alreadyMarked = ghostMarkers.Exists(m => m != null && m.linkedEnemy == e);
            if (alreadyMarked) continue;

            GameObject go = Instantiate(ghostMarkerPrefab, e.transform.position, Quaternion.identity);
            GhostMarker marker = go.GetComponent<GhostMarker>();
            marker.linkedEnemy = e;
            marker.playerEat = this;
            ghostMarkers.Add(marker);
        }
    }

    public void OnGhostMarkerClicked(GhostMarker marker)
    {
        Enemy2 e = marker.linkedEnemy;

        Debug.Log($"[Ghost] Click reçu — ennemi null ? {e == null}");

       // if (e == null) { ClearGhostMarkers(); return; }

        Debug.Log($"[Ghost] Destruction de {e.name}, valeur {e.Value}");

        marker.linkedEnemy = null;
        ghostMarkers.Remove(marker);
        Destroy(marker.gameObject);

        
        e.ClearChosenMove();
        aiManager.RemoveEnemy(e);
        score.AddScore(e.Value);
        SpawnPoint(e.Value, e.transform.position);
        Destroy(e.gameObject);
       
        Debug.Log("[Ghost] Destroy appelé");
    }
    private void ClearGhostMarkers()
    {
        foreach (GhostMarker m in ghostMarkers)
            if (m != null) Destroy(m.gameObject);
        ghostMarkers.Clear();
    }


    private void SpawnPoint(int value, Vector3 position)
    {
        
        GameObject pointInstance = Instantiate(point, position, Quaternion.identity);

        pointInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"+{value}";
    }
}