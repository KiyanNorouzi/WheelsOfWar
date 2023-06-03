using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GameAnalyticsSDK;

public class LoginSceneController : MonoBehaviour
{
    #region Singleton

    static LoginSceneController _instance;

    public static LoginSceneController Instance
    {
        get { return _instance; }
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



    public Text text;

    public Slider loadingSlider;
    public Animator blackScreenAnimator;

    


    /*public void LoadingSliderAddition(float _value, bool _add)
    {
        if (_add)
        {
            loadingSlider.value += _value;
            sliderTemp += _value;
        }
        else
        {
            loadingSlider.value = sliderTemp;
        }
    }*/



    void Log(string t)
    {
        text.text += t + System.Environment.NewLine;
    }
	
    string GenerateNewUsername(string firstWord = "Guest")
    {
        if (string.IsNullOrEmpty(firstWord))
            return string.Concat("Guest" + Random.Range(1000000, 9999999));
        else
            return string.Concat(firstWord + Random.Range(1000000, 9999999));
    }


    string lastGooglePlayIDSignedIn;
    UserLoginType lastUserType;
    void Start()
    {
        Debug.Log("Game Start");

        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("You've succselfully to Login");
            }
            else
            {
                Debug.Log("Login failed for some reason!");
            }
        }
        );


       


		lastGooglePlayIDSignedIn = PlayerPrefs.GetString("lastUser");
		lastUserType = (UserLoginType)PlayerPrefs.GetInt("lastUserType", 0);

		if( !IsSignedInGooglePlay() && lastUserType == UserLoginType.GooglePlay)
		{
			lastGooglePlayIDSignedIn = string.Empty;
			lastUserType = UserLoginType.Guest;
		}

        loadingSlider.value = 0;

        text.text = "";

        Log("Welcome to the Wheels of War");
        Log("");

        if (!IsSignedInGooglePlay() && string.IsNullOrEmpty(lastGooglePlayIDSignedIn)) // new user
        {
            Debug.Log("not signed in, new user");
            _RegisterNewGeneratedUser();
        }
        else if (!IsSignedInGooglePlay() && !string.IsNullOrEmpty(lastGooglePlayIDSignedIn)) // user has previous records on this device
        {
            Debug.Log("not signed in, last user=" + lastGooglePlayIDSignedIn);

            Accounting.Instance.Login(GetDeviceID(), lastGooglePlayIDSignedIn, lastUserType /* UserLoginType.Guest*/, _LoginSuccessfull, _LoginFailed);

            Log("Previous record found.");
            Log("Logging in as '" + lastGooglePlayIDSignedIn + "'...");
        }
        else if (IsSignedInGooglePlay())
        {
            Debug.Log("not signed in, last user=" + lastGooglePlayIDSignedIn);

            Accounting.Instance.Login(GetDeviceID(), GetGooglePlayID(), UserLoginType.GooglePlay, _LoginSuccessfull, _LoginFailed);

            Log("You are logged in your google play account.");
            Log("Logging in as '" + GetGooglePlayID() + "'...");
        }
    }


    void _RegisterNewGeneratedUser()
    {
        string newGeneratedUsername = GenerateNewUsername("Guest");
        Accounting.Instance.Register(GetDeviceID(), newGeneratedUsername, newGeneratedUsername, UserLoginType.Guest, _RegisterSuccessfull, _RegisterFailed);

        Log("Registering you as '" + newGeneratedUsername + "' on the network...");
    }


    private void _RegisterSuccessfull(string deviceID, string username, UserLoginType type)
    {
        //Game analatycs design event
        //GA.API.Design.NewEvent("Register is successfull");

        GameAnalytics.NewDesignEvent("Register is successfull");

        Log("Registering successfull.");
        Log("Logging you in as '" + username + "'...");

        Accounting.Instance.Login(deviceID, username, type, _LoginSuccessfull, _LoginFailed);

	}

    private void _RegisterFailed(bool serverError, string errorText, string deviceID, string username)
    {
        //Game analatycs design event
        //GA.API.Design.NewEvent("Register is failed");

        GameAnalytics.NewDesignEvent("Register is failed");

        if (errorText.IndexOf("duplicate") != -1)
        {
            if (IsSignedInGooglePlay())
            {
                Log("You have been found on our system.");
                Log("Logging in as " + username);
                Accounting.Instance.Login(deviceID, username, UserLoginType.GooglePlay, _LoginSuccessfull, _LoginFailed);
            }
            else
            {
                Log("Duplicate Username");
                _RegisterNewGeneratedUser();
            }
        }
        else
        {
            Log("Registeration failed.");
            _RegisterNewGeneratedUser();
        }
    }

    private void _LoginSuccessfull(WMUser[] matchedUsers)
    {
        if (matchedUsers == null || matchedUsers.Length == 0)
        {
            Log("No User found matching your credentials.");
            _RegisterNewGeneratedUser();
        }
        else
        {
            Log("Login was Successfull.");

            Accounting.Instance.currentUser = matchedUsers[0];
            if (Accounting.Instance.currentUser.DaysAfterLastLogin == 1)
                Accounting.Instance.currentUser.ConsecutiveDays++;
            else if (Accounting.Instance.currentUser.DaysAfterLastLogin > 1)
            {
                Accounting.Instance.currentUser.AutoSync = false;
                Accounting.Instance.currentUser.ConsecutiveDays = 0;
                Accounting.Instance.currentUser.AutoSync = true;
                Accounting.Instance.currentUser.ConsecutiveDaysRewardClaimed = 0;
            }

			PlayerPrefs.SetString("lastUser", Accounting.Instance.currentUser.Username);
			PlayerPrefs.SetInt("lastUserType", (int)Accounting.Instance.currentUser.UserType);

            DoTheRestOfLogin();
        }
    }

    private void _LoginFailed(bool serverError, string errorText, string tryedUsername)
    {
        if (errorText.ToLower().IndexOf(Accounting.NO_ITEM_STRING) != -1)
        {
            Log("Login failed.");
            Log("You are not in our system");


            string randomDisplayName = GenerateNewUsername("User");
            Accounting.Instance.Register(GetDeviceID(), tryedUsername, randomDisplayName, UserLoginType.GooglePlay, _RegisterSuccessfull, _RegisterFailed);

            Log("Registering new account for you as '" + randomDisplayName + "'");
        }
        else
        {
            Log("Login failed.");
            Log("Server Error:" + serverError.ToString());
            Log(errorText);
        }
    }




    bool[] operationDone;
    void DoTheRestOfLogin()
    {
        WMUser currentUser = Accounting.Instance.currentUser;

        operationDone = new bool[6];
        loadingSlider.maxValue = operationDone.Length;

        Accounting.Instance.GetCosmetics(currentUser.Id, CosmeticsFetched, CosmeticDownloadFailed);
        Accounting.Instance.GetVIPPackages(currentUser.Id, VIPPackagesFetched, VIPPackagesDownloadFailed);
        Accounting.Instance.GetUserCarUpgrades(currentUser.Id, CarUpgradesFetched, CarUpgradesDownloadFailed);
        Accounting.Instance.GetUserCars(currentUser.Id, CarsFetched, CarsDownloadFailed);
        Accounting.Instance.GetStatistics(currentUser.Id, StatisticsFetched, StatisticsDownloadFailed);
        Accounting.Instance.GetServerTime(TimeFetched, TimeDownloadFailed);
    }




    void OperationDone(int index)
    {
        operationDone[index] = true;

        int successfullOperations = 0;
        for (int i = 0; i < operationDone.Length; i++)
        {
            if (operationDone[i])
                successfullOperations++;
            /*
            if (!operationDone[i])
                return;*/
        }

        loadingSlider.value = successfullOperations;
        if (successfullOperations == operationDone.Length)
        {
            // Post Login Things

            /*int timeToRegenerate = (int)(GeneralSettings.Instance.GasRegenerateTime + (GeneralSettings.Instance.GasRegenerateTime * Accounting.Instance.currentUser.GasRegenerateTimeK));
            int addingGasUnits = Accounting.Instance.currentUser.SecondsAfterLastLogin / timeToRegenerate;
            Accounting.Instance.currentUser.Gas += addingGasUnits;*/


            StartCoroutine(WriteUserInfoAndGoToGame());
        }
    }

    IEnumerator WriteUserInfoAndGoToGame()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log("User Info=" + Accounting.Instance.currentUser.ToString());


        float timeK = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.GasRefillTime);
        float time = GeneralSettings.Instance.GasRegenerateTime * timeK; // - Accounting.Instance.currentUser.LastRefillTimeInSeconds;
        
        int addingGasSlots = Mathf.FloorToInt(Accounting.Instance.currentUser.SecondsAfterLastLogin / time);
        Accounting.Instance.currentUser.ExtraSecondsForGasRefill = Accounting.Instance.currentUser.SecondsAfterLastLogin % Mathf.CeilToInt(time);
        if (addingGasSlots > 0)
            Accounting.Instance.currentUser.Gas += addingGasSlots;

        

        for (int i = 0; i < Accounting.Instance.currentUser.cars.Count; i++)
		{
            if (Accounting.Instance.currentUser.cars[i].CarTag.ToLower() == "cheetah" && Accounting.Instance.currentUser.cars[i].Level == -1)
                Accounting.Instance.currentUser.BuyCar("cheetah");
		}
        

        Log("Entering the Game...");
        blackScreenAnimator.SetTrigger("on");
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(Scenes.Loading);
    }


    private void TimeFetched(int dayIndex, int hour, int minute)
    {
        ServerTime.Instance.SetTime(dayIndex, hour, minute);
        OperationDone(5);
        Log("Data and time fetched. " + GetRemainingText());
    }

    private void TimeDownloadFailed(bool arg1, string arg2)
    {
        Accounting.Instance.GetServerTime(TimeFetched, TimeDownloadFailed);

        Log("Statistics download failed. Retrying...");
    }

    private void StatisticsFetched(Statistics[] statistics)
    {
        Accounting.Instance.currentUser.SetStatistics(statistics[0]);
        OperationDone(4);
        Log("Statitics fetched. " + GetRemainingText());
    }

    private void StatisticsDownloadFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GetStatistics(Accounting.Instance.currentUser.Id, StatisticsFetched, StatisticsDownloadFailed);
        Log("Statistics download failed. Retrying...");
    }

    void CosmeticsFetched(CosmeticItemData[] cosmetics)
    {
        Accounting.Instance.currentUser.SetCosmetics(cosmetics);
        OperationDone(0);
        Log("Cosmetics fetched. " + GetRemainingText());
    }

    void CosmeticDownloadFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GetCosmetics(Accounting.Instance.currentUser.Id, CosmeticsFetched, CosmeticDownloadFailed);
        Log("Cosmetic download failed. Retrying...");
    }

    void VIPPackagesFetched(VIPPackage[] packages)
    {
        Accounting.Instance.currentUser.SetVIPPackages(packages);
        OperationDone(1);
        Log("VIP packages fetched. " + GetRemainingText());
    }

    void VIPPackagesDownloadFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GetVIPPackages(Accounting.Instance.currentUser.Id, VIPPackagesFetched, VIPPackagesDownloadFailed);
        Log("VIP packages download failed. Retrying...");
    }

    void CarUpgradesFetched(CarUpgrade[] upgrades)
    {
        Accounting.Instance.currentUser.SetCarUpgrades(upgrades);
        OperationDone(2);
        Log("Upgrades fetched. " + GetRemainingText());
    }

    void CarUpgradesDownloadFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GetUserCarUpgrades(Accounting.Instance.currentUser.Id, CarUpgradesFetched, CarUpgradesDownloadFailed);
        Log("upgrades download failed. Retrying...");
    }

    void CarsFetched(Car[] cars)
    {
        Accounting.Instance.currentUser.SetCars(cars);
        OperationDone(3);
        Log("Cars fetched. " + GetRemainingText());
    }

    void CarsDownloadFailed(bool serverError, string errorText)
    {
        Accounting.Instance.GetUserCars(Accounting.Instance.currentUser.Id, CarsFetched, CarsDownloadFailed);
        Log("Cars download failed. Retrying...");
    }

    string GetRemainingText()
    {
        int dones = 0;
        for (int i = 0; i < operationDone.Length; i++)
        {
            if (operationDone[i])
                dones++;
        }

        return string.Format("{0}/{1}", dones, operationDone.Length);
    }







    #region Simulation

    public bool isSignedInToGooglePlay;
    public string googlePlayID;

#if !UNITY_ANDROID

    public bool IsSignedInGooglePlay()
    {
        return isSignedInToGooglePlay;
    }

    public string GetGooglePlayID()
    {
        return googlePlayID;
    }

    public string GetLastGooglePlayIDSignedIn()
    {
        return lastGooglePlayIDSignedIn;
    }

#else

    public bool IsSignedInGooglePlay()
    {
        //return Social.Active.localUser.authenticated;
        return isSignedInToGooglePlay;
    }

    public string GetGooglePlayID()
    {
        //return PlayGamesPlatform.Instance.GetUserEmail();
        return googlePlayID;
    }

    public string GetLastGooglePlayIDSignedIn()
    {
        return lastGooglePlayIDSignedIn;
    }
#endif


    public string GetDeviceID()
    {
        return SystemInfo.deviceUniqueIdentifier;
        //return "ADSFDASD-456-" + Random.Range(100,999).ToString();
    }

    #endregion

}