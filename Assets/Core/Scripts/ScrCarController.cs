using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PhotonView))]
public class ScrCarController : Photon.MonoBehaviour 
{
    
    static ScrCarController instance;
    public static ScrCarController Instance
    {
        get { return instance; }
    }

    //[HideInInspector]
    public struct CarParameterStruct
    {
        public float armor;
        public float speed;
        public float mass;
    }

    public Transform myTransform;
    public CarType carType;
    public CarParameterStruct carParameter;
    public MeshRenderer mr;
    public Transform targetTransform;
    public Renderer[] allRenderers;
    //[HideInInspector]
    public HealthManagerStruct healthManager;
    public Transform usernameTransform;
    public LayerMask bulletsLayer;
    public GameObject backLampsGameObjects;
    public PhotonView nv;
    public Camera ShadowProjector;

    bool hasHealthRegeneration;
    public bool HasHealthRegeneration
    {
        get { return hasHealthRegeneration; }
    }

    bool hasFireBullet;
    public bool HasFireBullet
    {
        get { return hasFireBullet; }
    }




    float machineGunDamageMultiplyer = 1;
    public float MachineGunDamageMultiplyer
    {
        get { return machineGunDamageMultiplyer; }
        set { machineGunDamageMultiplyer = value; }
    }

    float healthRegenerationTime = 0;
    public float HealthRegenerationTime
    {
        get { return healthRegenerationTime; }
        set { healthRegenerationTime = value; }
    }

    float healthRegenerationAmount;
    public float HealthRegenerationAmount
    {
        get { return healthRegenerationAmount; }
        set { healthRegenerationAmount = value; }
    }



    bool isInvincible;
    public bool IsInvincible
    {
        get { return isInvincible; }
    }


    [ContextMenu("Collect all renderers")]
    void collect()
    {
        Renderer[] r = GetComponentsInChildren<MeshRenderer>();

        int count = 0;
        for (int i = 0; i < r.Length; i++)
        {
            if (r[i].name.IndexOf("Bullet") == -1)
                count++;
        }

        int index = 0;
        allRenderers = new Renderer[count];
        for (int i = 0; i < r.Length; i++)
        {
            if (r[i].name.IndexOf("Bullet") == -1)
                allRenderers[index++] = r[i];
        }
    }


    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioSource[] minigun;
    }

    public AudioStruct audioPlayer;

    [System.Serializable]
    public class HealthManagerStruct
    {
        private float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
            set 
            { 
                currentHealth = value;
            }
        }


        private float currentShiled;
        public float CurrentShiled
        {
            get { return currentShiled; }
            set { currentShiled = value;}
        }
    }



    UsernameIndicator usernameIndicator;



    Vector3 targetPoint;
    public Vector3 TargetPoint
    {
        get { return targetPoint; }
    }

    int deaths;
    public int Deaths
    {
        get { return deaths; }
    }

    int kills;
    public int Kills
    {
        get { return kills; }
    }

    int score;
    public int Score
    {
        get { return score; }
    }

    int cheats;
    public int Cheats
    {
        get { return cheats; }
    }


    
    PhotonPlayer owner;
    public PhotonPlayer Owner
    {
        get { return owner; }
    }


	void SetTeam(){
		if( GameplayDefaultSettings.Instance.isTeamMatch ){
			if( ScrController.Instance.GetBlueTeamCars().Length > ScrController.Instance.GetRedTeamCars().Length ){
				if( nv.owner.GetTeam() != PunTeams.Team.none )
					return;
				ScrController.Instance.AddCarTeam(this, PunTeams.Team.red, nv.isMine);
			}
			if (ScrController.Instance.GetBlueTeamCars ().Length < ScrController.Instance.GetRedTeamCars ().Length) {
				if( nv.owner.GetTeam() != PunTeams.Team.none )
					return;
				ScrController.Instance.AddCarTeam(this, PunTeams.Team.blue, nv.isMine);
			}
			else {
				if( ScrController.Instance.GetBlueTeamCars ().Length == ScrController.Instance.GetRedTeamCars ().Length ){
					if( nv.owner.GetTeam() != PunTeams.Team.none )
						return;
					ScrController.Instance.AddCarTeam(this, PunTeams.Team.blue, nv.isMine);
				}
			}
		}
	}

    void Awake()
    {
        AddToShield(GameplayDefaultSettings.Instance.defaultShield);
        AddToHealth(Information.Instance.carInfo[(int)carType].levels[0].health); // GameplayDefaultSettings.Instance.defaultHealth);


        nv = photonView;
        isOnLine = true;

        ScrController.Instance.AddCar(this, nv.isMine);

        SetCarInfo(Information.Instance.carInfo[(int)carType].levels[0]);

        Car = GetComponent<JControlledCar>();
        engineAudio = GetComponent<JCarEngineAudio>();

        owner = nv.owner;

        miner.GunInfo = Information.Instance.carInfo[(int)carType].minerInfo;
        machineGun.MachineGunInfo = Information.Instance.carInfo[(int)carType].machineGunInfo;
        rocketLauncher.GunInfo = Information.Instance.carInfo[(int)carType].rocketLauncherInfo;

        if (!nv.isMine)
        {
            this.name = string.Concat("Enemy - " + nv.viewID);
            GameplayUI.Instance.SetUsernameIndicator(this);

            setPositionAfterRespawn = true;
            setPositionAfterRespawnUntil = -1;
        }
        else
        {

            lastPosition = myTransform.position;
            flipJack = GetComponent<JFlipBack>();

            instance = this;

            EnvironmentController.Instance.followCam.SetInfo(Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].cameraHeight,
                Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].cameraDistance, Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].cameraTargetHeight);

            GameplayUI.Instance.targetTransform.anchoredPosition = new Vector2(0, Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].targetSignHeight);

            /*EnvironmentController.Instance.followCam.height = Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].cameraHeight;
            EnvironmentController.Instance.followCam.distance = Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].cameraDistance;
            EnvironmentController.Instance.followCam.targetHeight = Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].cameraTargetHeight;*/

            this.gameObject.layer = LayerMask.NameToLayer("Player");
            this.gameObject.tag = "Player";
            

            //Set Guns
            rocketLauncher.Bullets += GameplayDefaultSettings.Instance.Settings.defaultRocketsCount;
            miner.Bullets += GameplayDefaultSettings.Instance.Settings.defaultMinesCount;

            
            //rocketLauncher.damage = GameplayDefaultSettings.Instance.rocketDamage;
            rocketLauncher.InitBullets();

            
            //machineGun.damage = GameplayDefaultSettings.Instance.bulletDamage;
            machineGun.InitBullets();

            

            username = Accounting.Instance.currentUser.DisplayName;
