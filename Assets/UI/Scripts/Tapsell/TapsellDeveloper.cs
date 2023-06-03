using UnityEngine;
using System.Collections;
using System;

public class TapsellDeveloper 
{
	private TapsellObject tapsellObject;
	private static TapsellDeveloper instance;

#if UNITY_ANDROID
	private AndroidJavaObject tapsellDeveloperInfo;
#endif

	public static TapsellDeveloper getInstance()
    {
		if (instance == null) 
        {
			instance = new TapsellDeveloper ();
		}
		return instance;
	}

	public TapsellDeveloper()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellObject = new TapsellObject();
            AndroidJavaClass jc = new AndroidJavaClass("ir.tapsell.tapselldevelopersdk.developer.TapsellDeveloperInfo");
            tapsellDeveloperInfo = jc.CallStatic<AndroidJavaObject>("getInstance");
        }
#endif
	}

	public void setPurchaseNotifier(Action<String, String> action)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellObject.wrapper.setPurchaseNotifier(action);
        }
#endif
	}

	public void consumeProduct(String sku, Action<Boolean, Boolean> action)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("consumeProduct", sku);
            tapsellObject.wrapper.consumeProduct(sku, action);
        }
#endif
	}
	
	public void setKey(string key)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("setDeveloperKey", key);
        }
#endif
	}

	public void startTapsell()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.CallStatic("startActivity");
        }
        #endif
	}

	public void addHiddenSku(String sku)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("addHiddenSkua", sku);
        }
        #endif
	}

	public void removeHiddenSku(String sku)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("removeHiddenSku", sku);
        }
        #endif
	}

	public void isHiddenSku(String sku)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("isHiddenSku", sku);
        }
        #endif
	}

	public void setCurrentProduct(String sku)
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("setCurrentProduct", sku);
        }
        #endif
	}

	public void removeCurrentProduct()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            tapsellDeveloperInfo.Call("removeCurrentProduct");
        }
#endif
    }
}