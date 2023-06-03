using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PushNotificator : MonoBehaviour
{
    public Text TXT;
    string notificationText = "Pushwoosh is not initialized";

    // Use this for initialization
    void Start()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            Pushwoosh.Instance.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
            Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
            Pushwoosh.Instance.OnPushNotificationsReceived += onPushNotificationsReceived;
        }
#endif
    }

    void onRegisteredForPushNotifications(string token)
    {
        TXT.text = "Received token: \n" + token;

        //do handling here
        Debug.Log(notificationText);
    }

    void onFailedToRegisteredForPushNotifications(string error)
    {
        TXT.text =  "Error ocurred while registering to push notifications: \n" + error;

        //do handling here
        Debug.Log(notificationText);
    }

    void onPushNotificationsReceived(string payload)
    {
        TXT.text = "Received push notificaiton: \n" + payload;

        //do handling here
        Debug.Log(notificationText);
    }
}