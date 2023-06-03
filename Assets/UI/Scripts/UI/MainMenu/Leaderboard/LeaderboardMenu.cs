//using UnityEngine;
//using System.Collections;
//
//public class LeaderboardMenu : MonoBehaviour 
//{
//    public GameObject myGameObject, dataGameObject, myRankDataGameObject, loadingGameObject, championsGameObject;
//    public int linesCount;
//    public GameObject linePrefab;
//    public LeaderboardLine sampleLine, mySampleLine;
//    public Color[] linesColors;
//    public Color myLineColor;
//    public LeaderboardData leaderboardData;
//    public GameObject[] tabsContentGameObjects;
//    public LeaderboardTabs tabs;
//    public GameObject showMoreButtonGameObject, loadingMoreDataGameObject, myPlaceGameObject;
//	public RectTransform showMoreSectionTransform, contentTransform/*, myRankContentTransform*/;
//    public Animator myAnimator;
//    public UnityEngine.UI.Text[] monthlyTexts, weeklyTexts;
//
//
//    float defaultContentHeight, defaultMyContentHeight;
//
//
//    int currentCount;
//    LeaderboardLine[] lines, myLines;
//    LeaderboardLine myLine;
//
//    void GenerateSlots(int slotsNeeded)
//    {
//        if (lines != null && lines.Length >= slotsNeeded)
//            return;
//
//
//        LeaderboardLine[] oldLines = lines;
//        if (oldLines == null)
//            oldLines = new LeaderboardLine[0];
//
//        lines = new LeaderboardLine[slotsNeeded];
//        for (int i = 0; i < slotsNeeded; i++)
//        {
//            if (i < oldLines.Length)
//                lines[i] = oldLines[i];
//            else
//            {
//                if (i == 0)
//                    lines[i] = sampleLine;
//                else
//                {
//                    lines[i] = ((GameObject)Instantiate(linePrefab)).GetComponent<LeaderboardLine>();
//                    lines[i].myTransform.SetParent(lines[0].myTransform.parent);
//                    lines[i].myTransform.anchoredPosition = new Vector2(0, -16 - (i * lines[0].myTransform.rect.height));
//                    lines[i].myTransform.localScale = Vector3.one;
//                    lines[i].myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lines[0].myTransform.rect.width);
//                }
//            }
//
//            lines[i].name = string.Concat("Leaderboard line ", i);
//            lines[i].Deactivate();
//        }
//
//
//
//        myLine = ((GameObject)Instantiate(linePrefab)).GetComponent<LeaderboardLine>();
//
//        myLine.myTransform.SetParent(lines[0].myTransform.parent);
//        myLine.myTransform.anchoredPosition = new Vector2(0, -16 - (slotsNeeded * lines[0].myTransform.rect.height) - 75);
//        myLine.myTransform.localScale = Vector3.one;
//        myLine.myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lines[0].myTransform.rect.width);
//
//        myLine.name = "My Leaderboard line";
//        myLine.Deactivate();
//
//    }
//
//    private void GenerateSlotsMyRank(int slotsNeeded)
//    {
//        if (myLines != null && myLines.Length >= slotsNeeded)
//            return;
//
//        LeaderboardLine[] oldLines = myLines;
//        if (oldLines == null)
//            oldLines = new LeaderboardLine[0];
//
//        myLines = new LeaderboardLine[slotsNeeded];
//        for (int i = 0; i < slotsNeeded; i++)
//        {
//            if (i < oldLines.Length)
//                myLines[i] = oldLines[i];
//            else
//            {
//                if (i == 0)
//                    myLines[i] = mySampleLine;
//                else
//                {
//                    myLines[i] = ((GameObject)Instantiate(linePrefab)).GetComponent<LeaderboardLine>();
//                    myLines[i].myTransform.SetParent(myLines[0].myTransform.parent);
//                    myLines[i].myTransform.anchoredPosition = new Vector2(0, -16 - (i * myLines[0].myTransform.rect.height));
//                    myLines[i].myTransform.localScale = Vector3.one;
//                    myLines[i].myTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, myLines[0].myTransform.rect.width);
//                }
//            }
//
//            myLines[i].name = string.Concat("My Leaderboard line ", i);
//            myLines[i].Deactivate();
//        }
//    }
//
//
//
//    public void Activate()
//    {
//        if (defaultContentHeight == 0)
//        {
//            defaultContentHeight = contentTransform.rect.height;
////            defaultMyContentHeight = myRankContentTransform.rect.height;
//        }
//
//        currentCount = linesCount;
//
//        dataGameObject.SetActive(false);
//        loadingGameObject.SetActive(true);
//
//        myGameObject.SetActive(true);
//
//        currentTabIndex = -1;
//        Tab_Click(0);
//    }
//
//    void _DownloadDataDone(string text)
//    {
//        if (!myGameObject.activeInHierarchy)
//            return;
//
//        if (text == "ok")
//        {
//            GenerateSlots(leaderboardData.usersInfo.Length);
//            int lastItemIndex = 0;
//
//
//            OnlineUserInfo[] usersInfo = null;
//            switch (currentLeaderboardType)
//            {
//                case LeaderboardType.League:
//                    usersInfo = leaderboardData.usersInfo;
//                    break;
//                case LeaderboardType.Monthly:
//                    usersInfo = leaderboardData.monthlyUsersInfo;
//                    break;
//                case LeaderboardType.Global:
//                    usersInfo = leaderboardData.weeklyUsersInfo;
//                    break;
//            }
//
//            if (usersInfo == null || usersInfo.Length == 0)
//                return;
//
//            for (int i = 0; i < lines.Length; i++)
//            {
//                if (i >= usersInfo.Length || (currentLeaderboardType != LeaderboardType.League && i >= usersInfo.Length - 1))
//                    lines[i].Deactivate();
//                else
//                {
//                    lastItemIndex = i;
//
//                    string myUsername = "";
//                    if (Application.isEditor)
//                        myUsername = "miladzx";
//                    else
//                        myUsername = Data.UserName.ToLower();
//
//                    if (usersInfo[i].username.ToLower() == myUsername)
//                        lines[i].Activate(usersInfo[i], myLineColor);
//                    else
//                        lines[i].Activate(usersInfo[i], linesColors[i % linesColors.Length]);
//
//                    //lines[i].Activate(leaderboardData.usersInfo[i], linesColors[i % linesColors.Length]);
//                }
//            }
//
//
//            //Debug.Log("my place in " + currentLeaderboardType + " is " + usersInfo[usersInfo.Length - 1].score + "[" + usersInfo[usersInfo.Length - 1].username + "]");
//
//            float lastY = lines[lastItemIndex].myTransform.rect.yMin + lines[lastItemIndex].myTransform.anchoredPosition.y;
//            contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(defaultContentHeight, Mathf.Abs(lastY) + 130));
//            showMoreSectionTransform.anchoredPosition = new Vector2(0, lastY - 60);
//
//
//
//            loadingMoreDataGameObject.SetActive(false);
//            if (currentLeaderboardType == LeaderboardType.Global)
//            {
//                myPlaceGameObject.SetActive(false);
//                showMoreButtonGameObject.SetActive(true);
//
//                myLine.Deactivate();
//            }
//            else
//            {
//                myPlaceGameObject.SetActive(true);
//                showMoreButtonGameObject.SetActive(false);
//
//
//                OnlineUserInfo myInfo = new OnlineUserInfo();
//                myInfo.username =usersInfo[usersInfo.Length - 1].username;
//                myInfo.score = Accounting.Instance.currentUser.TotalScore;
//                myInfo.rank = usersInfo[usersInfo.Length - 1].score;
//
//                myLine.myTransform.anchoredPosition = new Vector2(0, -16 - (10 * lines[0].myTransform.rect.height) - 75);
//
//                myLine.Activate(myInfo, linesColors[lines.Length % linesColors.Length], true);
//            }
//            
//
//            dataGameObject.SetActive(true);
//            loadingGameObject.SetActive(false);
//        }
//    }
//
//    void _DownloadMyRankDone(string text)
//    {
//        if (!myGameObject.activeInHierarchy)
//            return;
//
//        if (text == "ok")
//        {
//            GenerateSlotsMyRank(leaderboardData.myNeighboursInfo.Length);
//
//            int lastItemIndex = 0;
//            for (int i = 0; i < myLines.Length; i++)
//            {
//                if (i >= leaderboardData.myNeighboursInfo.Length)
//                    myLines[i].Deactivate();
//                else
//                {
//                    lastItemIndex = i;
//
//                    if (leaderboardData.myNeighboursInfo[i].username.ToLower() == Data.UserName.ToLower())
//                        myLines[i].Activate(leaderboardData.myNeighboursInfo[i], myLineColor, true);
//                    else
//                        myLines[i].Activate(leaderboardData.myNeighboursInfo[i], linesColors[i % linesColors.Length]);
//                }
//            }
//
//            float lastY = myLines[lastItemIndex].myTransform.rect.yMin + myLines[lastItemIndex].myTransform.anchoredPosition.y;
////            myRankContentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(defaultMyContentHeight, Mathf.Abs(lastY)));
//
//            myRankDataGameObject.SetActive(true);
//            loadingGameObject.SetActive(false);
//        }
//    }
//
//    void _ChampionsDownloaded(string text)
//    {
//        if (!myGameObject.activeInHierarchy)
//            return;
//
//        text = text.ToLower();
//        if (text.StartsWith("ok"))
//        {
//            for (int i = 0; i < Mathf.Min(leaderboardData.monthChampions.Length, monthlyTexts.Length); i++)
//                monthlyTexts[i].text = leaderboardData.monthChampions[i];
//
//            for (int i = 0; i < Mathf.Min(leaderboardData.weekChampions.Length, weeklyTexts.Length); i++)
//                weeklyTexts[i].text = leaderboardData.weekChampions[i];
//
//            championsGameObject.SetActive(true);
//            loadingGameObject.SetActive(false);
//        }
//    }
//
//    LeaderboardType currentLeaderboardType;
//
//    int currentTabIndex = -1;
//    public void Tab_Click(int tabIndex)
//    {
//        if (tabIndex != currentTabIndex)
//        {
//            this.currentTabIndex = tabIndex;
////            for (int i = 0; i < tabsContentGameObjects.Length; i++)
////                tabsContentGameObjects[i].SetActive(i == tabIndex);
//
//            if (tabIndex == 0 || tabIndex == 2)
//                tabsContentGameObjects[0].SetActive(true);
//
//            tabs.ItemSelected(tabIndex);
//
//            dataGameObject.SetActive(false);
////            myRankDataGameObject.SetActive(false);
////            championsGameObject.SetActive(false);
//
//            loadingGameObject.SetActive(true);
//
//            switch (tabIndex)
//            {
//                case 0:
//                    currentLeaderboardType = LeaderboardType.League;
//                    leaderboardData.DownloadLeaderboard(LeaderboardType.League, currentCount, _DownloadDataDone);
//                    break;
//                case 1:
//                    leaderboardData.DownloadMyRank(Data.UserName, _DownloadMyRankDone);
//                    break;
//                case 2:
//                    currentLeaderboardType = LeaderboardType.Global;
//                    leaderboardData.DownloadLeaderboard(LeaderboardType.Global, 10, _DownloadDataDone);
//                    break;
//                case 3:
//                    currentLeaderboardType = LeaderboardType.Monthly;
//                     leaderboardData.DownloadLeaderboard(LeaderboardType.Monthly, 10, _DownloadDataDone);
//                    break;
//                case 4:
//                    leaderboardData.GetChampionsList( _ChampionsDownloaded);
//                    break;
//            }
//        }
//    }
//
//    public void CloseButton_Click()
//    {
//        myAnimator.SetTrigger("out");
//        //Deactivate();
//    }
//
//    void _CloseAnimDone()
//    {
//        Deactivate();
//    }
//
//    public void Deactivate()
//    {
//        myGameObject.SetActive(false);
//    }
//
//    public void ShowMoreButton_Click()
//    {
//        showMoreButtonGameObject.SetActive(false);
//        loadingMoreDataGameObject.SetActive(true);
//
//        currentCount += linesCount;
//        
//
//        leaderboardData.DownloadLeaderboard(currentLeaderboardType, currentCount, _DownloadDataDone);
//    }
//
//    public void MoreChampions_Click()
//    {
//        Application.OpenURL(GeneralSettings.Instance.championsUrl);
//    }
//}