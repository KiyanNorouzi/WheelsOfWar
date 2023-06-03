using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(QuestManager))]
public class QuestManagerEditor : Editor
{
    bool foldOut = false;
    bool[] folds;

    string[] states = { "Not Selected", "Selected as Quest #1", "Selected as Quest #2", "Selected as Quest #3" };
    string filePath = "c:\\wheelsofwar\\wheelsofwar_quests.txt";


    int filterIndex = -1;

    public override void OnInspectorGUI()
    {
        QuestManager questManager = (QuestManager)target;
        QuestSettings settings = Editor.FindObjectOfType<QuestSettings>();

        if (settings == null)
        {
            EditorGUILayout.HelpBox("Quests Setting cannot be found in the scene.", MessageType.Error);
            return;
        }


        string[] questOptions = new string[(int)QuestType.LastItem];
        for (int i = 0; i < questOptions.Length; i++)
            questOptions[i] = settings.descText[i];


        if (questManager.quests == null)
        {
            questManager.quests = new Quest[1];
        }




        AddQuestButton(questManager);


        questManager.testMode = EditorGUILayout.ToggleLeft("Test Mode", questManager.testMode);
        GUILayout.Space(20);


        if (folds == null)
        folds = new bool[questManager.quests.Length];
        else if (folds.Length < questManager.quests.Length)
        {
            bool[] temp = folds;
            folds = new bool[questManager.quests.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                folds[i] = temp[i];
            }
        }

        string[] allTheOptions = new string[((int)QuestType.LastItem) + 1];
        allTheOptions[0] = "All";
        for (int i = 1; i < allTheOptions.Length; i++)
            allTheOptions[i] = settings.descText[i - 1];

        filterIndex = EditorGUILayout.Popup("Show Only", filterIndex + 1, allTheOptions) - 1;


        foldOut = EditorGUILayout.Foldout(foldOut, string.Format("Quests ({0})", questManager.quests.Length));
        if (foldOut)
        {
            for (int i = 0; i < questManager.quests.Length; i++)
            {
                if (questManager.quests[i] == null)
                    questManager.quests[i] = new Quest();


                if (filterIndex != -1 && questManager.quests[i].type != (QuestType)filterIndex)
                    continue;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                folds[i] = EditorGUILayout.Foldout(folds[i], settings.descText[(int)questManager.quests[i].type].Replace("*", questManager.quests[i].param.ToString()) +
                    string.Format(" [{0}]", questManager.quests[i].rewardGold));
                EditorGUILayout.EndHorizontal();
                

                if (folds[i])
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    questManager.quests[i].type = (QuestType)EditorGUILayout.Popup("Quest", (int)questManager.quests[i].type, questOptions);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    questManager.quests[i].param = EditorGUILayout.IntField("Param", questManager.quests[i].param);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    questManager.quests[i].rewardGold = EditorGUILayout.IntField("Gold Rewards", questManager.quests[i].rewardGold);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(40);
                    int state = -1;


                    if (questManager.currentQuestIndexes == null || questManager.currentQuestIndexes.Length < 3)
                        questManager.currentQuestIndexes = new int[3];


                    for (int k = 0; k < questManager.currentQuestIndexes.Length; k++)
                    {
                        if (questManager.currentQuestIndexes[k] == i)
                        {
                            state = k;
                            break;
                        }
                    }

                    int newState = EditorGUILayout.Popup("State", state + 1, states) - 1;
                    if (newState > -1)
                        questManager.currentQuestIndexes[newState] = i;
                    else if (state != -1)
                        questManager.currentQuestIndexes[state] = -1;

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }

                

                
            }
        }

        AddQuestButton(questManager);

        if (Accounting.Instance != null)
            ClearQuestProgressButton(questManager);


        if (questManager.quests != null && questManager.quests.Length > 0)
        {
            GUILayout.Space(50);
            EditorGUILayout.LabelField("Current Quests");

            for (int i = 0; i < 3; i++)
            {
                if (i >= questManager.quests.Length)
                    continue;

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(40);

                Quest q = questManager.quests[questManager.currentQuestIndexes[i]];

                string s = (i + 1).ToString() + "- " + settings.descText[(int)q.type].Replace("*", q.param.ToString());
                s += string.Format(" [{0:0}%]", q.ProgressPercent * 100);

                EditorGUILayout.LabelField(s);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }

        GUILayout.Space(50);


        filePath = EditorGUILayout.TextField("File", filePath);

        GUILayout.Space(20);

        if (questManager.quests != null && questManager.quests.Length > 0 && GUILayout.Button(System.Environment.NewLine + "Export Quests" + System.Environment.NewLine))
        {
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(filePath));
            if (!dir.Exists)
                dir.Create();

            StreamWriter writer = new StreamWriter(filePath);
            for (int i = 0; i < questManager.quests.Length; i++)
            {
                writer.WriteLine(questManager.quests[i].type);
                writer.WriteLine((int)questManager.quests[i].type);
                writer.WriteLine("param");
                writer.WriteLine(questManager.quests[i].param);
                writer.WriteLine("pts");
                writer.WriteLine(questManager.quests[i].rewardGold);
                writer.WriteLine("------------------");
            }
            writer.Close();

            Debug.Log(questManager.quests.Length + " quest(s) exported to [" + filePath + "]");
        }

        if (GUILayout.Button(System.Environment.NewLine + "Import Quests" + System.Environment.NewLine))
        {
            StreamReader reader = new StreamReader(filePath);

            int count = 0;
            while (true)
            {
                string s = reader.ReadLine();
                if (s == null)
                    break;

                int questType = int.Parse(reader.ReadLine());
                reader.ReadLine(); // param
                int param = int.Parse(reader.ReadLine());
                reader.ReadLine(); // pts
                int points = int.Parse(reader.ReadLine());

                reader.ReadLine(); // -------------

                Quest newQuest = new Quest();
                newQuest.type = (QuestType)questType;
                newQuest.param = param;
                newQuest.rewardGold = points;

                ArrayUtility.Add<Quest>(ref questManager.quests, newQuest);
                count++;
            }
            reader.Close();

            Debug.Log(count + " quest(s) imported from [" + filePath + "]");
        }

        if (questManager.quests != null && questManager.quests.Length > 0 && GUILayout.Button(System.Environment.NewLine + "Clear Quests" + System.Environment.NewLine))
        {
            questManager.quests = new Quest[0];
            Debug.Log("Quests Cleared");
        }
    }

    private static void AddQuestButton(QuestManager questManager)
    {
        GUILayout.Space(20);
        if (GUILayout.Button(System.Environment.NewLine + "Add a Quest" + System.Environment.NewLine))
        {
            Quest[] temp = questManager.quests;
            questManager.quests = new Quest[temp.Length + 1];
            for (int i = 0; i < temp.Length; i++)
            {
                questManager.quests[i] = temp[i];
            }
        }
        GUILayout.Space(20);
    }

    public void ClearQuestProgressButton(QuestManager questManager)
    {
        if (GUILayout.Button(System.Environment.NewLine + "Reset Quests" + System.Environment.NewLine))
        {
            questManager.ResetProgresses();
        }
        GUILayout.Space(20);
    }
}