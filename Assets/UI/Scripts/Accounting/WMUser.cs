using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GameAnalyticsSDK;

public class WMUser : ParsableClass
{
    #region Properties

    int id;
    public int Id
    {
        get { return id; }
    }

    UserLoginType userType;
    public UserLoginType UserType
    {
        get { return userType; }
        //set { userType = value; }
    }

    string displayName;
    public string DisplayName
    {
        get { return displayName; }
        set 
        { 
            displayName = value;
            usernameChangesCount++;

            Debug.Log("autosync=" + autoSync);
            if (autoSync)
                Accounting.Instance.ChangeDisplayName(id, displayName, null, _DisplayNameChangeFailed);

            if (OnDisplayNameChanged != null)
                OnDisplayNameChanged();
        }
    }

    int usernameChangesCount;
    public int UsernameChangesCount
    {
        get { return usernameChangesCount; }
    }

    DateTime lastRefillTime;
    public int LastRefillTimeInSeconds
    {
        get 
        {
            TimeSpan span = DateTime.Now - lastRefillTime;
            return (int)(span.TotalSeconds + extraSecondsForGasRefill);
        }
    }

    int extraSecondsForGasRefill;
    public int ExtraSecondsForGasRefill
    {
        get { return extraSecondsForGasRefill; }
        set { extraSecondsForGasRefill = value; }
    }



    int leagueIndex;
    public int LeagueIndex
    {
        get { return leagueIndex; }
        set { leagueIndex = value; }
    }


    int scoreInLeague;
    public int ScoreInLeague
    {
        get { return scoreInLeague; }
        set { scoreInLeague = value; }
    }

    int rankInLeague;
    public int RankInLeague
    {
        get { return rankInLeague; }
        set { rankInLeague = value; }
    }


    int bills;
    public int Bills
    {
        get { return bills; }
        set 
        {
            bool changed = (value != bills);

            bills = value;
            if (AutoSync)
                Accounting.Instance.UpdateMoney(id, golds, bills, null, _SyncMoneyAgain);

            if (changed && OnBillsChanged != null)
                OnBillsChanged();
        }
    }

    int golds;
    public int Golds
    {
        get { return golds; }
        set 
        {
            bool changed = (value != golds);

            golds = value;
            if (AutoSync)
                Accounting.Instance.UpdateMoney(id, golds, bills, null, _SyncMoneyAgain);

            if (changed && OnGoldChanged != null)
                OnGoldChanged();
        }
    }

    int totalScore;
    public int TotalScore
    {
        get { return totalScore; }
        set 
        {
            int lastLevel = Level;

            int addingScore = value - totalScore;
            totalScore = value;
            if (AutoSync)
                Accounting.Instance.UpdateScore(id, addingScore, null, _SyncScoreAgain);

            if (OnTotalScoreChanged != null)
                OnTotalScoreChanged();

            if (OnLevelChanged != null && lastLevel != Level)
                OnLevelChanged();

        }
    }

    string deviceID;
    public string DeviceID
    {
        get { return deviceID; }
        set { deviceID = value; }
    }

    string username;
    public string Username
    {
        get { return username; }
        //set { googlePlayID = value; }
    }

    int gas;
    public int Gas
    {
        get { return Mathf.Clamp(gas, 0, MaxGas); }
        set 
        {
            gas = Mathf.Clamp(value, 0, MaxGas); // Mathf.Clamp(value, 0, MaxEnergy);
            if (OnEnergyChanged != null)
                OnEnergyChanged();

            if (AutoSync)
            {
                Accounting.Instance.UpdateGas(Id, Gas, null, _GasUpdateFailed);
            }
                
        }
    }

    


    public int MaxGas
    {
        get
        {
            int extraEnergyBars = 0;
            for (int i = 0; i < vipPackages.Count; i++)
			{
                for (int j = 0; j < VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions.Length; j++)
			    {
                    if (VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions[j].type == VIPActionType.GasSlotsOpen)
                        extraEnergyBars += Mathf.RoundToInt(VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions[j].multiplyer);
			    }
			}

            return 10 + extraEnergyBars;
        }
    }

    public bool IsGasTankFull
    {
        get
        {
            return Gas >= MaxGas;
        }
    }

    int consecutiveDays;
    public int ConsecutiveDays
    {
        get { return consecutiveDays; }
        set
        {
            consecutiveDays = value;
            if (AutoSync)
                Accounting.Instance.UpdateConsecutiveDays(Id, ConsecutiveDays, ConsecutiveDaysRewardClaimed, null, _UpdateConsecutiveDaysFailed);
        }
    }

    int consecutiveDaysRewardClaimed;
    public int ConsecutiveDaysRewardClaimed
    {
        get { return consecutiveDaysRewardClaimed; }
        set 
        { 
            consecutiveDaysRewardClaimed = value;
            if (AutoSync)
                Accounting.Instance.UpdateConsecutiveDays(Id, ConsecutiveDays, ConsecutiveDaysRewardClaimed, null, _UpdateConsecutiveDaysFailed);
        }
    }

    private void _UpdateConsecutiveDaysFailed(bool isServerError, string errorText)
    {
        Accounting.Instance.UpdateConsecutiveDays(Id, ConsecutiveDays, ConsecutiveDaysRewardClaimed, null, _UpdateConsecutiveDaysFailed);
    }

    private void _GasUpdateFailed(bool isServerError, string errorText)
    {
        Accounting.Instance.UpdateGas(Id, gas, null, _GasUpdateFailed);
    }
    


#endregion

    #region Sync Methods


    public void ChangeUsernameAndUserType(string newUsername, UserLoginType type)
    {
        this.userType = type;
        this.username = newUsername;

        Accounting.Instance.ChangeUsernameAndType(id, username, userType, null, _ChangeUsernameAndUserTypeAgain);
    }

    void _ChangeUsernameAndUserTypeAgain(bool serverError, string errorText)
    {
        Accounting.Instance.ChangeUsernameAndType(id, username, userType, null, _ChangeUsernameAndUserTypeAgain);
    }

