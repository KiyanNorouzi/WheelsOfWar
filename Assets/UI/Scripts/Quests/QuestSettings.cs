using UnityEngine;
using System.Collections;

public class QuestSettings : MonoBehaviour 
{
    #region Singleton

    static QuestSettings _instance;

    public static QuestSettings Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }



    #endregion



    public string[] descText, toGoText;
    public bool[] isOneGameRoundQuest;
    public Sprite[] logos;
}
