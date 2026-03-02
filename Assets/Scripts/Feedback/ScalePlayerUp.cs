using UnityEngine;
using System.Collections;
using System;

public class ScalePlayerUp : MonoBehaviour
{
    [SerializeField] private PlayerScore PS;
    private float duration = 0.25f;
    private Vector3 startScale = new(2,2,2) ;
    private Vector3 targetScale = new Vector3(2.5f, 2.5f, 2.5f);
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
