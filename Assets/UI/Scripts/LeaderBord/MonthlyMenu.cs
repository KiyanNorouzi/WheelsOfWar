using UnityEngine;
using System.Collections;

public class MonthlyMenu : MonoBehaviour {

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

	public GameObject[] explaneObj;
	
	void Start(){
		loadingText.SetActive (true);
		myButton.SetActive (false);
	}


	public void OnExplaneButtonClick(){
		ActiveExplane ();
	}
	
	public void ActiveExplane(){
		for( int cnt = 0; cnt < explaneObj.Length; cnt++ ){
			explaneObj[cnt].SetActive( true );
		}	
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
		Accounting.Instance.GetMonthlyUserData ( Accounting.Instance.currentUser.Id, LeaderdBoarSetting.Instance.GetUserDataFromText, SetupOtherRows, SetupOtherRowsFailed );
	}

	public void SetupOtherRowsFailed(){
		myButton.SetActive (true);
		loadingText.SetActive (false);
	}

	public void SetupOtherRows(){
		SetupMyRows ( LeaderdBoarSetting.Instance.myUserMonthlyData );
		myButton.SetActive (false);
		loadingText.SetActive (true);
		dataDownloaded = true;
		StartCoroutine ( _SetupOtherRow() );
	}
	
	IEnumerator _SetupOtherRow(){
		yield return new WaitForSeconds (1);
		if (LeaderBoardUIMenu.Instance.userMonthlyList.Count > 0) {	
			for (int cnt = 0; cnt < LeaderBoardUIMenu.Instance.userMonthlyList.Count; cnt++) {
				if( cnt > 99 )
					break;

				if (LeaderBoardUIMenu.Instance.userMonthlyList [cnt].IsMine == true) {
					LeaderBoardUIMenu.Instance.SetupOtherRowsLins (LeaderBoardUIMenu.Instance.userMonthlyList [cnt], true, rowsParent, LeaderMenuType.Monthly);
				} else {
					LeaderBoardUIMenu.Instance.SetupOtherRowsLins (LeaderBoardUIMenu.Instance.userMonthlyList [cnt], false, rowsParent, LeaderMenuType.Monthly);
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
	

	void SetupMyRows( LeagueUser user ){
		myBoard.Activate ( user, Color.white, true  );
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
