using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class LeaderBoardUIMenu : MonoBehaviour {

	private static LeaderBoardUIMenu instance;
	public static LeaderBoardUIMenu Instance{
		get{ return instance; }
	}
	void Awake(){
		instance = this;
		Deactivate ();
	}
	
	public List<LeagueUser> userLeagueList = new List<LeagueUser> ();
	public List<LeagueUser> userMonthlyList = new List<LeagueUser> ();
	public List<LeagueUser> userGlobalList = new List<LeagueUser> ();

    //public Animator myAnimator;
	public GameObject myGameObject;

	public LeagueMenu leagueScript;
	public MonthlyMenu monthlyScript;
	public GlobalMenu globalScript;
	
	public Button leagueButton;
	public Button monthlyButton;
	public Button globalButton;

	public Color intabColor;
	public Color isnttabColor;

	public GameObject rowobj;
	public GameObject[] explaneObj;

//	public LeagueData curLeague;

	public GameObject leaguePanel, leagueRequest , monthlyPanel, golbalPanel, loadingPanel;


	void Start(){
		LeagueActioned ();
		loadingPanel.SetActive ( false );
		DeactiveExplane ();
	}

	private LeaderMenuType typeMenu;
	public void OnButton_Clock( int index ){
		switch( index ){
		case 0:
			typeMenu = LeaderMenuType.League;
			break;
		case 1:
			typeMenu = LeaderMenuType.Monthly;
			break;
		case 2:
			typeMenu = LeaderMenuType.Global;
			break;
		}
		switch( typeMenu ){
		case LeaderMenuType.League:
			LeagueActioned();
			break;
		case LeaderMenuType.Monthly:
			MonthlyActioned();
			break;
		case LeaderMenuType.Global:
			GlobalActioned();
			break;
		}
	}



	public void SetupOtherRowsLins( LeagueUser user, bool isme, Transform rowsParent, LeaderMenuType type  ){
		LeaderboardLine row =  ((GameObject)Instantiate(rowobj)).GetComponent<LeaderboardLine>();
		row.transform.SetParent ( rowsParent );
		RectTransform rect = row.GetComponent<RectTransform>();
		rect.anchoredPosition = new Vector2 ( 0,0 );
		rect.localScale = new Vector3 ( 1,1,1 );
		switch( type ){
		case LeaderMenuType.League:
			if (leagueScript.RowsList.Count > 0) {
				rect.anchoredPosition = leagueScript.RowsList [leagueScript.RowsList.Count - 1].GetComponent<RectTransform> ().anchoredPosition + new Vector2 (0, -34);
				rowsParent.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Mathf.Abs (leagueScript.RowsList [leagueScript.RowsList.Count - 1].transform.GetComponent<RectTransform> ().anchoredPosition.y) + 150);
			}
			row.Activate ( user, Color.black, isme );
			leagueScript.RowsList.Add ( row );
			break;
		case LeaderMenuType.Monthly:
			if (monthlyScript.RowsList.Count > 0) {
				rect.anchoredPosition = monthlyScript.RowsList [monthlyScript.RowsList.Count - 1].GetComponent<RectTransform> ().anchoredPosition + new Vector2 (0, -34);
				rowsParent.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Mathf.Abs (monthlyScript.RowsList [monthlyScript.RowsList.Count - 1].transform.GetComponent<RectTransform> ().anchoredPosition.y) + 150);
			}
			row.Activate ( user, Color.black, isme );
			monthlyScript.RowsList.Add ( row );
			break;
		case LeaderMenuType.Global:
			if (globalScript.RowsList.Count > 0) {
				rect.anchoredPosition = globalScript.RowsList [globalScript.RowsList.Count - 1].GetComponent<RectTransform> ().anchoredPosition + new Vector2 (0, -34);
				rowsParent.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, Mathf.Abs (globalScript.RowsList [globalScript.RowsList.Count - 1].transform.GetComponent<RectTransform> ().anchoredPosition.y) + 150);
			}
			row.Activate ( user, Color.black, isme );
			globalScript.RowsList.Add ( row );
			break;
		}
	}

	public void DeactiveExplane(){
		for( int cnt = 0; cnt < explaneObj.Length; cnt++ ){
			explaneObj[cnt].SetActive(false);
		}
	}


	public void Deactivate()
	{
		myGameObject.SetActive(false);
	}

	public void LeagueActioned(){
		leagueScript.Activate ();
		monthlyScript.DeActiove ();
		globalScript.DeActiove ();
		globalButton.image.color = isnttabColor;
		monthlyButton.image.color = isnttabColor;
		leagueButton.image.color = intabColor;
	}
	void MonthlyActioned(){
		monthlyScript.Activate ();
		leagueScript.DeActiove ();
		globalScript.DeActiove ();
		globalButton.image.color = isnttabColor;
		monthlyButton.image.color = intabColor;
		leagueButton.image.color = isnttabColor;
	}
	void GlobalActioned(){
		globalScript.Activate ();
		monthlyScript.DeActiove ();
		leagueScript.DeActiove ();
		globalButton.image.color = intabColor;
		monthlyButton.image.color = isnttabColor;
		leagueButton.image.color = isnttabColor;	}
}
public enum LeaderMenuType{
	League,
	Monthly,
	Global
}
