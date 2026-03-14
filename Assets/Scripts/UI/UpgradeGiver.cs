using System;
using UnityEngine;

public class UpgradeGiver : MonoBehaviour
{
    public Upgrade dataUpgrade;
    [SerializeField] public ChangeMovement changeMovement;
    [SerializeField] public PlayerEat playerEat;
    [SerializeField] public PlayerScoreSnake scoreSnake;
     [SerializeField] public SnakeBody snakeBody;
    [SerializeField] public PlayerMovement playerMovement;
    public float upgradeValue;
    [SerializeField] GameObject Card;
    [SerializeField] GameObject Self;
    public UpgradeType typetoGive;
    [SerializeField] public WaveEnd waveEnd;

    public event Action UpgradeDone;

    public void OnEnable()
    {
        waveEnd.CloseMenuCard += CloseMenu;
    }
    public void GetTypeUpgrade(UpgradeType type)
    {
        typetoGive = type;
    }

    public void GiveUpgrade()
    {
        switch(typetoGive)
        {
            case UpgradeType.MultiplicateurFin:
                scoreSnake.multiplierValue += upgradeValue;
                scoreSnake.multiplierValueBase += upgradeValue;
                break;
            case UpgradeType.ComboMultiplier:
                scoreSnake.multiplierValueEnchainementValue += upgradeValue;
                scoreSnake.BasemultiplierValueEnchainementAdd += upgradeValue;
                break;
            case UpgradeType.ExtraSwap:
                changeMovement.movementChange = (int)upgradeValue;
                changeMovement.baseMoveChange = (int)upgradeValue;
                break;
            case UpgradeType.ExtraSize:
                snakeBody.GrowValue = (int)upgradeValue;
                break;
            case UpgradeType.ExtraMove:
                playerMovement.NumberOfMovesBonus += (int)upgradeValue;
                break;
            case UpgradeType.ExtraTimeCombo:
                playerEat.movetoLooseMult += (int)upgradeValue;
                break;
        }
        waveEnd.CloseCard();
        waveEnd.LaunchNewWave();
    }
      
    public void CloseMenu()
    {
        Card.SetActive(false);
        Self.SetActive(false);


    }


}