    void _SyncMoneyAgain(bool serverError, string errorText, int golds, int bills)
    {
        Accounting.Instance.UpdateMoney(id, golds, bills, null, _SyncMoneyAgain);
    }

    void _SyncScoreAgain(bool serverError, string errorText, int addingScore)
    {
        Accounting.Instance.UpdateScore(id, addingScore, null, _SyncScoreAgain);
    }

    void _DisplayNameChangeFailed(bool serverError, string errorText)
    {
        DisplayName = DisplayName; // will sync it again
    }

    #endregion

    public delegate void generalDelegate();
    public event generalDelegate OnLevelChanged, OnTotalScoreChanged, OnGoldChanged, OnBillsChanged, OnEnergyChanged, OnMaxEnergyChanged, OnVIPPackagesChanged, OnDisplayNameChanged;


    public Statistics statistics;
    public List<VIPPackage> vipPackages;
    public List<Car> cars;
    public List<CosmeticItemData> cosmeticItems;
    public List<CarUpgrade> carUpgrades;
    

    bool autoSync = true;
    public bool AutoSync
    {
        get { return autoSync; }
        set 
        { 
            autoSync = value;
        }
    }


    public int Level
    {
        get { return Leveling.Instance.GetLevelForScore(TotalScore); }
    }



    #region Constructors etc

    public override string ToString()
    {
        string s = "";
        s += string.Format("({3})-{0} ({1} golds, {2} bills), Gas={4}", displayName, golds, bills, id, gas);
        s += Environment.NewLine;

        s += Environment.NewLine;
        s += "---VIP Packages---";
        s += Environment.NewLine;
        for (int i = 0; i < vipPackages.Count; i++)
        {
            s += string.Format("Package Type {0}. Remaining Time {1}", vipPackages[i].TypeID, vipPackages[i].TimeRemaining);
            s += Environment.NewLine;
        }

        s += Environment.NewLine;
        s += "---Cars---";
        s += Environment.NewLine;
        for (int i = 0; i < cars.Count; i++)
        {
            s += string.Format("{0} (level {1})", cars[i].CarTag, cars[i].Level);
            s += Environment.NewLine;
        }

        s += Environment.NewLine;
        s += "---Car Upgrades(" + carUpgrades.Count + ") ---";
        s += Environment.NewLine;
        for (int i = 0; i < carUpgrades.Count; i++)
        {
            s += string.Format("{0} Part {1}({3}), Level {2}", carUpgrades[i].CarTag, (UpgradeParts)carUpgrades[i].PartIndex, carUpgrades[i].Level, carUpgrades[i].PartIndex);
            //s += ", time=" + carUpgrades[i].TimeRemaining;

            if (carUpgrades[i].TimeRemaining > 0)
                s += " will activate in " + carUpgrades[i].TimeRemaining;

            s += Environment.NewLine;
        }

        s += Environment.NewLine;
        s += "---Cosmetics(" + cosmeticItems.Count + ") ---";
        s += Environment.NewLine;
        for (int i = 0; i < cosmeticItems.Count; i++)
        {
            s += string.Format("{0} Part {1}, Cosmetic {2} is available", cosmeticItems[i].CarTag, (SideStates)cosmeticItems[i].PartIndex, cosmeticItems[i].CosmeticIndex);
            s += Environment.NewLine;
        }


        return s;
    }

    DateTime loginTime;
    public WMUser()
    {
        loginTime = DateTime.Now;
    }

    public WMUser(int id, string name, int golds, int bills, int totalScore, int gas): base()
	{
        this.id = id;
        this.displayName = name;
        this.golds = golds;
        this.bills = bills;
        this.totalScore = totalScore;
        this.gas = gas;

        vipPackages = new List<VIPPackage>();
        cars = new List<Car>();
        cosmeticItems = new List<CosmeticItemData>();
        carUpgrades = new List<CarUpgrade>();

        loginTime = DateTime.Now;
	}

    public WMUser(string json)
    {
        Parse(json);

        vipPackages = new List<VIPPackage>();
        cars = new List<Car>();
        cosmeticItems = new List<CosmeticItemData>();
        carUpgrades = new List<CarUpgrade>();

        lastRefillTime = DateTime.Now;

        loginTime = DateTime.Now;
    }

    #endregion


    public int CalculateSecondsFromLogin()
    {
        TimeSpan span = DateTime.Now - loginTime;
        return (int)span.TotalSeconds;
    }


    int daysAfterLastLogin;
    public int DaysAfterLastLogin
    {
        get { return daysAfterLastLogin; }
    }

    int secondsAfterLastLogin;
    public int SecondsAfterLastLogin
    {
        get { return secondsAfterLastLogin; }
    }



    public override void SetItem(int index, string value)
    {
        switch (index)
        {
            case 0: id = int.Parse(value); break;
            case 1: userType = (UserLoginType)int.Parse(value); break;
            case 2: displayName = value; break;
            case 3: usernameChangesCount = int.Parse(value); break;
            case 4: bills= int.Parse(value); break;
            case 5: golds = int.Parse(value); break;
            case 6: totalScore = int.Parse(value); break;
            case 7: secondsAfterLastLogin = int.Parse(value); break;
            case 8: username = value; break;
			case 9: deviceID = value; break;
			case 10: leagueIndex = int.Parse(value);break;
			case 11: rankInLeague = int.Parse(value);break;
			case 12: scoreInLeague = int.Parse(value);break;
            case 13: daysAfterLastLogin = int.Parse(value); break;
            case 14: consecutiveDays = int.Parse(value); break;
            case 15: consecutiveDaysRewardClaimed = int.Parse(value); break;
            case 16: gas = int.Parse(value); break;
        }
    }

    public static WMUser[] ParseUsers(string text)
    {
        string[] entities = text.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);

        WMUser[] users = new WMUser[entities.Length];
        for (int i = 0; i < entities.Length; i++)
            users[i] = new WMUser(entities[i]);

