using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[RequireComponent(typeof(PhotonView))]
public class ScrController : MonoBehaviour
{
    #region Singleton

    private static ScrController instance;

    public static ScrController Instance
    {
        get { return instance; }
        set 
        { 
            instance = value;
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    #endregion
    //public DataScore[] scores;
    List<ScrCarController> cars;
	List<ScrCarController>redTeamCars;
	List<ScrCarController>blueTeamCars;
	public List<int>spawnPointIndexList = new List<int> ();

	public int redTeamKills;
	public int blueTeamKills;

	public bool redTeamWon;
	public bool blueTeamWon;


    private PhotonView nv;


    void Awake()
    {
        instance = this;
        nv = GetComponent<PhotonView>();
        Accounting.Instance.SetInActiveModeUrl(Accounting.Instance.currentUser.Id, null , null);
    }

    public void DeathAnnouncement(ScrCarController diedCar, ScrCarController killerCar, KillMethod killMethod)
    {
		nv.RPC("_deathAnnounced", PhotonTargets.MasterClient, diedCar.photonView.viewID, killerCar.photonView.viewID, (int)killMethod, GameplayDefaultSettings.Instance.isTeamMatch);
        nv.RPC("_celebrateKill", killerCar.Owner, diedCar.photonView.viewID);
    }

    [RPC]
    void _deathAnnounced(int diedCarViewID, int killerCarViewID, int killMethodValue, bool isTeam)
    {
        //if (diedCarViewID == killerCarViewID)
            //return;
        ScrCarController diedCar = FindCar(diedCarViewID);
        ScrCarController killerCar = FindCar(killerCarViewID);
        KillMethod killMethod = (KillMethod)killMethodValue;

		if ( isTeam ) {
			GameplayUI.Instance.newsWall.SubmitText (killerCarViewID, killerCar.Username + " <*> " + diedCar.Username, killMethod);
		} 
		else {
        	GameplayUI.Instance.newsWall.SubmitText( killerCar.Username + " <*> " + diedCar.Username, killMethod);
		}

        if (killerCar.nv.viewID == ScrCarController.Instance.nv.viewID)
            GameplayUI.Instance.logPanel.SubmitText(diedCar.Username, LogPanelMessages.YouKilled);

        killerCar.GetKill();
        diedCar.AddDeath(killMethod == KillMethod.SelfDestruction);

        diedCar.nv.RPC("InstantiateAtSafePoint", diedCar.Owner, EnvironmentController.Instance.GetSafestSpawnPointIndex());
    }

    public void SuicideAnnounced(ScrCarController diedCar)
    {
        nv.RPC("_suicideAnnounced", PhotonTargets.MasterClient, diedCar.nv.viewID);
    }

    [RPC]
    void _suicideAnnounced(int diedCarViewID)
    {
        ScrCarController diedCar = FindCar(diedCarViewID);
        diedCar.nv.RPC("InstantiateAtSafePoint", diedCar.Owner, EnvironmentController.Instance.GetSafestSpawnPointIndex());
    }

    [RPC]
    void _celebrateKill(int diedCarViewID)
    {
        ScrCarController diedCar = FindCar(diedCarViewID);
        GameplayUI.Instance.logPanel.SubmitText(diedCar.Username, LogPanelMessages.YouKilled);
    }



    ScrCarController leadingCar;
	public PunTeams.Team leadingTeam = PunTeams.Team.blue;
    public void ScoreChanged(ScrCarController car)
    {
        if (PhotonNetwork.isMasterClient)
            _ScoreChanged(car.nv.viewID);
        else
            nv.RPC("_ScoreChanged", PhotonTargets.MasterClient, car.nv.viewID);
    }

    [RPC]
    void _ScoreChanged(int viewID)
    {
        //ScrCarController car = FindCar(viewID);

        if (Server.Instance.GameSessionState != GameSessionState.GamePlaying)
            return;

        //GameplayUI.Instance.rankMedal.Refresh();

        ScrCarController currentLeadingCar = cars[0];
        for (int i = 1; i < cars.Count; i++)
        {
            if (cars[i].Score > currentLeadingCar.Score)
                currentLeadingCar = cars[i];
        }

		if( !GameplayDefaultSettings.Instance.isTeamMatch ){
	        if (currentLeadingCar != leadingCar)
	        {
	            leadingCar = currentLeadingCar;
	            GameplayUI.Instance.newsWall.SubmitText(currentLeadingCar.Username + " *", ExtraSigns.GotTheLead);
	        }
		}

		int redKiil = GetRedTeamKill ();
		int blueKill = GetBlueTeamKill ();

		if( GameplayDefaultSettings.Instance.isTeamMatch && redKiil != blueKill ){

			PunTeams.Team firstTeam = PunTeams.Team.blue;

			if( redKiil > blueKill ){
				firstTeam = PunTeams.Team.red;
			}

			if( leadingTeam != firstTeam ){
				if( firstTeam == PunTeams.Team.blue ){
					GameplayUI.Instance.newsWall.SubmitText( GetBlueTeamCars()[0].photonView.viewID,"BlueTeam *", ExtraSigns.GotTheLead);
				}
				else if(firstTeam == PunTeams.Team.red){
					GameplayUI.Instance.newsWall.SubmitText(GetRedTeamCars()[0].photonView.viewID,"RedTeam *", ExtraSigns.GotTheLead);
				}
				leadingTeam = firstTeam;
			}
		}

        int maxKills = 0;
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].Kills > maxKills)
                maxKills = cars[i].Kills;

			if( !GameplayDefaultSettings.Instance.isTeamMatch  ){
				if( cars[i].nv.owner.GetTeam() == PunTeams.Team.none ){
					if (cars[i].Kills >= GameplayDefaultSettings.Instance.Settings.KillsNumberForWin)
					{
						Server.Instance.EndGameSession();
						//EndGameSession();
						return;
					}
				}
			}
			else{
//				if (GetBlueTeamKill() >= 1)
				if (GetBlueTeamKill() >= GameplayDefaultSettings.Instance.Settings.KillNumberForWinTeamDeathMatch)
				{
					SetBlueTeamWon();
					Server.Instance.EndGameSession();
					//EndGameSession();
					return;
				}
//				if(GetRedTeamKill() >= 1){
				if(GetRedTeamKill() >= GameplayDefaultSettings.Instance.Settings.KillNumberForWinTeamDeathMatch){
					SetRedTeamWon();
					Server.Instance.EndGameSession();
					//EndGameSession();
					return;
				}
			}
        }

