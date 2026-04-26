using TMPro;
using UnityEngine;

public class RowsScript : MonoBehaviour
{

    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void Init(int rank, string name, int score)
    {
        rankText.text = $"{rank} :";
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
