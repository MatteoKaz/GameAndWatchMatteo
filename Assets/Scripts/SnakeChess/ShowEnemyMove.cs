using UnityEngine;

public class ShowEnemyMove : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustomSnake  inputPlayerManagerCustomSnake;
    private bool active = false;
    

    public void Start()
    {
        inputPlayerManagerCustomSnake.canClickEnemies = true;
    }
    public void Show()
    {
        if (active == false)
        {
            inputPlayerManagerCustomSnake.canClickEnemies = true;
            active = true;
        }
        else
        {
            
            active = false;
        }
           
    }
}
