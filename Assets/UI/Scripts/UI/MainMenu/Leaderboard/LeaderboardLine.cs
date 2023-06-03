using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardLine : MonoBehaviour 
{
    public GameObject myGameObject;
    public Text rankText, usernameText, scoreText, levelText;
    public RectTransform myTransform;
    public Image backImage;

	LeagueUser currentUserInfo;

	public void Activate(LeagueUser userInfo, Color backColor, bool isMe = false)
    {
        this.currentUserInfo = userInfo;
        int level = Leveling.Instance.GetLevelForScore(userInfo.GlobalScore);
		if( userInfo.LeagueRank <= 100 ){
	        rankText.text = userInfo.LeagueRank.ToString();
		}
		else if( userInfo.LeagueRank > 100 && userInfo.LeagueRank < 500   ){
			rankText.text = " +100 ";
		}
		else if( userInfo.LeagueRank > 500 && userInfo.LeagueRank < 1000 ){
			rankText.text = " +500 ";
		}
		else {
			rankText.text = "+1000";
		}
        usernameText.text = userInfo.UserName;
        levelText.text = level.ToString();
        if(userInfo.Type == LeaderBoardType.Global)
            scoreText.text = userInfo.GlobalScore.ToString();
        else
        scoreText.text = MathHelper.GetStringWithComma(userInfo.LeagueScore);

        //backImage.color = backColor;
        if (isMe)
        {
            backImage.enabled = true;
        }
        else
        {
			backImage.enabled = false;
        }
            
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    
}

public struct OnlineUserInfo
{
    public int rank, score;
    public string username;
}
