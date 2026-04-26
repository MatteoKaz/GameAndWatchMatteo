using UnityEngine;

public class OpenRanking : MonoBehaviour
{
    [SerializeField] GameObject GWRank;
    [SerializeField] GameObject SnakeRank;
    [SerializeField] GameObject SnakeChess;
    
    public void OpenGW()
    {
        GWRank.SetActive(true);
    }
    public void CloseGW()
    {
        GWRank.SetActive(false);
    }
    public void OpenSnake()
    {
        SnakeRank.SetActive(true);
    }
    public void CloseSnake()
    {
        SnakeRank.SetActive(false);
    }
}
