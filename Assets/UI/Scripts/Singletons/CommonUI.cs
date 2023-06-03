using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonUI : MonoBehaviour 
{
    #region Singleton

    static CommonUI _instance;

    public static CommonUI Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (Accounting.Instance == null)
        {
            SceneManager.LoadScene(Scenes.Login);
            return;
        }
        else
            DontDestroyOnLoad(this.transform.parent.gameObject);



        if (_instance == null)
        {
            _instance = this;
            

            //DisableSystemUI.Run();



        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    #endregion



    public delegate void serverStateFetched(bool serversEnabled);
    public event serverStateFetched OnServerStateFetched;

    public static bool IsForcedAdOn;


    public Stores store;
    public AudioStruct audioPlayer;
    public Localization localization;
    public BuyMoneyWindow buyCoinsMenu;
    public MessageBox messageBox;
    public BlackScreen blackScreen;
    public MusicManager menuMusicManager;
    public RateUsWindow rateUsWindow;
    public LockOverlay lockOverlay;
    public Tutorial tutorial;
    public string appVersion;
    public List<RewardStruct> rewards = new List<RewardStruct>();
    public HeaderBar headerBar;
    public SceneControllerBase currentScene;
    public GameObject initialBlackScreen;


    bool isSignedIn;
    public bool IsSignedIn
    {
        get { return isSignedIn; }
        private set
        {
            isSignedIn = value;
        }
    }

    bool justSignedOut;
    public bool JustSignedOut
    {
        get { return justSignedOut; }
    }


    float lastRateOfferTime;


    bool isServerReady = true;
    public bool IsServerReady
    {
        get { return isServerReady; }
    }


    bool isTutorial;
    public bool IsTutorial
    {
        get { return isTutorial; }
        set { isTutorial = value; }
    }




    void Start()
    {
        GooglePlayGames.OurUtils.Logger.DebugLogEnabled = false;
        GooglePlayGames.OurUtils.Logger.WarningLogEnabled = false;


        initialBlackScreen.SetActive(true);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        messageBox.Deactivate();

        //isTutorial = true;
        
        //Screen.SetResolution(Screen.width, Screen.height, true);
        //PhotonNetwork.networkingPeer.MaximumTransferUnit = 500;
        //PhotonNetwork.networkingPeer.MaximumTransferUnit = 260;
        //PhotonNetwork.CrcCheckEnabled = true;
        //PhotonNetwork.GetPing();
        //PhotonNetwork.networkingPeer.RoundTripTime;
        CheckForServerState();
    }

    public void DestroyInitialBlackScreen()
    {
        //Debug.Log("destroyed");

        if (initialBlackScreen != null)
            Destroy(initialBlackScreen);
    }

    public void CheckForServerState()
    {
        Accounting.Instance.GetServerState(_ServerStateFetched, null);
    }

    private void _ServerStateFetched(bool isServerReady)
    {
        /*if (decodedText.StartsWith("systemerror"))
        {
            Debug.Log("error loading server state: " + decodedText);
            return;
        }


        //string version = decodedText;
        /*if (version != CommonUI.Instance.appVersion)
            CommonUI.Instance.question.ShowMessage(Messages.PleaseUpdate, _PleaseUpdate, true);
        else
        {
            if (OnServerStateFetched != null)
                OnServerStateFetched(isServerReady);
        }*/

        if (OnServerStateFetched != null)
            OnServerStateFetched(isServerReady);
    }

    private void _PleaseUpdate()
    {
        //LoginSceneController.IsUpdateNeeded = true;
        SceneManager.LoadScene(Scenes.Login);
    }


    [ContextMenu("Clear Player Prefs")]
    void clearPlayerData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player prefs cleared");
    }


    void OnLevelWasLoaded(int level)
    {
        SceneManager.SceneLoaded(level);
    }

    public void PlayButtonClick()
    {
        audioPlayer.Play(audioPlayer.buttonClick);
    }


    public void CheckForRateMessage()
    {
        if (!Data.IsRated)
        {

            if (Time.time - lastRateOfferTime >= GeneralSettings.Instance.rateOfferTime)
            {
                lastRateOfferTime = Time.time;
                rateUsWindow.Activate();
            }
        }
            
    }

    public void ShowRateWindow()
    {
        Data.IsRated = true;
        Application.OpenURL(StoreSpecificSettings.Instance.Setting.rateUsUrl);
    }

    
    public void SignedIn(string username, string password, string email, bool remember)
    {
        Data.LastLoginedUsername = username;
        Data.LastLoginedUsernamePassword = password;


        Data.UserName = username;
        Data.Email = email;

        SettingData.SetSoundSettings();
        Information.Instance.LoadPersonalData();

        localization.Refresh();

        IsSignedIn = true;
        SceneManager.LoadScene(Scenes.MainMenu);
    }


    public void SignOut()
    {
        justSignedOut = true;

        IsSignedIn = false;
        SceneManager.LoadScene(Scenes.Login);
    }


    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip buttonClick;
        public AudioClip loading;
        public AudioClip addingMoney;
    }



    /*void OnApplicationFocus()
    {
        DisableSystemUI.DisableNavUI();
    }*/




    public void DebugText(string text)
    {
        Debug.Log("IN-GAME DEBUG=" + text);
    }
}

public enum Stores
{
    CafeBazaar,
    IranApps,
    Myket,
    GooglePlay
}

public struct RewardStruct
{
    public int id;
    public int xp;
    public int money;
}