        return users;
    }

    #region Initialize

    public void SetCosmetics(CosmeticItemData[] cosmetics)
    {
        if (cosmetics == null || cosmetics.Length == 0)
            return;

        for (int i = 0; i < cosmetics.Length; i++)
            this.cosmeticItems.Add(cosmetics[i]);
    }

    public void SetCars(Car[] cars)
    {
        if (cars == null || cars.Length == 0)
            return;

        for (int i = 0; i < cars.Length; i++)
            this.cars.Add(cars[i]);
    }

    public void SetVIPPackages(VIPPackage[] packages)
    {
        if (packages == null || packages.Length == 0)
            return;


        
        vipPackages.Clear();
        for (int i = 0; i < packages.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < vipPackages.Count; j++)
            {
                if (vipPackages[j].TypeID == packages[i].TypeID)
                {
                    vipPackages[j].TimeRemaining = Mathf.Max(vipPackages[j].TimeRemaining, packages[i].TimeRemaining);

                    found = true;
                    break;
                }
            }

            if (found)
                continue;

            vipPackages.Add(packages[i]);
        }
            


        if (OnVIPPackagesChanged != null)
            OnVIPPackagesChanged();
    }

    public void SetCarUpgrades(CarUpgrade[] upgrades)
    {
        if (upgrades == null || upgrades.Length == 0)
            return;

        for (int i = 0; i < upgrades.Length; i++)
            carUpgrades.Add(upgrades[i]);

        //carUpgradeTime = DateTime.Now;
    }

    public void SetStatistics(Statistics _statistics)
    {
        this.statistics = _statistics;
    }

    #endregion

    #region Update


    public void BuyCar(string carTag)
    {
        carTag = carTag.ToLower();


        bool foundCar = false;
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].CarTag.ToLower() == carTag)
            {
                foundCar = true;
                cars[i].Level = 1;
                break;
            }
                
        }

        if (!foundCar)
            cars.Add(new Car(carTag, 0));

        Accounting.Instance.BuyCar(id, carTag, 1, null, _BuyingCarFailed);
    }

    void _BuyingCarFailed(bool serverError, string errorText, string carTag)
    {
        Accounting.Instance.BuyCar(id, carTag, 1, null, _BuyingCarFailed);
    }

    public void BuyCosmetic(string carTag, int partIndex, int cosmeticIndex)
    {
        cosmeticItems.Add(new CosmeticItemData(carTag, partIndex, cosmeticIndex));
        Accounting.Instance.BuyCosmetics(Id, carTag, partIndex, cosmeticIndex, null, _BuyCosmeticFailed);
    }

    public int CalculateSkipUpgradeTimePrice(int x)
    {
        if (x <= 60)
            return 0;
        else if (x <= 3600)
            return ((20 - 1) / (3600 - 60)) * (x - 60) + 1;
        else if (x <= 86400)
            return ((260 - 20) / (86400 - 3600)) * (x - 3600) + 20;
        else
            return ((1000 - 260) / (604800 - 86400)) * (x - 86400) + 260;
    }

    public void BuyCarUpgrade(string carTag, int partIndex, int level, int time)
    {
        carTag = carTag.ToLower();


        bool found = false;
        for (int i = 0; i < carUpgrades.Count; i++)
        {
            if (carUpgrades[i].CarTag.ToLower() == carTag && carUpgrades[i].PartIndex == partIndex)
            {
                carUpgrades[i].Level = level;
                carUpgrades[i].TimeRemaining = time + CalculateSecondsFromLogin();

                found = true;
                break;
            }
        }

        if (!found)
            carUpgrades.Add(new CarUpgrade(carTag, partIndex, level, time + CalculateSecondsFromLogin(), time <= 0));

        if (time > 0)
            level = -level;

        Accounting.Instance.BuyCarUpgrades(Id, carTag, partIndex + 1, level, time, null, _BuyingCarUpgradeFailed);
    }

    public void SkipUpgrade()
    {
        CarUpgrade upgrade = UpgradingPart;
        Accounting.Instance.UpdateCarUpgradeTime(Id, upgrade.CarTag, upgrade.PartIndex + 1, 0, null, _SkipUpgradeFailed);

        upgrade.TimeRemaining = 0;
    }

    void _SkipUpgradeFailed(bool isServerError, string errorText, string carTag, int partIndex, int time)
    {
        Accounting.Instance.UpdateCarUpgradeTime(Id, carTag, partIndex, time, null, _SkipUpgradeFailed);
    }

    public void CarUpgradeDelivered(string carTag, int partIndex, int level)
    {
        carTag = carTag.ToLower();
        for (int i = 0; i < carUpgrades.Count; i++)
        {
            if (carUpgrades[i].CarTag.ToLower() == carTag && carUpgrades[i].PartIndex == partIndex && (carUpgrades[i].Level == level || carUpgrades[i].Level == -level))
                carUpgrades[i].IsDelivered = true;
        }

        Accounting.Instance.BuyCarUpgrades(Id, carTag, partIndex + 1, Mathf.Abs(level), 0, null, _BuyingCarUpgradeFailed);
    }


    private void _BuyCosmeticFailed(bool serverError, string errorText, string carTag, int partIndex, int cosmeticIndex)
    {
        Accounting.Instance.BuyCosmetics(Id, carTag, partIndex, cosmeticIndex, null, _BuyCosmeticFailed);
    }


    void _BuyingCarUpgradeFailed(bool serverError, string errorText, string carTag, int partIndex, int level, int time)
    {
        Accounting.Instance.BuyCarUpgrades(id, carTag, partIndex, level, time, null, _BuyingCarUpgradeFailed);
    }


    public void BuyVIPPackage(int packageID)
    {
        Accounting.Instance.BuyVIPPackages(id, packageID, _BuyVIPPackageDone, _BuyingVIPPackageFailed);
    }

    void _BuyVIPPackageDone(int packageID)
    {

        Accounting.Instance.GetVIPPackages(Id, VIPPackagesFetched, VIPPackagesDownloadFailed);

        if (OnVIPPackagesChanged != null)
            OnVIPPackagesChanged();

        for (int i = 0; i < VIPPackagesSettings.Instance.packages[packageID].actions.Length; i++)
		{
            if (VIPPackagesSettings.Instance.packages[packageID].actions[i].type == VIPActionType.GasSlotsOpen)
            {
                if (OnMaxEnergyChanged != null)
                    OnMaxEnergyChanged();

                break;
            }
		}
    }

    void VIPPackagesFetched(VIPPackage[] packages)
    {
        SetVIPPackages(packages);
    }

    void VIPPackagesDownloadFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GetVIPPackages(Id, VIPPackagesFetched, VIPPackagesDownloadFailed);
    }



    void _BuyingVIPPackageFailed(bool serverError, string errorText, int packageID)
    {
        Accounting.Instance.BuyVIPPackages(id, packageID, null, _BuyingVIPPackageFailed);
    }


    public void GasRefilled()
    {
        Accounting.Instance.GasRefilled(id, null, _GasRefillFailed);
        lastRefillTime = DateTime.Now;
        extraSecondsForGasRefill = 0;
    }

    void _GasRefillFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GasRefilled(id, null, _GasRefillFailed);
    }



    #endregion


    public void SyncStatistics()
    {
        Accounting.Instance.UpdateStatistics(id, statistics.Battles, statistics.TeamMatchesWin, statistics.TeamMatchesLose, statistics.SingleMatchesFirstPlaces, statistics.SingleMatchesOtherPlaces,
            statistics.RocketsSuccessfull, statistics.MinesSuccessfull, statistics.BulletsSuccessfull, statistics.RocketsTotal, statistics.MinesTotal, statistics.BulletsTotal, statistics.Deaths,
            statistics.Kills, statistics.PickUpMines, statistics.PickupRockets, statistics.PickupHealths, statistics.PickupShields, statistics.PickupAll, (int)statistics.DistanceCovered, _SyncStatsSuccess, _SyncStatisticsFailed);

        Debug.Log("sync statics at " + Time.time);
    }

    void _SyncStatsSuccess()
    {
        Debug.Log("sync statics successfully at " + Time.time);
    }

    void _SyncStatisticsFailed(bool serverError, string errorText)
    {
        SyncStatistics();
    }

    public void Buy(PriceStructure price, Action doneMethod)
    {
        if (Golds >= price.Golds && Bills >= price.Bills)
        {
            if (price.Golds > 0)
            {
                Golds -= price.Golds;
                //GA.API.Business.NewEvent("BuyWithGold", "Gold", price.Golds);
                //GA_Tracking.SendUserEvent();
                GameAnalytics.NewBusinessEvent("Gold",price.Golds,"1_Gold","Gold_ID","");

            }

            if (price.Bills > 0)
            {
                Bills -= price.Bills;
                //GA.API.Business.NewEvent("BuyWithBill", "Bills", price.Bills);
                //GA_Tracking.SendUserEvent();
                GameAnalytics.NewBusinessEvent("Bills", price.Bills, "1_Bills", "Bills_ID", "");
            }

            if (doneMethod != null)
                doneMethod();
        }
        else
        {
            if (price.Bills > 0)
                CommonUI.Instance.messageBox.Ask(Messages.NotEnoughBills, _BuyBills, null, false);
            else
                CommonUI.Instance.messageBox.Ask(Messages.NotEnoughGolds, _BuyGolds, null, false);
        }
    }


    void _BuyBills()
    {
        CommonUI.Instance.buyCoinsMenu.Activate(BuyCurrencySections.Bills);
    }

    void _BuyGolds()
    {
        CommonUI.Instance.buyCoinsMenu.Activate(BuyCurrencySections.Golds);
    }



    public void UpdateCarUpgradeTime(string carTag, int part, int time)
    {
        carTag = carTag.ToLower();
        for (int i = 0; i < carUpgrades.Count; i++)
        {
            if (carUpgrades[i].CarTag.ToLower() == carTag && carUpgrades[i].PartIndex == part)
                carUpgrades[i].TimeRemaining = 0;
        }

        Accounting.Instance.UpdateCarUpgradeTime(Id, carTag, part, time, null, _UpdateCarUpgradeTimeFailed);
    }

    private void _UpdateCarUpgradeTimeFailed(bool isServerError, string errorText, string carTag, int part, int time)
    {
        Accounting.Instance.UpdateCarUpgradeTime(Id, carTag, part, time, null, _UpdateCarUpgradeTimeFailed);
    }

    public CarUpgrade GetCarUpgrade(string carTag, int partIndex)
    {
        /*TimeSpan t = DateTime.Now - carUpgradeTime;
        int totalSeconds = (int)t.TotalSeconds;*/

        carTag = carTag.ToLower();
        for (int i = 0; i < carUpgrades.Count; i++)
        {
            if (carUpgrades[i].CarTag.ToLower() == carTag && carUpgrades[i].PartIndex == partIndex)
                return carUpgrades[i];
        }

        return null;
    }

    public int GetCarUpgrade(string carTag, int partIndex, out int timeRemaining)
    {
        carTag = carTag.ToLower();
        for (int i = 0; i < carUpgrades.Count; i++)
        {
            if (carUpgrades[i].CarTag.ToLower() == carTag && carUpgrades[i].PartIndex == partIndex)
            {
                timeRemaining = Mathf.RoundToInt(carUpgrades[i].TimeRemaining);
                return carUpgrades[i].Level;
            }
        }

        timeRemaining = 0;
        return 0;
    }

    public bool CanUpgrade
    {
        get
        {
            for (int i = 0; i < carUpgrades.Count; i++)
            {
                if (carUpgrades[i].TimeRemaining > 0)
                    return false;
            }

            return true;
        }
    }

    public CarUpgrade UpgradingPart
    {
        get
        {
            for (int i = 0; i < carUpgrades.Count; i++)
            {
                if (carUpgrades[i].TimeRemaining > 0)
                    return carUpgrades[i];
            }

            return null;
        }
    }

    public int UpgradingPartIndex
    {
        get
        {
            for (int i = 0; i < carUpgrades.Count; i++)
            {
                if (carUpgrades[i].TimeRemaining > 0)
                    return i;
            }

            return -1;
        }
    }

    public bool HasCar(string carTag)
    {
        carTag = carTag.ToLower();

        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].CarTag.ToLower() == carTag && cars[i].Level > -1)
                return true;
        }

        for (int i = 0; i < Information.Instance.carInfo.Length; i++)
        {
            if (Information.Instance.carInfo[i].carTag == carTag)
                return (Information.Instance.carInfo[i].levels[0].price.IsFree());
        }

        return false;
    }

    public int SelectedCarIndex
    {
        get
        {
            return PlayerData.GetInt("_carIndex"/*, 0*/);
        }
        set
        {
            PlayerData.SetInt("_carIndex", value);
        }
    }



    public void SetCarCosmeticState(CarType car, SideStates side, int index, bool unlocked)
    {
        /*string key = string.Format("{0}_{1}_{2}", (int)car, (int)side, index);
        PlayerPrefs.SetInt(key, unlocked ? 1 : 0);*/

        
        Accounting.Instance.currentUser.BuyCosmetic(Information.Instance.carInfo[(int)car].carTag, (int)side, index);
    }

    public bool GetCarCosmeticState(CarType car, SideStates side, int index)
    {
        //Debug.Log("*** car=" + car + ", side=" + side + ", cosmetic index=" + index);

        string tag = Information.Instance.carInfo[(int)car].carTag.ToLower();
        for (int i = 0; i < Accounting.Instance.currentUser.cosmeticItems.Count; i++)
        {
            //Debug.Log("-------- Checking with cartag=" + Accounting.Instance.currentUser.cosmeticItems[i].CarTag + ", part=" + ((SideStates)Accounting.Instance.currentUser.cosmeticItems[i].PartIndex) + ", cosmetic=" + Accounting.Instance.currentUser.cosmeticItems[i].CosmeticIndex);

            if (Accounting.Instance.currentUser.cosmeticItems[i].CarTag.ToLower() == tag && Accounting.Instance.currentUser.cosmeticItems[i].PartIndex == (int)side
                && Accounting.Instance.currentUser.cosmeticItems[i].CosmeticIndex == index)
            {
                //Debug.Log("------ matched ------");
                return true;
            }
                
        }

        return false;
    }

    public void SetSelectedCosmetic(CarType car, SideStates side, int index)
    {
        string key = string.Format("selected_{0}_{1}", (int)car, (int)side);
        //Debug.Log("set " + side + " of " + car + "=" + index);
        PlayerPrefs.SetInt(key, index);
    }

    public int GetSelectedCosmetic(CarType car, SideStates side)
    {
        string key = string.Format("selected_{0}_{1}", (int)car, (int)side);
        //Debug.Log("get " + side + " of " + car  + "=" + PlayerPrefs.GetInt(key, 0));
        return PlayerPrefs.GetInt(key, 0);
    }

    public bool HasVIPPackage(int typeID)
    {
        for (int i = 0; i < vipPackages.Count; i++)
        {
            if (vipPackages[i].TypeID == typeID && vipPackages[i].TimeRemaining > 0)
                return true;
        }

        return false;
    }

    public bool HasVIPPackage(VIPActionType action)
    {
        for (int i = 0; i < vipPackages.Count; i++)
        {
            for (int j = 0; j < VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions.Length; j++)
            {
                if (VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions[j].type == action)
                    return true;
            }
        }

        return false;
    }

    public float GetVIPActionMultiplyer(VIPActionType action)
    {        
        for (int i = 0; i < vipPackages.Count; i++)
        {
            for (int j = 0; j < VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions.Length; j++)
            {
                if (VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions[j].type == action)
                    return VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions[j].multiplyer;
            }
        }

        return 1;
    }

    public bool HasAnyUnClaimedGifts
    {
        get
        {
            return consecutiveDays > consecutiveDaysRewardClaimed;
        }
    }

    public float GasRegenerateTimeK
    {
        get
        {
            for (int i = 0; i < vipPackages.Count; i++)
            {
                foreach (VIPAction action in VIPPackagesSettings.Instance.packages[vipPackages[i].TypeID].actions)
                {
                    if (action.type == VIPActionType.GasRefillTime)
                        return action.multiplyer;
                }
            }

            return 1;
        }
    }

    public void CheckForRefillGas()
    {
        float gasRefeelTimeMultiplyer = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.GasRefillTime);
        int addingGas = Accounting.Instance.currentUser.LastRefillTimeInSeconds / (int)(GeneralSettings.Instance.GasRegenerateTime * gasRefeelTimeMultiplyer);

        //Debug.Log("******** last refill=" + Accounting.Instance.currentUser.LastRefillTimeInSeconds + ", reg time=" + +(int)(GeneralSettings.Instance.GasRegenerateTime * gasRefeelTimeMultiplyer));
        //Debug.Log("adding gas=" + addingGas);

        if (addingGas > 0)
        {
            Accounting.Instance.currentUser.GasRefilled();
            Accounting.Instance.currentUser.Gas += addingGas;
            
        }
    }
}


