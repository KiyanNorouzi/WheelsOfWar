using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UpgradeInitializer))]
public class UpgradeInitializerEditor : Editor
{
    bool[] categories;
    bool[] parts;

    public override void OnInspectorGUI()
    {
        UpgradeInitializer upgrades = (UpgradeInitializer)target;

        if (categories == null || categories.Length < 6)
            categories = new bool[6];


        for (int i = 0; i < upgrades.allUpgrades.Length; i++)
        {
            EditorGUI.indentLevel = 1;
            categories[i] = EditorGUILayout.Foldout(categories[i], ((UpgradeParts)i).ToString());
            if (categories[i])
            {
                if (parts == null || parts.Length < upgrades.allUpgrades[i].upgrades.Length)
                    parts = new bool[upgrades.allUpgrades[i].upgrades.Length];

                
                for (int j = 0; j < upgrades.allUpgrades[i].upgrades.Length; j++)
                {
                    EditorGUI.indentLevel = 2;

                    string title = "";
                    if (upgrades.allUpgrades[i].upgrades[j].decreaseValue != 0)
                        title = string.Format("{0}  -  [stats: {1}, decrease {2} from {5}, {3} bills, {4} seconds]", j + 1, upgrades.allUpgrades[i].upgrades[j].stat, upgrades.allUpgrades[i].upgrades[j].decreaseValue, upgrades.allUpgrades[i].upgrades[j].upgradeCost.Bills, upgrades.allUpgrades[i].upgrades[j].upgradeTime, upgrades.allUpgrades[i].upgrades[j].affectingPart);
                    else
                        title = string.Format("{0}  -  [stats: {1}, {2} bills, {3} seconds]", j + 1, upgrades.allUpgrades[i].upgrades[j].stat, upgrades.allUpgrades[i].upgrades[j].upgradeCost.Bills, upgrades.allUpgrades[i].upgrades[j].upgradeTime);
                    parts[j] = EditorGUILayout.Foldout(parts[j], title);
                    if (parts[j])
                    {
                        EditorGUI.indentLevel = 3;
                        upgrades.allUpgrades[i].upgrades[j].stat = EditorGUILayout.IntField("Stat", upgrades.allUpgrades[i].upgrades[j].stat);

                        //if (upgrades.allUpgrades[i].upgrades[j].decreaseValue != 0)
                        upgrades.allUpgrades[i].upgrades[j].affectingPart = (UpgradeParts)EditorGUILayout.EnumPopup("Affecting Part", upgrades.allUpgrades[i].upgrades[j].affectingPart);

                        upgrades.allUpgrades[i].upgrades[j].decreaseValue = EditorGUILayout.IntField("Decrease from " + upgrades.allUpgrades[i].upgrades[j].affectingPart, upgrades.allUpgrades[i].upgrades[j].decreaseValue);
                        upgrades.allUpgrades[i].upgrades[j].upgradeCost.Bills = EditorGUILayout.IntField("Bills",upgrades.allUpgrades[i].upgrades[j].upgradeCost.Bills);
                        upgrades.allUpgrades[i].upgrades[j].upgradeTime = EditorGUILayout.FloatField("Time", upgrades.allUpgrades[i].upgrades[j].upgradeTime);
                    }
                }
            }
        }
    }
}
