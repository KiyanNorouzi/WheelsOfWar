using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(Tutorial))]
public class TutorialEditor : Editor
{
    string fileUrl = "C:\\wheelsofwar\\tutorial_frames.txt";
    bool[] foldout;
    int selectedIndex = -1;
    
    public override void OnInspectorGUI()
    {
        Tutorial t = (Tutorial)target;



        if (GUILayout.Button("Add Empty Frame"))
        {
            TutorialFrameInfo thisFrame = new TutorialFrameInfo();
            thisFrame.tag = string.Concat("Frame " + t.info.Length + " [Empty]");
            ArrayUtility.Add<TutorialFrameInfo>(ref t.info, thisFrame);
        }

        if (GUILayout.Button("Capture Current State into a frame"))
        {
            TutorialFrameInfo thisFrame = t.GetFrameInfoFromScene();
            thisFrame.tag = string.Concat("Frame " + t.info.Length);
            ArrayUtility.Add<TutorialFrameInfo>(ref t.info, thisFrame);
        }



        if (foldout == null || foldout.Length == 0)
        {
            foldout = new bool[t.info.Length];

            for (int i = 0; i < foldout.Length; i++)
                foldout[i] = (i == t.editingFrameIndex);
        }
        else if (foldout.Length < t.info.Length)
        {
            int diff = t.info.Length - foldout.Length;
            for (int i = 0; i < diff; i++)
                ArrayUtility.Add<bool>(ref foldout, false);
        }




        
        for (int i = 0; i < t.info.Length; i++)
        {
            TutorialFrameInfo thisFrame = t.info[i];
            EditorGUI.indentLevel = 1;

            bool prevValue = foldout[i];
            foldout[i] = EditorGUILayout.Foldout(prevValue, t.info[i].tag);

            if (foldout[i] && !prevValue)
            {
                selectedIndex = i;

                for (int j = 0; j < foldout.Length; j++)
                    foldout[j] = (j == selectedIndex);
            }

            if (prevValue && !foldout[i])
            {
                t.editingFrameIndex = -1;
                selectedIndex = -1;
            }


            if (foldout[i])
            {
                EditorGUI.indentLevel = 2;
                thisFrame.tag = EditorGUILayout.TextField("Tag", thisFrame.tag);

                GUILayout.Space(10);

                thisFrame.IsBarbodActive = EditorGUILayout.ToggleLeft("Barbod Active", thisFrame.IsBarbodActive);
                if (thisFrame.IsBarbodActive)
                {
                    EditorGUI.indentLevel = 3;
                    thisFrame.barbodPosition = EditorGUILayout.Vector2Field("Position", thisFrame.barbodPosition);
                    thisFrame.barbodRotation.eulerAngles = EditorGUILayout.Vector3Field("Rotation", thisFrame.barbodRotation.eulerAngles);
                    thisFrame.barbodScale = EditorGUILayout.Vector3Field("Scale", thisFrame.barbodScale);
                }

                GUILayout.Space(10);

                EditorGUI.indentLevel = 2;
                thisFrame.IsBaloonActive = EditorGUILayout.ToggleLeft("Baloon Active", thisFrame.IsBaloonActive);
                if (thisFrame.IsBaloonActive)
                {
                    EditorGUI.indentLevel = 3;
                    EditorGUILayout.LabelField("TEXT_FA");
                    thisFrame.text = EditorGUILayout.TextArea(thisFrame.text);
                    EditorGUILayout.LabelField("TEXT_EN");
                    thisFrame.text_En = EditorGUILayout.TextArea(thisFrame.text_En);


                    GUILayout.Space(5);

                    thisFrame.baloonPosition = EditorGUILayout.Vector2Field( "Position", thisFrame.baloonPosition);
                    thisFrame.baloonRotation.eulerAngles = EditorGUILayout.Vector3Field("Rotation", thisFrame.baloonRotation.eulerAngles);
                    thisFrame.baloonScale = EditorGUILayout.Vector3Field("Scale", thisFrame.baloonScale);

                    GUILayout.Space(5);

                    thisFrame.baloonPointerPos = EditorGUILayout.Vector2Field("Pointer Position", thisFrame.baloonPointerPos);
                    thisFrame.baloonPointerRotation.eulerAngles = EditorGUILayout.Vector3Field("Pointer Rotation", thisFrame.baloonPointerRotation.eulerAngles);
                    thisFrame.baloonPointerScale = EditorGUILayout.Vector3Field("Pointer Scale", thisFrame.baloonPointerScale);
                }

                GUILayout.Space(10);

                EditorGUI.indentLevel = 2;
                thisFrame.IsHighlightRectActive = EditorGUILayout.ToggleLeft("Highlight Rect", thisFrame.IsHighlightRectActive);
                if (thisFrame.IsHighlightRectActive)
                {
                    EditorGUI.indentLevel = 3;
                    thisFrame.RectPosition = EditorGUILayout.Vector2Field("Position", thisFrame.RectPosition);
                    thisFrame.RectSize = EditorGUILayout.Vector2Field("Size", thisFrame.RectSize);
                    thisFrame.AcceptClickOnlyInHighlightArea = EditorGUILayout.Toggle("Accept Clicks only in highlighted area", thisFrame.AcceptClickOnlyInHighlightArea);
                }
                GUILayout.Space(20);



                //t.info[i] = thisFrame;

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(System.Environment.NewLine +  "Load to Scene" + System.Environment.NewLine))
                {
                    t.editingFrameIndex = i;
                    t.LoadFrameToScene(t.info[i]);
                }

                GUILayout.Space(10);

                if (GUILayout.Button(System.Environment.NewLine + "Save from Scene" + System.Environment.NewLine))
                {
                    string tag = t.info[i].tag;
                    t.info[i] = t.GetFrameInfoFromScene();
                    t.info[i].tag = tag;

                    /*TutorialFrameInfo newInfo = t.GetFrameInfoFromScene();
                    t.info[i].IsBarbodActive = newInfo.IsBarbodActive;
                    t.info[i].barbodPosition = newInfo.barbodPosition;
                    t.info[i].barbodRotation = newInfo.barbodRotation;
                    t.info[i].barbodScale = newInfo.barbodScale;
                    t.info[i].IsBaloonActive = newInfo.IsBaloonActive;
                    t.info[i].baloonPosition = newInfo.baloonPosition;
                    t.info[i].baloonRotation = newInfo.baloonRotation;
                    t.info[i].baloonScale = newInfo.baloonScale;
                    t.info[i].text = newInfo.text;
                    t.info[i].IsHighlightRectActive = newInfo.IsHighlightRectActive;
                    t.info[i].RectPosition = newInfo.RectPosition;
                    t.info[i].RectSize = newInfo.RectSize;*/
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.Space(15);
            }
        }



        GUILayout.Space(120);
        //EditorGUILayout.LabelField("Selected Index", selectedIndex.ToString());
        //GUILayout.Space(10);


        fileUrl = EditorGUILayout.TextField("File", fileUrl);
        if (GUILayout.Button("Save File"))
        {
            Save(fileUrl, t);
        }

        if (GUILayout.Button("Load from File and Add to frames"))
        {
            TutorialFrameInfo[] newFrames = Load(fileUrl);
            for (int i = 0; i < newFrames.Length; i++)
            {
                ArrayUtility.Add<TutorialFrameInfo>(ref t.info, newFrames[i]);
            }
        }

        if (!sureToClear)
        {
            if (GUILayout.Button("Clear"))
                sureToClear = true;
        }
        else
        {
            GUI.color = Color.red;
            if (GUILayout.Button("Sure to Clear?"))
            {
                sureToClear = false;
                t.info = new TutorialFrameInfo[0];
            }

            GUI.color = Color.white;
        }
    }

