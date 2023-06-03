using UnityEngine;
using System.Collections;

public class DailyGifts : MonoBehaviour 
{
    #region Singleton

    static DailyGifts _instance;

    public static DailyGifts Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion 


    public DailyGiftItem[] items;
}

[System.Serializable]
public class DailyGiftItem
{
    public DailyGiftRewardType type;
    public int amount;
}