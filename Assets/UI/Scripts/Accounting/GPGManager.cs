
 using UnityEngine;
 using System.Collections;
 using GooglePlayGames;
 using UnityEngine.SocialPlatforms;

public class GPGManager : MonoBehaviour
{

    string status = "Authenticating...";

    // Use this for initialization
    void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }


    void Start()
    {
        PlayGamesPlatform.Activate();
        //Social.localUser.Authenticate(ProcessAuthentiation);
        Social.Active.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
               Debug.Log ("Signed In Successfuly");
            }
            else
            {
                Debug.Log("Failed To Sign In");
            }
        });
    }
}