        if (maxKills >= GameplayDefaultSettings.Instance.SealRoomAfterKills)
            Server.Instance.SealTheRoom();
    }
	
	public void SetRedTeamWon(){
		nv.RPC( "RPC_SetRedTeamWon", PhotonTargets.All );
	}

	public void SetBlueTeamWon (){
		nv.RPC("RPC_SetBlueTeamWon", PhotonTargets.All);
	}

	[RPC]
	void RPC_SetRedTeamWon(){
		redTeamWon = true;
	}

	[RPC]
	void RPC_SetBlueTeamWon(){
		blueTeamWon = true;
	}
	

    /*
    private void SetScoreForCar(int viewID, int totalScore = -1, int totalDeaths = -1, int totalKills = -1)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i].viewID == viewID)
            {
                if (totalScore != -1)
                {
                    ScrCarController car = FindCar(viewID);
                    Debug.Log(car.Username + ", score=" + totalScore);

                    scores[i].score = totalScore;
                }
                    

                if (totalDeaths != -1)
                {
                    ScrCarController car = FindCar(viewID);
                    Debug.Log(car.Username + ", deaths=" + totalDeaths);

                    scores[i].deaths = totalDeaths;
                }
                    

                if (totalKills != -1)
                {
                    ScrCarController car = FindCar(viewID);
                    Debug.Log(car.Username + ", kills=" + totalKills);

                    scores[i].kills = totalKills;
                }
                    

                break;
            }
        }
    }

    [RPC]
    private void SetDataScore(int[] packDataScore) // 0 - viewID  / 1 - totalScore / 2 - totalDeath / 3 - totalScore
    {
        Debug.Log("view id=" + packDataScore[0] + ", score=" + packDataScore[1] + ", deaths=" + packDataScore[2] + ", kills=" + packDataScore[3]);
        SetScoreForCar(packDataScore[0], packDataScore[1], packDataScore[2], packDataScore[3]);
    }


    public void SendDataScore(int viewID, int totalScore = -1, int totalDeaths = -1, int totalKills = -1)
    {
        int[] valuePack = new int[4];

        valuePack[0] = viewID;
        valuePack[1] = totalScore;
        valuePack[2] = totalDeaths;
        valuePack[3] = totalKills;

        nv.RPC("SetDataScore", PhotonTargets.MasterClient, valuePack);
    }
     */

    public void CallDataHit(ScrCarController carHiter, ScrCarController carDamaged, KillMethod typeDamage, Vector3 position)
    {
        //GameplayUI.Instance.newsWall.SubmitText(string.Format("{0} * {1}", carHiter.Username, carDamaged.Username), typeDamage);

        int score = 0;
        switch (typeDamage)
        {
            case KillMethod.MachineGun: score = ScoreSettings.Instance.machineGunBulletScore; break;
            case KillMethod.Rocket: score = ScoreSettings.Instance.rocketScore; break;
            case KillMethod.Mine: score = ScoreSettings.Instance.mineScore; break;
            case KillMethod.Crash: score = ScoreSettings.Instance.crashScore; break;
        }

        carHiter.AddScore(score);
        
        if (carHiter == ScrCarController.Instance)
        {
            switch (typeDamage)
            {
                case KillMethod.MachineGun:
                    break;
                case KillMethod.Rocket:
                    GameplayUI.Instance.logPanel.SubmitText(carDamaged.Username, LogPanelMessages.RocketHit);
                    break;
                case KillMethod.Mine:
                    GameplayUI.Instance.logPanel.SubmitText(carDamaged.Username, LogPanelMessages.MineHit);
                    break;
            }
        }
            

        /*
        if (carHiter == ScrCarController.Instance)
        {
            GameplayUI.Instance.AddScore(score, position);
            if (isFatalDamage)
                GameplayUI.Instance.KillNumber++;
        }
        else
        {
            carHiter.addScore(score);
            if (isFatalDamage)
                GameplayUI.Instance.KillNumber++;
            carHiter.
        }*/

    }


    public void AddCar(ScrCarController car, bool isMine)
    {
        if (cars == null)
            cars = new List<ScrCarController>();

        cars.Add(car);

		if (!GameplayDefaultSettings.Instance.isTeamMatch) {

			if (PhotonNetwork.isMasterClient && cars.Count >= GameplayDefaultSettings.Instance.MinimumPlayersToStartGame)
				Server.Instance.StartGameIfItIsntAlready ();
		} 
		else {
			if (PhotonNetwork.isMasterClient && cars.Count >= GameplayDefaultSettings.Instance.MinimumPlayersToStartGame )
				Server.Instance.StartGameIfItIsntAlready ();
		}
    }

	public void AddCarTeam(ScrCarController car, PunTeams.Team team, bool isMine)
	{
		if( team == PunTeams.Team.blue ){
			if( GetBlueTeamCars().Length < 4 ){
				
				if (blueTeamCars == null)
					blueTeamCars = new List<ScrCarController>();
				
				car.Owner.SetTeam( PunTeams.Team.blue );
				blueTeamCars.Add(car);
				GameplayUI.Instance.logPanel.SubmitText("", LogPanelMessages.YouAreInBlueTeam );
			}
			else{
				//ScrCarController.Instance.SetTeam();
			}
		}
		else if( team == PunTeams.Team.red ){
			
			if( GetRedTeamCars().Length < 4 ){
				
				if (redTeamCars == null)
					redTeamCars = new List<ScrCarController>();
				
				car.Owner.SetTeam( PunTeams.Team.red );
				
				redTeamCars.Add(car);
				GameplayUI.Instance.logPanel.SubmitText( "" ,LogPanelMessages.YouAreInRedTeam );
			}
			else{
			//	ScrCarController.Instance.SetTeam();
			}
		}
	}

	public void AddSafeIndexPoint( int index ){
		nv.RPC ( "RPC_AddIndexList", PhotonTargets.AllBuffered, index );
	}

	public void RemoveSafeIndexPoint( int index ){
		nv.RPC ( "RPC_RemoveIndexList", PhotonTargets.AllBuffered, index );
	}

	public bool IndexVild( int index ){
		if( !spawnPointIndexList.Contains ( index ) ){
			AddSafeIndexPoint(index);
			return true;
		}
		else{
			return  false;
		}
	}

	[RPC]
	void RPC_AddIndexList( int index ){
		if( !spawnPointIndexList.Contains( index ) ){
			spawnPointIndexList.Add ( index );
			StartCoroutine( _RemovingIndex(index));
		}
	}

	IEnumerator _RemovingIndex (int index){
		yield return new WaitForSeconds ( 7 );
		RemoveSafeIndexPoint ( index );
	}


	[RPC]
	void RPC_RemoveIndexList( int index ){
		if( spawnPointIndexList.Contains( index ) ){
			spawnPointIndexList.Remove ( index );
		}
	}

	public int FindASafeIndex(){
		int temp = -1;
		for( int cnt = 0; cnt < spawnPointIndexList.Count; cnt++ ){
			if( !spawnPointIndexList.Contains( spawnPointIndexList[cnt] ) ){
				temp = spawnPointIndexList[cnt];
			}
		}

		if (temp == -1)
			temp = UnityEngine.Random.Range( 0, spawnPointIndexList.Count );

		AddSafeIndexPoint ( temp );

		return temp;
	}


