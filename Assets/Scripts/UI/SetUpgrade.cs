using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetUpgrade : MonoBehaviour
{
    public Upgrade dataUpgrade;
    [SerializeField] WaveManager waveManager;
    [SerializeField] public List<GameObject> prefabCard;
    [SerializeField] public List<TMP_Text> ListOFTextTitle;
    [SerializeField] public List<TMP_Text> ListOFTextDescription;
    [SerializeField] public List<Image> ListOfIcone;
    [SerializeField] public List<UpgradeGiver> upgradeGivers;
    [SerializeField] public List<GameObject> Bouton;
    [SerializeField] public GraphicRaycaster backGroundToBlock;
    [SerializeField] AudioEventDispatcher1 _audioEventDispatcher;
    public int numberOfUse = 2;
    public void GenerateUpgrade()
    {
        int numberOfCard = 3;
        for (int i = 0; i < numberOfCard; i++)
        {
            int random = Random.Range(0, dataUpgrade.allUpgrades.Count );
           UpgradeData DataToChoose = dataUpgrade.allUpgrades[random];
            ListOFTextTitle[i].text = $"{DataToChoose.upgradeName}";
            ListOFTextDescription[i].text = $"{DataToChoose.description}";
            ListOfIcone[i].sprite = DataToChoose.icone;
            upgradeGivers[i].upgradeValue = DataToChoose.value;
            upgradeGivers[i].GetTypeUpgrade(DataToChoose.type);

            //prefabCard[i].SetActive(true);
            backGroundToBlock.blockingMask = LayerMask.GetMask("Everything"); 
             Bouton[i].SetActive(true);






        }

    }
    public void ResetUpgrade(int i)
    {
        if (numberOfUse == 0)
            return;
        _audioEventDispatcher.PlayAudio(AudioType1.EnnemyMovement);
        numberOfUse -= 1;
            int random = Random.Range(0, dataUpgrade.allUpgrades.Count);
        UpgradeData DataToChoose = dataUpgrade.allUpgrades[random];
        ListOFTextTitle[i].text = $"{DataToChoose.upgradeName}";
        ListOFTextDescription[i].text = $"{DataToChoose.description}";
        ListOfIcone[i].sprite = DataToChoose.icone;
        upgradeGivers[i].upgradeValue = DataToChoose.value;
        upgradeGivers[i].GetTypeUpgrade(DataToChoose.type);

        //prefabCard[i].SetActive(true);
        backGroundToBlock.blockingMask = LayerMask.GetMask("Everything");
        Bouton[i].SetActive(true);
    }
    public void resetUse()
    {
        numberOfUse = 2;
    }
}
