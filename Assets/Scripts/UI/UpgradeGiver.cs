using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Animator animator;
    private bool CanPress=false;

    [SerializeField] GameObject UpgradeMenuparent;
    [SerializeField] public GameObject UpgradeMenuPrefab;
    [SerializeField] public GraphicRaycaster backGroundToBlock;


    public event Action UpgradeDone;

    public void OnEnable()
    {
        waveEnd.CloseMenuCard += CloseMenu;
        StartCoroutine(showAnim());
    }


    public IEnumerator showAnim()
    {

        
        Card.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("ShowUp");
        yield return new WaitForSeconds(0.25f);
        CanPress = true;
        animator.SetTrigger("Idle");
    }
    public void OnDisable()
    {
        waveEnd.CloseMenuCard -= CloseMenu;
    }
    public void GetTypeUpgrade(UpgradeType type)
    {
        typetoGive = type;
    }

    public void GiveUpgrade()
    {
        if(CanPress == true)
        {
            switch (typetoGive)
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
                    playerEat.BasemovetoLooseMult += (int)upgradeValue;
                    break;
                case UpgradeType.TailleMax:
                    snakeBody.sizemax += (int)upgradeValue;
                    break;
                case UpgradeType.MoreChanceKing:
                    changeMovement.moveWeights[0].weight += (int)upgradeValue;
                    break;
                case UpgradeType.MoreChanceKinght:
                    changeMovement.moveWeights[1].weight += (int)upgradeValue;
                    break;
                case UpgradeType.MoreChanceBishop:
                    changeMovement.moveWeights[2].weight += (int)upgradeValue;
                    break;
                case UpgradeType.MoreChanceTour:
                    changeMovement.moveWeights[3].weight += (int)upgradeValue;
                    break;
                case UpgradeType.MoreChanceQueen:
                    changeMovement.moveWeights[4].weight += (int)upgradeValue;
                    break;

                case UpgradeType.RoiMultEnd:
                    scoreSnake.multRoi += upgradeValue;
                    break;
                case UpgradeType.CavalierMultEnd:
                    scoreSnake.multCavalier += upgradeValue;
                    break;
                case UpgradeType.FouMultEnd:
                    scoreSnake.multFou += upgradeValue;
                    break;
                case UpgradeType.TourMultEnd:
                    scoreSnake.multTour += upgradeValue;
                    break;
                case UpgradeType.DameMultEnd:
                    scoreSnake.multDame += upgradeValue;
                    break;


                case UpgradeType.RoiCombo:
                    scoreSnake.multRoiEnchainement += upgradeValue;
                    break;
                case UpgradeType.CavalierCombo:
                    scoreSnake.multCavalierEnchainement += upgradeValue;
                    break;
                case UpgradeType.FouCombo:
                    scoreSnake.multFouEnchainement += upgradeValue;
                    break;
                case UpgradeType.TourCombo:
                    scoreSnake.multTourEnchainement += upgradeValue;
                    break;
                case UpgradeType.DameCombo:
                    scoreSnake.multDameEnchainement += upgradeValue;
                    break;

                case UpgradeType.MultGlobal:
                    scoreSnake.multGlobal += upgradeValue;
                    break;
                case UpgradeType.MultOrdre: 
                    scoreSnake.multOrdre += upgradeValue;
                    break;
            }
            waveEnd.CloseCard();
            waveEnd.LaunchNewWave();
            UpgradeData upgrade = dataUpgrade.allUpgrades.FirstOrDefault(u => u.type == typetoGive);

            if (upgrade != null)
            {
                GameObject obj = Instantiate(UpgradeMenuPrefab, UpgradeMenuparent.transform);
                UpgradeItemUI ui = obj.GetComponent<UpgradeItemUI>();

                ui.Setup(
                    upgrade.upgradeName,
                    upgrade.description,
                    upgrade.icone
                );
            }
            else
            {
                Debug.LogWarning("Aucune upgrade correspondante trouvée pour le type " + typetoGive);
            }

        }
       
    }
      
    public void CloseMenu()
    {

        StartCoroutine(Close());


    }

    public IEnumerator Close()
    {

        animator.SetTrigger("Hide");
        CanPress = false;
        yield return new WaitForSeconds(0.8f);
        backGroundToBlock.blockingMask = LayerMask.GetMask("None");
        Card.SetActive(false);
        Self.SetActive(false);

    }


}
