using UnityEngine;

public class SampleUI
{
    public static void PrintText(string text)
    {
        CommonUI.Instance.DebugText(text);
        Debug.Log("IranAPPS: " + text);
        
    }
}
