using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    #region Singleton

    static GameplayUI _instance;
    public static GameplayUI Instance
    {
        get { return GameplayUI._instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }


    #endregion





    public AudioStruct audioPlayer;
    public GameObject hudGameObject;
    public RectTransform speedIndicatorTransform, rpmIndicatorTransform;
    public HealthBar healthBar;
    public Slider armorSlider, killSlider;
    public Text scoreText, killNumberText;
    public Joystick joystick;
    public Accelerometer accelerometer;
    public NewsWall newsWall;
    public BloodyScreen bloodyScreen;
    public GameObject controlMethodSelectionGameObject;
    public PlayMenu playMenu;
    public TutorialPreparingMenu tutorialPreparingMenu;
    public PauseMenu pauseMenu;
    public CountDown countDown;
    public Scoreboard scoreboard;
    public Scoreboard scoreboardTeam;
	public GameObject redteamKillShowPanel;
	public GameObject blueteamKillShowPanel;
	public GameObject redteamKillShowHaloImage;
	public GameObject blueteamKillShowHaloImage;
	public Text redTeamKillText;
	public Text blueTeamKillText;
	public Text TeamNameText;
    public WeaponButton rocketButton, mineButton;
    public Text timeText;
    public GameOverMenu gameOverMenu;
    public GameStartWindow gameStartWindow;
    public WaitForOtherPlayersWindow waitForOtherPlayersWindow;
    public RectTransform targetTransform;
    public UsernameIndicator[] usernameIndicators;
    public HitDirection hitDirection;
    public LogPanel logPanel;
    public ViberatingIcon viberatingIcon;
    public Text fpsText, pingText;
    public RocketLockSign rocketLockSign;
    public Animator pingAlarmAnimator, timeAlarmAnimator;
    public LittleMessageWindow cantSetMineGameObject;
    public Text gearAndRPMText;
    public GameObject finishGameButtonGameObject;
    public HUDController hud;
    public GameObject gameplayUITutorial;
    public GameObject infoGameObject;
    public BoosterPanel boosterPanel;
    public Material RedfireMaterial;
    public RankMedal rankMedal;


    public static bool IsTutorial
    {
        get { return EnvironmentController.Instance.map == Maps.Tutorial; }
    }


    ControlLayout controlLayout;
    public ControlLayout ControlLayout
    {
        get { return controlLayout; }
        set
        {
            controlLayout = value;
            switch (controlLayout)
            {
                case ControlLayout.Joystick:
                    joystick.Activate();
                    accelerometer.Deactivate();
                    break;

                case ControlLayout.Accelerometer:
                    joystick.Deactivate();
                    accelerometer.Activate();
                    break;
            }
        }
    }


    float speedKM;
    public float SpeedKM
    {
        get { return speedKM; }
        set
        {
            speedKM = value;

            float degrees = speedKM / 60f;
            degrees *= 90;

            if (speedIndicatorTransform)
                speedIndicatorTransform.localRotation = Quaternion.AngleAxis(-degrees, Vector3.forward);
        }
    }

    float rpm;
    public float RPM
    {
        get { return rpm; }
        set
        {
            rpm = value / 1000f;
            float degrees = rpm * 32 + 62;

            if (rpmIndicatorTransform)
                rpmIndicatorTransform.localRotation = Quaternion.AngleAxis(-degrees, Vector3.forward);
        }
    }

    float health;
    public float Health
    {
        get { return health; }
        set { healthBar.Value = health = Mathf.Clamp01(value); }
    }


    public void AddRegeneratedHealth(float healthRegenerationAmount)
    {
        healthBar.AddShadowHealth(healthRegenerationAmount);
        health += healthRegenerationAmount;
    }



    float shield;
    public float Shield
    {
        get { return shield; }
        set { armorSlider.value = shield = Mathf.Clamp01(value); }
    }

    int killNumber;
    public int KillNumber
    {
        get { return killNumber; }
        set
        {
            killNumber = value;
            killNumberText.text = killNumber.ToString();

            float percent = (float)killNumber / (float)GameplayDefaultSettings.Instance.Settings.KillsNumberForWin;
            if (killSlider)
                killSlider.value = Mathf.Clamp01(percent);
        }
    }


    int deathNumber;
    public int DeathNumber
    {
        get { return deathNumber; }
        set { deathNumber = value; }
    }


    int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }



    void Start()
    {
        if (CommonUI.Instance == null)
            SceneManager.LoadGame(Scenes.Gameplay);
        else
        {
            CommonUI.Instance.headerBar.Disable();

            if (EnvironmentController.Instance == null)
                SceneManager.LoadMap(Maps.TrainStation);
            else
            {
                logPanel.Deactivate();


                if (IsTutorial)
                    ControlLayout = global::ControlLayout.Accelerometer;
                else
                    ControlLayout = global::ControlLayout.Joystick;

                SetHUDActive(false);

                scoreboard.Deactivate();
                scoreboardTeam.Deactivate();
                gameOverMenu.Deactivate();
                pauseMenu.Deactivate();
                GameplayUI.Instance.boosterPanel.Deactivate();

                if (IsTutorial)
                    tutorialPreparingMenu.Activate();
                else
                    playMenu.Activate();

                Server.Instance.OnLeftTheRoom += Instance_OnLeftRoom;

                mineButton.OnClick += mineCoolDown_OnClick;
                rocketButton.OnClick += rocketCoolDown_OnClick;

                if (EnvironmentController.Instance.map != Maps.Tutorial && CommonUI.Instance.IsTutorial)
                    gameplayUITutorial.SetActive(true);

                GameplayUI.Instance.boosterPanel.OnDone += BoosterPanel_Done;
                //Inventory.Instance.SetOpenButtonEnable(false);
                //Inventory.Instance.Disable();
            }

            lastWaitingMessageTime = Time.time;
            finishGameButtonGameObject.SetActive(Debug.isDebugBuild);

            //finishGameButtonGameObject.SetActive(true);
        }
    }

	public void isTeamHeaderOrFFa( ScrCarController car ){
		redteamKillShowHaloImage.SetActive(false);
		blueteamKillShowHaloImage.SetActive(false);
		blueteamKillShowPanel.gameObject.SetActive (false);
		redteamKillShowPanel.gameObject.SetActive (false);
		fpsText.transform.parent.gameObject.SetActive (false);
		pingText.transform.parent.gameObject.SetActive (false);
		TeamNameText.gameObject.SetActive (false);
		if (!GameplayDefaultSettings.Instance.isTeamMatch) {
			if (GameplayDefaultSettings.Instance.isTeamMatch) 
				return;
			fpsText.transform.parent.gameObject.SetActive (true);
			pingText.transform.parent.gameObject.SetActive (true);
		}
		else{
			if (!GameplayDefaultSettings.Instance.isTeamMatch) 
				return;

			if (car.nv.owner.GetTeam () == PunTeams.Team.blue) {
				blueteamKillShowPanel.gameObject.SetActive (true);
				redteamKillShowPanel.gameObject.SetActive (true);
				blueteamKillShowHaloImage.SetActive (true);
				TeamNameText.gameObject.SetActive (true);
				TeamNameText.text = "Blue Team";
				TeamNameText.color = Color.blue;
			}
			else if( car.nv.owner.GetTeam () == PunTeams.Team.red ){
				blueteamKillShowPanel.gameObject.SetActive (true);
				redteamKillShowPanel.gameObject.SetActive (true);
				redteamKillShowHaloImage.SetActive(true);
				TeamNameText.gameObject.SetActive (false);
				TeamNameText.gameObject.SetActive (true);
				TeamNameText.text = "Red Team";
				TeamNameText.color = Color.red;
			}
		}
	}


    public void BoosterPanel_Done()
    {
        //controlMethodSelectionGameObject.SetActive(true);

//        Inventory.Instance.SetOpenButtonEnable(false);
//        Inventory.Instance.Disable();

        if (Data.InputMethod == 0)
            AccelerometerButton_Click();

        else
            JoystickButton_Click();
    }


    public void EndGameSession()
    {
        Server.Instance.EndGameSession();
    }

    public void ScoreboardButton_Down()
    {
		if (GameplayDefaultSettings.Instance.isTeamMatch && ScrCarController.Instance.nv.owner.GetTeam () != PunTeams.Team.none) {
			scoreboardTeam.ActivateTemporaryWindow ();
		}
		else {
			scoreboard.ActivateTemporaryWindow ();
		}
    }

    public void ScoreboardButton_Up()
    {
        scoreboard.DeactivateTemporaryWindow();
        scoreboardTeam.DeactivateTemporaryWindow();
    }

    void mineCoolDown_OnClick()
    {
        float x = Mathf.Min(ScrCarController.Instance.myTransform.rotation.eulerAngles.x, 360 - ScrCarController.Instance.myTransform.rotation.eulerAngles.x);
        float z = Mathf.Min(ScrCarController.Instance.myTransform.rotation.eulerAngles.z, 360 - ScrCarController.Instance.myTransform.rotation.eulerAngles.z);

        if (x < 5 && z < 5 && Mathf.Abs(ScrCarController.Instance.rigidbody.velocity.y) <= 0.1f)
        {
            audioPlayer.Play(audioPlayer.setMine);
            ScrCarController.Instance.Fire(GunType.Miner);
            mineButton.CoolDown();
        }
        else
        {
            audioPlayer.Play(audioPlayer.emptyGun);
            cantSetMineGameObject.Activate();
        }
    }

    void rocketCoolDown_OnClick()
    {
        ScrCarController.Instance.Fire(GunType.RocketLauncher);
    }

    void Instance_OnLeftRoom()
    {
        ScrController.Instance.RemoveCar(ScrCarController.Instance);
    }

    public void ShowScoreboard(bool autoRefresh)
    {
        SetHUDActive(false);
		if (GameplayDefaultSettings.Instance.isTeamMatch && ScrCarController.Instance.nv.owner.GetTeam () != PunTeams.Team.none) {
			scoreboardTeam.Activate (autoRefresh);
		}
		else {
			scoreboard.Activate (autoRefresh);
		}
    }

    public void JoystickButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        ControlLayout = global::ControlLayout.Joystick;
        controlMethodSelectionGameObject.SetActive(false);
        SpawnPlayer();
    }

    public void AccelerometerButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        ControlLayout = global::ControlLayout.Accelerometer;
        controlMethodSelectionGameObject.SetActive(false);
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Spawn.Instance.SpawnCar();

        if (SettingData.LanguageIndex == 0) // english
			newsWall.SubmitText(string.Concat(Data.UserName, " *"), ExtraSigns.Joined);
        else if (SettingData.LanguageIndex == 1) // persian
			newsWall.SubmitText(string.Concat( "* ", Data.UserName), ExtraSigns.Joined);
        //newsWall.SubmitText("* " + Data.UserName, ExtraSigns.Joined);

        CommonUI.Instance.menuMusicManager.musics[1].SetVolume(0.4f);
        CommonUI.Instance.menuMusicManager.Play(1);
    }

    void rocketLauncher_OnBulletCountChanged(int currentNumber)
    {
        SetRocketsCount(currentNumber);
    }

    void miner_OnBulletCountChanged(int currentNumber)
    {
        SetMinesCount(currentNumber);
    }

    void Instance_OnKill()
    {
        KillNumber = ScrCarController.Instance.Kills;
    }

    void Instance_OnDeath()
    {
        DeathNumber = ScrCarController.Instance.Deaths;
    }

    void Instance_OnGetScore(int addingScore)
    {
        Score = ScrCarController.Instance.Score;
    }

    void Instance_OnDamage(float damage, float health, float shield)
    {
        Health = health;
        Shield = shield;

        if (damage > 5)
            bloodyScreen.Activate(damage);

        int shakePower = (damage > 5) ? 5 : (int)damage;
        EnvironmentController.Instance.followCam.mainCameraMover.Shake(shakePower);
    }

    public void TestDamage()
    {
        ScrCarController.Instance.ApplyDamage(50, 0, (int)KillMethod.SelfDestruction, ScrCarController.Instance.LifeIndex);
        //newsWall.SubmitText("Damage to " + Data.UserName);
    }

    public void SetMinesCount(int mines)
    {
        this.mines = mines;
        mineButton.BulletsCount = mines;
    }

    int rockets, mines;
    public void SetRocketsCount(int rockets)
    {
        this.rockets = rockets;
        rocketButton.BulletsCount = rockets;
    }

    bool uiInitialized;
    public void SetHUDActive(bool enabled)
    {
        hudGameObject.SetActive(enabled);

        if (Server.Instance.IsGameStartedYet)
        {
            infoGameObject.SetActive(true);
            waitForOtherPlayersWindow.Deactivate();
        }
        else
        {
            infoGameObject.SetActive(false);

            if (!GameplayUI.IsTutorial)
                waitForOtherPlayersWindow.Activate();
        }


        if (enabled)
        {
            if (!uiInitialized)
            {
                ScrCarController.Instance.OnDamage += Instance_OnDamage;
                ScrCarController.Instance.OnGetScore += Instance_OnGetScore;
                ScrCarController.Instance.OnKill += Instance_OnKill;
                ScrCarController.Instance.OnDeath += Instance_OnDeath;

                ScrCarController.Instance.miner.OnBulletCountChanged += miner_OnBulletCountChanged;
                ScrCarController.Instance.rocketLauncher.OnBulletCountChanged += rocketLauncher_OnBulletCountChanged;

                miner_OnBulletCountChanged(ScrCarController.Instance.miner.Bullets);
                rocketLauncher_OnBulletCountChanged(ScrCarController.Instance.rocketLauncher.Bullets);

                GameplayUI.Instance.KillNumber = 0;
                GameplayUI.Instance.Score = 0;

                mineButton.coolDownDuration = ScrCarController.Instance.miner.GunInfo.coolDownTime;
                rocketButton.coolDownDuration = ScrCarController.Instance.rocketLauncher.GunInfo.coolDownTime;

                uiInitialized = true;
            }
            else
                pingAlarmAnimator.SetBool("alarm", false);

            flipButtonGameObject.SetActive(false);
            logPanel.DeactivateAndClearQueue();
            cantSetMineGameObject.Deactivate();
        }
        else
        {
            if (ScrCarController.Instance != null)
            {
                ScrCarController.Instance.Steer(Vector2.zero);
                ScrCarController.Instance.Accelerate(0);

                ScrCarController.Instance.StopFire(GunType.MachineGun);
                MachineGun_Released();
            }



            bloodyScreen.Deactivate();
            logPanel.DeactivateAndClearQueue();
            rocketLockSign.Deactivate();
            hitDirection.Clear();
        }
    }

    public void MachineGun_Pressed()
    {
        if (ScrCarController.Instance != null)
            ScrCarController.Instance.Fire(GunType.MachineGun);
    }

    public void MachineGun_Released()
    {
        if (ScrCarController.Instance != null)
            ScrCarController.Instance.StopFire(GunType.MachineGun);
    }

    public void PauseButton_Clicked()
    {
        pauseMenu.Activate();
    }

    float lastWaitingMessageTime, lastPingMessage = -1000;
    float pingRefreshTime;

    public Text masterClientText;
    public GameObject flipButtonGameObject;



    public void CarJump()
    {
        if (ScrCarController.Instance != null)
            ScrCarController.Instance.Jump();
    }


    float isntInBalanceTime;
    void Update()
    {
        if (Debug.isDebugBuild || GeneralSettings.Instance.AllowCheat)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CarJump();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Q))
            {
                if (ScrCarController.Instance != null)
                {
                    ScrCarController.Instance.AddItems(1, CollectibleType.RocketLauncher);
                    ScrCarController.Instance.AddCheats();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.E))
            {
                if (ScrCarController.Instance != null)
                {
                    ScrCarController.Instance.AddItems(1, CollectibleType.Miner);
                    ScrCarController.Instance.AddCheats();
                }
            }
        }

        if (Application.isEditor || GeneralSettings.Instance.AllowKeyboardControl)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                ScoreboardButton_Down();

            if (Input.GetKeyUp(KeyCode.Tab))
                ScoreboardButton_Up();

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                MachineGun_Pressed();
            }

            if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Keypad2))
            {
                MachineGun_Released();
            }
        }




        if (!IsTutorial && Server.Instance.GameSessionState == GameSessionState.FreeRide && Time.time - lastWaitingMessageTime > GameplayDefaultSettings.Instance.WaitingMessageTime)
        {
            logPanel.ShowWaitingMessage();
            lastWaitingMessageTime = Time.time;
        }

        if (ScrCarController.Instance != null && ScrCarController.Instance.Car1 != null)
        {
            if (!ScrCarController.Instance.IsBalanced)
            {
                isntInBalanceTime += Time.deltaTime;
                if (isntInBalanceTime >= 0.5f)
                    flipButtonGameObject.SetActive(true);
            }
            else
            {
                isntInBalanceTime = 0;
                flipButtonGameObject.SetActive(false);
            }



            float rpm = ScrCarController.Instance.Car1.MotorRPM / 1000f;
            float wRPM = ScrCarController.Instance.Car1.wheelsRPM / 1000f;
            int onfloorWheels = -1;

            int wheelsCount = ScrCarController.Instance.Car1.GetWheelsCount();

            string mT = "";
            for (int i = 0; i < wheelsCount; i++)
            {
                JCar.WheelData wdata = ScrCarController.Instance.Car1.GetWheelData(i);
                mT += (wdata.col.motorTorque / 100f).ToString("0.0") + ";";
                if (wdata == null)
                {
                    onfloorWheels = 0;
                    break;
                }

                if (wdata.col.isGrounded)
                    onfloorWheels++;
            }


            gearAndRPMText.text = "motorON=" + ScrCarController.Instance.Car1.isMotor + "," + rpm.ToString("0.0") + ", Wheels=" + wRPM.ToString("0.0") + "---" + mT;
        }



        if (!IsTutorial && PhotonNetwork.connected)
        {
            masterClientText.text = (PhotonNetwork.isMasterClient) ? "Master Client" : "Client";

            pingRefreshTime += Time.deltaTime;
            if (pingRefreshTime >= 2)
            {
                pingRefreshTime = 0;
                int ping = PhotonNetwork.GetPing();
                pingText.text = ping.ToString();


                if (ping >= GameplayDefaultSettings.Instance.MinimumPing)
                {
                    pingAlarmAnimator.SetBool("alarm", true);

                    if (Time.time - lastPingMessage > GameplayDefaultSettings.Instance.PingAlarmTime)
                    {
                        logPanel.SubmitPingAlarm();
                        lastPingMessage = Time.time;
                    }
                }
                else
                {
                    if (hudGameObject.activeInHierarchy)
                        pingAlarmAnimator.SetBool("alarm", false);
                }
            }
        }

        /*if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            mineButton.SkipCoolDown();
            rocketButton.SkipCoolDown();
        }*/


        if (ScrCarController.Instance != null)
        {
            RPM = ScrCarController.Instance.RPM;
            SpeedKM = ScrCarController.Instance.Veclocity;

            switch (controlLayout)
            {
                case ControlLayout.Joystick:
                    if (hudGameObject.activeInHierarchy)
                    {
                        float sensivity = Mathf.Lerp(ControlSettings.Instance.JoystickMinSensivity, ControlSettings.Instance.JoystickMaxSensivity, SettingData.JoystickControlSensivity);
                        ScrCarController.Instance.Steer(joystick.MoveAmount * sensivity);

                        if (!joystick.IsDragging)
                            ScrCarController.Instance.Accelerate(0);
                        else if (joystick.MoveAmount.y > 0)
                            ScrCarController.Instance.Accelerate(1);
                        else if (joystick.MoveAmount.y < 0)
                            ScrCarController.Instance.Accelerate(-1);

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            ScrCarController.Instance.Brake(true);
                        }


                        if (Input.GetKeyUp(KeyCode.Space))
                            ScrCarController.Instance.Brake(false);
                    }

                    //accelerationText.text = joystick.MoveAmount.ToString();
                    break;

                case ControlLayout.Accelerometer:
                    if (hudGameObject.activeInHierarchy)
                    {
                        float sensivity = Mathf.Lerp(ControlSettings.Instance.AccelerometerMinSensivity, ControlSettings.Instance.AccelerometerMaxSensivity, SettingData.AccelerometerControlSensivity);

                        //Debug.Log("sensitivity=" + SettingData.AccelerometerControlSensivity);

                        ScrCarController.Instance.Steer(accelerometer.MoveAmount * sensivity);
                        ScrCarController.Instance.Accelerate(accelerometer.MoveAmount.y);
                    }
                    //accelerationText.text = accelerometer.MoveAmount.ToString();
                    break;
            }
        }

        if (Server.Instance != null)
        {
            switch (Server.Instance.GameSessionState)
            {
                case GameSessionState.FreeRide:
                    timeText.text = "-:-";
                    break;
                case GameSessionState.GamePlaying:
                    if (Server.Instance.RemainingTime < 0)
                    {
                        timeText.text = "00:00";

                        if (PhotonNetwork.isMasterClient)
                            Server.Instance.EndGameSession();
                    }
                    else
                    {
                        int remainingTime = Server.Instance.RemainingTime;

                        int min = remainingTime / 60;
                        int sec = remainingTime % 60;
                        timeText.text = string.Format("{0:00}:{1:00}", min, sec);

                        if (hudGameObject.activeInHierarchy)
                        {
                            if (remainingTime < GameplayDefaultSettings.Instance.TimeAlarmThreshold)
                                timeAlarmAnimator.SetBool("alarm", true);
                            else
                                timeAlarmAnimator.SetBool("alarm", false);
                        }
                    }
                    break;
                case GameSessionState.Finished:
                    timeText.text = "00:00";
                    break;
            }
        }
        else
            timeText.text = "00:00";



        fpsTime -= Time.deltaTime;
        if (fpsTime <= 0)
        {
            fpsTime = 0.5f;
            fpsText.text = (1f / Time.deltaTime).ToString("0.0");
        }

        /*
        newsTime -= Time.deltaTime;
        if (newsTime <= 0)
        {
            newsTime = Random.Range(3f, 6f);

            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0: newsWall.SubmitText("Ali * Milad", KillMethod.MachineGun); break;
                case 1: newsWall.SubmitText("Soroush * Ali", KillMethod.Mine); break;
                case 2: newsWall.SubmitText("Milad * Masoud", KillMethod.Rocket); break;
                case 3: newsWall.SubmitText("Masoud * Soroush", KillMethod.Environment); break;
            }
        }
         */
    }


    bool droppedForPause;
    public void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            if (!IsTutorial && PhotonNetwork.inRoom)
            {
                PhotonNetwork.Disconnect();
                droppedForPause = true;
            }
        }
        else
        {
            if (droppedForPause)
            {
                CommonUI.Instance.messageBox.ShowMessage(Messages.YouDisconnected, GoToMainMenu, true);
            }
        }
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }


    float fpsTime;
    float newsTime;

    public void CountDownForRespawn()
    {
        bloodyScreen.Deactivate();
        SetHUDActive(false);
        countDown.Activate(3, RespawnTheCar);
    }
	

    public void RespawnTheCar()
    {
        SetHUDActive(true);
        ScrCarController.Instance.Respawn();
    }

    public void RespawnTheCarFromPauseMenu()
    {
        countDown.Activate(5, true, _SlefDestruction, null, "♣");
    }

    void _SlefDestruction()
    {
        ScrCarController.Instance.ApplyDamage(float.MaxValue, 0, (int)KillMethod.SelfDestruction, ScrCarController.Instance.LifeIndex);
		newsWall.SubmitText( "<*> " + Data.UserName, KillMethod.SelfDestruction);
    }

    public void SetScore(int currentNumber)
    {
        scoreText.text = currentNumber.ToString();
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    public void AddScore(int score, Vector3 position)
    {
        Score += score;
    }

    public void GameStarted()
    {
        if (ScrCarController.Instance != null)
            ScrCarController.Instance.ResetScores();

        countDown.Deactivate();
        gameStartWindow.Activate();
    }

    int indicatorIndex;
    public void SetUsernameIndicator(ScrCarController car)
    {
        if (indicatorIndex >= usernameIndicators.Length)
            return;

        car.SetUsernameIndicator(usernameIndicators[indicatorIndex]);
        usernameIndicators[indicatorIndex].Activate(car);

        indicatorIndex++;

        /*
        Debug.Log("-----------------------");
        for (int i = 0; i < usernameIndicators.Length; i++)
        {
            if (!usernameIndicators[i].IsActive)
            {
                Debug.Log("indicator " + i + "is not active. " + name, usernameIndicators[i]);

                usernameIndicators[i].Activate(car);
                car.SetUsernameIndicator(usernameIndicators[i]);
                break;
            }
            else
                Debug.Log("indicator " + i + "is active. " + name, usernameIndicators[i]);
        }
        Debug.Log("-----------------------");*/
    }


    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip emptyGun;
        public AudioClip health, rockets, mine, armor, all;
        public AudioClip setMine, startEngine;
    }

    public void PlayCollectingItemSound(CollectibleType type)
    {
        AudioClip clip = null;
        switch (type)
        {
            case CollectibleType.RocketLauncher:
                clip = audioPlayer.rockets;
                break;
            case CollectibleType.Miner:
                clip = audioPlayer.mine;
                break;
            case CollectibleType.Health:
                clip = audioPlayer.health;
                break;
            case CollectibleType.Armor:
                clip = audioPlayer.armor;
                break;
            case CollectibleType.All:
                clip = audioPlayer.all;
                break;
        }

        if (clip != null)
            audioPlayer.Play(clip);
    }

    public void testGameOver()
    {
        gameOverMenu.Activate(1);
    }

    public void Viberate()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (SettingData.IsViberationOn)
            Handheld.Vibrate();
