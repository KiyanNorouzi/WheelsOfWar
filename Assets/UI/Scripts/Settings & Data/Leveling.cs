using UnityEngine;
using System.Collections;

public class Leveling : MonoBehaviour 
{
    #region Singleton

    static Leveling _instance;
    public static Leveling Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion

    public int firstLevelScore, firstLevelReward;
    public int maxLevelUpMoney;

    public int GetLevelForScore(int score)
    {
        float a = (8f * score) / firstLevelScore;
        float a2 = 1 + a;
        float l = (float)((1 + Mathf.Sqrt(a2)) / 2f);

        return (int)l;
    }

    public float GetRemainingToNextLevel(int score)
    {
        float a = (8f * score) / firstLevelScore;
        float a2 = 1 + a;
        float l = (float)((1 + Mathf.Sqrt(a2)) / 2f);

        return l - (int)l;
    }

    public int GetScoreForLevel(int level)
    {
        return (int)(((level * (level - 1)) / 2f) * firstLevelScore);
    }

    public int GetRewardForLevel(int level)
    {
        int rewardAmount = (int)(((level * (level - 1)) / 2f) * firstLevelReward);
        return Mathf.Min(maxLevelUpMoney, rewardAmount);
    }
}
