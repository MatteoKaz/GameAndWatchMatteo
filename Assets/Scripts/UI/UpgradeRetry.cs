using TMPro;
using UnityEngine;

public class UpgradeRetry : MonoBehaviour
{
    [SerializeField] SetUpgrade upgrade;
    [SerializeField] TextMeshProUGUI text;
   

    // Update is called once per frame
    void Update()
    {
        text.text = $"{upgrade.numberOfUse}";
    }
}