public class VIPPackage : ParsableClass
{
    int id;
    int typeID;
    int timeRemaining;

    public int TimeRemaining
    {
        get { return timeRemaining; }
        set
        {
            timeRemaining = value;
        }
    }


    public int Id
    {
        get { return id; }
    }

    public int TypeID
    {
        get { return typeID; }
    }



    public VIPPackage(string json)
    {
        Parse(json);
    }

    public override void SetItem(int index, string value)
    {
        switch (index)
        {
            case 0: id = int.Parse(value); break;
            case 1: typeID = int.Parse(value); break;
            case 2: timeRemaining = int.Parse(value); break;
        }
    }

    public static VIPPackage[] ParseVIPPackages(string text)
    {
        string[] entities = text.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);

        VIPPackage[] packages = new VIPPackage[entities.Length];
        for (int i = 0; i < entities.Length; i++)
        {
            packages[i] = new VIPPackage(entities[i]);
            //Debug.Log("package " + i + ", id=" + packages[i].id + ", type=" + packages[i].typeID + ", time=" + packages[i].timeRemaining);

            if (packages[i].timeRemaining <= 0)
                Accounting.Instance.ReportExpirePackageID(packages[i].id);
        }
            

        return packages;
    }
}

public class LeagueId
{
//	string myLeagueID;
//	public string MyLeagueID{
//		get{ return myLeagueID; }
//		set{ myLeagueID = value; }
//	}
//
//	public static LeagueId[] ParseLeagueID(string text)
//	{
//		string[] entities = text.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);
//
//		LeagueId[] ids = new LeagueId[0];
//		for (int i = 0; i < entities.Length; i++)
//			ids [i] = new LeagueId ();
//		
//		return ids;
//	}
}

