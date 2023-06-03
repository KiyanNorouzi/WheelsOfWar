using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeagueTitle : MonoBehaviour {
	
	public Image leagueIcon;
	public Text leagueCategory;
	public Text leagueName;
	public Text leagueTime;

	public LeagueRowLevel[] Levels = new LeagueRowLevel[0];

	public void SetDatas( LeagueData data, float time ){
		leagueIcon.sprite = data.leagueIcon;
		leagueName.text = data.LeagueID.ToString ();
		leagueCategory.text = data.leagueCategory.ToString();
		leagueTime.text = time.ToString();
		Levels[0].reward.text = data.LeagueRewardIII.ToString();
		Levels[1].reward.text = data.LeagueRewardII.ToString();
		Levels[2].reward.text = data.LeagueRewardI.ToString();
		switch( data.leagueCategory ){
		case LeagueCategory.I:
			Levels[0].highLightImage.enabled = false;
			Levels[1].highLightImage.enabled = false;
			Levels[2].claim.SetActive(false);
			break;
		case LeagueCategory.II:
			Levels[0].highLightImage.enabled = false;
			Levels[2].highLightImage.enabled = false;
			Levels[2].claim.SetActive(false);
			Levels[1].claim.SetActive(false);
			break;
		case LeagueCategory.III:
			Levels[2].highLightImage.enabled = false;
			Levels[1].highLightImage.enabled = false;
			Levels[2].claim.SetActive(false);
			Levels[1].claim.SetActive(false);
			Levels[0].claim.SetActive(false);
			break;
		}
	}
}
[System.Serializable]
public class LeagueRowLevel{
	public Image highLightImage;
	public GameObject levelGameObject, claim;
	public Text levelTitle, reward;
}
