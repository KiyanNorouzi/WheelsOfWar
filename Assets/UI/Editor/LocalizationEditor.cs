using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Localization))]
public class LocalizationEditor : Editor
{
    int languagesCount = -1;
    string message;
    MessageType messageType = MessageType.Info;

    bool[] foldouts;
    public override void OnInspectorGUI()
    {
        Localization localization = (Localization)target;
        if (languagesCount == -1)
            languagesCount = localization.languages.Length;


        if (localization.localizedObjects == null)
        {
            localization.localizedObjects = new LocalizedObject[languagesCount];
        }

        if (foldouts == null)
            foldouts = new bool[localization.localizedObjects.Length];


        GUILayout.Space(10);
        GameObject newItem = (GameObject)EditorGUILayout.ObjectField("Drag Items Here", null, typeof(GameObject), true);
        if (newItem != null)
        {
            ClearMessage();
            ShowMessage("Adding item [" + newItem.name + "]...");

            for (int i = 0; i < localization.localizedObjects.Length; i++)
            {
                for (int j = 0; j < localization.localizedObjects[i].gameobjects.Length; j++)
                {
                    if (newItem == localization.localizedObjects[i].gameobjects[j])
                    {
                        ShowMessage("Duplicate Item. Found in item #" + i + " for " +  localization.languages[j].title.ToString() + ", adding item failed", MessageType.Error);
                        foldouts[i] = true;
                        return;
                    }
                }
            }



            LocalizedObject newObject = new LocalizedObject();
            newObject.desc = newItem.name;
            newObject.gameobjects = new GameObject[languagesCount];


            
            

            bool foundLanguage = false;
            for (int i = 0; i < localization.languages.Length; i++)
            {
                if (newItem.name.ToUpper().EndsWith(localization.languages[i].shortString.ToUpper()))
                {
                    newObject.desc = newItem.name.Replace(localization.languages[i].shortString, "");
                    newObject.gameobjects[i] = newItem;

                    string nameFormat = newItem.name.Replace(localization.languages[i].shortString.ToUpper(), "{0}");

                    for (int j = 0; j < localization.languages.Length; j++)
                    {
                        if (j == i)
                            continue;

                        GameObject nn = GameObject.Find(nameFormat.Replace("{0}", localization.languages[j].shortString));
                        if (nn != null)
                        {
                            ShowMessage("searching for [" + nameFormat.Replace("{0}", localization.languages[j].shortString) + "] ... Found it!");
                            //Debug.Log();
                            newObject.gameobjects[j] = nn;
                        }
                        else
                        {
                            ShowMessage("searching for [" + nameFormat.Replace("{0}", localization.languages[j].shortString) + "] ... Came up Empty!");
                            Debug.Log("searching for [" + nameFormat.Replace("{0}", localization.languages[j].shortString) + "] ... Came up Empty!");
                        }
                    }

                    foundLanguage = true;
                    break;
                }
            }

            if (!foundLanguage)
            {
                newObject.gameobjects[0] = newItem;
                ShowMessage("Can't recognize the language. Put it under " + localization.languages[0].title.ToString(), MessageType.Warning);
            }
                

            ArrayUtility.Add<LocalizedObject>(ref localization.localizedObjects, newObject);
            ArrayUtility.Add<bool>(ref foldouts, true);
        }

        GUILayout.Space(30);


        if (!string.IsNullOrEmpty(message))
        {
            EditorGUILayout.HelpBox(message, messageType);
            GUILayout.Space(30);
        }
        

        for (int i = 0; i < localization.localizedObjects.Length; i++)
        {
            if (localization.localizedObjects == null)
                localization.localizedObjects = new LocalizedObject[0];

            EditorGUI.indentLevel = 1;


            Color c = Color.green;
            for (int l = 0; l < localization.localizedObjects[i].gameobjects.Length; l++)
            {
                if (localization.localizedObjects[i].gameobjects[l] == null)
                {
                    c = Color.red;
                    break;
                }
            }

            EditorGUILayout.BeginHorizontal();

            GUI.color = c;
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], string.Concat(i , "-" , localization.localizedObjects[i].desc));
            GUI.color = Color.white;

            if (GUILayout.Button("Delete"))
            {
                ArrayUtility.RemoveAt(ref localization.localizedObjects, i);
                ArrayUtility.RemoveAt(ref foldouts, i);
            }
                

            EditorGUILayout.EndHorizontal();

            if (foldouts[i])
            {
                EditorGUI.indentLevel = 2;
                localization.localizedObjects[i].desc = EditorGUILayout.TextField("Description", localization.localizedObjects[i].desc);
                GUILayout.Space(5);

                if (localization.localizedObjects[i].gameobjects == null || localization.localizedObjects[i].gameobjects.Length == 0)
                    localization.localizedObjects[i].gameobjects = new GameObject[languagesCount];

                if (localization.localizedObjects[i].gameobjects.Length < languagesCount)
                {
                    for (int k = 0; k < languagesCount - localization.localizedObjects[k].gameobjects.Length; k++)
                    {
                        ArrayUtility.Add<GameObject>(ref localization.localizedObjects[i].gameobjects, null);
                    }

                }

                for (int j = 0; j < localization.localizedObjects[i].gameobjects.Length; j++)
                {
                    localization.localizedObjects[i].gameobjects[j] = (GameObject)EditorGUILayout.ObjectField(localization.languages[j].title.ToString(), localization.localizedObjects[i].gameobjects[j], typeof(GameObject), true);
                }
            }

            //GUILayout.Space(10);
        }

        GUILayout.Space(50);

        if (GUILayout.Button("\nAdd Empty Item\n"))
        {
            LocalizedObject emptyLocalizedObject = new LocalizedObject();
            emptyLocalizedObject.gameobjects = new GameObject[languagesCount]; 

            ArrayUtility.Add<LocalizedObject>(ref localization.localizedObjects, emptyLocalizedObject);
            ArrayUtility.Add<bool>(ref foldouts, true);
        }

        GUILayout.Space(50);
    }

    private void ClearMessage()
    {
        message = "";
        messageType = MessageType.Info;
    }

    private void ShowMessage(string p)
    {
        if (!string.IsNullOrEmpty(message))
            message += System.Environment.NewLine;

        message += p;
    }

    private void ShowMessage(string p, MessageType type)
    {
        messageType = type;

        if (!string.IsNullOrEmpty(message))
            message += System.Environment.NewLine;

        message += p;
    }
}