public class Car : ParsableClass
{
    string carTag;
    int level;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public string CarTag
    {
        get { return carTag; }
    }


    public Car(string carTag, int level)
    {
        this.carTag = carTag;
        this.level = level;
    }

    public Car(string json)
    {
        Parse(json);
    }

    public override void SetItem(int index, string value)
    {
        switch (index)
        {
            case 0: carTag = value; break;
            case 1: level = int.Parse(value); break;
        }
    }

    public static Car[] ParseCars(string text)
    {
        string[] entities = text.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);

        Car[] cars = new Car[entities.Length];
        for (int i = 0; i < entities.Length; i++)
            cars[i] = new Car(entities[i]);

        return cars;
    }
}

//public class LeagueDatas : ParsableClass{
//	string userName;
//	int globalScor;
//	int leagueScore;
//	int leagueRank;
//
//	public string UserName{
//		get{ return userName; }
//		set{ userName = value; }
//	}
//	public int GlobalScore{
//		get{ return globalScor; }
//		set{ globalScor = value; }
//	}
//	public int LeagueScore{
//		get{ return leagueScore; }
//		set{ leagueScore = value; }
//	}
//	public int LeagueRank{
//		get{ return leagueRank; }
//		set{ leagueRank = value; }
//	}
//
//	public LeagueDatas( string json ){
//		Parse ( json );
//	}
//
//	public LeagueDatas( string uName, int scor, int leagScor, int learank ){
//		this.userName = uName;
//		this.globalScor = scor;
//		this.leagueScore = leagScor;
//		this.leagueRank = learank;
//	}
//
//	public override void SetItem(int index, string value)
//	{
//		switch (index){
//		case 0: UserName = value; break;
//		case 1: GlobalScore = int.Parse(value); break;
//		case 2: leagueScore = int.Parse(value); break;
//		case 3: LeagueRank = int.Parse(value); break;
//			//case 3: partIndex = int.Parse(value); Debug.Log("part=" + partIndex); break;
//		}
//	}
//
//	public static LeagueDatas[] parsLeagueDatas(string text)
//	{
//		string[] entities = text.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);
//		
//		LeagueDatas[] onlineDatas = new LeagueDatas[entities.Length];
//		for (int i = 0; i < entities.Length; i++)
//			onlineDatas [i] = new LeagueDatas ( entities[i] );
//		
//		return onlineDatas;
//	}
//}

