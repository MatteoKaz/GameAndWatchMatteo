
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    MultiplicateurFin,
    ComboMultiplier,
    ExtraMove,
    ExtraSize,
    ExtraSwap,
    ExtraTimeCombo,
    CavalierMultEnd,
    FouMultEnd,
    DameMultEnd,
    RoiMultEnd,
    TourMultEnd,
    CavalierCombo,
    FouCombo,
    DameCombo,
    RoiCombo,
    TourCombo,

}

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public string description;
    public UpgradeType type;
    public float value;      // +0.25, +0.5, etc.
    public int formIndex;    // 0 = Roi, 1 = Cavalier, etc. si spécifique
    public Sprite icone;
}

[CreateAssetMenu(fileName = "Upgrades", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public List<UpgradeData> allUpgrades;
}
