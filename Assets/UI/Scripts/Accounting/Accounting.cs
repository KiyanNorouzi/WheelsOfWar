
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Accounting : MonoBehaviour
{
    #region Singleton

    static Accounting _instance;
    public static Accounting Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion


    public const string NO_ITEM_STRING = "no_item";


    public WMUser currentUser;
    public string serverUrl;
    public string registerUrl, loginUrl, getCarsUrl, buyCarUrl, getCosmeticsUrl, buyCosmeticUrl, getVIPPackagesUrl, buyVIPPackageUrl, reportExpiredVIPPackageUrl;
    public string energyRefilledUrl, getCarUpgradesUrl, buyCarUpgradesUrl, updateMoneyUrl, updateScoreUrl, getServerStateUrl;
    public string changeDisplayNameUrl, changeUsernameAndTypeUrl, getStatisticsUrl, updateStatisticsUrl, getServerTimeUrl, updateConsecutiveDaysUrl, carUpgradeTimeUpdateUrl, updateGasUrl;
	public string getMyLeagueDataUrl, getMyteamMateLeaguesDataUrl, getMonthlyUserDataUrl, GetGlobalUserDataUrl, setInActiveModeUrl;
	public string getMessagesUrl, deletingMessageUrl;

    /*[Header("Defualt step value for loading slider")]
    public float loadingValue;*/



    public void Register(string deviceID, string googlePlayID, string displayName, UserLoginType userType, System.Action<string, string, UserLoginType> doneMethod, Action<bool, string, string, string> failMethod)
    {
        StartCoroutine(_Register(deviceID, googlePlayID, displayName, userType, doneMethod, failMethod));

    }

    IEnumerator _Register(string deviceID, string googlePlayID, string displayName, UserLoginType userType, System.Action<string, string, UserLoginType> doneMethod, System.Action<bool, string, string, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("G_play", googlePlayID);
        form.AddField("D_id", deviceID);
        form.AddField("UT", EncryptInt((int)userType));
        form.AddField("Name", displayName);

        Debug.Log("registering at " + deviceID + ", username=" + googlePlayID + ", usertype=" + userType);

        string url = string.Concat(serverUrl, registerUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("registering done with=" + www.text);

            if (www.text.StartsWith("web_"))
            {
                if (www.text.ToLower().IndexOf("duplicate") != -1)
                {
                    if (failMethod != null)
                        failMethod(false, "duplicate username", deviceID, googlePlayID);
                }
                else if (www.text.ToLower().IndexOf("no") != -1)
                {
                    if (failMethod != null)
                        failMethod(false, "unknown error", deviceID, googlePlayID);
                }
                else
                {
                    if (doneMethod != null)
                        doneMethod(deviceID, googlePlayID, userType);
                }
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, deviceID, googlePlayID);
            }
        }
        else
        {
            Debug.Log("registering failed with=" + www.error);

            if (failMethod != null)
                failMethod(false, www.error, deviceID, googlePlayID);
        }

    }




    public void Login(string deviceID, string googlePlayID, UserLoginType userType, System.Action<WMUser[]> doneMethod, Action<bool, string, string> failMethod)
    {
        StartCoroutine(_Login(deviceID, googlePlayID, userType, doneMethod, failMethod));
    }

    IEnumerator _Login(string deviceID, string googlePlayID, UserLoginType userType, System.Action<WMUser[]> doneMethod, Action<bool, string, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("G_play", googlePlayID);
        form.AddField("D_id", deviceID);
        form.AddField("UT", EncryptInt((int)userType));

        Debug.Log("logging in as " + googlePlayID + "[" + deviceID + "]" + ", type=" + userType + "(" + ((int)userType) + ")");


        string url = string.Concat(serverUrl, loginUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.StartsWith("web_"))
            {
                Debug.Log("login done with=" + www.text); ///////////////////////////

                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


                string text = www.text.Substring(4);
                if (text.ToLower().IndexOf(NO_ITEM_STRING) != -1)
                {
                    //if (doneMethod != null)
                    //doneMethod(null);

                    if (failMethod != null)
                        failMethod(false, www.text, googlePlayID);
                }
                else
                {
                    WMUser[] users = WMUser.ParseUsers(text);
                    if (doneMethod != null)
                        doneMethod(users);

                    /*for (int i = 0; i < users.Length; i++)
                        Debug.Log(i + "-username=" + users[i].Name + ", gold=" + users[i].Golds + ", Energy=" + users[i].Energy);*/
                }
            }
            else
            {
                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

                Debug.Log(www.text);
                if (failMethod != null)
                    failMethod(true, www.text, googlePlayID);
            }
        }
        else
        {
            Debug.Log(www.error);
            if (failMethod != null)
                failMethod(false, www.error, googlePlayID);
        }
    }

    public void UpdateConsecutiveDays(int userID, int consecutiveDays, int consecutiveDaysRewardsClaimed, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_UpdateConsecutiveDays(userID, consecutiveDays, consecutiveDaysRewardsClaimed, doneMethod, failMethod));
    }

    IEnumerator _UpdateConsecutiveDays(int userID, int consecutiveDays, int consecutiveDaysRewardsClaimed, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));
        form.AddField("C_days", EncryptInt(consecutiveDays));
        form.AddField("C_daysg", EncryptInt(consecutiveDaysRewardsClaimed));


        string url = string.Concat(serverUrl, updateConsecutiveDaysUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod(userID);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }

    }

    public void UpdateGas(int userID, int gas, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_UpdateGas(userID, gas, doneMethod, failMethod));
    }

    IEnumerator _UpdateGas(int userID, int gas,System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));
        form.AddField("Gass", gas);


        string url = string.Concat(serverUrl, updateGasUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod(userID);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }

    }


    public void ChangeDisplayName(int userID, string displayName, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_ChangeUserInfo(userID, displayName, doneMethod, failMethod));
    }

    IEnumerator _ChangeUserInfo(int userID, string displayName, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", displayName);
        form.AddField("ID", EncryptInt(userID));

        string url = string.Concat(serverUrl, changeDisplayNameUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod(userID);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }

    }

    public void ChangeUsernameAndType(int userID, string googlePlayID, UserLoginType userType, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_ChangeUsernameAndType(userID, googlePlayID, (int)userType, doneMethod, failMethod));

        Debug.Log("4");
    }

    IEnumerator _ChangeUsernameAndType(int userID, string googlePlayID, int userType, System.Action<int> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));
        form.AddField("UT", EncryptInt(userType));
        form.AddField("GP", googlePlayID);

        string url = string.Concat(serverUrl, changeUsernameAndTypeUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


            Debug.Log("change user done with " + www.text);

            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod(userID);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

            if (failMethod != null)
                failMethod(false, www.error);
        }

    }

    public void GetStatistics(int userID, System.Action<Statistics[]> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetStatistics(userID, doneMethod, failMethod));
    }

    IEnumerator _GetStatistics(int userID, System.Action<Statistics[]> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));

        string url = string.Concat(serverUrl, getStatisticsUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("get statistics done with " + www.text);

            if (www.text.StartsWith("web_"))
            {
                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


                if (doneMethod != null)
                {
                    Statistics[] statistics = Statistics.ParseStatistics(www.text.Substring(4));
                    doneMethod(statistics);
                }

            }
            else
            {
                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }

    }

    public delegate void _UpdateCarUpgradeTimeDelegate(bool isServerError, string errorText, string carTag, int partIndex, int time);
    public void UpdateCarUpgradeTime(int userID, string carTag, int part, int time, System.Action<Statistics[]> doneMethod, _UpdateCarUpgradeTimeDelegate failMethod)
    {
        StartCoroutine(_UpdateCarUpgradeTime(userID, carTag, part, time, doneMethod, failMethod));
    }

    IEnumerator _UpdateCarUpgradeTime(int userID, string carTag, int part, int time, System.Action<Statistics[]> doneMethod, _UpdateCarUpgradeTimeDelegate failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));
        form.AddField("car", carTag);
        form.AddField("part", part);
        form.AddField("time", time);

        string url = string.Concat(serverUrl, carUpgradeTimeUpdateUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("get statistics done with " + www.text);

            if (www.text.Trim().StartsWith("web_"))
            {
                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


                if (doneMethod != null)
                {
                    Statistics[] statistics = Statistics.ParseStatistics(www.text.Substring(4));
                    doneMethod(statistics);
                }

            }
            else
            {
                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

                if (failMethod != null)
                    failMethod(true, www.text, carTag, part, time);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error, carTag, part, time);
        }

    }

    public void UpdateStatistics(int userID, int bullets, int Team_wins, int teamLoses, int singleWins, int singleLoses,
        int rocketsSuccessfull, int mineSuccessfull, int bulletsSuccessfull, int rocketsTotal, int minesTotal, int bulletsTotal,
        int deaths, int kills, int minesPickedUp, int rocetsPickedUp, int healthsPickedUp, int shieldsPickedUp, int allPickedUp,
        int distanceCovered, System.Action doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_UpdateStatistics(userID, bullets, Team_wins, teamLoses, singleWins, singleLoses,
        rocketsSuccessfull, mineSuccessfull, bulletsSuccessfull, rocketsTotal, minesTotal, bulletsTotal,
        deaths, kills, minesPickedUp, rocetsPickedUp, healthsPickedUp, shieldsPickedUp, allPickedUp,
        distanceCovered, doneMethod, failMethod));
    }


    IEnumerator _UpdateStatistics(int userID, int bullets, int Team_wins, int teamLoses, int singleWins, int singleLoses,
        int rocketsSuccessfull, int mineSuccessfull, int bulletsSuccessfull, int rocketsTotal, int minesTotal, int bulletsTotal,
        int deaths, int kills, int minesPickedUp, int rocetsPickedUp, int healthsPickedUp, int shieldsPickedUp, int allPickedUp,
        int distanceCovered, System.Action doneMethod, Action<bool, string> failMethod)
    {
        Debug.Log("update stats for " + userID);

        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));
        form.AddField("b", EncryptInt(bullets));
        form.AddField("tw", EncryptInt(Team_wins));
        form.AddField("tl", EncryptInt(teamLoses));
        form.AddField("sw", EncryptInt(singleWins));
        form.AddField("sl", EncryptInt(singleLoses));
        form.AddField("rs", EncryptInt(rocketsSuccessfull));
        form.AddField("ms", EncryptInt(mineSuccessfull));
        form.AddField("bs", EncryptInt(bulletsSuccessfull));
        form.AddField("rt", EncryptInt(rocketsTotal));
        form.AddField("mt", EncryptInt(minesTotal));
        form.AddField("bt", EncryptInt(bulletsTotal));
        form.AddField("d", EncryptInt(deaths));
        form.AddField("k", EncryptInt(kills));
        form.AddField("mp", EncryptInt(minesPickedUp));
        form.AddField("rp", EncryptInt(rocetsPickedUp));
        form.AddField("hp", EncryptInt(healthsPickedUp));
        form.AddField("sp", EncryptInt(shieldsPickedUp));
        form.AddField("ap", EncryptInt(allPickedUp));
        form.AddField("distance", EncryptInt(distanceCovered));

        string url = string.Concat(serverUrl, updateStatisticsUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("update statistics done with " + www.text);

            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }

    }
	

	public void GetMyLeagueDatas(int userId, Action<string> doneMethod, Action failMethod)
	{
		StartCoroutine(_GetMyLeagueDatas(userId, doneMethod, failMethod));
	}

	IEnumerator _GetMyLeagueDatas(int userId, Action<string> doneMethod, Action failMethod){
		WWWForm form = new WWWForm();
		form.AddField("ID", EncryptInt(userId));
		string usrl = string.Concat( serverUrl, getMyLeagueDataUrl );
		WWW www = new WWW( usrl, form );
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
            Debug.LogError("League data    :  " + www.text);
			if (www.text.Trim ().StartsWith ("web_")) {
				if (doneMethod != null)
					doneMethod (www.text);
			}
			else {
				if( failMethod != null )
					failMethod();
			}
		}
		else {
			Debug.Log( "Fail" );
			if( failMethod != null )
				failMethod();
		}
	}

	public void GetMyLeagueTeammateDatas(int userId, Action<string> doneMethod, Action doneMethod2, Action failedMethod  )
	{
		StartCoroutine(_GetMyLeagueTeammateDatas(userId, doneMethod, doneMethod2, failedMethod));
	}
	
	IEnumerator _GetMyLeagueTeammateDatas(int userId, Action<string> doneMethod, Action doneMethod2, Action failedMethod){
		WWWForm form = new WWWForm();
		form.AddField("ID", EncryptInt(userId));
		
		string usrl = string.Concat( serverUrl, getMyteamMateLeaguesDataUrl );
		WWW www = new WWW( usrl, form );
		yield return www;
		if( string.IsNullOrEmpty(www.error ) ){	
			if ( www.text.Trim().StartsWith("web_") )
			{
				if (doneMethod != null){
					doneMethod(www.text);
					if( doneMethod2 != null ){
						doneMethod2();
					}
				}
			}
			
			else{
				Debug.LogError( "Faild : " + www.text );
				if( failedMethod != null ){
					failedMethod();
				}
			}
		}
		else{
			Debug.Log( "fail" );
			if( failedMethod != null ){
				failedMethod();
			}
		}
	}

	public void GetMonthlyUserData( int userId, Action<string> doneMethod, Action doneMethod2, Action failedMethod ){
		StartCoroutine ( _GetMonthlyUserData( userId, doneMethod, doneMethod2, failedMethod ) );
	}

	IEnumerator _GetMonthlyUserData(int userId, Action<string> doneMethod, Action doneMethod2, Action failedMethod){
		WWWForm form = new WWWForm();
		form.AddField("ID", EncryptInt(userId));
		
		string usrl = string.Concat( serverUrl, getMonthlyUserDataUrl );
		WWW www = new WWW( usrl, form );
		yield return www;
		if( string.IsNullOrEmpty(www.error ) ){	
			if ( www.text.Trim().StartsWith("web_") )
			{
//				Debug.LogError( "id :" + userId);
				if (doneMethod != null){
					doneMethod(www.text);
					if( doneMethod2 != null ){
						doneMethod2();
					}
				}
			}
			else{
				Debug.LogError( "Faild : " + www.text );
				if( failedMethod != null ){
					failedMethod();
				}
			}
		}
		else{
			Debug.Log( "fail" );
			if( failedMethod != null ){
				failedMethod();
			}
		}
	}

	public void GetGlobalUserData( int userId, Action<string> doneMethod, Action doneMethod2, Action failedMethod ){
		StartCoroutine ( _GetGlobalUserData( userId, doneMethod, doneMethod2, failedMethod ) );
	}
	
	IEnumerator _GetGlobalUserData(int userId, Action<string> doneMethod, Action doneMethod2, Action failedMethod){
		WWWForm form = new WWWForm ();
		form.AddField ( "ID", EncryptInt( userId ) );
		string uurl = string.Concat ( serverUrl, GetGlobalUserDataUrl );
		WWW www = new WWW ( uurl, form );
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			if (www.text.Trim ().StartsWith ("web_")) {
				if (doneMethod != null) {
					doneMethod (www.text);
					if (doneMethod2 != null) {
						doneMethod2 ();
					}
				}
			}
			else {
				if (failedMethod != null) {
					failedMethod ();
				}
			}
		}
		else {
			if (failedMethod != null) {
				failedMethod ();
			}
		}
	}


	public void SetInActiveModeUrl( int userId, Action<string> doneMethod, Action failedMethod ){
		StartCoroutine ( _SetInActiveModeUrl( userId, doneMethod, failedMethod ) );
	}
	
	IEnumerator _SetInActiveModeUrl(int userId, Action<string> doneMethod, Action failedMethod){
		WWWForm form = new WWWForm ();
		form.AddField ( "ID", EncryptInt( userId ) );
		string uurl = string.Concat ( serverUrl, setInActiveModeUrl );
		WWW www = new WWW ( uurl, form );
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			if (www.text.Trim ().StartsWith ("web_")) {
				if (doneMethod != null) {
					doneMethod (www.text);
				}
			}
			else {
				if (failedMethod != null) {
					failedMethod ();
				}
			}
		}
		else {
			if (failedMethod != null) {
				failedMethod ();
			}
		}
	}

	public void GetMessages( int userId, Action<string> doneMethod, Action failedMethod ){
		StartCoroutine ( _GetMessages( userId, doneMethod, failedMethod ) );
	}
	
	IEnumerator _GetMessages(int userId, Action<string> doneMethod, Action failedMethod){
		Debug.Log ("Started");
		WWWForm form = new WWWForm ();
		form.AddField ( "ID", EncryptInt( userId ) );
		string uurl = string.Concat ( serverUrl, getMessagesUrl );
		WWW www = new WWW ( uurl, form );
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			Debug.Log( www.text );
			if (www.text.Trim ().StartsWith ("web_")) {
				if (doneMethod != null) {
					doneMethod (www.text);
				}
			}
			else {
				if (failedMethod != null) {
					failedMethod ();
				}
			}
		}
		else {
			if (failedMethod != null) {
				failedMethod ();
			}
		}
	}
	


	public void DeletingMessages( int userId, int messageId, Action doneMethod, Action failedMethod ){
		StartCoroutine ( _DeletingMessages( userId, messageId, doneMethod, failedMethod ) );
	}
	
	IEnumerator _DeletingMessages(int userId, int messageId, Action doneMethod, Action failedMethod){
		WWWForm form = new WWWForm ();
		form.AddField ( "ID", EncryptInt( userId ) );
		form.AddField ( "MID", EncryptInt( messageId ) );
		string uurl = string.Concat ( serverUrl, deletingMessageUrl );
		WWW www = new WWW ( uurl, form );
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			if(	www.text == "done" ){
				if (doneMethod != null) {
					doneMethod ();
				}
			}
			else {
				if (failedMethod != null) {
					failedMethod ();
					Debug.Log("Fail");
				}
			}
		}
		else {
			if (failedMethod != null) {
				failedMethod ();
				Debug.Log("Fail");
			}
		}
	}


    public void GetUserCars(int userId, System.Action<Car[]> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetUserCars(userId, doneMethod, failMethod));
    }


    IEnumerator _GetUserCars(int userId, System.Action<Car[]> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));

        string url = string.Concat(serverUrl, getCarsUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("_GetUserCars");

            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


            if (www.text.Trim().StartsWith("web_"))
            {
                Car[] cars = Car.ParseCars(www.text.Trim().Substring(4));
                if (doneMethod != null)
                    doneMethod(cars);
            }
            else
            {

                if (failMethod != null)
                    failMethod(true, www.text);
            }

        }
        else
        {
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);
            if (failMethod != null)
                failMethod(false, www.error);
        }
    }






    public void BuyCar(int userId, string carTag, int carLevel, System.Action doneMethod, Action<bool, string, string> failMethod)
    {
        StartCoroutine(_BuyCar(userId, carTag, carLevel, doneMethod, failMethod));
    }


    IEnumerator _BuyCar(int userId, string carTag, int carLevel, System.Action doneMethod, Action<bool, string, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("tag", carTag);
        form.AddField("level", EncryptInt(carLevel));


        string url = serverUrl + buyCarUrl;
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.Trim().StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, carTag);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error, carTag);
        }

    }





    public void GetCosmetics(int userId, System.Action<CosmeticItemData[]> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetCosmetics(userId, doneMethod, failMethod));
    }

    IEnumerator _GetCosmetics(int userId, System.Action<CosmeticItemData[]> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));

        string url = string.Concat(serverUrl, getCosmeticsUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log(" _GetCosmetics");

            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


            if (www.text.Trim().StartsWith("web_"))
            {
                CosmeticItemData[] cosmeticItems = CosmeticItemData.ParseCosmeticItems(www.text.Trim().Substring(4));
                if (doneMethod != null)
                    doneMethod(cosmeticItems);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);

                Debug.Log("error=" + www.text);
            }
        }
        else
        {
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

            Debug.Log("error=" + www.error);
            if (failMethod != null)
                failMethod(false, www.error);
        }
    }


    public delegate void _BuyCosmeticFailed(bool serverError, string errorText, string carTag, int partIndex, int cosmeticIndex);
    public void BuyCosmetics(int userId, string carTag, int partIndex, int cosmeticIndex, System.Action doneMethod, _BuyCosmeticFailed failMethod)
    {
        StartCoroutine(_BuyCosmetics(userId, carTag, partIndex, cosmeticIndex, doneMethod, failMethod));   
    }

    IEnumerator _BuyCosmetics(int userId, string carTag, int partIndex, int cosmeticIndex, System.Action doneMethod, _BuyCosmeticFailed failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("P_index", EncryptInt(partIndex));
        form.AddField("Cos_index", EncryptInt(cosmeticIndex));
        form.AddField("C_tag", carTag);

        Debug.Log("car=" + carTag + ", part=" + partIndex + ", cosmetic=" + cosmeticIndex);

        string url = string.Concat(serverUrl, buyCosmeticUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.Trim().StartsWith("web_ok"))
            {
                if (doneMethod != null)
                    doneMethod();
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, carTag, partIndex, cosmeticIndex);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error, carTag, partIndex, cosmeticIndex);
        }
    }




    public void GetVIPPackages(int userId, System.Action<VIPPackage[]> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetVIPPackages(userId, doneMethod, failMethod));    
    }


    IEnumerator _GetVIPPackages(int userId, System.Action<VIPPackage[]> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));


        string url = string.Concat(serverUrl, getVIPPackagesUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("_GetVIPPackages");
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


            if (www.text.Trim().ToLower().StartsWith("web_"))
            {
                string text = www.text.Trim().Substring(4);

                if (doneMethod != null)
                {
                    if (text.ToLower().IndexOf(NO_ITEM_STRING) != -1)
                    {
                        doneMethod(null);
                    }
                    else
                    {
                        VIPPackage[] vipPackages = VIPPackage.ParseVIPPackages(text);
                        if (doneMethod != null)
                            doneMethod(vipPackages);
                    }
                }
            }
            else
            {
                //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }
    }

    public void ReportExpirePackageID(int packageID)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(packageID));

        string url = string.Concat(serverUrl, reportExpiredVIPPackageUrl);
        WWW www = new WWW(url, form);

        Debug.Log("report expired vip package, id=" + packageID);
   
    }


    public void BuyVIPPackages(int userId, int packageID, System.Action<int> doneMethod, Action<bool, string, int> failMethod)
    {
        StartCoroutine(_BuyVIPPackages(userId, packageID, doneMethod, failMethod)); 
    }


    IEnumerator _BuyVIPPackages(int userId, int packageID, System.Action<int> doneMethod, Action<bool, string, int> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("Vip_ID", EncryptInt(packageID));

        string url = string.Concat(serverUrl, buyVIPPackageUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("_BuyVIPPackages");

            if (www.text.Trim().StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod(packageID);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, packageID);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error, packageID);
        }
    }


    public void GasRefilled(int userID, System.Action doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GasRefilled(userID, doneMethod, failMethod));  
    }


    IEnumerator _GasRefilled(int userID, System.Action doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));

        //text.text = "Connecting...";

        string url = string.Concat(serverUrl, energyRefilledUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

            if (www.text.Trim().StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();

            }
            else
            {
                if (failMethod != null)
                    failMethod(false, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }
    }

    public void GetUserCarUpgrades(int userID, Action<CarUpgrade[]> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetUserCarUpgrades(userID, doneMethod, failMethod));  
    }


    IEnumerator _GetUserCarUpgrades(int userID, Action<CarUpgrade[]> doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userID));


        //text.text = "Connecting...";

        string url = string.Concat(serverUrl, getCarUpgradesUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("_GetUserCarUpgrades");
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


            if (www.text.Trim().StartsWith("web_"))
            {
                if (doneMethod != null)
                {
                    if (www.text.Trim().ToLower().IndexOf(NO_ITEM_STRING) != -1)
                        doneMethod(null);
                    else
                    {
                        CarUpgrade[] upgrades = CarUpgrade.ParseCarUpgrades(www.text.Trim().Substring(4));
                        doneMethod(upgrades);
                    }
                }
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

            if (failMethod != null)
                failMethod(false, www.error);
        }
    }


    public delegate void _BuyCarUpgradeFailed(bool serverError, string errorText, string carTag, int partIndex, int level, int time);
    public void BuyCarUpgrades(int userId, string carTag, int partIndex, int level, int time, Action doneMethod, _BuyCarUpgradeFailed failMethod)
    {
        StartCoroutine(_BuyCarUpgrades(userId, carTag, partIndex, level, time, doneMethod, failMethod));  
    }


    IEnumerator _BuyCarUpgrades(int userId, string carTag, int partIndex, int level, int time, Action doneMethod, _BuyCarUpgradeFailed failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("car", carTag);
        form.AddField("part", partIndex);
        form.AddField("level", level);
        form.AddField("time", time);


        //text.text = "Connecting...";

        string url = serverUrl + buyCarUpgradesUrl;
        /*if (level == 1)
            url += buyCarUpgradesUrl;
        else
            url += updateCarUpgradeUrl;*/

        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("buy car upgrade for user id " + userId + " done, text=" + www.text);
            if (www.text.Trim().StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();
                //text.text = www.text.Trim().Substring(4);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, carTag, partIndex, level, time);
            }
        }
        else
        {
            Debug.Log("buy car upgrade for user id " + userId + " failed, text=" + www.error);
            if (failMethod != null)
                failMethod(false, www.error, carTag, partIndex, level, time);
        }
    }

    public void UpdateMoney(int userId, int gold, int money, Action doneMethod, Action<bool, string, int, int> failMethod)
    {
        StartCoroutine(_UpdateMoney(userId, gold, money, doneMethod, failMethod)); 
    }


    IEnumerator _UpdateMoney(int userId, int gold, int money, Action doneMethod, Action<bool, string, int, int> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("G", EncryptInt(gold));
        form.AddField("B", EncryptInt(money));

        //text.text = "Connecting...";

        string url = string.Concat(serverUrl, updateMoneyUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.Trim().StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();
                //text.text = www.text.Substring(4);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, gold, money);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error, gold, money);
        }
    }

    /*
    public void UpdateGas(int userId, int gas, Action doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_UpdateGas(userId, gas, doneMethod, failMethod));
    }

    IEnumerator _UpdateGas(int userId, int gas, Action doneMethod, Action<bool, string> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("gas", EncryptInt(gas));

        //text.text = "Connecting...";

        string url = string.Concat(serverUrl, updateGasUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();
                //text.text = www.text.Substring(4);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }
    }*/



    public void UpdateScore(int userId, int addingScore, Action doneMethod, Action<bool, string, int> failMethod)
    {
        StartCoroutine(_UpdateScore(userId, addingScore, doneMethod, failMethod)); 
    }

    IEnumerator _UpdateScore(int userId, int addingScore, Action doneMethod, Action<bool, string, int> failMethod)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", EncryptInt(userId));
        form.AddField("S", EncryptInt(addingScore));

        //text.text = "Connecting...";

        string url = string.Concat(serverUrl, updateScoreUrl);
        WWW www = new WWW(url, form);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            if (www.text.StartsWith("web_"))
            {
                if (doneMethod != null)
                    doneMethod();
                //text.text = www.text.Substring(4);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text, addingScore);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error, addingScore);
        }
    }


    public void GetServerState(Action<bool> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetServerState(doneMethod, failMethod));
    }


    IEnumerator _GetServerState(Action<bool> doneMethod, Action<bool, string> failMethod)
    {
        //text.text = "Connecting...";

        string url = string.Concat(serverUrl, getServerStateUrl);
        WWW www = new WWW(url);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {

            if (www.text.Trim().StartsWith("web_"))
            {
                string answer = www.text.Trim().Substring(4);
                if (doneMethod != null)
                    doneMethod(answer.ToUpper().StartsWith("[ON"));
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            if (failMethod != null)
                failMethod(false, www.error);
        }
    }

    public void GetServerTime(Action<int, int, int> doneMethod, Action<bool, string> failMethod)
    {
        StartCoroutine(_GetServerTime(doneMethod, failMethod)); 
    }

    IEnumerator _GetServerTime(Action<int, int, int> doneMethod, Action<bool, string> failMethod)
    {
        string url = string.Concat(serverUrl, getServerTimeUrl);
        WWW www = new WWW(url);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("_GetServerTime");

            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, true);


            if (www.text.StartsWith("web_"))
            {
                int startIndex = www.text.IndexOf("[") + 1;
                int endIndex = www.text.IndexOf("]");

                string content = www.text.Substring(startIndex, endIndex - startIndex);

                int commaIndex = content.IndexOf(",");
                int day = int.Parse(content.Substring(0, commaIndex));
                string time = content.Substring(commaIndex + 1);
                int colonIndex = time.IndexOf(":");
                int hour = int.Parse(time.Substring(0, colonIndex));
                int minute = int.Parse(time.Substring(colonIndex + 1));

                if (doneMethod != null)
                    doneMethod(day, hour, minute);
                //text.text = www.text.Substring(4);
            }
            else
            {
                if (failMethod != null)
                    failMethod(true, www.text);
            }
        }
        else
        {
            //LoginSceneController.Instance.LoadingSliderAddition(loadingValue, false);

            if (failMethod != null)
                failMethod(false, www.error);
        }
    }





    public static int EncryptInt(int n)
    {
        n += 24;
        n *= 2;
        n += 57;

        return n;
    }

    public static int DecryptInt(int n)
    {
        n -= 57;
        n /= 2;
        n -= 24;

        return n;
    }
}

public enum UserLoginType
{
    Guest = 0,
    GooglePlay = 1
}