using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChangeMovement : MonoBehaviour
{
    [SerializeField] public PlayerMovement pm;
    public int movementChange = 1;
    public int baseMoveChange = 1;
    [SerializeField] TMP_Text ScoreTexte;

    [System.Serializable]
    public class MoveWeight
    {
        public PlayerMovement.MoveType moveType;
        public int weight;
    }

    public List<MoveWeight> moveWeights = new List<MoveWeight>()
    {
        new MoveWeight { moveType = PlayerMovement.MoveType.Roi, weight = 5 },
        new MoveWeight { moveType = PlayerMovement.MoveType.Cavalier, weight = 4 },
        new MoveWeight { moveType = PlayerMovement.MoveType.Fou, weight = 3 },
        new MoveWeight { moveType = PlayerMovement.MoveType.Tour, weight = 2 },
        new MoveWeight { moveType = PlayerMovement.MoveType.Dame, weight = 1 }
    };

    public void ChangeMovementButton()
    {
        if (movementChange <= 0) return;

        movementChange--;

        pm.currentMoveType = GetRandomMove();

        pm.ColorCell();
        pm.PlayerStuckCheck();
    }

    PlayerMovement.MoveType GetRandomMove()
    {

        PlayerMovement.MoveType currentType = pm.currentMoveType;
        int totalWeight = 0;
        foreach (var move in moveWeights)
        {
            if (move.moveType != currentType)
                totalWeight += move.weight;
        }

        // Tirage aléatoire
        int random = Random.Range(0, totalWeight);

        foreach (var move in moveWeights)
        {
            if (move.moveType == currentType)
                continue; // ignorer le move actuel

            if (random < move.weight)
                return move.moveType;

            random -= move.weight;
        }

        // fallback si quelque chose se passe mal
        foreach (var move in moveWeights)
        {
            if (move.moveType != currentType)
                return move.moveType;
        }

        return PlayerMovement.MoveType.Roi;
    }


        void Update()
    {

        ScoreTexte.text = $"{movementChange} ";
    }
}
