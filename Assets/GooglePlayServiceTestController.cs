using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class GooglePlayServiceTestController : MonoBehaviour 
{
    public Text signedInText, gmailText, deviceIDText, logText;
    public Button checkButton;


    void Awake()
    {
        checkButton.interactable = false;

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();


        PlayGamesClientConfiguration c = new PlayGamesClientConfiguration();
        PlayGamesPlatform.InitializeInstance(c);
    }

    public bool IsSignedInGooglePlay()
    {
        //return Social.Active.localUser.authenticated;
        return PlayGamesPlatform.Instance.localUser.authenticated;
    }

    public string GetGooglePlayID()
    {
        return Social.localUser.userName;
        //return PlayGamesPlatform.Instance.GetUserEmail();
    }

    public string GetDeviceID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }


    void Update()
    {
        if (PlayGamesPlatform.Instance != null)
        {
            checkButton.interactable = true;
            enabled = false;
        }
    }


    public void Check_Click()
    {
        signedInText.text = (IsSignedInGooglePlay()) ? "Signed In" : "Not Signed In";
        gmailText.text = GetGooglePlayID();
        deviceIDText.text = GetDeviceID();

        if (!IsSignedInGooglePlay())
        {
            logText.text = "Try to authenticate...";
            PlayGamesPlatform.Instance.localUser.Authenticate(_LoginDone);
            logText.text = "Try to authenticateS...";
        }
    }

    private void _LoginDone(bool isSignedIn)
    {
        logText.text = "Authenticate done, bool=" + isSignedIn;
        Check_Click();
    }
}
