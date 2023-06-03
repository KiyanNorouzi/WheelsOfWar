using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenu : Window
{
    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip emblem;
        public AudioClip countLoop;
        public AudioClip continueButtonAppear;

        public float continueButtonPitch;
    }


    public AudioStruct audioPlayer;
    public GameObject badgesGameObject;
    public GameObject[] singleBadges, teamBadges;
	public Text rankMultiplyerText, rankText, totalScoreText, totalMoneyText, battleRewardText, rankRewardText, leagueRewardText, vipRewardText, otherRewardText;
    public GameObject continueButtonGameObject;
    public Slider xpSlider;
    public Text levelText, xpText;
    public GameObject skipButtonGameObject;
	public Color blueTeamColorBadges;
	public Color redTeamColorBadges;


    bool isTeamMatch;
    int rank, totalScore, totalMoney, battleReward, rankReward, leagueReward, vipReward, otherReward;
    int sliderStart, sliderEnd;




    public void SkipButton_Click()
    {
        Time.timeScale++;
    }

    public void ContinueButton_Click()
    {
		SceneManager.LoadScene(Scenes.MainMenu);
		Deactivate();
    }

	
    public void Activate(int totalScore)
    {

    }

	public void CheckFindATeamWinner(){
		if( ScrController.Instance.GetBlueTeamKill() > ScrController.Instance.GetRedTeamKill() ){

		}
		else if( ScrController.Instance.GetRedTeamKill() > ScrController.Instance.GetBlueTeamKill() ){
			ScrController.Instance.SetRedTeamWon();
		}
		else if( ScrController.Instance.GetBlueTeamKill() == ScrController.Instance.GetRedTeamKill()  ){
			if( ScrController.Instance.GetRedTeamScores() > ScrController.Instance.GetBlueTeamScores() ){
				ScrController.Instance.SetRedTeamWon();
			}
			else if( ScrController.Instance.GetRedTeamScores() < ScrController.Instance.GetBlueTeamScores() ){
				ScrController.Instance.SetBlueTeamWon();
			}
			else{
				int i = Random.Range( 0,2 );
				if( i == 0 ){
					ScrController.Instance.SetBlueTeamWon();
				}
				else{
					ScrController.Instance.SetRedTeamWon();
				}
			}
		}
	}

    public void Activate(bool isTeamMatch, int rank, int totalScore, int battleReward, int rankReward, int leagueReward, int vipReward, int otherReward)
    {
		this.isTeamMatch = isTeamMatch;
        this.rank = rank;
        this.totalScore = totalScore;
        this.battleReward = battleReward;
        this.rankReward = rankReward;
        this.leagueReward = leagueReward;
        this.vipReward = vipReward;
        this.otherReward = otherReward;


		if( GameplayDefaultSettings.Instance.isTeamMatch ){
			CheckFindATeamWinner ();
		}



        Accounting.Instance.currentUser.statistics.AddBattle();

        if (EnvironmentController.Instance.map != Maps.Tutorial)
        {
            if (isTeamMatch)
            {
                if (rank == 1)
                    Accounting.Instance.currentUser.statistics.AddTeamMatchWin();
                else
                    Accounting.Instance.currentUser.statistics.AddTeamMatchLose();
            }
            else
            {
                if (rank == 1)
                    Accounting.Instance.currentUser.statistics.AddSingleMatchFirstPlace();
                else
                    Accounting.Instance.currentUser.statistics.AddSingleMatchOtherPlaces();
            }

            QuestManager.Instance.PlayedMatch(EnvironmentController.Instance.map, isTeamMatch);
            QuestManager.Instance.PointsEarned(totalScore, isTeamMatch);
            QuestManager.Instance.UsedBoosters(GameplayUI.Instance.boosterPanel.EquippedPackages.Count);

            if (rank == 1 && !isTeamMatch)
                QuestManager.Instance.EarnedFirstPlace();
        }

        QuestManager.Instance.NewGameStarted();


        if (EnvironmentController.Instance.map != Maps.Tutorial)
        {
            PriceStructure boostersPrice = new PriceStructure();

            for (int i = 0; i < GameplayUI.Instance.boosterPanel.EquippedPackages.Count; i++)
            {
                int packageIndex = (int)GameplayUI.Instance.boosterPanel.EquippedPackages[i];
                boostersPrice += StoreData.Instance.packages[packageIndex].price;
            }


            Accounting.Instance.currentUser.AutoSync = false;
            Accounting.Instance.currentUser.Golds -= boostersPrice.Golds;
            Accounting.Instance.currentUser.Bills -= boostersPrice.Bills;
        }
//		this.sliderStart = Accounting.Instance.currentUser.TotalScore;
//		xpSlider.value = Accounting.Instance.currentUser.TotalScore;

		leagueReward = LeaderdBoarSetting.Instance.curLeague.reward;
		this.leagueReward = leagueReward;

        totalMoney = battleReward + leagueReward + vipReward + rankReward + otherReward;

		int levelMinScore = Leveling.Instance.GetScoreForLevel(Accounting.Instance.currentUser.Level);
		int nextLevelMinScore = Leveling.Instance.GetScoreForLevel(Accounting.Instance.currentUser.Level + 1);
		int scoreInLevel = Accounting.Instance.currentUser.TotalScore - levelMinScore;

		sliderEnd = scoreInLevel + totalScore;

		xpSlider.minValue = 0;
		xpSlider.maxValue = nextLevelMinScore;
		xpSlider.value = Accounting.Instance.currentUser.TotalScore;
//		xpText.text = Accounting.Instance.currentUser.TotalScore.ToString ();
		xpText.text = string.Format("{0}/{1}", xpSlider.value, xpSlider.maxValue - xpSlider.minValue);
		levelText.text = Accounting.Instance.currentUser.Level.ToString();
		
		continueButtonGameObject.SetActive(false);

        if (EnvironmentController.Instance.map != Maps.Tutorial)
        {
            Accounting.Instance.currentUser.AutoSync = true;
            Accounting.Instance.currentUser.Gas -= EnvironmentSpecificSettings.Instance.settings[((int)EnvironmentController.Instance.map) - 1].gasConsume;
            Accounting.Instance.currentUser.TotalScore += totalScore;
            Accounting.Instance.currentUser.Bills += totalMoney;
        }


        Accounting.Instance.currentUser.SyncStatistics();

        badgesGameObject.SetActive(false);

        for (int i = 0; i < teamBadges.Length; i++)
                teamBadges[i].SetActive(false);
        
        for (int i = 0; i < singleBadges.Length; i++)
                singleBadges[i].SetActive(false);


        rankMultiplyerText.text = totalScoreText.text = totalMoneyText.text = battleRewardText.text = 
        rankRewardText.text = leagueRewardText.text = vipRewardText.text = otherRewardText.text = "0";


		if (!isTeamMatch) {
			rankText.text = "Rank Reward";
			switch (rank) {
			case 1:
				rankReward = (int)(battleReward * ScoreSettings.Instance.scoreMultiplyers [0]);
				this.rankReward = rankReward;
				rankMultiplyerText.text = "2X";
				break;
			case 2:
				rankReward = (int)(battleReward * ScoreSettings.Instance.scoreMultiplyers [1]);
				this.rankReward = rankReward;
				rankMultiplyerText.text = "1.5X";
				break;
			case 3:
				rankReward = (int)(battleReward * ScoreSettings.Instance.scoreMultiplyers [2]);
				this.rankReward = rankReward;
				rankMultiplyerText.text = "1X"; 
				break;
			default:
				battleReward = 0;
				rankMultiplyerText.text = "0X";
				break;
			}
		}
		else {
			rankText.text = "Team Reward";
			if( ScrCarController.Instance.Owner.GetTeam() == PunTeams.Team.blue ){
				if( ScrController.Instance.blueTeamWon){
					rank = 1;
					rankReward = GameplayDefaultSettings.Instance.rewardOfWinnerTeam;
					this.rankReward = rankReward;
				}
				else{
					rank = 2;
					rankReward = 0;
					this.rankReward = rankReward;
				}
			}
			else if(ScrCarController.Instance.Owner.GetTeam() == PunTeams.Team.red){
				if( ScrController.Instance.redTeamWon){
					rank = 1;
					rankReward = GameplayDefaultSettings.Instance.rewardOfWinnerTeam;
					this.rankReward = rankReward;
				}
				else{
					rank = 2;
					rankReward = 0;
					this.rankReward = rankReward;
				}
			}
		}

		if( Accounting.Instance.currentUser.HasVIPPackage( VIPActionType.ExtraRewardBills ) ){
		float priceMultiplyer = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.ExtraRewardBills);
			float viprew = (rankReward + battleReward) * priceMultiplyer;
			this.vipReward = (int)viprew;
		}
        duration = 0.5f;
        time = 0;

        turn = 0;
        base.Activate();
        StartCoroutine(Process(isTeamMatch, rank, totalScore, battleReward, rankReward, leagueReward, vipReward, otherReward));
    }

    IEnumerator Process(bool isTeamMatch, int rank, int totalScore, int battleReward, int rankReward, int leagueReward, int vipReward, int otherReward)
    {
        yield return new WaitForSeconds(1);

        if (isTeamMatch) {
			if (ScrCarController.Instance.Owner.GetTeam () == PunTeams.Team.red) {
            	teamBadges[Mathf.Clamp(rank - 1,0, teamBadges.Length - 1)].SetActive(true);
            	teamBadges[Mathf.Clamp(rank - 1,0, teamBadges.Length - 1)].GetComponent<Image>().color = redTeamColorBadges;
			}
			else if(ScrCarController.Instance.Owner.GetTeam () == PunTeams.Team.blue){
            	teamBadges[Mathf.Clamp(rank - 1,0, teamBadges.Length - 1)].SetActive(true);
				teamBadges[Mathf.Clamp(rank - 1,0, teamBadges.Length - 1)].GetComponent<Image>().color = blueTeamColorBadges;
			}
		}
        else
            singleBadges[Mathf.Clamp(rank - 1, 0, singleBadges.Length - 1)].SetActive(true);

        badgesGameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        
        turn = 1;
        time = 0;
        isCounting = true;

        yield return new WaitForSeconds(0.75f);

        turn = 2;
        time = 0;
        isCounting = true;

        yield return new WaitForSeconds(0.75f);

        turn = 3;
        time = 0;
        isCounting = true;

        yield return new WaitForSeconds(0.75f);

        turn = 4;
        time = 0;
        duration = 1.25f;
        isCounting = true;

        yield return new WaitForSeconds(1.5f);

        skipButtonGameObject.SetActive(false);
        continueButtonGameObject.SetActive(true);

        Time.timeScale = 1;
    }


    float time, flow, duration;
    int turn;
    bool isCounting;


    void Update()
    {
        if (isCounting)
        {
            time += Time.deltaTime;
            flow = time / duration;

            if (flow >= 1)
            {
                flow= 1;
                isCounting = false;
            }


            switch (turn)
            {
                case 1:
                    totalScoreText.text = (flow * totalScore).ToString("0");
                    break;

                case 2:
                    battleRewardText.text = (flow * battleReward).ToString("0");
                    rankRewardText.text = (flow * rankReward).ToString("0");
                    leagueRewardText.text = (flow * leagueReward).ToString("0");
                    vipRewardText.text = (flow * vipReward).ToString("0");
                    otherRewardText.text = (flow * otherReward).ToString("0");
                    break;

                case 3:
                    totalMoneyText.text = (flow * totalMoney).ToString("0");
                    break;

                case 4:
				int startSlider = Accounting.Instance.currentUser.TotalScore;

				int amount = startSlider + ((int)((sliderEnd - sliderStart) * flow));
//              int amount = sliderStart + ((int)((sliderEnd - sliderStart) * flow));
                xpSlider.value = amount;

                xpText.text = string.Format("{0}/{1}", xpSlider.value, xpSlider.maxValue - xpSlider.minValue);
                break;
            }
        }
    }
}