using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
 
    public Vector2Int coord;  
    public Button button;
    public Color cellColor;


    // Initialisation
    public void Init(int x, int y, UnityEngine.Events.UnityAction callback)
    {
        coord = new Vector2Int(x, y);

        
    }
    public void Test()
    {
        Debug.Log("Click");
    }

}
