using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(QuestSettings))]
public class QuestSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        QuestSettings settings = (QuestSettings)target;

        int questCounts = (int)QuestType.LastItem;

        if (settings.descText == null)
            settings.descText = new string[questCounts];
        else if (settings.descText.Length < questCounts)
        {
            string[] tempArray = settings.descText;
            settings.descText = new string[questCounts];

            for (int i = 0; i < tempArray.Length; i++)
                settings.descText[i] = tempArray[i];
        }


        if (settings.logos == null || settings.logos.Length == 0)
            settings.logos = new Sprite[questCounts];
        else if (settings.logos.Length < questCounts)
        {
            int diff = questCounts - settings.logos.Length;
            ArrayUtility.AddRange<Sprite>(ref settings.logos, new Sprite[diff]);
        }

        

        if (settings.toGoText == null)
            settings.toGoText = new string[questCounts];
        else if (settings.toGoText.Length < questCounts)
        {
            string[] tempArray = settings.toGoText;
            settings.toGoText = new string[questCounts];

            for (int i = 0; i < tempArray.Length; i++)
                settings.toGoText[i] = tempArray[i];
        }

        if (settings.isOneGameRoundQuest == null)
            settings.isOneGameRoundQuest = new bool[questCounts];
        else if (settings.isOneGameRoundQuest.Length < questCounts)
        {
            bool[] tempArray = settings.isOneGameRoundQuest;
            settings.isOneGameRoundQuest = new bool[questCounts];

            for (int i = 0; i < tempArray.Length; i++)
                settings.isOneGameRoundQuest[i] = tempArray[i];
        }


        if (GUILayout.Button(System.Environment.NewLine +  "Import" + System.Environment.NewLine))
        {
            StreamReader reader = new StreamReader("C:\\wheelsofwar\\quests.txt");

            int index = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                settings.descText[index] = line.Trim();
                settings.toGoText[index] = reader.ReadLine().Trim();
                reader.ReadLine(); // -----------
                index++;
            }

            reader.Close();
            /*
            reader = new StreamReader("D:\\CG\\quests original.txt");

            index = 0;
            while (!reader.EndOfStream)
            {
                settings.editorDescText[index] = reader.ReadLine().Trim();
                reader.ReadLine(); // -----------
                reader.ReadLine(); // -----------
                index++;
            }

            reader.Close();*/
            Debug.Log("Import Done");
        }

        GUILayout.Space(30);

        for (int i = 0; i < questCounts; i++)
        {
            EditorGUILayout.LabelField(((QuestType)i).ToString());
            settings.descText[i] = EditorGUILayout.TextField(settings.descText[i]);
            settings.toGoText[i] = EditorGUILayout.TextField(settings.toGoText[i]);
            settings.logos[i] = (Sprite)EditorGUILayout.ObjectField("Logo", settings.logos[i], typeof(Sprite));
            settings.isOneGameRoundQuest[i] = EditorGUILayout.Toggle("Is One Game Quest", settings.isOneGameRoundQuest[i]);

            EditorGUILayout.Space();
        }
    }
}