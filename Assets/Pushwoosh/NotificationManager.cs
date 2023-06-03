using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour 
{
    #region Singleton

    static NotificationManager _instance;

    public static NotificationManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion 

    public PushNotificationsAndroid android;
    public string[] carRepairTexts;


	public int SetNotification(string text, int delaySeconds)
    {
#if UNITY_ANDROID
        return android.scheduleLocalNotification(text, delaySeconds);
#endif

        return -1;
    }

    public int SetRepairNotificationForCar(int carIndex, int seconds)
    {
        //string text = string.Format(carRepairTexts[carIndex], seconds);
        return SetNotification(carRepairTexts[carIndex], seconds);
    }

    public void CancelNotification(int notifIndex)
    {
#if UNITY_ANDROID
        android.clearLocalNotification(notifIndex);
#endif
    }
}