#endif

        GameplayUI.Instance.viberatingIcon.Activate();
    }

    public void AddBoosters()
    {
        Debug.Log("adding boosters");

        if (GameplayUI.Instance.boosterPanel.EquippedPackages != null && GameplayUI.Instance.boosterPanel.EquippedPackages.Count > 0)
        {
            
            for (int i = 0; i < GameplayUI.Instance.boosterPanel.EquippedPackages.Count; i++)
            {
                int packageIndex = (int)GameplayUI.Instance.boosterPanel.EquippedPackages[i];
                Debug.Log("--- has package " + GameplayUI.Instance.boosterPanel.EquippedPackages[i]);

                for (int j = 0; j < StoreData.Instance.packages[packageIndex].items.Length; j++)
                {
                    Debug.Log("------ action " + StoreData.Instance.packages[packageIndex].items[j].type + "[" + StoreData.Instance.packages[packageIndex].items[j].count + "]");
                    switch (StoreData.Instance.packages[packageIndex].items[j].type)
                    {
                        case ItemType.Rocket:
                            ScrCarController.Instance.rocketLauncher.Bullets += (int)StoreData.Instance.packages[packageIndex].items[j].count;
                            break;
                        case ItemType.Mine:
                            ScrCarController.Instance.miner.Bullets += (int)StoreData.Instance.packages[packageIndex].items[j].count;
                            break;
                        case ItemType.Health:
                            ScrCarController.Instance.AddToHealth(StoreData.Instance.packages[packageIndex].items[j].count);
                            break;
                        case ItemType.Shield:
                            ScrCarController.Instance.AddToShield(StoreData.Instance.packages[packageIndex].items[j].count);
                            break;
                        case ItemType.MachinegunDamageMultiplyer:
                            ScrCarController.Instance.SetEnhancedFireBullet(StoreData.Instance.packages[packageIndex].items[j].count);
                            break;
                        case ItemType.HealthRegeneration:
                            ScrCarController.Instance.SetHealthRegeneration(StoreData.Instance.packages[packageIndex].items[j].parameters[1], StoreData.Instance.packages[packageIndex].items[j].parameters[0]);
                            break;
                    }
                }

                //Debug.Log("item " + GameplayUI.Instance.boosterPanel.EquippedPackages[i] + " is equipped.");


                
                switch (GameplayUI.Instance.boosterPanel.EquippedPackages[i])
                {
                    case BoosterPackageType.Minesx2:
                        

                        ScrCarController.Instance.miner.Bullets += 3;
                        break;
                    case BoosterPackageType.Rocketsx2:
                        ScrCarController.Instance.rocketLauncher.Bullets += 2;
                        break;
                    case BoosterPackageType.Rocketsx3:
                        ScrCarController.Instance.rocketLauncher.Bullets += 3;
                        break;
                    case BoosterPackageType.Shield100:
                        ScrCarController.Instance.AddToShield(1);
                        break;
                    case BoosterPackageType.Minesx2Rocketsx3Shield50:
                        ScrCarController.Instance.miner.Bullets += 2;
                        ScrCarController.Instance.rocketLauncher.Bullets += 3;
                        ScrCarController.Instance.AddToShield(0.5f);
                        break;
                    case BoosterPackageType.MachinegunDamagex120:
                        ScrCarController.Instance.SetEnhancedFireBullet(1.2f); 
                        break;
                    case BoosterPackageType.HealthRegenerationOneSecond:
                        ScrCarController.Instance.SetHealthRegeneration(.05f, 1);
                        break;
                }
            }
        }
    }
}

public enum ControlLayout
{
    Joystick,
    Accelerometer
}