using UnityEngine;
using System.Collections;

public class Scoreboard : Photon.MonoBehaviour 
{
    public GameObject myGameObject, nextButtonGameObject;
    public ScoreRow[] rows, blueRows, redRows;
    public Color[] topRowsColor;
    public Color normalColor;
    public float refreshTime;
	public RectTransform blueTeamPanel, redTeamPanel ,firstTeam, secondTeam;


    int playersNumber, redTeamNumber, blueTeamNumber, playerRank;
    ScrCarController[] cars, redCars, blueCars;


    public void Activate(bool autoRefresh)
    {

		if (!GameplayDefaultSettings.Instance.isTeamMatch) {
			//playersNumber = ScrController.Instance.GetCarsCount();
			cars = ScrController.Instance.GetCars ();
			playersNumber = cars.Length;


			int[] scores = new int[playersNumber];
			int[] ranks = new int[playersNumber];

			for (int i = 0; i < playersNumber; i++) {
				if (!cars [i].nv.isMine && cars [i].IsDropped)
					scores [i] = -1; // cars[i].Score;
	            else
					scores [i] = cars [i].Score;

				ranks [i] = i;
			}


			int temp;
			for (int i = 0; i < scores.Length; i++) {
				for (int j = 0; j < scores.Length - 1; j++) {
					if (scores [j] < scores [j + 1]) {
						temp = scores [j + 1];
						scores [j + 1] = scores [j];
						scores [j] = temp;

						temp = ranks [j + 1];
						ranks [j + 1] = ranks [j];
						ranks [j] = temp;
					}
				}
			}


			for (int i = 0; i < rows.Length; i++) {
				if (i < playersNumber) {
					//ScrCarController car = ScrController.Instance.GetCar(ranks[i]);
					ScrCarController car = cars [ranks [i]]; // ScrController.Instance.GetCar(ranks[i]);
					//if (!car.IsDropped && car.nv.isMine)
					if (car == ScrCarController.Instance) {
						playerRank = i + 1;

						Color c = normalColor;
						if (i < topRowsColor.Length)
							c = topRowsColor [i];

						rows [i].Activate (i + 1, car.Username, car.Kills, car.Deaths, car.Score, car.Cheats, car.IsDropped, true, c);
					} else
						rows [i].Activate (i + 1, car.Username, car.Kills, car.Deaths, car.Score, car.Cheats, car.IsDropped);
				} else
					rows [i].Deactivate ();
			}
		}
		else {

			if( !GameplayDefaultSettings.Instance.isTeamMatch )
				return;

			if( ScrController.Instance.GetBlueTeamKill() < ScrController.Instance.GetRedTeamKill()){
				redTeamPanel.anchoredPosition = firstTeam.anchoredPosition;
				blueTeamPanel.anchoredPosition = secondTeam.anchoredPosition;
			}
			else if(ScrController.Instance.GetBlueTeamKill() > ScrController.Instance.GetRedTeamKill()){
				blueTeamPanel.anchoredPosition = firstTeam.anchoredPosition;
				redTeamPanel.anchoredPosition = secondTeam.anchoredPosition;
			}
			else{
				if( ScrController.Instance.GetBlueTeamScores() > ScrController.Instance.GetRedTeamScores() ){
					blueTeamPanel.anchoredPosition = firstTeam.anchoredPosition;
					redTeamPanel.anchoredPosition = secondTeam.anchoredPosition;
				}
				else if(ScrController.Instance.GetBlueTeamScores() < ScrController.Instance.GetRedTeamScores()){
					redTeamPanel.anchoredPosition = firstTeam.anchoredPosition;
					blueTeamPanel.anchoredPosition = secondTeam.anchoredPosition;
				}
			}


			blueCars = ScrController.Instance.GetBlueTeamCars ();
			blueTeamNumber = blueCars.Length;

			redCars = ScrController.Instance.GetRedTeamCars ();
			redTeamNumber = redCars.Length;

			int[] blueScore = new int[blueTeamNumber];
			int[] blueRank = new int[blueTeamNumber];

			int[] redScore = new int[redTeamNumber];
			int[] redRank = new int[redTeamNumber];


			for (int i = 0; i < blueTeamNumber; i++) {
				if (!blueCars [i].nv.isMine && blueCars [i].IsDropped)
					blueScore [i] = -1; // cars[i].Score;
				else
					blueScore [i] = blueCars [i].Score;
				
				blueRank [i] = i;
			}

			for (int i = 0; i < redTeamNumber; i++) {
				if (!redCars [i].nv.isMine && redCars [i].IsDropped)
					redScore [i] = -1; // cars[i].Score;
				else
					redScore [i] = redCars [i].Score;
				
				redRank [i] = i;
			}

			
			int blueTemp;
			for (int i = 0; i < blueScore.Length; i++) {
				for (int j = 0; j < blueScore.Length - 1; j++) {
					if (blueScore [j] < blueScore [j + 1]) {
						blueTemp = blueScore [j + 1];
						blueScore [j + 1] = blueScore [j];
						blueScore [j] = blueTemp;
						
						blueTemp = blueRank [j + 1];
						blueRank [j + 1] = blueRank [j];
						blueRank [j] = blueTemp;
					}
				}
			}

			int redTemp;
			for (int i = 0; i < redScore.Length; i++) {
				for (int j = 0; j < redScore.Length - 1; j++) {
					if (redScore [j] < redScore [j + 1]) {
						redTemp = redScore [j + 1];
						redScore [j + 1] = redScore [j];
						redScore [j] = redTemp;
						
						redTemp = redRank [j + 1];
						redRank [j + 1] = redRank [j];
						redRank [j] = redTemp;
					}
				}
			}

			
			for (int i = 0; i < blueRows.Length; i++) {
				if (i < blueTeamNumber) {
					//ScrCarController car = ScrController.Instance.GetCar(ranks[i]);
					ScrCarController car = blueCars [blueRank [i]]; // ScrController.Instance.GetCar(ranks[i]);
					if(  car.Owner.GetTeam() != PunTeams.Team.blue ){
						return;
					}
					//if (!car.IsDropped && car.nv.isMine)

					if (car == ScrCarController.Instance) {

						playerRank = i + 1;
						
						Color c = normalColor;
						if (i < topRowsColor.Length)
							c = topRowsColor [i];
						
						blueRows [i].Activate (i + 1, car.Username, car.Kills, car.Deaths, car.Score, car.Cheats, car.IsDropped, true, c);
					} else
						blueRows [i].Activate (i + 1, car.Username, car.Kills, car.Deaths, car.Score, car.Cheats, car.IsDropped);
				}
				else
					blueRows [i].Deactivate ();
			}

			for (int i = 0; i < redRows.Length; i++) {
				if (i < redTeamNumber) {
					//ScrCarController car = ScrController.Instance.GetCar(ranks[i]);
					ScrCarController car = redCars [redRank [i]]; // ScrController.Instance.GetCar(ranks[i]);
					if(  car.Owner.GetTeam() != PunTeams.Team.red ){
						return;
					}
					//if (!car.IsDropped && car.nv.isMine)
					
					if (car == ScrCarController.Instance) {
						
						playerRank = i + 1;
						
						Color c = normalColor;
						if (i < topRowsColor.Length)
							c = topRowsColor [i];
						
						redRows [i].Activate (i + 1, car.Username, car.Kills, car.Deaths, car.Score, car.Cheats, car.IsDropped, true, c);
					} else
						redRows [i].Activate (i + 1, car.Username, car.Kills, car.Deaths, car.Score, car.Cheats, car.IsDropped);
				}
				else
					redRows [i].Deactivate ();
			}

		}

        if (autoRefresh)
            rTime = refreshTime;
        else
            rTime = -1;

        enabled = autoRefresh;

        nextButtonGameObject.SetActive(!autoRefresh);
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    float rTime;
    void Update()
    {
        if (rTime != -1)
        {
            rTime -= Time.deltaTime;
            if (rTime <= 0)
            {
                rTime = refreshTime;
                Activate(true);
            }
        }
    }

    public void OKButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();
        //GameplayUI.Instance.gameOverMenu.Activate(playerRank);

		ScrCarController.Instance.FinishingEnding ();
        enabled = false;
        Deactivate();
    }

    public void DeactivateTemporaryWindow()
    {
        if (rTime != -1)
            Deactivate();
    }

    public void ActivateTemporaryWindow()
    {
        if (rTime != -1)
            Activate(true);
    }
}