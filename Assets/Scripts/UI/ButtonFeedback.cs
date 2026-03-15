using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour
{
    [SerializeField] private Sprite spriteBase;
    [SerializeField] private Sprite spriteClick;
    [SerializeField] private Image image;

    public void OnClick()
    {
        StartCoroutine(Click());    
    }

    public IEnumerator Click()
    {
        image.sprite = spriteClick;
        yield return new WaitForSeconds(0.2f);
        image.sprite = spriteBase;
    }
}
