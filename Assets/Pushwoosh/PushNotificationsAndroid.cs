using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PushNotificationsAndroid : MonoBehaviour 
{
#if UNITY_ANDROID

	public event Pushwoosh.RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event Pushwoosh.RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event Pushwoosh.NotificationHandler OnPushNotificationsReceived = delegate {};

	// Use this for initialization
	void Start()
    {
        if (Application.isEditor)
            return;

        InitPushwoosh();
		registerForPushNotifications();
		
		Debug.Log(this.gameObject.name);
		Debug.Log(getPushToken());

	}

	private static AndroidJavaObject pushwoosh = null;
	
	void InitPushwoosh() 
    {
        if (Application.isEditor)
            return;

		if(pushwoosh != null)
			return;
		
		using(var pluginClass = new AndroidJavaClass("com.arellomobile.android.push.PushwooshProxy")) {
			pluginClass.CallStatic("initialize", Pushwoosh.APP_CODE, Pushwoosh.GCM_PROJECT_NUMBER);
			pushwoosh = pluginClass.CallStatic<AndroidJavaObject>("instance");
		}
		
		pushwoosh.Call("setListenerName", this.gameObject.name);
	}
 
	public void setIntTag(string tagName, int tagValue)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setIntTag", tagName, tagValue);
	}

	public void registerForPushNotifications()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("registerForPushNotifications");
	}

	public void unregisterForPushNotifications()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("unregisterFromPushNotifications");
	}

	public void setStringTag(string tagName, string tagValue)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setStringTag", tagName, tagValue);
	}

	public void setListTag(string tagName, List<object> tagValues)
	{
        if (Application.isEditor)
            return;

		AndroidJavaObject tags = new AndroidJavaObject ("com.arellomobile.android.push.TagValues");

		foreach( var tagValue in tagValues )
		{
			tags.Call ("addValue", tagValue);
		}

		pushwoosh.Call ("setListTag", tagName, tags);
	}

	public String[] getPushHistory()
	{
        if (Application.isEditor)
            return null;

		AndroidJavaObject history = pushwoosh.Call<AndroidJavaObject>("getPushHistory");
		if (history.GetRawObject().ToInt32() == 0)
		{
			return new String[0];
		}
		
		String[] result = AndroidJNIHelper.ConvertFromJNIArray<String[]>(history.GetRawObject());
		history.Dispose();
		
		return result;
	}
	
	public void clearPushHistory()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("clearPushHistory");
	}

	public void sendLocation(double lat, double lon)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("sendLocation", lat, lon);
	}

	public void startTrackingGeoPushes()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("startTrackingGeoPushes");
	}

	public void stopTrackingGeoPushes()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("stopTrackingGeoPushes");
	}
	
	public void startTrackingBeaconPushes()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("startTrackingBeaconPushes");
	}

	public void stopTrackingBeaconPushes()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("stopTrackingBeaconPushes");
	}

	public void setBeaconBackgroundMode(bool backgroundMode)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setBeaconBackgroundMode", backgroundMode);
	}
	
	public void clearLocalNotifications()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("clearLocalNotifications");
	}

	public void clearNotificationCenter()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("clearNotificationCenter");
	}

	public int scheduleLocalNotification(string message, int seconds)
	{
        if (Application.isEditor)
            return 0;

		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds);
	}

	public int scheduleLocalNotification(string message, int seconds, string userdata)
	{
        if (Application.isEditor)
            return 0;

		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds, userdata);
	}

	public void clearLocalNotification(int id)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("clearLocalNotification", id);
	}
	
	public void setMultiNotificationMode()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setMultiNotificationMode");
	}

	public void setSimpleNotificationMode()
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setSimpleNotificationMode");
	}

	/* 
	 * Sound notification types:
	 * 0 - default mode
	 * 1 - no sound
	 * 2 - always
	 */
	public void setSoundNotificationType(int soundNotificationType)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setSoundNotificationType", soundNotificationType);
	}

	/* 
	 * Vibrate notification types:
	 * 0 - default mode
	 * 1 - no vibrate
	 * 2 - always
	 */
	public void setVibrateNotificationType(int vibrateNotificationType)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setVibrateNotificationType", vibrateNotificationType);
	}

	public void setLightScreenOnNotification(bool lightsOn)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setLightScreenOnNotification", lightsOn);
	}

	public void setEnableLED(bool ledOn)
	{
        if (Application.isEditor)
            return;

		pushwoosh.Call("setEnableLED", ledOn);
	}
	
	public string getPushToken()
	{
        if (Application.isEditor)
            return "";

		return pushwoosh.Call<string>("getPushToken");
	}

	public string getPushwooshHWID()
	{
        if (Application.isEditor)
            return "";

		return pushwoosh.Call<string>("getPushwooshHWID");
	}

	void onRegisteredForPushNotifications(string token)
	{
		OnRegisteredForPushNotifications (token);
	}

	void onFailedToRegisteredForPushNotifications(string error)
	{
		OnFailedToRegisteredForPushNotifications (error);
	}

	void onPushNotificationsReceived(string payload)
	{
		OnPushNotificationsReceived (payload);
	}

	void OnApplicationPause(bool paused)
    {
        if (Application.isEditor)
            return;

        //make sure everything runs smoothly even if pushwoosh is not initialized yet
		if (pushwoosh == null)
			InitPushwoosh();

		if(paused)
		{
			pushwoosh.Call("onPause");
		}
		else
		{
			pushwoosh.Call("onResume");
		}
	}
#endif
}
