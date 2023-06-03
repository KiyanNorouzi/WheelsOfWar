using UnityEngine;
using System.Collections;

public class GlobalMenu : MonoBehaviour {

	public GameObject myGameObject;

	public System.Collections.Generic.List<LeaderboardLine> RowsList = new System.Collections.Generic.List<LeaderboardLine> ();

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

	public LeaderboardLine myBoard;
	
	public Transform rowsParent;
	
	public GameObject myButton;
	public GameObject loadingText;

	
	void Start(){
		loadingText.SetActive (true);
		myButton.SetActive (false);
	}

	public void OnClickButton(){
		if (!dataDownloaded)
			GetDataFromUrl ();
		else
			if (!rowsSetuped)
				SetupOtherRows ();


		myButton.SetActive (false);
	}

	void GetDataFromUrl(){
		Accounting.Instance.GetGlobalUserData ( Accounting.Instance.currentUser.Id, LeaderdBoarSetting.Instance.GetUserDataFromText, SetupOtherRows, SetupOtherRowsFailed );
		loadingText.SetActive (true);
	}

	public void SetupRows( LeagueUser[] infos ){
		for( int cnt = 0; cnt < infos.Length; cnt++ ){
			LeaderBoardUIMenu.Instance.SetupOtherRowsLins ( infos[cnt], false, rowsParent, LeaderMenuType.Global );
		}
		myButton.SetActive (false);
		loadingText.SetActive (false);
	}

	public void SetupOtherRows(){
		SetupMyRows ( LeaderdBoarSetting.Instance.myUserGlobalData );
		dataDownloaded = true;
		myButton.SetActive (false);
		loadingText.SetActive (true);
		StartCoroutine ( _SetupOtherRow() );
	}
	
	IEnumerator _SetupOtherRow(){
		yield return new WaitForSeconds (1);
		if (LeaderBoardUIMenu.Instance.userGlobalList.Count > 0) {	
			for (int cnt = 0; cnt < LeaderBoardUIMenu.Instance.userGlobalList.Count; cnt++) {
				if( cnt > 99 )
					break;

				if (LeaderBoardUIMenu.Instance.userGlobalList [cnt].IsMine == true) {
					LeaderBoardUIMenu.Instance.SetupOtherRowsLins (LeaderBoardUIMenu.Instance.userGlobalList [cnt], true, rowsParent, LeaderMenuType.Global);
				} else {
					LeaderBoardUIMenu.Instance.SetupOtherRowsLins (LeaderBoardUIMenu.Instance.userGlobalList [cnt], false, rowsParent, LeaderMenuType.Global);
				}
			}
			rowsSetuped = true;
			myButton.SetActive (false);
			loadingText.SetActive (false);
		}
		else {
			myButton.SetActive (true);
		}
	}

	
	void SetupMyRows( LeagueUser user ){
		myBoard.Activate ( user, Color.white, true  );
	}

	public void SetupOtherRowsFailed(){
		myButton.SetActive (true);
		loadingText.SetActive (false);
	}

	public void Activate(){
		myGameObject.SetActive ( true );
		if( LeaderBoardUIMenu.Instance.myGameObject.activeSelf == true ){
			OnClickButton ();
		}
	}
	public void DeActiove(){
		CancelInvoke ("_SetupOtherRow");
		myGameObject.SetActive ( false );
	}
}
