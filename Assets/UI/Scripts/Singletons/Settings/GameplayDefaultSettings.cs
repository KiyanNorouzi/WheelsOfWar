using UnityEngine;
using System.Collections;

public class GameplayDefaultSettings : MonoBehaviour 
{
    #region Singleton

    static GameplayDefaultSettings _instance;

    public static GameplayDefaultSettings Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    #endregion 
	

    public float maxHealth, maxShield;
    public float defaultHealth, defaultShield;
    public float crashDamage, environmentDamage;
    //public int defaultMinesCount, defaultRocketsCount;
    //public float bulletDamage, mineDamage, rocketDamage;

    //public int GameTime;
    public int SealRoomAfterTime, SealRoomAfterKills, CloseGameAfter;
    public int MinimumPlayersToStartGame;
    public int TimeForStartGameAfterLastPlayerJoined;

    public float invincibleTime;
    public float carAttakMultiplyer, carArmorMultiplyer, carSpeedMultiplyer;
    public int MinimumPing;
    public float PingAlarmTime;
    public float TimeAlarmThreshold;
    public float WaitingMessageTime;
    public float TimeSync;
    public Color MyMineColor, EnemyMineColor;
    public float JumpForce;
    public float BulletSpeed, RocketSpeed;
    public float AdTime;
	public bool isTeamMatch;
	public Material blueTeamMaterial;
	public Material redTeamMaterial;
	public int rewardOfWinnerTeam = 100;



    public GameplaySettingSet[] settingSets;

 

    public GameplaySettingSet Settings
    {
        get 
        {  
            int index = 0;
            if (Server.Instance != null)
            {
                if (!Server.Instance.IsLevel1Game)
                    index = 1;
            }

            return settingSets[index];
        }
    }


   
    [System.Serializable]
    public class GameplaySettingSet
    {
        public string tag;
        public int defaultMinesCount, defaultRocketsCount;
        public int GameTime, KillsNumberForWin, KillNumberForWinTeamDeathMatch;
    }
}