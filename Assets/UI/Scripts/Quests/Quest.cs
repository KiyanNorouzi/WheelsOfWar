using UnityEngine;
using System.Collections;

[System.Serializable()]
public class Quest 
{
    public delegate void questDone(Quest sender);
    public event questDone OnDone, OnStateChanged;



    public QuestType type;
    public int param;
    public int rewardGold = 1;

    QuestState state;
    public QuestState State
    {
        get { return state; }
        set
        {
            state = value;
            if (OnStateChanged != null)
                OnStateChanged(this);
        }
    }



    float progress;

    public float Progress
    {
        get { return progress; }
        private set 
        { 
            progress = value;
            if (progress >= param && OnDone != null)
                OnDone(this);
        }
    }

    public float ProgressPercent
    {
        get
        {
            if (progress > param)
                return 1;
            else
                return ((float)progress) / ((float)param);
        }
    }

    public bool Finished
    {
        get
        {
            return progress >= param;
        }
    }

    public void Reset()
    {
        Progress = 0;
        state = QuestState.Available;
    }

    public void ProgressHappened(float progressAmount)
    {
        Progress += progressAmount;
        if (Finished && state == QuestState.Available)
            state = QuestState.Done;
    }

    public void Skip()
    {
        //Progress = param;
        State = QuestState.Skipped;
    }

    public void Claimed()
    {
        state = QuestState.Claimed;
    }

    public void SetProgress(float progress)
    {
        this.progress = progress;
    }


    public static bool IsThisQuestsFromAFamily(QuestType q1, QuestType q2)
    {
        if (q1 == q2)
            return true;

        string st1 = q1.ToString().ToLower().Replace("onegame", "");
        string st2 = q2.ToString().ToLower().Replace("onegame", "");

        if (st1 == st2)
            return true;



        /*if (IsThere(q1, q2, QuestType.GetHitFromNissanAfter, QuestType.GetHitFromTaxiAfter, QuestType.GetHitFromVanetteAfter))
            return true;*/

        return false;
    }

    private static bool IsThere(QuestType q1, QuestType q2, params QuestType[] questTypes)
    {
        bool foundFirstOne = false;
        for (int i = 0; i < questTypes.Length; i++)
        {
            if (q1 == questTypes[i])
            {
                foundFirstOne = true;
                break;
            }
        }

        if (!foundFirstOne)
            return false;
        else
        {
            for (int i = 0; i < questTypes.Length; i++)
            {
                if (q2 == questTypes[i])
                    return true;
            }
        }

        return false;
    }


    public void SetState(QuestState questState)
    {
        this.state = questState;
    }
}

public enum QuestState
{
    Available,
    Done,
    Claimed,
    Skipped
}

public enum QuestType
{
    MineHit,
    MineHitOneGame,
    RocketHit,
    RocketHitOneGame,
    PickedupRockets,
    PickedUpMine,
    PickedUpAll,
    PickedUpShield,
    PickedupRocketsOneGame,
    PickedUpMineOneGame,
    PickedUpAllOneGame,
    PickedUpShieldOneGame,
    DestroyCar,
    DestroyCarOneGame,
    EarnPoints,
    EarnPointsOneGame,
    EarnPointsInSingleMatches,
    EarnPointsInTeamMatches,
    FireBullets,
    EarnPlaceInMatch,
    UseBoosters,
    PlayMatches,
    PlaySingleMatches,
    PlayTeamMatches,
    DriveMeters,
    PlayInTrainStation,
    PlayInJunkyard,

    


    LastItem,
}