public class CosmeticItemData : ParsableClass
{
    string carTag;
    int carIndex;
    int partIndex;
    int cosmeticIndex;


    public int CosmeticIndex
    {
        get { return cosmeticIndex; }
        set { cosmeticIndex = value; }
    }

    public int PartIndex
    {
        get { return partIndex; }
        set { partIndex = value; }
    }

    public int CarIndex
    {
        get { return carIndex; }
        set { carIndex = value; }
    }

    public string CarTag
    {
        get { return carTag; }
        set { carTag = value; }
    }



    public CosmeticItemData(string carTag, int partIndex, int cosmeticIndex)
    {
        this.carTag = carTag;
        this.partIndex = partIndex;
        this.cosmeticIndex = cosmeticIndex;

    }

    public CosmeticItemData(string json)
    {
        Parse(json);
    }


    public override void SetItem(int index, string value)
    {
        
        switch (index)
        {
            case 0: carTag = value; break;
            case 1: partIndex = int.Parse(value); break;
            case 2: cosmeticIndex = int.Parse(value); break;
            //case 3: partIndex = int.Parse(value); Debug.Log("part=" + partIndex); break;
        }
    }

    public static CosmeticItemData[] ParseCosmeticItems(string json)
    {
        if (json.IndexOf("No_Item") != -1)
            return null;

        string[] entities = json.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);

        CosmeticItemData[] items = new CosmeticItemData[entities.Length];
        for (int i = 0; i < entities.Length; i++)
            items[i] = new CosmeticItemData(entities[i]);

        return items;
    }
}

public class CarUpgrade : ParsableClass
{
    int id;
    string carTag;
    int carIndex;
    int partIndex;
    int level;
    float timeRemaining;
    bool isDelivered;


    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public float TimeRemaining
    {
        get
        {
            return timeRemaining - Accounting.Instance.currentUser.CalculateSecondsFromLogin();
        }
        set { timeRemaining = value; }
    }

    public int PartIndex
    {
        get { return partIndex; }
        set { partIndex = value; }
    }

    public string CarTag
    {
        get { return carTag; }
        set { carTag = value; }
    }

    public bool IsDelivered
    {
        get { return isDelivered; }
        set { isDelivered = value; }
    }


    public CarUpgrade()
    {

    }

    public CarUpgrade(string json)
    {
        Parse(json);
    }

    public CarUpgrade(string cartag, int partIndex, int level, int timeRemaining, bool isDelivered)
    {
        this.carTag = cartag;
        this.partIndex = partIndex;
        this.isDelivered = isDelivered;
        this.level = level;
        this.timeRemaining = timeRemaining;
    }

    public override void SetItem(int index, string value)
    {
        switch (index)
        {
            //case 0: id = int.Parse(value); break;
            case 0: carTag = value; break;
            //case 2: carIndex = int.Parse(value); break;
            case 1: partIndex = int.Parse(value); break;
            case 2: level = int.Parse(value); break;
        }

        Application.LoadLevel(0);
    }

