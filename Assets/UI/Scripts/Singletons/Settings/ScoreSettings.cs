using UnityEngine;
using System.Collections;

public class ScoreSettings : MonoBehaviour 
{
    #region Singleton

    static ScoreSettings _instance;

    public static ScoreSettings Instance
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


    public int machineGunBulletScore;
    public int rocketScore;
    public int mineScore;
    public int crashScore;
    public int suicideScore;
    public int killScore;
	public int scoreLeage;
	public int otherReward;

    //public int killNumberForWin;

    public float ScoreMoneyConvertRate;
    public float CoolDowntimeToMoneyConvertRate;
    public float[] scoreMultiplyers;
}
