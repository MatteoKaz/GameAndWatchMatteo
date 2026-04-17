using UnityEngine;

public class EatData : MonoBehaviour
{
    public int value = 1;

    public enum EatType
    {
        Normal,
        Fire,
        Glass,
        Coin
    }

    public EatType type;

 
}