    public static CarUpgrade[] ParseCarUpgrades(string json)
    {
        string[] entities = json.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);

        Debug.Log("car upgrades=" + json);
        
        CarUpgrade[] upgrades = new CarUpgrade[entities.Length * 7];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = entities[i].Substring(0, entities[i].Length - 1);


            string[] properties = entities[i].Split(',');
            string carTag = properties[0];

            int partIndex = 1;
            for (int j = 1; j < properties.Length; j+=2)
            {
                int index = i * 7 + partIndex - 1;

                upgrades[index] = new CarUpgrade();
                upgrades[index].carTag = carTag;
                upgrades[index].partIndex = partIndex - 1;
                upgrades[index].level = int.Parse(properties[j]);
                if (upgrades[index].level < 0)
                {
                    upgrades[index].isDelivered = false;
                    upgrades[index].level = -upgrades[index].level;
                }
                else
                    upgrades[index].isDelivered = true;

                upgrades[index].timeRemaining = int.Parse(properties[j + 1]);

                partIndex++;
            }
        }
            

        return upgrades;
    }
}



public abstract class ParsableClass
{
    public abstract void SetItem(int index, string value);
   
    public void Parse(string json)
    {
        string[] entities = json.Split(new char[] {'['}, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].EndsWith("]"))
            {
                entities[i] = entities[i].Trim().Substring(0, entities[i].Length - 1);

                string[] values = entities[i].Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    if (values[j].StartsWith("["))
                        values[j] = values[j].Substring(1);

                    if (values[j].EndsWith("]"))
                        values[j] = values[j].Substring(0, values[j].Length - 1);

                    SetItem(j, values[j]);
                }
            }
        }
    }
}

[System.Serializable]
public class PriceStructure
{
    public int Golds, Bills;

    public bool IsFree()
    {
        return (Golds == 0 && Bills == 0);
    }

    public static PriceStructure operator +(PriceStructure c1, PriceStructure c2)
    {
        PriceStructure s  = new PriceStructure();
        s.Bills = c1.Bills + c2.Bills;
        s.Golds = c1.Golds + c2.Golds;

        return s;
    }

    public static PriceStructure operator *(PriceStructure c1, float f)
    {
        PriceStructure s = new PriceStructure();
        s.Bills = Mathf.RoundToInt(c1.Bills * f);
        s.Golds = Mathf.RoundToInt(c1.Golds * f);

        return s;
    }

    public override string ToString()
    {
        return "{Gold: " + Golds + ", Bills:" + Bills + "}";
    }
}

[System.Serializable]
public class Statistics : ParsableClass
{
    int battles, teamMatchesWin, teamMatchesLose, singleMatchesFirstPlace, singleMatchesOtherPlaces;
    int rocketsSuccessfull, minesSuccessfull, bulletsSuccessfull, rocketsTotal, minesTotal, bulletsTotal;
    int deaths, kills;
    int minesPickedUp, rocketsPickedUp, healthsPickedUp, shieldsPickedUp, allPickedUp;
    float distanceCovered;





    int rocketsSuccessfullDaily, minesSuccessfullDaily, bulletsSuccessfullDaily, rocketsTotalDaily, minesTotalDaily, bulletsTotalDaily;
    int deathsDaily, killsDaily;
    int minesPickedUpDaily, rocketsPickedUpDaily, healthsPickedUpDaily, shieldsPickedUpDaily, allPickedUpDaily;
    float distanceCoveredDaily;





    public static Statistics[] ParseStatistics(string text)
    {
        if (text.ToLower().IndexOf(Accounting.NO_ITEM_STRING) != -1)
            return new Statistics[] { Statistics.EmptyInstance() };
        

        string[] entities = text.Split(new char[] { '[' }, StringSplitOptions.RemoveEmptyEntries);

        Statistics[] stats = new Statistics[entities.Length];
        for (int i = 0; i < entities.Length; i++)
            stats[i] = new Statistics(entities[i]);


        return stats;
    }

    private static Statistics EmptyInstance()
    {
        return new Statistics();
    }




    #region Properties


    public int SingleMatchesOtherPlaces
    {
        get { return singleMatchesOtherPlaces; }
        set { singleMatchesOtherPlaces = value; }
    }

    public int SingleMatchesFirstPlaces
    {
        get { return singleMatchesFirstPlace; }
        set { singleMatchesFirstPlace = value; }
    }

    public int TeamMatchesLose
    {
        get { return teamMatchesLose; }
        set { teamMatchesLose = value; }
    }

    public int TeamMatchesWin
    {
        get { return teamMatchesWin; }
        set { teamMatchesWin = value; }
    }

    public int Battles
    {
        get { return battles; }
        set { battles = value; }
    }

    public int Kills
    {
        get { return kills; }
        set { kills = value; }
    }

    public int Deaths
    {
        get { return deaths; }
        set { deaths = value; }
    }

    public float DistanceCovered
    {
        get { return distanceCovered; }
        set { distanceCovered = value; }
    }

    public int PickupAll
    {
        get { return allPickedUp; }
        set { allPickedUp = value; }
    }

    public int PickupShields
    {
        get { return shieldsPickedUp; }
        set { shieldsPickedUp = value; }
    }

    public int PickupHealths
    {
        get { return healthsPickedUp; }
        set { healthsPickedUp = value; }
    }

    public int PickupRockets
    {
        get { return rocketsPickedUp; }
        set { rocketsPickedUp = value; }
    }

    public int PickUpMines
    {
        get { return minesPickedUp; }
        set { minesPickedUp = value; }
    }

    public int BulletsTotal
    {
        get { return bulletsTotal; }
        set { bulletsTotal = value; }
    }

    public int MinesTotal
    {
        get { return minesTotal; }
        set { minesTotal = value; }
    }

    public int RocketsTotal
    {
        get { return rocketsTotal; }
        set { rocketsTotal = value; }
    }

    public int BulletsSuccessfull
    {
        get { return bulletsSuccessfull; }
        set { bulletsSuccessfull = value; }
    }

