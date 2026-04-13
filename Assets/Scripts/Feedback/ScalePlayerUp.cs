using UnityEngine;
using System.Collections;
using System;

public class ScalePlayerUp : MonoBehaviour
{
    [SerializeField] private PlayerScore PS;
    private float duration = 0.25f;
    private Vector3 startScale = new(6.95f, 6.95f, 6.95f) ;
    private Vector3 targetScale = new Vector3(8f, 8f, 8f);
    IEnumerator ScalePlayer()
    {
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
       
        

       transform.localScale = targetScale;

        if (elapsed >= duration)
        {
            transform.localScale = startScale;
        }
    }

    private void OnEnable()
    {
        PS.ONBonus += ScaleUP;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void ScaleUP()
    {
        StartCoroutine(ScalePlayer());
    }

  
}
