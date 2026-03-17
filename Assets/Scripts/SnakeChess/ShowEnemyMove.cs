using UnityEngine;

public class ShowEnemyMove : MonoBehaviour
{
    [SerializeField] private InputPlayerManagerCustomSnake  inputPlayerManagerCustomSnake;
    private bool active = false;


    public void Show()
    {
        if (active == false)
        {
            inputPlayerManagerCustomSnake.canClickEnemies = true;
            active = true;
        }
        else
        {
            inputPlayerManagerCustomSnake.canClickEnemies = false;
            active = false;
        }
           
    }
}
