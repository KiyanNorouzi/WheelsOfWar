using UnityEngine;
using System;
using System.Collections.Generic;

public class DeveloperCtaInterface
{
	public static int ALL_AD = 0;
	public static int PAY_PER_INSTALL = 1;
	public static int PAY_PER_CHARGE = 2;
	public static int VIDEO_PLAY = 3;
	public static int WEB_VIEW = 4;

	Dictionary<int, Action<Boolean, Boolean>> actionPool = new Dictionary<int, Action<Boolean, Boolean>>();
	Action<Boolean, Boolean, int> directAdAction;

	int MAX_TYPE_COUNT = 100;
	static DeveloperCtaInterface instance;
	
#if UNITY_ANDROID
    AndroidJavaObject developerCtaInterface;
#endif

	public static DeveloperCtaInterface getInstance(){
		if (instance == null) {
			instance = new DeveloperCtaInterface ();
			instance.setJavaObject();
		}
		return instance;
	}
	
	public void setJavaObject()
    {
        
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
		    AndroidJavaClass jc = new AndroidJavaClass("ir.tapsell.tapselldevelopersdk.developer.DeveloperCtaInterface");
		    developerCtaInterface = jc.CallStatic<AndroidJavaObject>("getInstance");
        }
#endif
	}

	public void checkCtaAvailability(int type, int minimumAward, Boolean isDirect, Action<Boolean, Boolean> action)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
		    developerCtaInterface.Call("checkCtaAvailability", type, minimumAward, isDirect);
		    if (actionPool.ContainsKey (minimumAward * MAX_TYPE_COUNT + type))
			    actionPool.Remove (minimumAward * MAX_TYPE_COUNT + type);
		    actionPool.Add (minimumAward * MAX_TYPE_COUNT + type, action);
        }
#endif
	}

	public void showNewCta(int type, int minimumAward, Action<Boolean, Boolean, int> action)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            developerCtaInterface.Call("showNewCta", type, minimumAward);
            directAdAction = action;
        }
#endif
    }

	public void notifyCtaAvailability(String ans){
		Boolean first, second;
		if (ans.Length == 0) {
			return;
		}
		if (ans [0] == 't')
			first = true;
		else
			first = false;
		if (ans [1] == 't')
			second = true;
		else
			second = false;
		String str = ans.Substring (2);
		Int32 key = Int32.Parse(str);
		if (actionPool.ContainsKey(key)){
			actionPool[key](first, second);
		}
	}

	public void notifyDirectAd(String ans){
		Boolean first, second;
		if (ans.Length == 0) {
			directAdAction(false, false, 0);
			return;
		}
		if (ans [0] == 't')
			first = true;
		else
			first = false;
		if (ans [1] == 't')
			second = true;
		else
			second = false;
		String str = ans.Substring (2);
		Int32 award = Int32.Parse(str);
		directAdAction(first, second, award);
	}
}