    public int MinesSuccessfull
    {
        get { return minesSuccessfull; }
        set { minesSuccessfull = value; }
    }

    public int RocketsSuccessfull
    {
        get { return rocketsSuccessfull; }
        set { rocketsSuccessfull = value; }
    }


    /// Daily

    public int KillsDaily
    {
        get { return killsDaily; }
        set { killsDaily = value; }
    }

    public int DeathsDaily
    {
        get { return deathsDaily; }
        set { deathsDaily = value; }
    }

    public float DistanceCoveredDaily
    {
        get { return distanceCoveredDaily; }
        set { distanceCoveredDaily = value; }
    }

    public int PickupAllDaily
    {
        get { return allPickedUpDaily; }
        set { allPickedUpDaily = value; }
    }

    public int PickupShieldsDaily
    {
        get { return shieldsPickedUpDaily; }
        set { shieldsPickedUpDaily = value; }
    }

    public int PickupHealthsDaily
    {
        get { return healthsPickedUpDaily; }
        set { healthsPickedUpDaily = value; }
    }

    public int PickupRocketsDaily
    {
        get { return rocketsPickedUpDaily; }
        set { rocketsPickedUpDaily = value; }
    }

    public int PickUpMinesDaily
    {
        get { return minesPickedUpDaily; }
        set { minesPickedUpDaily = value; }
    }

    public int TotalBulletsDaily
    {
        get { return bulletsTotalDaily; }
        set { bulletsTotalDaily = value; }
    }

    public int TotalMinesDaily
    {
        get { return minesTotalDaily; }
        set { minesTotalDaily = value; }
    }

    public int TotalRocketsDaily
    {
        get { return rocketsTotalDaily; }
        set { rocketsTotalDaily = value; }
    }

    public int BulletsSuccessfullDaily
    {
        get { return bulletsSuccessfullDaily; }
        set { bulletsSuccessfullDaily = value; }
    }

    public int MinesSuccessfullDaily
    {
        get { return minesSuccessfullDaily; }
        set { minesSuccessfullDaily = value; }
    }

    public int RocketsSuccessfullDaily
    {
        get { return rocketsSuccessfullDaily; }
        set { rocketsSuccessfullDaily = value; }
    }


    #endregion




    private Statistics()
	{

	}

    public Statistics(string text)
    {
        Parse(text);
    }

    public override void SetItem(int index, string value)
    {
        switch (index)
        {
            case 0: battles = int.Parse(value); break;
            case 1: teamMatchesWin = int.Parse(value); break;
            case 2: teamMatchesLose = int.Parse(value); break;
            case 3: singleMatchesFirstPlace = int.Parse(value); break;
            case 4: singleMatchesOtherPlaces = int.Parse(value); break;
            case 5: rocketsSuccessfull = int.Parse(value); break;
            case 6: minesSuccessfull = int.Parse(value); break;
            case 7: bulletsSuccessfull = int.Parse(value); break;
            case 8: rocketsTotal = int.Parse(value); break;
            case 9: minesTotal = int.Parse(value); break;
            case 10: bulletsTotal = int.Parse(value); break;
            case 11: deaths = int.Parse(value); break;
            case 12: kills = int.Parse(value); break;
            case 13: minesPickedUp = int.Parse(value); break;
            case 14: rocketsPickedUp = int.Parse(value); break;
            case 15: healthsPickedUp = int.Parse(value); break;
            case 16: shieldsPickedUp = int.Parse(value); break;
            case 17: allPickedUp = int.Parse(value); break;
            case 18: distanceCovered = int.Parse(value); break;
        }
    }


    
    public void AddSingleMatchFirstPlace(int amount = 1)
    {
        SingleMatchesFirstPlaces += amount;
    }

    public void AddSingleMatchOtherPlaces(int amount = 1)
    {
        SingleMatchesOtherPlaces += amount;
    }

    public void AddTeamMatchWin(int amount = 1)
    {
        TeamMatchesWin += amount;
    }

    public void AddTeamMatchLose(int amount = 1)
    {
        teamMatchesLose += amount;
    }



    public void AddBattle(int amount = 1)
    {
        Battles += amount;
    }

    public void AddSuccessfullRockets(int amount = 1)
    {
        RocketsSuccessfull += amount;
        RocketsSuccessfullDaily += amount;
    }

    public void AddSuccessfullMines(int amount = 1)
    {
        MinesSuccessfull += amount;
        MinesSuccessfullDaily += amount;
    }

    public void AddSuccessfullBullets(int amount = 1)
    {
        BulletsSuccessfull += amount;
        BulletsSuccessfullDaily += amount;
    }

    public void AddRockets(int amount = 1)
    {
        RocketsTotal+= amount;
        TotalRocketsDaily += amount;

        Debug.Log(amount + "rocket fired, total=" + RocketsTotal);
    }

    public void AddMines(int amount = 1)
    {
        MinesTotal += amount;
        TotalMinesDaily += amount;
    }

    public void AddBullets(int amount = 1)
    {
        BulletsTotal += amount;
        TotalBulletsDaily += amount;
    }

    public void AddKill(int amount = 1)
    {
        Kills += amount;
        KillsDaily += amount;
    }

    public void AddDeath(int amount = 1)
    {
        Deaths += amount;
        DeathsDaily += amount;
    }

    public void PickUpCollected(CollectibleType item, int amount = 1)
    {
        switch (item)
        {
            case CollectibleType.RocketLauncher:
                PickupRockets += amount;
                PickupRocketsDaily += amount;
                break;

            case CollectibleType.Miner:
                PickUpMines += amount;
                PickUpMinesDaily += amount;
                break;

            case CollectibleType.Health:
                PickupHealths += amount;
                PickupHealthsDaily += amount;
                break;

            case CollectibleType.Armor:
                PickupShields += amount;
                PickupShieldsDaily += amount;
                break;

            case CollectibleType.All:
                PickupAll += amount;
                PickupAllDaily += amount;
                break;
        }
    }

    public void CoveredDistance(float distance)
    {
        DistanceCovered += distance;
        DistanceCoveredDaily += distance;
    }

}