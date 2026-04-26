using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{

  public int score;
    
    public event Action ONBonus;
    [SerializeField] private AudioEventDispatcher _audioEventDispatcher;
    public bool GodMod;
    public Coroutine God;
    public String Name;
    [SerializeField]public TMP_InputField inputField;
    void Update()
    {
        
    }
   
    void OnTextChanged(string value)
    {
        Name = value;
    }
    IEnumerator AddPoint()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            yield return new WaitForSeconds(10f);
            score += 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bonus>() != null)
        {
            score += 25;
            var actor = collision.gameObject;
            _audioEventDispatcher.PlayAudio(AudioType.Win);
            Destroy(actor);
            ONBonus?.Invoke();

        }
        if (collision.GetComponent<Bonus2>() != null)
        {
            GodMod = true;
            if (God != null)
                StopCoroutine(God);
            God = StartCoroutine(GodMode());
            var actor = collision.gameObject;
            Destroy(actor);
            _audioEventDispatcher.PlayAudio(AudioType.Win);
            ONBonus?.Invoke();

        }
    }

    public IEnumerator GodMode()
    {
        float t = 0f;
        float duration = 15f;


       float rainbowSpeed = 25f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float hue = Mathf.Repeat((t / duration)* rainbowSpeed, 1f);
            Color rainbow = Color.HSVToRGB(hue, 1f, 1f);
            GetComponent<SpriteRenderer>().color = rainbow;
            yield return null;
        }

        GodMod = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void AddScore()
    {
        score += 50;
        ONBonus?.Invoke();
        _audioEventDispatcher.PlayAudio(AudioType.Win);
    }
    public void EndScore()
    {
        GameManager.Instance.saveData.topScoresGameAndWatch.AddScore(Name,score);


        if (score > GameManager.Instance.saveData.highScoreGameAndWatch)
            GameManager.Instance.saveData.highScoreGameAndWatch = score;
       

        SaveManager.Save(GameManager.Instance.saveData);
    }
   public void SetName(String name)
    {
        Name = name; 
    }
    private void Start()
    {
        StartTime();
        inputField.onValueChanged.AddListener(OnTextChanged);

    }
    private void StartTime()
    {
        
        StartCoroutine(AddPoint());
    }
}
