using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour
{
    [SerializeField] private GameObject icePrefab;
    [SerializeField] private AiManager2 aiManager;
    [SerializeField] private float iceDuration = 15f;

    private bool isActive = false;

    public void SetActive(bool active)
    {
        isActive = active;

        if (active)
            FreezeAllEnemies();
        else
            UnfreezeAllEnemies();
    }

    
    private void FreezeAllEnemies()
    {
        foreach (Enemy2 e in aiManager.enemies)
            if (e != null)
                StartCoroutine(FreezeEnemy(e));
    }

    private IEnumerator FreezeEnemy(Enemy2 e)
    {
        e.Freeze(icePrefab);
        yield return new WaitForSeconds(iceDuration);

        
    }

    private void UnfreezeAllEnemies()
    {
        foreach (Enemy2 e in aiManager.enemies)
            if (e != null)
                e.Unfreeze();
    }
}