using UnityEngine;
using TMPro;

public class ChangeMovement : MonoBehaviour
{
    [SerializeField] public PlayerMovement pm;
    public int movementChange = 3;
    [SerializeField] TMP_Text ScoreTexte;


    public void ChangeMovementButton()
    {
        if (movementChange != 0)
        {
            Debug.Log("clickk");
            int RandomPick = Random.Range(0, 15);
            movementChange -= 1;

            switch (RandomPick)
            {
                case 0:
                    pm.currentMoveType = PlayerMovement.MoveType.Roi;
                    pm.ColorCell();
                    return;
                case 1: 
                        pm.currentMoveType = PlayerMovement.MoveType.Roi;
                    pm.ColorCell();
                    return;

                case 2:  
                    pm.currentMoveType = PlayerMovement.MoveType.Roi;
                    pm.ColorCell();
                    return;
                case 3:
                    pm.currentMoveType = PlayerMovement.MoveType.Cavalier;
                    pm.ColorCell();
                    return;
                case 4:
                    pm.currentMoveType = PlayerMovement.MoveType.Cavalier;
                    pm.ColorCell();
                    return;
                case 5: 
                    pm.currentMoveType = PlayerMovement.MoveType.Fou;
                    pm.ColorCell();
                    return;
                case 6: 
                    pm.currentMoveType = PlayerMovement.MoveType.Fou;
                    pm.ColorCell();
                    return;
                case 7: 
                    pm.currentMoveType = PlayerMovement.MoveType.Cavalier;
                    pm.ColorCell();
                    return;
                case 8: 

                    pm.currentMoveType = PlayerMovement.MoveType.Tour;
                    pm.ColorCell();
                    return;
                case 9: 
                    pm.currentMoveType = PlayerMovement.MoveType.Tour;
                    pm.ColorCell();
                    return;
                case 10: 
                    pm.currentMoveType = PlayerMovement.MoveType.Dame;
                    pm.ColorCell();
                    return;
                case 11: 
                    pm.currentMoveType = PlayerMovement.MoveType.Fou;
                    pm.ColorCell();
                    return;
                case 12:  
                    pm.currentMoveType = PlayerMovement.MoveType.Roi;
                    pm.ColorCell();
                    return;
                case 13:
                    pm.currentMoveType = PlayerMovement.MoveType.Cavalier;
                    pm.ColorCell();
                    return;
                case 14:
                    pm.currentMoveType = PlayerMovement.MoveType.Roi;
                    pm.ColorCell();
                    return;
                default:
                    pm.currentMoveType = PlayerMovement.MoveType.Roi;
                    pm.ColorCell();
                    return;

            }
            
        }
    }

    void Update()
    {

        ScoreTexte.text = $"{movementChange} ";
    }
}
