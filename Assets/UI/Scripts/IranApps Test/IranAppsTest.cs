using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IranAppsTest : MonoBehaviour 
{
    #region Singleton

    static IranAppsTest _instance;

    public static IranAppsTest Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion


    public Text text;

    public void Button_Click()
    {
        text.text = "attemp to buy...";
        InAppPurchases.Instance.Request(0, InAppSuccessfullyDone);
        text.text += "after attemp to buy...";
    }

    void InAppSuccessfullyDone(int index, bool successfull)
    {
        if (successfull)
            text.text += "inapp successfully done";
        else
            text.text += "inapp failed";
    }
}