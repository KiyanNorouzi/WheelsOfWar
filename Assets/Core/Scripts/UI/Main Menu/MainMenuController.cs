using UnityEngine;
using System.Collections;

public class MainMenuController : SceneControllerBase
{
    #region Singleton

    static MainMenuController _instance;

    public static MainMenuController Instance
    {
        get { return _instance; }
    }

    protected override void Awake()
    {
        base.Awake();

        if (_instance == null)
            _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion 


    static int lastLevel = -1;

    public static int coinsForShow;
    public static int scoreForShow;
    public static int menuShowTime;


    [System.Serializable]
    public class AudioStruct: AudioStructBase
    {
        public AudioClip startButton;
        
    }


    public AudioStruct audioPlayer;
    public float musicVolume;


    public OptionsMenu optionsMenu;
    public CreditsMenu creditsMenu;
    public GameObject sampleCar;
  

    public GameObject[] carPrefabs;
    public Transform carParent;
    public Animator[] carCrashAnimators;

    public GameObject carParentGameObject;
    public GameObject serverUnderMaintenanceGameObject; //, gearIconGameObject;
    public UnityEngine.UI.Button[] playButton;
    public GameObject leaderboardMenu;
    public GameObject mainmenuTutorial;
    public DailyGiftWindow dailyGiftWindow;
    public RewardWindow rewardWindow;
    public QuestsWindow questsWindow;
    public UnityEngine.UI.Text questsText;
    public DeliveryPanel deliveryPanel;
    public OfferPanel offerPanel;
    public GameObject debugMode;

    IEnumerator Start()
    {

        debugMode.SetActive(Debug.isDebugBuild);
        if (CommonUI.Instance == null)
        {
            SceneManager.LoadGame(Scenes.MainMenu);
            yield break;
        }

        CommonUI.Instance.headerBar.Enable();
        menuShowTime++;

        for (int i = 0; i < carCrashAnimators.Length; i++)
            carCrashAnimators[i].enabled = false;

        dailyGiftWindow.Deactivate();
        questsWindow.Deactivate();



        carParentGameObject.SetActive(false);
        leaderboardMenu.gameObject.SetActive(false);

        if (sampleCar != null)
            sampleCar.SetActive(false);

        GameObject g = ((GameObject)Instantiate(carPrefabs[Accounting.Instance.currentUser.SelectedCarIndex]));
        CosmeticPerCars cosmetic = g.GetComponent<CosmeticPerCars>();

        CarType carType = (CarType)Accounting.Instance.currentUser.SelectedCarIndex;
        cosmetic.ColorIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.COLOR_SIDE);
        cosmetic.SideBackIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.BACK_SIDE);
        cosmetic.SideFrontIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.FRONT_SIDE);
        cosmetic.SideLeftIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.LEFT_SIDE);
        cosmetic.SideRightIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RIGHT_SIDE);
        cosmetic.SideTopIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.TOP_SIDE);
        cosmetic.TireIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RING_SIDE);

        g.GetComponent<Collider>().isTrigger = true;
        g.GetComponent<Rigidbody>().isKinematic = true;

        g.transform.parent = carParent;
        g.transform.localPosition = Vector3.zero;
        g.transform.localRotation = Quaternion.identity;
        g.transform.localScale = Vector3.one;
        g.SetActive(true);

        StartCoroutine(PlayCarCrashAnimationAfter(5));
        



        CommonUI.Instance.menuMusicManager.Stop(1, 0.25f);
        CommonUI.Instance.menuMusicManager.SetVolume(0, musicVolume);
        CommonUI.Instance.menuMusicManager.Play(0);


        Accounting.Instance.currentUser.CheckForRefillGas();




        if (coinsForShow > 0)
        {
         //   MoneyTablet.Instance.RemoveCoinsForShowing(coinsForShow);
         //   MoneyTablet.Instance.RemoveScoreForShowing(scoreForShow);

            yield return new WaitForSeconds(1);

         //   MoneyTablet.Instance.ShowAdding(coinsForShow);
         //   MoneyTablet.Instance.ShowAddingScore(scoreForShow);

            scoreForShow = 0;
            coinsForShow = 0;
        }


        //gearIconGameObject.SetActive(false);
        serverUnderMaintenanceGameObject.SetActive(false);
        for (int i = 0; i < playButton.Length; i++)
            playButton[i].interactable = true;


        OnServerStateFetched(CommonUI.Instance.IsServerReady);
        CommonUI.Instance.CheckForServerState();

        CommonUI.Instance.CheckForRateMessage();
        RefreshQuestsText();


        if (menuShowTime == 1)
        {
            //Accounting.Instance.GetOnlinePlayers(_OnlinePlayersFetched);
            Prices.Instance.CheckForSaleOffer();
            //CheckForRewards();
        }
            
        mainmenuTutorial.SetActive(CommonUI.Instance.IsTutorial);

        if (!CommonUI.Instance.IsTutorial && (Prices.Instance.BillsMultiplyer != 1 || Prices.Instance.PriceMultiplyer != 1))
            CommonUI.Instance.buyCoinsMenu.Activate();


        if (Accounting.Instance.currentUser.HasAnyUnClaimedGifts)
            dailyGiftWindow.Activate();

        if (Accounting.Instance.currentUser.CanUpgrade)
            deliveryPanel.Deactivate();
        else
        {
            /*int upgradingPartIndex = Accounting.Instance.currentUser.UpgradingPartIndex;
            deliveryPanel.Activate(upgradingPartIndex,
                Accounting.Instance.currentUser.carUpgrades[upgradingPartIndex].PartIndex, (int)Accounting.Instance.currentUser.carUpgrades[upgradingPartIndex].TimeRemaining);*/

            CarUpgrade currentUpgradingPart = Accounting.Instance.currentUser.UpgradingPart;

            int carIndex = Information.Instance.GetCarIndex(currentUpgradingPart.CarTag);
            deliveryPanel.Activate(carIndex,
                currentUpgradingPart.PartIndex, (int)currentUpgradingPart.TimeRemaining);
        }

        offerPanel.Activate(Accounting.Instance.currentUser.SelectedCarIndex);

        if (lastLevel != -1 && Accounting.Instance.currentUser.Level > lastLevel)
            rewardWindow.ActivateLevelUp(Accounting.Instance.currentUser.Level);

        lastLevel = Accounting.Instance.currentUser.Level;
        Accounting.Instance.GetMyLeagueDatas(Accounting.Instance.currentUser.Id, LeaderdBoarSetting.Instance.GetMyLeagueData, null);
    }

    public override void BackButton_Click()
    {
        Debug.Log("Back button pressed");
    }


    public void DailyGiftButton_Click()
    {
        dailyGiftWindow.Activate();
    }

    public void DailyQuestButton_Click()
    {
        questsWindow.Activate();
    }

    public void Tutorial_Click()
    {
        SceneManager.LoadScene(Scenes.TutorialStartScene);
    }
    public void notif_Click()
    {
        NotificationManager.Instance.SetNotification(NotificationManager.Instance.carRepairTexts[0], 20);
    }

    void OnEnable()
    {
        CommonUI.Instance.OnServerStateFetched += OnServerStateFetched;
    }

    void OnDisable()
    {
        CommonUI.Instance.OnServerStateFetched -= OnServerStateFetched;
    }


    void OnServerStateFetched(bool serversEnabled)
    {
        if (playButton[0].interactable && !serversEnabled)
            CommonUI.Instance.messageBox.ShowMessage(Messages.ServersUnderMaintenance, null, true);

        for (int i = 0; i < playButton.Length; i++)
            playButton[i].interactable = serversEnabled;
        
        serverUnderMaintenanceGameObject.SetActive(!serversEnabled);
        //gearIconGameObject.SetActive(false);
    }

    public void ServersUnderMaintenance_Click()
    {
        CommonUI.Instance.CheckForServerState();
        //gearIconGameObject.SetActive(true);
    }

    IEnumerator PlayCarCrashAnimationAfter(float wait)
    {
        yield return new WaitForSeconds(wait);

        carParentGameObject.SetActive(true);

        for (int i = 0; i < carCrashAnimators.Length; i++)
            carCrashAnimators[i].enabled = true;
    }


    public void StartGame_Clicked()
    {
        audioPlayer.Play(audioPlayer.startButton);
        StartCoroutine(_startAfter(0.5f));
    }

    IEnumerator _startAfter(float wait)
    {
        yield return new WaitForSeconds(wait);

        MainGarageUIController.sectionIndex = -1;
        SceneManager.LoadScene(Scenes.Garage);
    }

    public void Credits_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        creditsMenu.Activate();
    }

    public void Options_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        optionsMenu.Activate();
    }

    public void LeaderboardMenu_Clicked()
    {
        leaderboardMenu.gameObject.SetActive(true);
    }

    public void Store_Clicked()
    {
        PlayerData.SetInt("storebuttonseen", 1);

        CommonUI.Instance.PlayButtonClick();
        //Research.SetActive(true);
        SceneManager.LoadScene(Scenes.Store);
    }

    public void Garage_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        ScrGarageController.BackToMainMenuAfterSelect = true;
        SceneManager.LoadScene(Scenes.Garage);
    }

    public void Exit_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        Application.Quit();
    }

    
    /*
    public void SwitchLanguage()
    {
        CommonUI.Instance.PlayButtonClick();

        int lang = SettingData.LanguageIndex;
        lang++;
        if (lang >= languageNames.Length)
            lang = 0;


        if (lang == 0)
            languageText.text = languageNames[1];
        else
            languageText.text = languageNames[0];
        SettingData.LanguageIndex = lang;
    }*/

    public void RateButton_Click()
    {
        CommonUI.Instance.ShowRateWindow();
    }

    public void FacebookButton_Click()
    {
        leaveGameReason = LeaveGameReason.Facebook;
        Application.OpenURL(GeneralSettings.Instance.facebookUrl);
    }

    public void InstagramButton_Click()
    {
        leaveGameReason = LeaveGameReason.Instagram;
        Application.OpenURL(GeneralSettings.Instance.instagramUrl);
    }

    public void VoteButton_Click()
    {
        leaveGameReason = LeaveGameReason.Vote;
        Application.OpenURL("sms:20000717?body=10");
    }

    LeaveGameReason leaveGameReason;
    /*int coins = 0;
    void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            switch (leaveGameReason)
            {
                case LeaveGameReason.Facebook:
                    if (!Information.Instance.IsFacebookCoinsCollected)
                    {
                        coins = 100;
                        Information.Instance.IsFacebookCoinsCollected = true;
                    }
                    else
                        coins = 0;

                    CommonUI.Instance.question.ShowMessage(Messages.ThanksForFacebook, _giveCoins, true);
                    break;
                case LeaveGameReason.Instagram:
                    if (!Information.Instance.IsInstagramCoinsCollected)
                    {
                        coins = 200;
                        Information.Instance.IsInstagramCoinsCollected = true;
                    }
                    else
                        coins = 0;

                    CommonUI.Instance.question.ShowMessage(Messages.ThanksForInstagram, _giveCoins, true);
                    break;

                case LeaveGameReason.Vote:
                    break;
                case LeaveGameReason.None:
                    break;
            }

            leaveGameReason = LeaveGameReason.None;
        }
    }

    private void _giveCoins()
    {
        if (coins > 0)
            MoneyTablet.Instance.Add(coins);
    }*/

    public void RefreshQuestsText()
    {
        questsText.text = QuestManager.Instance.DoneQuests.ToString();
    }
}

public enum LeaveGameReason
{
    None,
    Facebook,
    Instagram,
    Vote
}