using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image icon;

    public void Setup(string title, string description, Sprite sprite)
    {
        titleText.text = title;
        descriptionText.text = description;
        icon.sprite = sprite;
    }
}