using UnityEngine;


public class GhostMarker : MonoBehaviour
{
    public Enemy2 linkedEnemy;
    public PlayerEat2 playerEat;

   

    public void Press()
    {
        playerEat.OnGhostMarkerClicked(this);
    }
}