//            username = Data.UserName;

            //this.name = "Player";
			nv.RPC("SetName", PhotonTargets.AllBuffered, username);
 //           nv.RPC("SetName", PhotonTargets.AllBuffered, Data.UserName);

            GameplayUI.Instance.audioPlayer.Play(GameplayUI.Instance.audioPlayer.startEngine);
            engineAudio.StopFor(1);

            if (!GameplayUI.IsTutorial)
                GameplayUI.Instance.AddBoosters();


			CosmeticPerCars cosmeticManager = GetComponent<CosmeticPerCars>();

			//Debug.Log(Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.BACK_SIDE));

			cosmeticManager.ColorIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.COLOR_SIDE);
            cosmeticManager.SideBackIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.BACK_SIDE);
            cosmeticManager.SideFrontIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.FRONT_SIDE);
            cosmeticManager.SideLeftIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.LEFT_SIDE);
            cosmeticManager.SideRightIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RIGHT_SIDE);
            cosmeticManager.SideTopIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.TOP_SIDE);
            cosmeticManager.TireIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RING_SIDE);

			cosmeticManager.Finalize();

			nv.RPC("_SyncCosmeticsRPC", PhotonTargets.OthersBuffered, cosmeticManager.SideBackIndex, cosmeticManager.SideFrontIndex, cosmeticManager.SideLeftIndex,
			       cosmeticManager.SideRightIndex, cosmeticManager.SideTopIndex, cosmeticManager.TireIndex, cosmeticManager.ColorIndex );

            cosmeticManager = null;
        }

        lastResoawnTime = Time.time;
    }

    [RPC]
	void _SyncCosmeticsRPC(int backIndex, int frontIndex, int leftIndex, int rightIndex, int topIndex, int tireIndex,int colorIndex)
    {
        CosmeticPerCars cosmeticManager = GetComponent<CosmeticPerCars>();
        cosmeticManager.SideBackIndex = backIndex;
        cosmeticManager.SideFrontIndex = frontIndex;
        cosmeticManager.SideLeftIndex = leftIndex;
        cosmeticManager.SideRightIndex = rightIndex;
        cosmeticManager.SideTopIndex = topIndex;
        cosmeticManager.TireIndex = tireIndex;
		cosmeticManager.ColorIndex = colorIndex;

        cosmeticManager.Finalize();
        cosmeticManager = null;
    }

	[RPC]
	void _SyncColorCusm( int colorIndex ){
		CosmeticPerCars cosmeticManager = GetComponent<CosmeticPerCars>();
		cosmeticManager.ColorIndex = colorIndex;
		cosmeticManager = null;
	}

    public void SetUsernameIndicator(UsernameIndicator usernameIndicator)
    {
        this.usernameIndicator = usernameIndicator;
		this.usernameIndicator.SetUsername(username);
    }

    void Start()
    {
        ShadowProjector.gameObject.SetActive(false);
		RefreshingTeamShowKills ();
        isAlive = true;
		if(SettingData.VFX){
            ShadowProjector.gameObject.SetActive(true) ;
		}

		if( nv.isMine ){
			SetTeam ();
			GameplayUI.Instance.isTeamHeaderOrFFa(this);

			CosmeticPerCars cosmeticManager = GetComponent<CosmeticPerCars>();


			if( GameplayDefaultSettings.Instance.isTeamMatch ){
				if( nv.owner.GetTeam() == PunTeams.Team.red ){
					int index = 10;
					cosmeticManager.ColorIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.COLOR_SIDE);
					nv.RPC( "_SyncColorCusm", PhotonTargets.AllBuffered, index );
				}
				else if(nv.owner.GetTeam() == PunTeams.Team.blue){
					int index = 11;
					cosmeticManager.ColorIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.COLOR_SIDE);
					nv.RPC( "_SyncColorCusm", PhotonTargets.AllBuffered, index );
				}
			}
		}
    }


    bool gameEnded;

    [RPC]
    public void EndGame()
    {
        if (nv.isMine)
        {
            gameEnded = true;

            machineGun.DeactivateAllBullets();
            rocketLauncher.DeactivateAllBullets();
            EnvironmentController.Instance.mineManager.DeactivateAllMines();

//			int vipReward = 0;
//
//			int rank = GameplayUI.Instance.rankMedal.playerRank;			

            GameplayUI.Instance.ShowScoreboard(false);

			//GameplayUI.Instance.gameOverMenu.Activate ( GameplayDefaultSettings.Instance.isTeamMatch,rank,score,(int)(score * ScoreSettings.Instance.ScoreMoneyConvertRate),0,0,vipReward,0 );
        }
    }

	public void FinishingEnding(){
		int vipReward = 0;
		int rank = GameplayUI.Instance.rankMedal.playerRank;			
		GameplayUI.Instance.gameOverMenu.Activate ( GameplayDefaultSettings.Instance.isTeamMatch,rank,score,(int)(score * ScoreSettings.Instance.ScoreMoneyConvertRate),0,0,vipReward,0 );


        GameAnalytics.NewDesignEvent("One round is done", 1f);
	}


    string username;
    public string Username
    {
        get { return username; }
    }


    [RPC]
    public void SetName(string username)
    {
        this.username = username;
        this.name = "Car [" + username + "] - " + nv.viewID;

        if (usernameIndicator != null)
            usernameIndicator.SetUsername(username);
    }



    public void CrashedByNaturalCause(DestructionCause cause)
    {
        if (nv.isMine)
        {
            healthManager.CurrentShiled = healthManager.CurrentHealth = 0;
            _RefreshHealthAndArmorUI();

            isAlive = false;
            Invoke("KillCar", 0.1f);

            ScrController.Instance.SuicideAnnounced(this);
        }
    }

    int lifeIndex;
    public int LifeIndex
    {
        get { return lifeIndex; }
    }


    [RPC]
    public void ApplyDamage(float damage,int id, int killMethodValue, int damageLifeIndex)
    {
        if (lifeIndex != damageLifeIndex)
            return;

        if (!nv.isMine)
            return;

        if (!isAlive)
            return;

        if (isInvincible)
            return;



        KillMethod method = (KillMethod)killMethodValue;
        ScrCarController hitterCar = ScrController.Instance.FindCar(id);
        if (hitterCar == null)
            Debug.Log("car not found, id=" + id);
        else if (method != KillMethod.Mine)
            GameplayUI.Instance.hitDirection.ShowRelative(myTransform.position, myTransform.forward, hitterCar.transform.position);

        ApplyDamage(damage, hitterCar, method);

        //Debug.Log("damage=" + damage + ", my armor=" + carParameter.armor + ", final damage=" + (damage / carParameter.armor) + ", health=" + healthManager.CurrentHealth);

    }

    public void ApplyDamage(float damage, ScrCarController hitterCar, KillMethod method)
    {
        if (!nv.isMine)
            return;

        if (!isAlive)
            return;

        if (isInvincible)
            return;

        damage /= carParameter.armor;
		//Debug.Log("damage=" + damage + " from " + method + " from " + hitterCar.Username);

        if (!Server.Instance.IsGameStartedYet && hitterCar != null)
        {
            if (OnDamage != null)
                OnDamage(damage, healthManager.CurrentHealth, healthManager.CurrentShiled);

            return;
        }

        if (healthManager.CurrentShiled > 0)
        {
            if (damage <= healthManager.CurrentShiled)
                healthManager.CurrentShiled -= damage;
            else
            {
                float remainingDamage = damage - healthManager.CurrentShiled;
                healthManager.CurrentShiled = 0;
                healthManager.CurrentHealth -= remainingDamage;
            }
        }
        else
        {
            healthManager.CurrentHealth -= damage;
        }

        if (OnDamage != null)
            OnDamage(damage, healthManager.CurrentHealth, healthManager.CurrentShiled);

        _RefreshHealthAndArmorUI();
        nv.RPC("SyncHealthAndArmor", PhotonTargets.Others, healthManager.CurrentHealth, healthManager.CurrentShiled);

        if (healthManager.CurrentHealth <= 0)
        {
            GameplayUI.Instance.Viberate();
            nv.RPC("KillCar", PhotonTargets.All);

            //Invoke("KillCar", 0.1f);
            if (hitterCar != null)
                ScrController.Instance.DeathAnnouncement(this, hitterCar, method);
            else
            {
                ScrController.Instance.SuicideAnnounced(this);
                AddScore(ScoreSettings.Instance.suicideScore);
            }

            isAlive = false;
        }
    }


    [RPC]
    public void SyncHealthAndArmor(float realHealth, float realShield)
    {
        healthManager.CurrentHealth = realHealth;
        healthManager.CurrentShiled = realShield;

        if (usernameIndicator != null)
            usernameIndicator.RefreshColor();
    }

    protected void _RefreshHealthAndArmorUI()
    {

		GameplayUI.Instance.Health = healthManager.CurrentHealth /  Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health; // GameplayDefaultSettings.Instance.maxHealth;
//        GameplayUI.Instance.Health = healthManager.CurrentHealth / Information.Instance.carInfo[0].levels[0].health; // GameplayDefaultSettings.Instance.maxHealth;
		GameplayUI.Instance.Shield = healthManager.CurrentShiled / Information.Instance.carInfo [Accounting.Instance.currentUser.SelectedCarIndex].levels [0].health;
//        GameplayUI.Instance.Shield = healthManager.CurrentShiled / GameplayDefaultSettings.Instance.maxShield;
    }


    void _SetVisible(bool visible)
    {
        for (int i = 0; i < allRenderers.Length; i++)
            if (allRenderers[i] != null)
                allRenderers[i].enabled = visible;
    }


    public void AddToHealth(float newHealth)
    {
        healthManager.CurrentHealth += newHealth;
		if (healthManager.CurrentHealth > Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health)
			healthManager.CurrentHealth = Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health;

        _RefreshHealthAndArmorUI();
        
    }

    public void AddToShield(float newArmor)
    {
        healthManager.CurrentShiled += newArmor;
		if (healthManager.CurrentShiled >  Information.Instance.carInfo [Accounting.Instance.currentUser.SelectedCarIndex].levels [0].health)
			healthManager.CurrentShiled =  Information.Instance.carInfo [Accounting.Instance.currentUser.SelectedCarIndex].levels [0].health;
//        if (healthManager.CurrentShiled > GameplayDefaultSettings.Instance.maxShield)

        _RefreshHealthAndArmorUI();
    }

    bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
    }


    bool isDropped;
    public bool IsDropped
    {
        get { return isDropped; }
    }

    bool isOnLine;
    public bool IsOnLine
    {
        get { return isOnLine; }
    }


    
    [RPC]
    void KillCar()
    {
        isAlive = false;

        Transform burnedCarTransform = Spawn.Instance.SpawnBurnedCar(carType, myTransform.position, myTransform.rotation).transform;

        if (nv.isMine)
        {
            EnvironmentController.Instance.followCam.Activate(burnedCarTransform);

            GameplayUI.Instance.CountDownForRespawn();
            //GameplayUI.Instance.scoreboard.Activate(true);

            StopAccelerate();
            Steer(Vector2.zero);
            Brake(false);
        }

        myTransform.Translate(new Vector3(0, -100, 0));

        healthManager.CurrentShiled = 0;
        healthManager.CurrentHealth = Information.Instance.carInfo[(int)carType].levels[0].health;
        _RefreshHealthAndArmorUI();

        engineAudio.Stop();

        //AddToArmor(0);
        //AddToHealth(GameplayDefaultSettings.Instance.defaultHealth);
    }


    public void AddScore(int score)
    {
        if (nv.isMine)
            _AddScore(score);
        else
            nv.RPC("_AddScore", PhotonTargets.Others, score);
    }
	
    [RPC]
    void _AddScore(int score)
    {
        GameplayUI.Instance.rankMedal.Refresh();

        if (!nv.isMine)
            return;

        this.score = Mathf.Max(this.score + score, 0);
        if (OnGetScore != null)
            OnGetScore(score);

        //ScrController.Instance.SendDataScore(nv.viewID, score, -1, -1);
        if (PhotonNetwork.isMasterClient)
            ScrController.Instance.ScoreChanged(this);


        nv.RPC("_SyncScores", PhotonTargets.Others, this.score, this.deaths, this.kills, this.cheats);
    }

    public void GetKill()
    {
        if (nv.isMine)
            _GetKill();
        else
            nv.RPC("_GetKill", PhotonTargets.Others);

		RefreshingTeamShowKills ();
    }

	public void RefreshingTeamShowKills(){
		nv.RPC ( "RPC_RefreshingTeamShowKills", PhotonTargets.AllBuffered );
	}

	[RPC]
	void RPC_RefreshingTeamShowKills(){
		GameplayUI.Instance.redTeamKillText.text = ScrController.Instance.GetRedTeamKill ().ToString();
		GameplayUI.Instance.blueTeamKillText.text = ScrController.Instance.GetBlueTeamKill ().ToString();
	}


    [RPC]
    void _GetKill()
    {
       if (!nv.isMine)
            return;

        this.kills++;

        Accounting.Instance.currentUser.statistics.AddKill();
        QuestManager.Instance.DestroyedCar();

        this.score += ScoreSettings.Instance.killScore;


        if (OnGetScore != null)
            OnGetScore(score);

        if (OnKill != null)
            OnKill();

        if (PhotonNetwork.isMasterClient)
            ScrController.Instance.ScoreChanged(this);

        nv.RPC("_SyncScores", PhotonTargets.Others, this.score, this.deaths, this.kills, this.cheats);
		RefreshingTeamShowKills ();

    }

    public void AddDeath(bool isSuicide)
    {
        nv.RPC("_AddDeath", PhotonTargets.All, isSuicide);
		RefreshingTeamShowKills ();
    }

    [RPC]
    void _AddDeath(bool suicide)
    {
        if (!nv.isMine)
            return;

        this.deaths++;
        Accounting.Instance.currentUser.statistics.AddDeath();

        if (suicide)
        {
            this.score = Mathf.Max(0, ScoreSettings.Instance.suicideScore + this.score);
            if (OnGetScore != null)
                OnGetScore(score);
        }

        if (OnDeath != null)
            OnDeath();

        if (PhotonNetwork.isMasterClient)
            ScrController.Instance.ScoreChanged(this);

        nv.RPC("_SyncScores", PhotonTargets.Others, this.score, this.deaths, this.kills, this.cheats);
    }

    [RPC]
    void _SyncScores(int score, int deaths, int kills, int cheats)
    {
        this.score = score;
        this.deaths = deaths;
        this.kills = kills;
        this.cheats = cheats;

        GameplayUI.Instance.rankMedal.Refresh();

        if (PhotonNetwork.isMasterClient)
            ScrController.Instance.ScoreChanged(this);
    }

    /*void ResetPlayer()
    {
        StartCoroutine(_ActuallyResetPlayerAfter(3f));
    }

    IEnumerator _ActuallyResetPlayerAfter(float p)
    {
        yield return new WaitForSeconds(p);
        tr.position = EnvironmentController.Instance.GetRandomSpawnPoint().position;
        EnvironmentController.Instance.followCam.Activate(tr);

        healthManager.currentHealth = GameplayDefaultSettings.Instance.defaultHealth;
        healthManager.currentArmor = GameplayDefaultSettings.Instance.defaultArmor;

        _RefreshHealthAndArmorUI();
    }*/

    void OnApplicationExit()
    {
        //ScrGunController.RemoveCar(this);
    }


    
    public void LeaveRoom()
    {
        //nv.RPC("_LeaveRoom", PhotonTargets.Others);
        //_LeaveRoom();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    /*
    [RPC]
    void _LeaveRoom()
    {
        if (!nv.isMine)
            usernameIndicator.Deactivate();

        ScrController.Instance.RemoveCar(this);

        if (nv.isMine)
            PhotonNetwork.Destroy(nv);
    }*/

    //---------------------------------------------- UI Fumc --------------------------------------------
    public delegate void GetScore(int addingScore);
    public event GetScore OnGetScore;

    public delegate void Kill();
    public event Kill OnKill;

    public delegate void Died();
    public event Died OnDeath;

    public delegate void Damage(float damage,float health,float armor);
    public event Damage OnDamage;


    public void SetCarInfo(CarData info)
    {
        //carParameter.armor = info.armor + GameplayDefaultSettings.Instance.carArmorMultiplyer;
        carParameter.armor = 1f / (1f - info.blockingDamage);
        carParameter.speed = info.speed * GameplayDefaultSettings.Instance.carSpeedMultiplyer;
    }

    public void Steer(Vector2 direction)
    {
        //Debug.Log("steer " + direction);

        if (Car != null)
        {
            Car.steer = direction.x;
            //Car.accel = direction.y;
        }
    }

    public void Accelerate(float acceleration)
    {
        if (Car != null)
            Car.accel = acceleration;
    }

    public void StopAccelerate()
    {
        if (Car!= null)
            Car.accel = 0;
    }


    public void Brake(bool v)
    {
        if (Car != null)
        {
            Car.brake = v;
            Car.accel = -1;
        }
    }


    
    public void Fire(GunType t)
    {
        switch (t)
        {
            case GunType.MachineGun:
                machineGun.SetFireState(true);
                break;

            case GunType.RocketLauncher:
                rocketLauncher.SetFireState(true);
                break;

            case GunType.Miner:
                miner.SetFireState(true);
                nv.RPC("SetMine", PhotonTargets.Others);
                break;
        }
    }

    public void StopFire(GunType t)
    {
        switch (t)
        {
            case GunType.MachineGun:
                machineGun.SetFireState(false);
                break;
            case GunType.RocketLauncher:
                rocketLauncher.SetFireState(false);
                break;

            case GunType.Miner:
                miner.SetFireState(false);
                break;
        }
    }


    int lastMachineGunIndex;
    public void PlayMachineGunSound()
    {
        int index = 0;
        do
        {
            index = Random.Range(0, audioPlayer.minigun.Length);
        } while (index == lastMachineGunIndex);
        
        audioPlayer.minigun[index].Play();
        lastMachineGunIndex = index;
    }

    [RPC]
    public void SetMine()
    {
        if (!nv.isMine)
        {
            miner.SetFireState(true);
        }
            
    }

    public int numberBullets(GunType t)
    {
        switch (t)
        {
            case GunType.MachineGun:
                return machineGun.Bullets;

            case GunType.RocketLauncher:
                return machineGun.Bullets;

            case GunType.Miner:
                return machineGun.Bullets;
        }

        return 0;
    }


    public float RPM
    {
        get { return Car.MotorRPM; }
    }

    public float Veclocity
    {
        get { return Car.rigidbody.velocity.magnitude; }
    }

    public Transform showingCube;



    private Vector3 _correctPlayerPos = Vector3.zero;
    private Quaternion _correctPlayerRot = Quaternion.identity;
    private float _lastSynchronizationTime;
    private float _syncTime = 0f;
    private float _syncDelay = 0f;
    private Vector3 _syncEndPosition;
    private Vector3 _syncStartPosition;
    private Quaternion _syncEndRotation;
    private Quaternion _syncStartRotation;



    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(rigidbody.velocity);

            stream.SendNext(transform.rotation);
            stream.SendNext(rigidbody.angularVelocity);
        }
        else
        {
            _correctPlayerPos = (Vector3)stream.ReceiveNext();
            Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

            this._correctPlayerRot = (Quaternion)stream.ReceiveNext();
            Vector3 syncAngularVelocity = (Vector3)stream.ReceiveNext();

            _syncTime = 0f;
            _syncDelay = Time.time - _lastSynchronizationTime;
            _lastSynchronizationTime = Time.time;

            _syncEndPosition = _correctPlayerPos + syncVelocity * _syncDelay;
            _syncStartPosition = transform.position;

            _syncEndRotation = _correctPlayerRot * Quaternion.Euler(syncAngularVelocity * _syncDelay);
            _syncStartRotation = transform.rotation;

            if (setPositionAfterRespawn && setPositionAfterRespawnUntil == -1)
            {
                setPositionAfterRespawnUntil = Time.time + 1;
            }
        }
    }


    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Server.Instance.ServerController.PlayerLeft(otherPlayer);
    }



    bool isBalanced;
    public bool IsBalanced
    {
        get { return isBalanced; }
    }

    float healthRegTime;

    void Update()
    {
        if (brakeDuration > 0)
        {
            brakeTime += Time.deltaTime;
            float flow = brakeTime / brakeDuration;
            flow = MathHelper.GetEaseFlow(flow, NemoEaseMode.CubicOut);

            if (flow >= 1)
            {
                brakeDuration = 0;
                Car1.rigidbody.velocity = Vector3.zero;
            }
            else
                Car1.rigidbody.velocity = Vector3.Lerp(defaultVelocity, Vector3.zero, flow);
        }


        if (!nv.isMine)
        {
            _syncTime += Time.deltaTime;
            if (!setPositionAfterRespawn)
            {
                Vector3 lastPosition = transform.position;

                transform.position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);
                transform.rotation = Quaternion.Slerp(_syncStartRotation, _syncEndRotation, _syncTime / _syncDelay);

                if (Car != null)
                {
                    Vector3 currentPosition = transform.position;
                    Vector3 diff = currentPosition - lastPosition;
                    diff.y = 0;

                    Vector3 rotateAxis = Vector3.right;
                    if ((diff.x > 0 && transform.forward.x < 0)   || (diff.z > 0 && transform.forward.z < 0))
                        rotateAxis = Vector3.left;

                    float distanceWent = (currentPosition - lastPosition).magnitude;

                    Car.wheelBL.Rotate(rotateAxis, distanceWent * GeneralSettings.Instance.RemoteWheelRotationRate);
                    Car.wheelFL.Rotate(rotateAxis, distanceWent * GeneralSettings.Instance.RemoteWheelRotationRate);
                    Car.wheelBR.Rotate(rotateAxis, distanceWent * GeneralSettings.Instance.RemoteWheelRotationRate);
                    Car.wheelFR.Rotate(rotateAxis, distanceWent * GeneralSettings.Instance.RemoteWheelRotationRate);
                }
            }
            else
            {
                transform.position = this._correctPlayerPos;
                transform.rotation = this._correctPlayerRot;

                if (setPositionAfterRespawnUntil != -1 && setPositionAfterRespawnUntil < Time.time)
                {
                    _syncStartPosition = _syncEndPosition = myTransform.position;
                    _syncStartRotation = _syncEndRotation = myTransform.rotation;

                    setPositionAfterRespawn = false;
                }
                    
            }
        }


        //Debug.Log(Car.GetOnGroundWheels() + " of " + Car.GetWheelsCount() + " wheels are on the ground, rigidbody=" + rigidbody.velocity);
        isBalanced = !(Car.GetOnGroundWheels() < Car.GetWheelsCount() && Mathf.Abs(rigidbody.velocity.x) < 0.1f && Mathf.Abs(rigidbody.velocity.y) < 0.1f && Mathf.Abs(rigidbody.velocity.z) < 0.1f);

        //Debug.Log("on ground wheels=" + Car.GetOnGroundWheels() + ", wheels=" + Car.GetWheelsCount());




        if (nv.isMine && !PhotonNetwork.connected)
        {
            CommonUI.Instance.messageBox.ShowMessage(Messages.DisconnectedFromServer, _RestartScene, true);
        }

        if (nv.isMine)
        {
            /*float x = Mathf.Min(ScrCarController.Instance.tr.rotation.eulerAngles.x, 360 - ScrCarController.Instance.tr.rotation.eulerAngles.x);
            float z = Mathf.Min(ScrCarController.Instance.tr.rotation.eulerAngles.z, 360 - ScrCarController.Instance.tr.rotation.eulerAngles.z);

            Debug.Log("rotation=" + x + "," + z);*/

			if (HasHealthRegeneration && healthManager.CurrentHealth < Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health)
            {
//                healthRegTime += Time.deltaTime;

				if (Time.time >= healthRegTime)
                {
					healthRegTime = Time.time + HealthRegenerationTime;
					float targetHealth = Mathf.Min(Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health, healthManager.CurrentHealth + Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health * HealthRegenerationAmount);
                    float diff = targetHealth - healthManager.CurrentHealth;


                    healthManager.CurrentHealth = targetHealth;
					GameplayUI.Instance.AddRegeneratedHealth(diff / Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health);
                }
            }

            Vector3 camDir = EnvironmentController.Instance.followCam.myTransform.forward;
            camDir.y = 0;

            Vector3 camPos = EnvironmentController.Instance.followCam.myTransform.position;
            camPos.y = (camPos.y + myTransform.position.y) / 2f;

            //Vector3 camPos = targetTransform.position;

            //Debug.DrawRay(camPos, camDir * 100, Color.red, 1);

            RaycastHit hitInfo;

            Ray ray = RectTransformUtility.ScreenPointToRay(EnvironmentController.Instance.followCam.camera, GameplayUI.Instance.targetTransform.position);
            //if (Physics.Raycast(camPos, camDir, out hitInfo, GameplayDefaultSettings.Instance.TargetDistance, bulletsLayer.value))

            float range = machineGun.MachineGunInfo.range;
            if (Physics.Raycast(ray, out hitInfo, range, bulletsLayer.value))
            {
                targetPoint = hitInfo.point;
                if (showingCube != null)
                {
                    showingCube.renderer.enabled = true;
                    showingCube.position = targetPoint;
                }
            }
            else
            {
                if (showingCube != null)
                    showingCube.renderer.enabled = false;

                targetPoint = camPos + camDir * range;
            }

            Vector3 deltaPosition = myTransform.position - lastPosition;
            deltaPosition.y = 0; // vertical movements doesn't count
            float moveAmount = deltaPosition.magnitude;

            Accounting.Instance.currentUser.statistics.CoveredDistance(moveAmount);
            QuestManager.Instance.Drove(moveAmount);

            lastPosition = myTransform.position;
        }
        /*else
        {
            if (Car != null)
            {
                float diff = (transform.position - lastPosition).sqrMagnitude;
                lastPosition = transform.position;

                Car.RotateWheels(diff);
            }
        }*/
    }

    Vector3 lastPosition;

    public void Jump()
    {
        //rigidbody.AddForce(0, GameplayDefaultSettings.Instance.JumpForce * rigidbody.mass, 0);

        //Vector3 force = new Vector3(0, GameplayDefaultSettings.Instance.JumpForce * rigidbody.mass, 0);

        Vector3 force = myTransform.up * (GameplayDefaultSettings.Instance.JumpForce * rigidbody.mass);
        Vector3 pos = myTransform.position - (myTransform.up * 2);

        rigidbody.AddForceAtPosition(force, pos);
    }

    void _RestartScene()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }


    private JControlledCar Car;
    public JControlledCar Car1
    {
        get { return Car; }
    }

    private JFlipBack flipJack;
    private AudioSource AS;

    public ScrGunsBaseController machineGun;
    public ScrGunsBaseController rocketLauncher;
    public ScrGunsBaseController miner;
    

    float lastResoawnTime;
    public float LastResoawnTime
    {
        get { return lastResoawnTime; }
    }


    JCarEngineAudio engineAudio;
    float setPositionAfterRespawnUntil;
    bool setPositionAfterRespawn;

    public void Respawn(int respawnPointIndex)
    {
        lastResoawnTime = Time.time; 
        setPositionAfterRespawn = true;
        setPositionAfterRespawnUntil = -1;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;


        //Transform t = EnvironmentController.Instance.GetSpawnPointByIndex(respawnPointIndex);
        Transform t = EnvironmentController.Instance.GetSafestSpawnPoint();
        flipJack.Respawn(t.position, t.rotation);

        if (nv.isMine)
        {
            lifeIndex++;
            nv.RPC("_SyncLifeIndex", PhotonTargets.AllBuffered, lifeIndex);
        }

        EnvironmentController.Instance.followCam.Activate(myTransform);


        if (!gameEnded)
            GameplayUI.Instance.scoreboard.Deactivate();
        Init();

        GameplayUI.Instance.audioPlayer.Play(GameplayUI.Instance.audioPlayer.startEngine);
        engineAudio.StopFor(1f);
    }

    int safeSpawnPointIndex = -1;

    [RPC]
    void InstantiateAtSafePoint(int spawnPointIndex)
    {
        if (safeSpawnPointIndex == -2)
        {
            safeSpawnPointIndex = spawnPointIndex;
            Respawn();
        }
        else
            safeSpawnPointIndex = spawnPointIndex;
    }

    public void Respawn()
    {
        if (rigidbody == null)
            return;

//        if (safeSpawnPointIndex == -1)
//        {
//            safeSpawnPointIndex = -2;
//            return;
//        }

        lastResoawnTime = Time.time;
        setPositionAfterRespawn = true;
        setPositionAfterRespawnUntil = -1;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        //Transform t = EnvironmentController.Instance.GetNearestSpawnPoint(transform.position);
        //Transform t = EnvironmentController.Instance.GetMostFarSpawnPoint();
        //Transform t = EnvironmentController.Instance.GetRandomSpawnPoint();
        //Transform t = EnvironmentController.Instance.GetSafestSpawnPoint();

		Transform t = EnvironmentController.Instance.GetSpawnPointByIndex( ScrController.Instance.FindASafeIndex ());
        flipJack.Respawn(t.position, t.rotation);

        safeSpawnPointIndex = -1;

        if (nv.isMine)
        {
            lifeIndex++;
            nv.RPC("_SyncLifeIndex", PhotonTargets.AllBuffered, lifeIndex);
        }

        if (!gameEnded)
            GameplayUI.Instance.scoreboard.Deactivate();

        EnvironmentController.Instance.followCam.Activate(myTransform);
        Init();

        nv.RPC("MakeInvincible", PhotonTargets.All);

        GameplayUI.Instance.audioPlayer.Play(GameplayUI.Instance.audioPlayer.startEngine);
        engineAudio.StopFor(1f);
        MakeInvincible();
    }

    

    public void ResetScores()
    {
        kills = deaths = score = 0;
        nv.RPC("_SyncScores", PhotonTargets.Others, this.score, this.deaths, this.kills, this.cheats);

        if (OnGetScore!=null)
            OnGetScore(score);

        if (OnKill != null)
            OnKill();

        if (OnDeath != null)
            OnDeath();
    }



    [RPC]
    void MakeInvincible()
    {
        isInvincible = true;
        StartCoroutine(_Blink(GameplayDefaultSettings.Instance.invincibleTime));

        if (usernameIndicator != null)
            usernameIndicator.ShowInvincible();
    }

    IEnumerator _Blink(float duration)
    {
        float singleTimeDuration = duration / 12f;
        float onTime = singleTimeDuration * 0.6f;
        float offTime = singleTimeDuration - onTime;
        float fraction = onTime / 10f;

        for (int i = 0; i < 12; i++)
        {
            _SetVisible(false);
            yield return new WaitForSeconds(offTime);

            _SetVisible(true);
            yield return new WaitForSeconds(onTime - (i - 6) * fraction);
        }

        isInvincible = false;
        if (usernameIndicator != null)
            usernameIndicator.HideInvincible();
    }

    [RPC]
    void _SyncLifeIndex(int lifeIndex)
    {
        this.lifeIndex = lifeIndex;
    }

    

    public void Init()
    {
        healthManager.CurrentHealth = Information.Instance.carInfo[(int)carType].levels[0].health;
        healthManager.CurrentShiled = GameplayDefaultSettings.Instance.defaultShield;

        if (nv.isMine)
            nv.RPC("SyncHealthAndArmor", PhotonTargets.Others, healthManager.CurrentHealth, healthManager.CurrentShiled);

        _RefreshHealthAndArmorUI();
        isAlive = true;
    }
  
    public void AddMines(int mine)
    {
        miner.Bullets += mine;
    }

    public void AddRockets(int rockets)
    {
        rocketLauncher.Bullets += rockets;

    }

    public void AddItems(int n, CollectibleType t)
    {
        switch (t)
        {
            case CollectibleType.RocketLauncher:
                rocketLauncher.Bullets += n;
                break;

            case CollectibleType.Miner:
                miner.Bullets += n;
                break;

            case CollectibleType.Health:
				int heal = Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].levels[0].health / n;
				AddToHealth(heal);
                break;

            case CollectibleType.Armor:
			    int arm = Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].levels[0].health / n;
				AddToShield(arm);
                break;
        }

        if (GameplayUI.IsTutorial)
            TutorialSceneManager.Instance.CarCollectedItem(t, n);
        else
            nv.RPC("SyncHealthAndArmor", PhotonTargets.Others, healthManager.CurrentHealth, healthManager.CurrentShiled);
    }

    public void AddItems(int rocket, int mine, int health, int armor)
    {
        rocketLauncher.Bullets += rocket;
        miner.Bullets += mine;
        AddToHealth(health);
        AddToShield(armor);

        nv.RPC("SyncHealthAndArmor", PhotonTargets.Others, healthManager.CurrentHealth, healthManager.CurrentShiled);
    }

    //---------------------------------------------------TouchPad-------------------------------------------------------

    public void AddCheats()
    {
        cheats++;
    }

    public void Deactivate()
    {
        if (usernameIndicator != null)
        {
            usernameIndicator.Deactivate();
            usernameIndicator = null;
        }

        isOnLine = false;
        if (Server.Instance.GameSessionState != GameSessionState.Finished && Server.Instance.RemainingTime > 10)
            isDropped = true;
    }

    public void DeactivateAllBullets()
    {
        machineGun.DeactivateAllBullets();
        rocketLauncher.DeactivateAllBullets();
    }

    public void StopImmediately()
    {
        Car1.rigidbody.velocity = new Vector3();
    }

    float brakeTime;
    float brakeDuration;
    Vector3 defaultVelocity;

    public void StopIn(float duration)
    {
        defaultVelocity = Car1.rigidbody.velocity;
        brakeDuration = duration;
        brakeTime = 0;
    }

    public void SetHealthRegeneration(float amount, float timerInterval)
    {
        //Debug.Log(name + " has health regeneration");
        nv.RPC("_SetHealthRegeneration", PhotonTargets.AllBuffered, amount, timerInterval);
    }

    [RPC]
    void _SetHealthRegeneration(float amount, float timerInterval)
    {
        HealthRegenerationAmount = amount;
        HealthRegenerationTime = timerInterval;

        hasHealthRegeneration = true;

        if (usernameIndicator != null)
            usernameIndicator.Refresh();
    }

    public void SetEnhancedFireBullet(float amount)
    {
        nv.RPC("_SetEnhancedFireBullet", PhotonTargets.AllBuffered, amount);
    }

    [RPC]
    void _SetEnhancedFireBullet(float amount)
    {
        MachineGunDamageMultiplyer = amount;
        hasFireBullet = true;

        ScrGunController machineGun2 = (ScrGunController)machineGun;
        machineGun2.SetRedFire();

        if (usernameIndicator != null)
            usernameIndicator.Refresh();
    }

}

public enum GunType
{
    MachineGun,
    RocketLauncher,
    Miner,
}

public enum CollectibleType
{
    RocketLauncher,
    Miner,
    Health,
    Armor,
    All,
}

public enum CarType
{
    Cheetah,
    Eldorado,
    Ram,
    Nissan,
    Forg,
    Dragon,
}