    bool sureToClear;


    Vector4 QuaternionToVector4(Quaternion rot) 
    {
		return new Vector4(rot.x, rot.y, rot.z, rot.w);
	}

    Quaternion Vector4ToQuaternion(Vector4 v)
    {
        return new Quaternion(v.x, v.y, v.z, v.w);
    }



    public TutorialFrameInfo[] Load(string fileUrl)
    {
        List<TutorialFrameInfo> result = new List<TutorialFrameInfo>();

        StreamReader reader = new StreamReader(fileUrl);

        while (!reader.EndOfStream)
        {
            string tagText = reader.ReadLine();
            if (tagText == null)
                break;

            TutorialFrameInfo thisFrame = new TutorialFrameInfo();

            thisFrame.tag = reader.ReadLine();

            reader.ReadLine(); //barbod active
            thisFrame.IsBarbodActive = (reader.ReadLine() == "1");

            reader.ReadLine(); //barbod pos
            thisFrame.barbodPosition.x = float.Parse(reader.ReadLine());
            thisFrame.barbodPosition.y = float.Parse(reader.ReadLine());

            reader.ReadLine(); //barbod rot
            Vector3 eulerAngles = new Vector3();
            eulerAngles.x = float.Parse(reader.ReadLine());
            eulerAngles.y = float.Parse(reader.ReadLine());
            eulerAngles.z = float.Parse(reader.ReadLine());
            thisFrame.barbodRotation.eulerAngles = eulerAngles;

            reader.ReadLine(); //barbod scale
            thisFrame.barbodScale.x = float.Parse(reader.ReadLine());
            thisFrame.barbodScale.y = float.Parse(reader.ReadLine());
            thisFrame.barbodScale.z = float.Parse(reader.ReadLine());

            reader.ReadLine(); //baloon active
            thisFrame.IsBaloonActive = (reader.ReadLine().Trim() == "1");
            //reader.WriteLine(t.info[i].IsBaloonActive ? 1 : 0);

            reader.ReadLine(); //baloon pos
            thisFrame.baloonPosition.x = float.Parse(reader.ReadLine());
            thisFrame.baloonPosition.y = float.Parse(reader.ReadLine());

            reader.ReadLine(); //baloon rot
            eulerAngles = new Vector3();
            eulerAngles.x = float.Parse(reader.ReadLine());
            eulerAngles.y = float.Parse(reader.ReadLine());
            eulerAngles.z = float.Parse(reader.ReadLine());
            thisFrame.baloonRotation.eulerAngles = eulerAngles;

            reader.ReadLine(); //baloon scale
            thisFrame.baloonScale.x = float.Parse(reader.ReadLine());
            thisFrame.baloonScale.y = float.Parse(reader.ReadLine());
            thisFrame.baloonScale.z = float.Parse(reader.ReadLine());


            reader.ReadLine(); //baloon pointer pos
            thisFrame.baloonPointerPos.x = float.Parse(reader.ReadLine());
            thisFrame.baloonPointerPos.y = float.Parse(reader.ReadLine());

            reader.ReadLine(); //baloon pointer rot

            eulerAngles = new Vector3();
            eulerAngles.x = float.Parse(reader.ReadLine());
            eulerAngles.y = float.Parse(reader.ReadLine());
            eulerAngles.z = float.Parse(reader.ReadLine());
            thisFrame.baloonPointerRotation.eulerAngles = eulerAngles;
            //thisFrame.baloonPointerRotation.y = float.Parse(reader.ReadLine());
            //thisFrame.baloonPointerRotation.z = float.Parse(reader.ReadLine());

            reader.ReadLine(); //baloon pointer scale
            thisFrame.baloonPointerScale.x = float.Parse(reader.ReadLine());
            thisFrame.baloonPointerScale.y = float.Parse(reader.ReadLine());
            thisFrame.baloonPointerScale.z = float.Parse(reader.ReadLine());


            reader.ReadLine(); //text-fa

            do
            {
                string temp = reader.ReadLine();
                if (temp == "***EOT")
                    break;

                if (!string.IsNullOrEmpty(thisFrame.text))
                    thisFrame.text += System.Environment.NewLine;

                thisFrame.text += temp;
            } while (true);


            reader.ReadLine(); //text-en

            do
            {
                string temp = reader.ReadLine();
                if (temp == "***TOV")
                    break;

                if (!string.IsNullOrEmpty(thisFrame.text_En))
                    thisFrame.text_En += System.Environment.NewLine;

                thisFrame.text_En += temp;
            } while (true);


            reader.ReadLine(); //highlight active
            thisFrame.IsHighlightRectActive = (reader.ReadLine().Trim() == "1");

            reader.ReadLine(); //rect pos
            thisFrame.RectPosition.x = float.Parse(reader.ReadLine());
            thisFrame.RectPosition.y = float.Parse(reader.ReadLine());

            reader.ReadLine(); //rect size
            thisFrame.RectSize.x = float.Parse(reader.ReadLine());
            thisFrame.RectSize.y = float.Parse(reader.ReadLine());

            reader.ReadLine(); // --------------------------


            result.Add(thisFrame);
        }

        reader.Close();
        Debug.Log(result.Count + " frame(s) succefully imported from " + fileUrl);

        return result.ToArray();
    }



