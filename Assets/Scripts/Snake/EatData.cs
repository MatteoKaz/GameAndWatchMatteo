using System.Collections;
using UnityEngine;

public class EatData : MonoBehaviour
{
    public int value = 1;
    public float duration = 1.0f;
    public float TimeAlive = 10f;
    public enum EatType
    {
        Normal,
        Fire,
        Ice,
        Ghost
    }

    public EatType type;

    public void Start()
    {
        StartCoroutine(DurationLife());
    }
    IEnumerator DurationLife()
    {
        yield return new WaitForSeconds(TimeAlive-2.5f);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float elapsed = 0f;
        float blinkSpeed = 0.15f;

        while (elapsed < 2.5f)
        {
            
            float alpha = Mathf.PingPong(elapsed / blinkSpeed, 1f);
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }


        Color final = sr.color;
        final.a = 1f;
        sr.color = final;
        Destroy (gameObject);   
    }
}