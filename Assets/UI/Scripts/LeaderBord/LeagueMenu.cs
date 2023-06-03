using UnityEngine;
using System.Collections;

public class LeagueMenu : MonoBehaviour {

	private static LeagueMenu instance;
	public static LeagueMenu Instance{
		get{ return instance; }
	}
	void Awake(){
		instance = this;
	}

	private bool dataDownloaded;
	public bool DataDownloaded{
		get{ return dataDownloaded; }
		set{ dataDownloaded = value; }
	}
	private bool rowsSetuped;
	public bool RowsSetuped{
		get{ return rowsSetuped; }
		set{ rowsSetuped = value; }
	}

    public GameObject notInLeageImageObject;


	public System.Collections.Generic.List<LeaderboardLine> RowsList = new System.Collections.Generic.List<LeaderboardLine> ();
	public LeagueTitle leagueTiteRow;
	public LeaderboardLine myBoard;
	public Transform rowsParent;

	public GameObject rowObj;
	public GameObject myGameObject;
	public GameObject myButton;
	public GameObject loadingText;
	public GameObject[] explaneObj;
	
	void Start(){
		loadingText.SetActive (true);
		myButton.SetActive (false);
		SetupTitleLeagueRow ();
	}

	public void OnClickButton(){

		if (!DataDownloaded) 
			GetDataFromUrl ();
		else 
			if( !rowsSetuped )
				SetupOtherRows();

		myButton.SetActive (false);
		//Get
	}

	void GetDataFromUrl(){
		Accounting.Instance.GetMyLeagueTeammateDatas (Accounting.Instance.currentUser.Id, LeaderdBoarSetting.Instance.GetUserDataFromText, SetupOtherRows, SetupOtherRowsFailed);
	}

	public void OnExplaneButtonClick(){
		ActiveExplane ();
	}

	public void ActiveExplane(){
		for( int cnt = 0; cnt < explaneObj.Length; cnt++ ){
			explaneObj[cnt].SetActive( true );
		}	
	}

	public void SetupOtherRows(){
		SetupMyRows ( LeaderdBoarSetting.Instance.myUserLeagueData );
		myButton.SetActive (false);
		loadingText.SetActive (true);
		DataDownloaded = true;
		StartCoroutine ( _SetupOtherRow() );
	}

	IEnumerator _SetupOtherRow(){
		yield return new WaitForSeconds (1);
		if (LeaderBoardUIMenu.Instance.userLeagueList.Count > 0) {	
			for (int cnt = 1; cnt < LeaderBoardUIMenu.Instance.userLeagueList.Count; cnt++) {
				if( cnt > 99 )
					break;
				if( LeaderBoardUIMenu.Instance.userLeagueList[cnt].LeagueRank == 0 ){
					continue;
				}

				if (LeaderBoardUIMenu.Instance.userLeagueList [cnt].IsMine == true) {
					LeaderBoardUIMenu.Instance.SetupOtherRowsLins (LeaderBoardUIMenu.Instance.userLeagueList [cnt], true, rowsParent, LeaderMenuType.League);
				} else {
					LeaderBoardUIMenu.Instance.SetupOtherRowsLins (LeaderBoardUIMenu.Instance.userLeagueList [cnt], false, rowsParent, LeaderMenuType.League);
				}
			}
			myButton.SetActive (false);
			loadingText.SetActive (false);
			rowsSetuped = true;
		}
		else {
			myButton.SetActive (true);
		}

	}

	public void SetupOtherRowsFailed(){
		myButton.SetActive (true);
		loadingText.SetActive (false);
	}

	public void SetupTitleLeagueRow(){
        if (LeaderdBoarSetting.Instance.curLeague != null)
            leagueTiteRow.SetDatas(LeaderdBoarSetting.Instance.curLeague, 11);
        else {
            notInLeageImageObject.SetActive(true);
        }
	}

	void SetupMyRows( LeagueUser user ){
		myBoard.Activate ( user, Color.white, true  );
	}

	public void Activate(){
		myGameObject.SetActive ( true );
		if( LeaderBoardUIMenu.Instance.myGameObject.activeSelf == true) {
			OnClickButton ();
		}
	}
	public void DeActiove(){
		CancelInvoke ("_SetupOtherRow");
		myGameObject.SetActive ( false );
	}
}