    public void Save(string fileUrl, Tutorial t)
    {
        StreamWriter writer = new StreamWriter(fileUrl);

        for (int i = 0; i < t.info.Length; i++)
        {
            writer.WriteLine("tag");
            writer.WriteLine(t.info[i].tag);

            writer.WriteLine("barbod active");
            writer.WriteLine(t.info[i].IsBarbodActive?1:0);

            writer.WriteLine("barbod pos");
            writer.WriteLine(t.info[i].barbodPosition.x);
            writer.WriteLine(t.info[i].barbodPosition.y);

            writer.WriteLine("barbod rot");
            writer.WriteLine(t.info[i].barbodRotation.eulerAngles.x);
            writer.WriteLine(t.info[i].barbodRotation.eulerAngles.y);
            writer.WriteLine(t.info[i].barbodRotation.eulerAngles.z);

            Debug.Log("barbod scale save=" + t.info[i].barbodScale);
            writer.WriteLine("barbod scale");
            writer.WriteLine(t.info[i].barbodScale.x);
            writer.WriteLine(t.info[i].barbodScale.y);
            writer.WriteLine(t.info[i].barbodScale.z);

            writer.WriteLine("baloon active");
            writer.WriteLine(t.info[i].IsBaloonActive ? 1 : 0);

            writer.WriteLine("baloon pos");
            writer.WriteLine(t.info[i].baloonPosition.x);
            writer.WriteLine(t.info[i].baloonPosition.y);

            writer.WriteLine("baloon rot");
            writer.WriteLine(t.info[i].baloonRotation.eulerAngles.x);
            writer.WriteLine(t.info[i].baloonRotation.eulerAngles.y);
            writer.WriteLine(t.info[i].baloonRotation.eulerAngles.z);

            writer.WriteLine("baloon scale");
            writer.WriteLine(t.info[i].baloonScale.x);
            writer.WriteLine(t.info[i].baloonScale.y);
            writer.WriteLine(t.info[i].baloonScale.z);

            writer.WriteLine("baloon pointer pos");
            writer.WriteLine(t.info[i].baloonPointerPos.x);
            writer.WriteLine(t.info[i].baloonPointerPos.y);

            writer.WriteLine("baloon pointer rot");
            writer.WriteLine(t.info[i].baloonPointerRotation.eulerAngles.x);
            writer.WriteLine(t.info[i].baloonPointerRotation.eulerAngles.y);
            writer.WriteLine(t.info[i].baloonPointerRotation.eulerAngles.z);

            writer.WriteLine("baloon pointer scale");
            writer.WriteLine(t.info[i].baloonPointerScale.x);
            writer.WriteLine(t.info[i].baloonPointerScale.y);
            writer.WriteLine(t.info[i].baloonPointerScale.z);



            writer.WriteLine("text");
            writer.WriteLine(t.info[i].text);
            writer.WriteLine("***EOT");

            writer.WriteLine("text_en");
            writer.WriteLine(t.info[i].text_En);
            writer.WriteLine("***TOV");

            writer.WriteLine("highlight active");
            writer.WriteLine(t.info[i].IsHighlightRectActive ? 1 : 0);

            writer.WriteLine("rect pos");
            writer.WriteLine(t.info[i].RectPosition.x);
            writer.WriteLine(t.info[i].RectPosition.y);

            writer.WriteLine("rect size");
            writer.WriteLine(t.info[i].RectSize.x);
            writer.WriteLine(t.info[i].RectSize.y);


            writer.WriteLine("--------------------------");

            //t.info[i].IsBarbodActive = newInfo.IsBarbodActive;
            //t.info[i].barbodPosition = newInfo.barbodPosition;
            //t.info[i].barbodRotation = newInfo.barbodRotation;
            //t.info[i].barbodScale = newInfo.barbodScale;
            //t.info[i].IsBaloonActive = newInfo.IsBaloonActive;
            //t.info[i].baloonPosition = newInfo.baloonPosition;
            //t.info[i].baloonRotation = newInfo.baloonRotation;
            //t.info[i].baloonScale = newInfo.baloonScale;
            //t.info[i].text = newInfo.text;
            //t.info[i].IsHighlightRectActive = newInfo.IsHighlightRectActive;
            //t.info[i].RectPosition = newInfo.RectPosition;
            //t.info[i].RectSize = newInfo.RectSize;
        }

        writer.Close();
        Debug.Log(t.info.Length + " frame(s) succefully exported to " + fileUrl);
    }
}
