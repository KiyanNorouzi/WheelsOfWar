using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserProfileWindow : Window 
{
    public Text usernameText, levelText, totalScoreText, scoreLevelText, deathsText, killsText, killDeathRatioText;
    public Text scoreInLevelText, battlesText, pickUpsText, firstPlaceText, distanceText;
    public Text scoreInLeagueText, rankInLeagureText;
    public Text bulletsFired, rocketsFired, minesFired;
	public Text teamGamesText, singlePlayerGamesText, teamGamesPercentText, singlePlayerGamesPercentText, leagueScore, leagueRank, leagueCategory, inActiveText;
	public Image leagueImage;

    public Slider scoreSlider, teamGamesSlider, singlePlayerGamesSlider;

    public Image renderTextureImage;

    public GameObject[] ImageCarLogo;

	public Sprite notInLeagueImage;


	public override void Activate()
    {
        Accounting.Instance.currentUser.OnLevelChanged += OnLevelChanged;
        Accounting.Instance.currentUser.OnDisplayNameChanged += OnDisplayNameChanged;

        _RefreshInfo();
        base.Activate();
        LoadPictureOnUserProfile();
        ActiveLogoOnUserProfile();
        ActiveLogoOnUserProfile();

		ActiveCurrentLeague();
    }

	void ActiveCurrentLeague()
    {
        if (LeaderdBoarSetting.Instance.curLeague != null)
        {
            leagueScore.text = LeaderdBoarSetting.Instance.myUserLeagueData.LeagueScore.ToString();
            leagueRank.text = LeaderdBoarSetting.Instance.myUserLeagueData.LeagueRank.ToString();
            leagueImage.sprite = LeaderdBoarSetting.Instance.curLeague.leagueIcon;
            leagueCategory.text = LeaderdBoarSetting.Instance.curLeague.leagueCategory.ToString();
        }
        else {
            leagueImage.sprite = notInLeagueImage;
            leagueCategory.text = "";
            inActiveText.text = "DeActive";
            leagueScore.text = "0";
            leagueRank.text = "0";
        }
	}

    public override void Deactivate()
    {
        Accounting.Instance.currentUser.OnLevelChanged -= OnLevelChanged;
        Accounting.Instance.currentUser.OnDisplayNameChanged -= OnDisplayNameChanged;

        base.Deactivate();
    }

    private void OnDisplayNameChanged()
    {
        usernameText.text = Accounting.Instance.currentUser.DisplayName;
    }

    private void OnLevelChanged()
    {
        levelText.text = string.Concat("LV ", Accounting.Instance.currentUser.Level);
    }



    void _RefreshInfo()
    {
        usernameText.text = Accounting.Instance.currentUser.DisplayName;
        levelText.text = string.Concat("LV ", Accounting.Instance.currentUser.Level);
        totalScoreText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.TotalScore);



        int levelMinScore = Leveling.Instance.GetScoreForLevel(Accounting.Instance.currentUser.Level);
        int nextLevelMinScore = Leveling.Instance.GetScoreForLevel(Accounting.Instance.currentUser.Level + 1);
        int scoreInLevel = Accounting.Instance.currentUser.TotalScore - levelMinScore;
        scoreInLevelText.text = string.Format("{0}/{1}", scoreInLevel, nextLevelMinScore);

        scoreSlider.minValue = 0;
        scoreSlider.maxValue = nextLevelMinScore;
        scoreSlider.value = scoreInLevel;



        int allPickups = Accounting.Instance.currentUser.statistics.PickupAll + Accounting.Instance.currentUser.statistics.PickUpMines +
            Accounting.Instance.currentUser.statistics.PickupRockets + Accounting.Instance.currentUser.statistics.PickupShields +
            Accounting.Instance.currentUser.statistics.PickupHealths;

        battlesText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.Battles);
        pickUpsText.text = MathHelper.GetStringWithComma(allPickups);
        firstPlaceText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.SingleMatchesFirstPlaces + Accounting.Instance.currentUser.statistics.TeamMatchesWin);
        distanceText.text = MathHelper.GetStringWithComma((int)Accounting.Instance.currentUser.statistics.DistanceCovered);

        

        killsText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.Kills);
        deathsText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.Deaths);


        float ratio = 0;
        if (Accounting.Instance.currentUser.statistics.Deaths > 0)
            ratio = Accounting.Instance.currentUser.statistics.Kills / ((float)Accounting.Instance.currentUser.statistics.Deaths);


        killDeathRatioText.text = ratio.ToString("0.00");


        scoreInLeagueText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.ScoreInLeague);
        rankInLeagureText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.RankInLeague);


        bulletsFired.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.BulletsTotal);
        minesFired.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.MinesTotal);
        rocketsFired.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.RocketsTotal);



        float percent = 0;
        int total = Accounting.Instance.currentUser.statistics.SingleMatchesOtherPlaces + Accounting.Instance.currentUser.statistics.SingleMatchesFirstPlaces;
        if (total > 0)
        {
            percent = ((float)Accounting.Instance.currentUser.statistics.SingleMatchesFirstPlaces) / total;
            percent *= 100;
        }

        singlePlayerGamesText.text = string.Format("{0}/{1}", MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.SingleMatchesFirstPlaces),
            MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.SingleMatchesOtherPlaces));
        singlePlayerGamesPercentText.text = string.Format("{0}%", (int)percent);
        singlePlayerGamesSlider.value = percent;


        
        total = (Accounting.Instance.currentUser.statistics.TeamMatchesWin + Accounting.Instance.currentUser.statistics.TeamMatchesLose);
        if (total > 0)
        {
            percent = ((float)Accounting.Instance.currentUser.statistics.TeamMatchesWin) / total;
            percent *= 100;
        }
        else
            percent = 0;

        teamGamesPercentText.text = string.Format("{0}%", (int)percent);
        teamGamesText.text = string.Format("{0}/{1}", MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.TeamMatchesWin),
            MathHelper.GetStringWithComma(Accounting.Instance.currentUser.statistics.TeamMatchesLose));
        teamGamesSlider.value = percent;
    }




    void LoadPictureOnUserProfile()
    {
        StartCoroutine(LoadOnPicture());
    }

    IEnumerator LoadOnPicture()
    {
        string url = string.Concat("file:///" , Application.persistentDataPath , "/",  Accounting.Instance.currentUser.Username , "-UserProfileImage.png");
        Debug.Log(url);
        WWW www = new WWW(url);

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D tex = www.texture;
            tex.EncodeToPNG();

            renderTextureImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2f, tex.height / 2f));
        }
    }

    void ActiveLogoOnUserProfile()
    {
        for (int i = 0; i < ImageCarLogo.Length; i++)
        {
            ImageCarLogo[i].SetActive(false);
        }

        ImageCarLogo[Accounting.Instance.currentUser.SelectedCarIndex].SetActive(true);
    }







    public void EditUsernameButton_Click()
    {
        CommonUI.Instance.headerBar.editUsernameWindow.Activate();
    }
}