//	IEnumerator WaitForSetTeam( ScrCarController car, PunTeams.Team team, bool isMine ){
//
//		yield return new WaitForSeconds ( 1 );
//
//	}

    public void RemoveCar(ScrCarController car)
    {
        /*if (cars != null)
        {
            cars.Remove(car);
            GameplayUI.Instance.newsWall.SubmitText("* " + Data.UserName, ExtraSigns.Left);
            Debug.Log("car " + car.name + " removed. username=" + car.Username);
        }*/
            
    }

    public string GetUsername(int index)
    {
        return cars[index].Username;
    }

    public int GetCarsCount()
    {
        if (cars == null)
            return 0;
        else
            return cars.Count;
    }

    public ScrCarController GetCar(int i)
    {
        return cars[i];
    }

    
    public ScrCarController FindCar(int viewID)
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].nv.viewID == viewID)
                return cars[i];
        }

        return null;
    }

    public ScrCarController[] GetCars()
    {
        return cars.ToArray();
    }


	public int GetBlueTeamKill(){
		int k = 0;
		for( int cnt = 0; cnt < GetBlueTeamCars().Length; cnt++ ){
			k += GetBlueTeamCars()[cnt].Kills;
		}
		return k;
	}

	public int GetBlueTeamScores(){
		int s = 0;
		for( int cnt = 0; cnt < GetBlueTeamCars().Length; cnt++ ){
			s += GetBlueTeamCars()[cnt].Score;
		}
		return s;
	}

	public int GetRedTeamKill(){
		int k = 0;
		for( int cnt = 0; cnt < GetRedTeamCars().Length; cnt++ ){
			k += GetRedTeamCars()[cnt].Kills;
		}
		return k;
	}

	public int GetRedTeamScores(){
		int s= 0;
		for( int cnt = 0; cnt < GetRedTeamCars().Length; cnt++ ){
			s += GetRedTeamCars()[cnt].Score;
		}
		return s;
	}

	public ScrCarController[] GetRedTeamCars()
	{
		List<ScrCarController> tempList = new List<ScrCarController> ();
		for( int cnt = 0 ; cnt < cars.Count; cnt++ ){

			if( cars[cnt].nv.owner.GetTeam() == PunTeams.Team.red ){
				tempList.Add( cars[cnt] );
			}
		}
		return tempList.ToArray ();
	}

	public ScrCarController[] GetBlueTeamCars()
	{
		List<ScrCarController> tempList =  new List<ScrCarController> ();
		for( int cnt = 0 ; cnt < cars.Count; cnt++ ){
			
			if( cars[cnt].nv.owner.GetTeam() == PunTeams.Team.blue ){
				tempList.Add( cars[cnt] );
			}
		}
		return tempList.ToArray ();
	}

    void Update()
    {
//		if(Input.GetKeyDown( KeyCode.A ))
//			Server.Instance.EndGameSession();

        /*if (cars != null && cars.Count > 0)
        {
            int index = 0;
            while (true)
            {
                if (index >= cars.Count)
                {
                    //GameplayUI.Instance.newsWall.SubmitText("* " + index, ExtraSigns.Left);
                    break;
                }
                else if (cars[index]==null || cars[index].nv == null)
                {
                    //GameplayUI.Instance.newsWall.SubmitText("* " + index, ExtraSigns.Left);
                    cars.RemoveAt(index);
                    break;
                }
                else
                {
                    index++;
                    if (index >= cars.Count)
                        break;
                }


                if (index >= cars.Count)
                    break;
            }
        }*/
    }

    public int GetMyCarID()
    {
        return (ScrCarController.Instance.nv.viewID / 1000) - 1;


        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i] == ScrCarController.Instance)
                return i;
        }

        return -1;
    }
}


[Serializable()]
public class DataScore
{
    public int viewID;
    public int kills, deaths, score;
}