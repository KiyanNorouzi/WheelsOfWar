using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DailyGifts))]
public class DailyGiftsEditor : Editor 
{

    bool[] isOpen;



    public override void OnInspectorGUI()
    {
        DailyGifts gifts = (DailyGifts)target;




        if (isOpen == null || isOpen.Length == 0)
            isOpen = new bool[gifts.items.Length];
        else if (isOpen.Length < gifts.items.Length)
        {
            int diff = gifts.items.Length - isOpen.Length;
            ArrayUtility.AddRange<bool>(ref isOpen, new bool[diff]);
        }


        GUILayout.Space(10);
        EditorGUILayout.LabelField(gifts.items.Length + " gift(s) for " + gifts.items.Length + " consecutive day(s)");
        GUILayout.Space(10);

        string[] vipPackageNames = new string[] { "Power", "Research", "Oil" };


        for (int i = 0; i < gifts.items.Length; i++)
        {
            EditorGUI.indentLevel = 2;

            string text = "";
            
            if (gifts.items[i].type == DailyGiftRewardType.VIP)
                text = string.Format("Day {0} [VIP: {1}]", i + 1, vipPackageNames[gifts.items[i].amount]);
            else
                text = string.Format("Day {0} [{1} {2}]", i + 1, gifts.items[i].amount, gifts.items[i].type);

            isOpen[i] = EditorGUILayout.Foldout(isOpen[i], text);

            if (isOpen[i])
            {
                EditorGUI.indentLevel = 3;

                gifts.items[i].type = (DailyGiftRewardType) EditorGUILayout.EnumPopup("Reward Type", gifts.items[i].type);
                if (gifts.items[i].type == DailyGiftRewardType.VIP)
                {
                    gifts.items[i].amount = Mathf.Clamp(gifts.items[i].amount, 0, vipPackageNames.Length - 1);
                    gifts.items[i].amount = EditorGUILayout.Popup("Package Name", gifts.items[i].amount, vipPackageNames);
                }
                else
                    gifts.items[i].amount = EditorGUILayout.IntField("Amount", gifts.items[i].amount);
            }
        }

        GUILayout.Space(30);

        if (GUILayout.Button(System.Environment.NewLine + "Add Gift"+System.Environment.NewLine))
        {
            ArrayUtility.Add<DailyGiftItem>(ref gifts.items, new DailyGiftItem());
            ArrayUtility.Add<bool>(ref isOpen, true);
        }
    }
}
