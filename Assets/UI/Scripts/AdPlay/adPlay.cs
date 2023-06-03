using UnityEngine;
using System.Collections;
using System;
namespace ir.adPlay.plugin
{


    public class adPlay : MonoBehaviour
    {
#if UNITY_ANDROID

        private static string applicationId;
        private static string developerId;
        private static GameObject adPlayObject = null;
        private static AndroidJavaClass UniyInterface = null;
        public static Action myOnVideoComplete;
        public static Action myOnInstallationComplete;
        public static Action myonAdAvailable;
        public static Action myonAdFail;
        void Awake()
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;

            DontDestroyOnLoad(this.gameObject);
            AndroidJNI.AttachCurrentThread();
            UniyInterface = new AndroidJavaClass("ir.adPlay.Unity.UniyInterface");
            AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
            UniyInterface.CallStatic("init", activity, applicationId, developerId);

        }
        void Start()
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            doAutoRequestAndShowJob();
        }
        void OnLevelWasLoaded(int level)
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            doAutoRequestAndShowJob();

        }

        public static void init(string _applicationId, string _developerId, Action _onVideoCompleteListener, Action _onInstallationCompleteListener, Action _onAdAvailable, Action _onAdFail)
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;

            if (adPlayObject != null) return;
            developerId = _developerId;
            applicationId = _applicationId;
            myOnVideoComplete = _onVideoCompleteListener;
            myOnInstallationComplete = _onInstallationCompleteListener;
            myonAdAvailable = _onAdAvailable;
            myonAdFail = _onAdFail;
            adPlayObject = new GameObject();
            adPlayObject.name = "adPlay";
            GameObject.DontDestroyOnLoad(adPlayObject);
             adPlayObject.AddComponent<adPlay>();
            


        }
        public static void setAutoDialogueDisplay(bool enable)
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            if (adPlay.adPlayObject == null) return;
            UniyInterface.CallStatic("setAutoDialogueDisplay", enable);

        }
        public static void setAutoDownlaodContents(bool enable)
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            if (adPlay.adPlayObject == null) return;
            UniyInterface.CallStatic("setAutoDownlaodContents", enable);
        }
        private static void doAutoRequestAndShowJob()
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            if (adPlay.adPlayObject == null) return;
            UniyInterface.CallStatic("doAutoRequestAndShowJob");
        }
        public static void showAdIfAvailable(bool showDialogue)
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            if (adPlay.adPlayObject == null) return;
            UniyInterface.CallStatic("showAdIfAvailable", showDialogue);
        }
        public static void setTestMode(bool enable)
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return;
            if (adPlay.adPlayObject == null) return;
            UniyInterface.CallStatic("setTestMode", enable);
        }
        public static bool checkAdAvailibility()
        {
            if (!((SystemInfo.operatingSystem.Contains("android") || SystemInfo.operatingSystem.Contains("Android")))) return false;
            if (adPlay.adPlayObject == null) return false;
            return UniyInterface.CallStatic<bool>("checkAdAvailibility");
        }
        private void onVideoComplete(string s)
        {
            myOnVideoComplete();
        }
        private void onInstallationComplete(string s)
        {
            myOnInstallationComplete();
        }
        private void onAdAvailable(string s)
        {
            myonAdAvailable();
        }
        private void onAdFail(string s)
        {
            myonAdFail();
        }
#else
        public static void init(string _applicationId, string _developerId, Action _onVideoCompleteListener, Action _onInstallationCompleteListener, Action _onAdAvailable, Action _onAdFail)
        {
        }
        public static void setAutoDialogueDisplay(bool enable)
        {
        }
        public static void setAutoDownlaodContents(bool enable)
        {
        }
        public static void showAdIfAvailable(bool showDialogue)
        {
        }
        public static void setTestMode(bool enable)
        {
        }
        public static bool checkAdAvailibility()
        {
            return false;
        }

#endif
    }
}