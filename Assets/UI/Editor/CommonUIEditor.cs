using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomEditor(typeof(CommonUI))]
public class CommonUIEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(30);

        CommonUI c = (CommonUI)target;


        string androidDir = Application.dataPath + "/Plugins/Android";
        string manifestsDir = androidDir + "/Manifest Files/";


        //EditorGUILayout.LabelField(androidDir);
        //EditorGUILayout.LabelField(manifestsDir);

        
        if (GUILayout.Button("\r\n\r\nChange Manifest File to \r\n" + c.store.ToString() + "\r\n\r\n"))
        {
            string[] manifestFileNames = new string[] { "AndroidManifest [CafeBazaar].xml", "AndroidManifest [IranApps].xml", "AndroidManifest [Myket].xml", "AndroidManifest[GooglePlay].xml" };
            string thisStoreFile = manifestsDir + manifestFileNames[(int)c.store];

            File.Copy(androidDir + "/AndroidManifest.xml", androidDir + "/~AndroidManifest.xml", true);
            File.Copy(thisStoreFile, androidDir + "/AndroidManifest.xml", true);

            Debug.Log("Manifest File successfullt changed to " + manifestFileNames[(int)c.store]);
        }

        GUILayout.Space(20);
    }
}