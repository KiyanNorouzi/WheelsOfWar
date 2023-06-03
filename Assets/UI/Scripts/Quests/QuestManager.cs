using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour 
{
    #region Singleton

    static QuestManager _instance;

    public static QuestManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion

    public Quest[] quests;
    public int[] currentQuestIndexes;


    Quest[] realQuests;

    public bool testMode;
    public float paramsMultiply, singleRoundParamsMultiply, paramsMaxMultiply, singleRoundMaxMultiply;
    public float scoreMultiply, scoreMaxMultiply;


    int maxQuestIndex;
    public int MaxQuestIndex
    {
        get { return maxQuestIndex; }
    }


    [ContextMenu("Clear Quest Info")]
    void clearQuestInfo()
    {
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.DeleteKey(string.Concat("q", i));
            PlayerPrefs.DeleteKey(string.Concat("qv", i));
        }
    }

    public int RemainingQuests
    {
        get
        {
            int remainingQuests = 0;
            for (int i = 0; i < currentQuestIndexes.Length; i++)
            {
                if (this[i].State == QuestState.Available)
                    remainingQuests++;
            }

            return remainingQuests;
        }
    }

    public int DoneQuests
    {
        get
        {
            int doneQuests = 0;
            for (int i = 0; i < currentQuestIndexes.Length; i++)
            {
                if (this[i].State != QuestState.Available)
                    doneQuests++;
            }

            return doneQuests;
        }
    }



    void Start()
    {
        Load();
    }

    public void Save()
    {
        for (int i = 0; i < currentQuestIndexes.Length; i++)
        {
            PlayerData.SetInt(string.Concat("q", i), currentQuestIndexes[i]);
            PlayerData.SetFloat(string.Concat("qv", i), this[i].Progress);
            PlayerData.SetInt(string.Concat("qs", i), (int)this[i].State);
        }
    }

    public void Load()
    {
        int repeatCount = 20;
        realQuests = new Quest[quests.Length * repeatCount];
        
        float paramMultiplyK, singleRoundParamsMultiplyK, scoreMultiplyK;
        for (int i = 0; i < repeatCount; i++)
        {
            if (i == 0)
                paramMultiplyK = scoreMultiplyK = singleRoundParamsMultiplyK = 1;
            else
            {
                scoreMultiplyK = Mathf.Min(i * scoreMultiply, scoreMaxMultiply);
                paramMultiplyK = Mathf.Min(i * paramsMultiply, paramsMaxMultiply);
                singleRoundParamsMultiplyK = Mathf.Min(i * singleRoundParamsMultiply, singleRoundMaxMultiply);
            }


            for (int j = 0; j < quests.Length; j++)
            {
                int index = j + i * quests.Length;
                realQuests[index] = new Quest();
                realQuests[index].type = quests[j].type;

                if (QuestSettings.Instance.isOneGameRoundQuest[(int)realQuests[index].type])
                    realQuests[index].param = (int)(quests[j].param * singleRoundParamsMultiplyK);
                else
                    realQuests[index].param = (int)(quests[j].param * paramMultiplyK);

                realQuests[index].rewardGold = (int)(quests[j].rewardGold * scoreMultiplyK);
                realQuests[index].OnDone += _QuestCompleted;
                realQuests[index].OnStateChanged += _QuestStateChanged;
            }
        }



        currentQuestIndexes = new int[3];
        maxQuestIndex = 0;

        //int j = 0;
        if (Application.isEditor && testMode)
        {
            /*for (int i = 0; i < quests.Length; i++)
            {
                if (quests[i].
            }*/
        }
        else
        {
            for (int i = 0; i < currentQuestIndexes.Length; i++)
            {
                //if (!Application.isEditor || !testMode)
                //{
                    currentQuestIndexes[i] = PlayerPrefs.GetInt(string.Concat("q", i), i);
                    //Debug.Log("quest " + i + "=" + currentQuestIndexes[i]);
                //}

                realQuests[currentQuestIndexes[i]].SetProgress(PlayerData.GetFloat(string.Concat("qv", i), 0));
                realQuests[currentQuestIndexes[i]].SetState((QuestState)PlayerData.GetInt(string.Concat("qs", i), 0));

                //Debug.Log("Quest " + i + " loaded, progress=" + this[i].Progress + ", state=" + this[i].State);


                if (currentQuestIndexes[i] > maxQuestIndex)
                    maxQuestIndex = currentQuestIndexes[i];
            }
        }




        CheckForMakingNewQuestsIfDayChanged();
        ServerTime.Instance.OnDayChanged += CheckForMakingNewQuestsIfDayChanged;
    }

    public void CheckForMakingNewQuestsIfDayChanged()
    {
        int questsDayIndex = PlayerData.GetInt("questsDay", 0);

        if (questsDayIndex < ServerTime.Instance.DayIndex)
        {
            MakeNewQuests();
            PlayerData.SetInt("questsDay", ServerTime.Instance.DayIndex);
        }
    }

    void _QuestStateChanged(Quest sender)
    {
        if (sender.State == QuestState.Claimed || sender.State == QuestState.Skipped)
            Save();
    }


    /*
    public void GuessQuestsIfNeeded()
    {
        _GuessQuests(true);
    }

    public void FixQuests()
    {
        _GuessQuests(false);
    }

    /*
    void _GuessQuests(bool checkForCurrentQuests)
    {
        int level = Information.Instance.NicknameIndex;
        int pointsNeededForTheLevel = 0;
        for (int i = 0; i <= level; i++)
        {
            pointsNeededForTheLevel += NickNameSettings.Instance.nicknames[i].points;
        }


        int leastQuestIndex = 0;
        int pointsEarnedByQuests = 0;
        for (int i = 0; i < realQuests.Length; i++)
        {
            pointsEarnedByQuests += realQuests[i].points;
            if (pointsEarnedByQuests >= pointsNeededForTheLevel)
            {
                leastQuestIndex = i;
                break;
            }
        }


        int minQuestIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            minQuestIndex = Mathf.Min(minQuestIndex, currentQuestIndexes[i]);
        }

        //Debug.Log("min quest index it should be= " + leastQuestIndex + ", current min quest index=" + minQuestIndex);

        if (checkForCurrentQuests) 
        {
            if (minQuestIndex < leastQuestIndex)
            {
                for (int j = 0; j < 3; j++)
                    currentQuestIndexes[j] = leastQuestIndex + j;
            }
        }
        else
        {
            for (int j = 0; j < 3; j++)
                currentQuestIndexes[j] = leastQuestIndex + j;
        }
    }
    */



    public Quest this[int index]
    {
        get
        {
            return realQuests[currentQuestIndexes[index]];
            
            
            /*
            int questIndex = currentQuestIndexes[index];

            int qIndex = questIndex % realQuests.Length;
            int multiplyIndex= questIndex / realQuests.Length;
            
            if (multiplyIndex > 0)
            {
                Quest newQuest = realQuests[qIndex];
                newQuest.param *=(int)(multiplyIndex * paramsMultiply);
                newQuest.points = (int)(multiplyIndex * scoreMultiply);

                return newQuest;
            }
            else
                return realQuests[qIndex]; */
        }
    }

    /*
    void _QuestCompleted(int i)
    {
        Quest q = this[i];
        string text = QuestSettings.Instance.descText[(int)q.type].Replace("*", q.param.ToString());
        //UI.Instance.questCompleteMenu.Activate(text);
        Debug.Log("Quest " + i + " done.");
    }*/

    void _QuestCompleted(Quest sender)
    {
        int index = -1;
        for (int i = 0; i < currentQuestIndexes.Length; i++)
        {
            if (sender == realQuests[currentQuestIndexes[i]])
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            Debug.Log("Quest not found. " + sender.type + ", amount=" + sender.param);
        else
            _QuestCompleted(index);
    }

    void _QuestCompleted(int i)
    {

    }

    void _AddProgressForTypes(float progressAmount, params QuestType[] types)
    {
        //_AddProgressForTypesIfParamIsLessThan(progressAmount, float.MaxValue, types);
        for (int i = 0; i < currentQuestIndexes.Length; i++)
        {
            Quest q = this[i];
            if (q.Finished)
                continue;

            for (int j = 0; j < types.Length; j++)
            {
                if (q.type == types[j])
                {
                    q.ProgressHappened(progressAmount);
                    /*if (q.ProgressPercent >= 1)
                    {
                        _QuestCompleted(i);
                    }*/

                    break;
                }
            }
        }
    }

    void _AddProgressForTypesIfParamIsLessThan(float progressAmount, float p, params QuestType[] types)
    {
        for (int i = 0; i < currentQuestIndexes.Length; i++)
        {
            Quest q = this[i];

            if (q.Finished)
                continue;

            for (int j = 0; j < types.Length; j++)
            {
                if (q.type == types[j] && q.param <= p)
                {
                    q.ProgressHappened(progressAmount);
                    if (q.ProgressPercent >= 1)
                    {
                        _QuestCompleted(i);
                    }

                    break;
                }
            }
        }
    }

    public void ResetProgressForOneGameTypes()
    {
        for (int i = 0; i < currentQuestIndexes.Length; i++)
        {
            Quest q = this[i];

            if (q.State != QuestState.Available)
                continue;

            if (QuestSettings.Instance.isOneGameRoundQuest[(int)q.type])
                q.Reset();
        }
    }



    #region Helper Methods

    public void HitMine()
    {
        _AddProgressForTypes(1, QuestType.MineHit, QuestType.MineHitOneGame);
    }

    public void HitRocket()
    {
        _AddProgressForTypes(1, QuestType.RocketHit, QuestType.RocketHitOneGame);
    }

    public void PickedUp(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.RocketLauncher:
                _AddProgressForTypes(1, QuestType.PickedupRockets, QuestType.PickedupRocketsOneGame);
                break;
            case CollectibleType.Miner:
                _AddProgressForTypes(1, QuestType.PickedUpMine, QuestType.PickedUpMineOneGame);
                break;
            case CollectibleType.Health:
                
                break;
            case CollectibleType.Armor:
                _AddProgressForTypes(1, QuestType.PickedUpShield, QuestType.PickedUpShieldOneGame);
                break;
            case CollectibleType.All:
                _AddProgressForTypes(1, QuestType.PickedUpAll, QuestType.PickedUpAllOneGame);
                break;
        }
    }

    public void DestroyedCar()
    {
        _AddProgressForTypes(1, QuestType.DestroyCar, QuestType.DestroyCarOneGame);
    }

    public void PointsEarned(int points, bool isTeamMatch)
    {
        if (isTeamMatch)
            _AddProgressForTypes(points, QuestType.EarnPoints, QuestType.EarnPointsOneGame, QuestType.EarnPointsInTeamMatches);
        else
            _AddProgressForTypes(points, QuestType.EarnPoints, QuestType.EarnPointsOneGame, QuestType.EarnPointsInSingleMatches);
    }

    public void FiredBullets(int bullets = 1)
    {
        _AddProgressForTypes(bullets, QuestType.FireBullets);
    }

    public void EarnedFirstPlace()
    {
        _AddProgressForTypes(1, QuestType.EarnPlaceInMatch);
    }

    public void UsedBoosters(int boosters)
    {
        _AddProgressForTypes(boosters, QuestType.UseBoosters);
    }

    public void Drove(float meters)
    {
        _AddProgressForTypes(meters, QuestType.DriveMeters);
    }

    public void PlayedMatch(Maps map, bool isTeamMatch)
    {
        if (map == Maps.Junkyard)
            _AddProgressForTypes(1, QuestType.PlayInJunkyard);
        else if (map == Maps.TrainStation)
            _AddProgressForTypes(1, QuestType.PlayInTrainStation);

        if (isTeamMatch)
            _AddProgressForTypes(1, QuestType.PlayTeamMatches);
        else
            _AddProgressForTypes(1, QuestType.PlaySingleMatches);

        _AddProgressForTypes(1, QuestType.PlayMatches);
    }

    public void ResetProgresses()
    {
        for (int i = 0; i < currentQuestIndexes.Length; i++)
        {
            //PlayerData.SetInt(string.Concat("q", i), currentQuestIndexes[i]);
            PlayerData.SetFloat(string.Concat("qv", i), 0);
            PlayerData.SetInt(string.Concat("qs", i), (int)QuestState.Available);
        }

        Load();
    }

    public void NewGameStarted()
    {
        ResetProgressForOneGameTypes();
        Save();
    }

    #endregion



    public bool HasCompletedQuest()
    {
        for (int i = 0; i < 3; i++)
        {
            if (this[i].Finished)
                return true;
        }

        return false;
    }

    public void MakeNewQuests()
    {
        bool isSomethingChanged = false;
        for (int i = 0; i < 3; i++)
        {
            if (this[i].State == QuestState.Skipped || this[i].State == QuestState.Claimed)
            {
                isSomethingChanged = true;

                maxQuestIndex++;
                if (maxQuestIndex >= realQuests.Length)
                    maxQuestIndex = 0;

                if (IsQuestActive(maxQuestIndex))
                {
                    i--;
                    continue;
                }

                /*if (realQuests[maxQuestIndex].type == QuestType.BuySomething && !StoreDB.Instance.IsAnythingLeftToBuy)
                {
                    i--;
                    continue;
                }*/

                bool isQuestDuplicate = false;
                for (int j = 0; j < 3; j++)
                {
                    if (j != i && !this[j].Finished && Quest.IsThisQuestsFromAFamily(realQuests[maxQuestIndex].type, this[j].type))
                    {
                        i--;
                        isQuestDuplicate = true;
                        break;
                    }
                }

                if (isQuestDuplicate)
                {
                    i--;
                    continue;
                }

                currentQuestIndexes[i] = maxQuestIndex;
                this[i].Reset();
            }
        }

        if (isSomethingChanged)
            Save();
    }

    public bool IsQuestActive(int questIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentQuestIndexes[i] == questIndex)
                return true;
        }

        return false;
    }

    public bool IsThisTyppeOfQuestActiveNow(QuestType t)
    {
        for (int i = 0; i < 3; i++)
        {
            if (realQuests[currentQuestIndexes[i]].type == t)
                return true;
        }

        return false;
    }

}