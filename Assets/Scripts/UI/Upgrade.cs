
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    MultiplicateurFin,
    ComboMultiplier,
    ExtraMove,
    ExtraSize,
    ExtraSwap,

}

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public string description;
    public UpgradeType type;
    public float value;      // +0.25, +0.5, etc.
    public int formIndex;    // 0 = Roi, 1 = Cavalier, etc. si spécifique
}

[CreateAssetMenu(fileName = "DataUpgrades", menuName = "Scriptable Objects/DataUpgrades")]
public class DataUpgrades : ScriptableObject
{
    public List<UpgradeData> allUpgrades;
}
