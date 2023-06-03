using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LeaderdBoarSetting : MonoBehaviour {

	private static LeaderdBoarSetting instance;
	public static LeaderdBoarSetting Instance{
		get{ return instance; }
	}
	void Awake(){
		instance = this;
	}

	public List<LeagueData> leagueList = new List<LeagueData> ();
	public string leaguePosition;

	public LeagueUser myUserLeagueData;
	public LeagueUser myUserMonthlyData;
	public LeagueUser myUserGlobalData;
	public LeagueData curLeague;


	void Start(){
		Accounting.Instance.GetMyLeagueDatas ( Accounting.Instance.currentUser.Id, GetMyLeagueData, null );
//		Accounting.Instance.SetInActiveModeUrl ( Accounting.Instance.currentUser.Id, GetRequestOfSetInActive, null );
	}

	public string FindLeagueIDFromData( string str ){
		string temp = "";
		if( str.IndexOf("A") != -1 ) temp = "A";
		if( str.IndexOf("B") != -1 ) temp = "B";
		if( str.IndexOf("C") != -1 ) temp = "C";
		if( str.IndexOf("D") != -1 ) temp = "D";
		if( str.IndexOf("E") != -1 ) temp = "E";
		if( str.IndexOf("F") != -1 ) temp = "F";
		if( str.IndexOf("G") != -1 ) temp = "G";
		if( str.IndexOf("I") != -1 ) temp = "I";
		if( str.IndexOf("J") != -1 ) temp = "J";
		if( str.IndexOf("K") != -1 ) temp = "K";
		if( str.IndexOf("L") != -1 ) temp = "L";
		if( str.IndexOf("M") != -1 ) temp = "M";
		if( str.IndexOf("N") != -1 ) temp = "N";
		if( str.IndexOf("O") != -1 ) temp = "O";
		if( str.IndexOf("P") != -1 ) temp = "P";

		if (temp == "")
			return "";

		return temp;
	}

	void GetRequestOfSetInActive( string text ){
		Debug.Log ( " In Active  : " +text );
	}

	public void GetMyLeagueData( string text ){
		string[] spliedData = GetSplitedUserDatas ( text );
		for( int cnt = 0; cnt < spliedData.Length; cnt++ ){
			Spliter( spliedData[cnt], out myUserLeagueData );
		}
	}

	public void Spliter( string text, out LeagueUser user ){
		string[] splitedText = text.Split( new char[]{','} );
		LeagueUser temp = new LeagueUser();
		temp.UserName = string.Empty;
		temp.GlobalScore = 0;
		temp.LeagueScore = 0;
		temp.LeagueRank = 0;
		temp.IsMine = false;
		temp.Type = LeaderBoardType.League;
		
		for( int cnt = 0; cnt < splitedText.Length; cnt++ ){
			if( splitedText[cnt].StartsWith( "U:" ) ){
				temp.UserName = TrimUserName( splitedText[cnt] );
			}
			if( splitedText[cnt].StartsWith( "S:" ) ){
				temp.GlobalScore = TrimGlobalScore( splitedText[cnt] );
			}
			if( splitedText[cnt].StartsWith( "R:" ) ){
				temp.LeagueRank = TrimLeagueRank( splitedText[cnt] );
			}
			if( splitedText[cnt].StartsWith( "L:" ) ){
				temp.LeagueScore = TrimLeagueScore( splitedText[cnt] );
			}
			if( splitedText[cnt].StartsWith( "T:" ) ){
				temp.Type = TrimType( splitedText[cnt] );
			}
			if( splitedText[cnt].StartsWith( "I:" ) ){
				temp.IsMine = true;
			}
			if( splitedText[cnt].StartsWith( "P:" ) ){
				if( LeaderdBoarSetting.Instance.leaguePosition != LeaderdBoarSetting.Instance.FindLeagueIDFromData( TrimLeaguePosition( splitedText[cnt] ))){
					LeaderdBoarSetting.Instance.leaguePosition = LeaderdBoarSetting.Instance.FindLeagueIDFromData( TrimLeaguePosition( splitedText[cnt] ));
                    ConvertToLeagueData(LeaderdBoarSetting.Instance.leaguePosition, out curLeague);
				}
			}
		}
		if( temp.IsMine ){
			switch( temp.Type ){
			case LeaderBoardType.League:
				myUserLeagueData = temp;
				break;
			case LeaderBoardType.Monthly:
				myUserMonthlyData = temp;
				break;
			case LeaderBoardType.Global:
				myUserGlobalData = temp;
				break;
			}
		}
		user = temp;
	}


	public string TrimUserName( string text ){
		text = text.TrimStart (new char[]{ 'U' });
		return text.TrimStart (new char[]{ ':' });
	}
	
	public int TrimGlobalScore( string text ){
		text = text.TrimStart (new char[]{ 'S' });
		string s = text.TrimStart (new char[]{ ':' });
		if( s.EndsWith( "]" ) ){
			s = s.TrimEnd( new char[]{']'} );
		}
		int scor = 0;
		int.TryParse (s, out scor);
		return scor;
	}
	
	public int TrimLeagueRank( string text ){
		text = text.TrimStart (new char[]{ 'R' });
		string s = text.TrimStart (new char[]{ ':' });
		if( s.EndsWith( "]" ) ){
			s = s.TrimEnd( new char[]{']'} );
		}
		int rank = 0;
		int.TryParse (s, out rank);
		return rank;
	}
	
	public int TrimLeagueScore( string text ){
		text = text.TrimStart (new char[]{ 'L' });
		string s = text.TrimStart (new char[]{ ':' });
		if( s.EndsWith( "]" ) ){
			s = s.TrimEnd( new char[]{']'} );
		}
		int lScor = 0;
		int.TryParse (s, out lScor);
		return lScor;
	}
	
	public string TrimLeaguePosition( string text ){
		text = text.TrimStart (new char[]{ 'P' });
		string s = text.TrimStart (new char[]{ ':' });
		if( s.EndsWith( "]" ) ){
			s = s.TrimEnd( new char[]{']'} );
		}
		return s;
	}
	
	public LeaderBoardType TrimType( string text ){
		LeaderBoardType tempType = LeaderBoardType.League;
		text = text.TrimStart (new char[]{ 'T' });
		string s = text.TrimStart (new char[]{ ':' });
		if( s.EndsWith( "]" ) ){
			s = s.TrimEnd( new char[]{']'} );
		}
		int typeIndex = 0;
		int.TryParse (s, out typeIndex);
		switch( typeIndex ){
		case 0:
			tempType = LeaderBoardType.League;
			break;
		case 1:
			tempType = LeaderBoardType.Monthly;
			break;
		case 2:
			tempType = LeaderBoardType.Global;
			break;
		}
		
		return tempType;
	}



	public string[] GetSplitedUserDatas( string text ){
		string[] datas = text.Split ( new char[]{'['} );
		return datas;
	}



	public void GetUserDataFromText( string text ){
		string[] spliedData = GetSplitedUserDatas ( text );
		LeagueUser newUser = new LeagueUser ();
		for( int cnt = 0; cnt < spliedData.Length; cnt++ ){
			Spliter( spliedData[cnt], out newUser );
			switch( newUser.Type ){
			case LeaderBoardType.League:
				if( !LeaderBoardUIMenu.Instance.userLeagueList.Contains( newUser ) ){
					LeaderBoardUIMenu.Instance.userLeagueList.Add(newUser);
				}	
				break;
			case LeaderBoardType.Monthly:
				if( !LeaderBoardUIMenu.Instance.userMonthlyList.Contains( newUser ) ){
					LeaderBoardUIMenu.Instance.userMonthlyList.Add(newUser);
				}	
				break;
			case LeaderBoardType.Global:
				if( !LeaderBoardUIMenu.Instance.userGlobalList.Contains( newUser ) ){
					LeaderBoardUIMenu.Instance.userGlobalList.Add(newUser);
				}	
				break;
			}

		}
	}

	public void ConvertToLeagueData( string strData ,out LeagueData data ){
	
		LeagueData tempdata = new LeagueData();

		switch( strData ){
		case "A":
			tempdata.LeagueID = leagueList[0].LeagueID;
			tempdata.leagueCategory = leagueList[0].leagueCategory;
			tempdata.leagueIcon = leagueList[0].leagueIcon;
			tempdata.leagueName_EN = leagueList[0].leagueName_EN;
			tempdata.leagueName_FA = leagueList[0].leagueName_FA;
			tempdata.reward = leagueList[0].reward;
			tempdata.LeagueRewardI = leagueList[2].reward;
			tempdata.LeagueRewardII = leagueList[1].reward;
			tempdata.LeagueRewardIII = leagueList[0].reward;
			break;
		case "B":
			tempdata.LeagueID = leagueList[1].LeagueID;
			tempdata.leagueCategory = leagueList[1].leagueCategory;
			tempdata.leagueIcon = leagueList[1].leagueIcon;
			tempdata.leagueName_EN = leagueList[1].leagueName_EN;
			tempdata.leagueName_FA = leagueList[1].leagueName_FA;
			tempdata.reward = leagueList[1].reward;
			tempdata.LeagueRewardI = leagueList[2].reward;
			tempdata.LeagueRewardII = leagueList[1].reward;
			tempdata.LeagueRewardIII = leagueList[0].reward;
			break;
		case "C":
			tempdata.LeagueID = leagueList[2].LeagueID;
			tempdata.leagueCategory = leagueList[2].leagueCategory;
			tempdata.leagueIcon = leagueList[2].leagueIcon;
			tempdata.leagueName_EN = leagueList[2].leagueName_EN;
			tempdata.leagueName_FA = leagueList[2].leagueName_FA;
			tempdata.reward = leagueList[2].reward;
			tempdata.LeagueRewardI = leagueList[2].reward;
			tempdata.LeagueRewardII = leagueList[1].reward;
			tempdata.LeagueRewardIII = leagueList[0].reward;
			break;
		case "D":
			tempdata.LeagueID = leagueList[3].LeagueID;
			tempdata.leagueCategory = leagueList[3].leagueCategory;
			tempdata.leagueIcon = leagueList[3].leagueIcon;
			tempdata.leagueName_EN = leagueList[3].leagueName_EN;
			tempdata.leagueName_FA = leagueList[3].leagueName_FA;
			tempdata.reward = leagueList[3].reward;
			tempdata.LeagueRewardI = leagueList[5].reward;
			tempdata.LeagueRewardII = leagueList[4].reward;
			tempdata.LeagueRewardIII = leagueList[3].reward;
			break;
		case "E":
			tempdata.LeagueID = leagueList[4].LeagueID;
			tempdata.leagueCategory = leagueList[4].leagueCategory;
			tempdata.leagueIcon = leagueList[4].leagueIcon;
			tempdata.leagueName_EN = leagueList[4].leagueName_EN;
			tempdata.leagueName_FA = leagueList[4].leagueName_FA;
			tempdata.reward = leagueList[4].reward;
			tempdata.LeagueRewardI = leagueList[5].reward;
			tempdata.LeagueRewardII = leagueList[4].reward;
			tempdata.LeagueRewardIII = leagueList[3].reward;
			break;
		case "F":
			tempdata.LeagueID = leagueList[5].LeagueID;
			tempdata.leagueCategory = leagueList[5].leagueCategory;
			tempdata.leagueIcon = leagueList[5].leagueIcon;
			tempdata.leagueName_EN = leagueList[5].leagueName_EN;
			tempdata.leagueName_FA = leagueList[5].leagueName_FA;
			tempdata.LeagueRewardI = leagueList[5].LeagueRewardI;
			tempdata.LeagueRewardII = leagueList[4].LeagueRewardII;
			tempdata.reward = leagueList[5].LeagueRewardIII;
			tempdata.LeagueRewardIII = leagueList[3].LeagueRewardIII;
			break;
		case "G":
			tempdata.LeagueID = leagueList[6].LeagueID;
			tempdata.leagueCategory = leagueList[6].leagueCategory;
			tempdata.leagueIcon = leagueList[6].leagueIcon;
			tempdata.leagueName_EN = leagueList[6].leagueName_EN;
			tempdata.leagueName_FA = leagueList[6].leagueName_FA;
			tempdata.reward = leagueList[6].reward;
			tempdata.LeagueRewardI = leagueList[8].reward;
			tempdata.LeagueRewardII = leagueList[7].reward;
			tempdata.LeagueRewardIII = leagueList[6].reward;
			break;
		case "I":
			tempdata.LeagueID = leagueList[7].LeagueID;
			tempdata.leagueCategory = leagueList[7].leagueCategory;
			tempdata.leagueIcon = leagueList[7].leagueIcon;
			tempdata.leagueName_EN = leagueList[7].leagueName_EN;
			tempdata.leagueName_FA = leagueList[7].leagueName_FA;
			tempdata.reward = leagueList[7].reward;
			tempdata.LeagueRewardI = leagueList[8].reward;
			tempdata.LeagueRewardII = leagueList[7].reward;
			tempdata.LeagueRewardIII = leagueList[6].reward;
			break;
		case "J":
			tempdata.LeagueID = leagueList[8].LeagueID;
			tempdata.leagueCategory = leagueList[8].leagueCategory;
			tempdata.leagueIcon = leagueList[8].leagueIcon;
			tempdata.leagueName_EN = leagueList[8].leagueName_EN;
			tempdata.leagueName_FA = leagueList[8].leagueName_FA;
			tempdata.reward = leagueList[8].reward;
			tempdata.LeagueRewardI = leagueList[8].reward;
			tempdata.LeagueRewardII = leagueList[7].reward;
			tempdata.LeagueRewardIII = leagueList[6].reward;
			break;
		case "K":
			tempdata.LeagueID = leagueList[9].LeagueID;
			tempdata.leagueCategory = leagueList[9].leagueCategory;
			tempdata.leagueIcon = leagueList[9].leagueIcon;
			tempdata.leagueName_EN = leagueList[9].leagueName_EN;
			tempdata.leagueName_FA = leagueList[9].leagueName_FA;
			tempdata.reward = leagueList[9].reward;
			tempdata.LeagueRewardI = leagueList[11].reward;
			tempdata.LeagueRewardII = leagueList[10].reward;
			tempdata.LeagueRewardIII = leagueList[9].reward;
			break;
		case "L":
			tempdata.LeagueID = leagueList[10].LeagueID;
			tempdata.leagueCategory = leagueList[10].leagueCategory;
			tempdata.leagueIcon = leagueList[10].leagueIcon;
			tempdata.leagueName_EN = leagueList[10].leagueName_EN;
			tempdata.leagueName_FA = leagueList[10].leagueName_FA;
			tempdata.reward = leagueList[10].reward;
			tempdata.LeagueRewardI = leagueList[11].reward;
			tempdata.LeagueRewardII = leagueList[10].reward;
			tempdata.LeagueRewardIII = leagueList[9].reward;
			break;
		case "M":
			tempdata.LeagueID = leagueList[11].LeagueID;
			tempdata.leagueCategory = leagueList[11].leagueCategory;
			tempdata.leagueIcon = leagueList[11].leagueIcon;
			tempdata.leagueName_EN = leagueList[11].leagueName_EN;
			tempdata.leagueName_FA = leagueList[11].leagueName_FA;
			tempdata.reward = leagueList[11].reward;
			tempdata.LeagueRewardI = leagueList[11].reward;
			tempdata.LeagueRewardII = leagueList[10].reward;
			tempdata.LeagueRewardIII = leagueList[9].reward;
			break;
		case "N":
			tempdata.LeagueID = leagueList[12].LeagueID;
			tempdata.leagueCategory = leagueList[12].leagueCategory;
			tempdata.leagueIcon = leagueList[12].leagueIcon;
			tempdata.leagueName_EN = leagueList[12].leagueName_EN;
			tempdata.leagueName_FA = leagueList[12].leagueName_FA;
			tempdata.reward = leagueList[12].reward;
			tempdata.LeagueRewardI = leagueList[14].reward;
			tempdata.LeagueRewardII = leagueList[13].reward;
			tempdata.LeagueRewardIII = leagueList[12].reward;
			break;
		case "O":
			tempdata.LeagueID = leagueList[13].LeagueID;
			tempdata.leagueCategory = leagueList[13].leagueCategory;
			tempdata.leagueIcon = leagueList[13].leagueIcon;
			tempdata.leagueName_EN = leagueList[13].leagueName_EN;
			tempdata.leagueName_FA = leagueList[13].leagueName_FA;
			tempdata.reward = leagueList[13].reward;
			tempdata.LeagueRewardI = leagueList[14].reward;
			tempdata.LeagueRewardII = leagueList[13].reward;
			tempdata.LeagueRewardIII = leagueList[12].reward;
			break;
		case "P":
			tempdata.LeagueID = leagueList[14].LeagueID;
			tempdata.leagueCategory = leagueList[14].leagueCategory;
			tempdata.leagueIcon = leagueList[14].leagueIcon;
			tempdata.leagueName_EN = leagueList[14].leagueName_EN;
			tempdata.leagueName_FA = leagueList[14].leagueName_FA;
			tempdata.reward = leagueList[14].reward;
			tempdata.LeagueRewardI = leagueList[14].reward;
			tempdata.LeagueRewardII = leagueList[13].reward;
			tempdata.LeagueRewardIII = leagueList[12].reward;
			break;
		}
        Debug.Log( curLeague.leagueCategory.ToString() );
		curLeague = tempdata;
		data = tempdata;
	}
}

public class LeagueUser{
	string userName;
	int globalScor;
	int leagueScore;
	int leagueRank;
	bool isMine;
	LeaderBoardType type;
	
	public string UserName{
		get{ return userName; }
		set{ userName = value; }
	}
	public int GlobalScore{
		get{ return globalScor; }
		set{ globalScor = value; }
	}
	public int LeagueScore{
		get{ return leagueScore; }
		set{ leagueScore = value; }
	}
	public int LeagueRank{
		get{ return leagueRank; }
		set{ leagueRank = value; }
	}
	public bool IsMine{
		get{ return isMine; }
		set{ isMine = value; }
	}
	public LeaderBoardType Type{
		get{ return type; }
		set{ type = value; }
	}
	LeaderBoardType asd;
}

public enum LeaderBoardType{
	League,
	Monthly,
	Global
}

public enum LeagueCategory{
	I,II,III
}
