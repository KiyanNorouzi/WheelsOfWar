using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class LeagueData {
			
	public LeagueID LeagueID;

	public string leagueName_EN;
	public string leagueName_FA;

	public int reward;

	private int leagueRewardI;
	public int LeagueRewardI{
		get{ return leagueRewardI; }
		set{ leagueRewardI = value; }
	}

	private int leagueRewardII;
	public int LeagueRewardII{
		get{ return leagueRewardII; }
		set{ leagueRewardII = value; }
	}

	private int leagueRewardIII;
	public int LeagueRewardIII{
		get{ return leagueRewardIII; }
		set{ leagueRewardIII = value; }
	}

	public Sprite leagueIcon;
	
	public LeagueCategory leagueCategory;
}

public enum LeagueID{
	Boronze,
	Silver,
	Gold,
	Platinum,
	Diamond
}
