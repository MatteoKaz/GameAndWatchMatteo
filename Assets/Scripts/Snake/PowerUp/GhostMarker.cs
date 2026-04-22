using UnityEngine;


public class GhostMarker : MonoBehaviour
{
    public Enemy2 linkedEnemy;
    public PlayerEat2 playerEat;

   

    public void Press()
    {
        Debug.LogWarning("Clique sur transparence");
        playerEat.OnGhostMarkerClicked(this);
        Debug.LogWarning("Clique sur transparence");
    }
}
