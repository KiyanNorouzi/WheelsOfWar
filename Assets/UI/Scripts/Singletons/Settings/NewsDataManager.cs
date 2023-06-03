using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewsDataManager : MonoBehaviour
{


	public List<NewsInfo> newsInfos = new List<NewsInfo> ();
	
	public List<NewsData> newsEventList = new List<NewsData> ();
	public List<NewsData> newsInboxList = new List<NewsData> ();
	
	#region Singleton
	
	static NewsDataManager _instance;
	
	public static NewsDataManager Instance
	{
		get { return _instance; }
	}
	
	void Awake()
	{
		if (_instance == null)
			_instance = this;
	}
	
	#endregion 
	
	void Start(){
		Accounting.Instance.GetMessages ( Accounting.Instance.currentUser.Id, StartCutingMessages, faild );

	}

	void StartCutingMessages( string text ){
		CutTheEventsFromTextAndGetMeNews ( text, out newsEventList, out newsInboxList  );
	}
	void faild(){
		Debug.Log ("failer");
	}

	public void CutTheEventsFromTextAndGetMeNews( string text, out List<NewsData> eventList, out List<NewsData> inboxList  ){
		string[] splitedMessages = text.Split ( new char[]{ '[' } );
		List<NewsData> _eventList = new List<NewsData> ();
		List<NewsData> _inboxList = new List<NewsData> ();
		
		for( int cnt = 1; cnt < splitedMessages.Length; cnt++ ){
			NewsData news = new NewsData();
			GetNewsFromText( splitedMessages[cnt], out news );
			switch( news.newsType ){
			case NewsTypeEnum._event:
				if( !_eventList.Contains( news ) )
					_eventList.Add(news);
				break;
			case NewsTypeEnum._Inbox:
				if( !_inboxList.Contains( news ) )
					_inboxList.Add(news);
				break;
			}
		}
		eventList = _eventList;
		inboxList = _inboxList;
	}
	
	public void GetNewsFromText( string text, out NewsData newNews ){
		string[] splitedNews = text.Split ( new char[]{','} );
		NewsData tempNews = new NewsData();
		for( int cnt = 0; cnt < splitedNews.Length; cnt++ ){
			
			if( splitedNews[cnt].StartsWith( "NT:" ) ){
				tempNews.newsType = GetNewsTypeFromText( splitedNews[cnt] );
			}

			if( splitedNews[cnt].StartsWith( "MT:" ) ){
				tempNews.messageType = GetMessageTypeFromText( splitedNews[cnt] );
				GetButtonNameFromType( tempNews.messageType, out tempNews.buttonTextEn, out tempNews.buttonTextFa );
				tempNews.Image = GetImageFromType( tempNews.messageType );
			}

			if( splitedNews[cnt].StartsWith( "BR:" ) ){
				tempNews.billsReward = GetBillsFromText( splitedNews[cnt] );
			}

			if( splitedNews[cnt].StartsWith( "GR:" ) ){
				tempNews.goldReward = GetGoldFromText( splitedNews[cnt] );
			}

			if( splitedNews[cnt].StartsWith( "ME:" ) ){
				tempNews.messageTextEn = EN_GetMessageFromText( splitedNews[cnt] );
			}
			
			if( splitedNews[cnt].StartsWith( "MF:" ) ){
				tempNews.messageTextFa = FA_GetMessageFromText( splitedNews[cnt] );
			}

			if( splitedNews[cnt].StartsWith( "URL:" ) ){
				tempNews.URL = GetURLFromText( splitedNews[cnt] );
			}
		}
		
		newNews = tempNews;
	}
	
	
	public NewsTypeEnum GetNewsTypeFromText( string text ){
		NewsTypeEnum tempType = NewsTypeEnum._event;
		text = text.TrimStart (new char[]{ 'N', 'T'});
		string s = text.TrimStart (new char[]{':'});
		if (s.EndsWith ("]")) {
			s = s.TrimEnd (new char[]{']'});
		}		
		switch (s) {
		case "event":
			tempType = NewsTypeEnum._event;
			break;
		case "inbox":
			tempType = NewsTypeEnum._Inbox;
			break;
		}
		return tempType;
	}
	
	public MessageTypeEnum GetMessageTypeFromText( string text ){
		MessageTypeEnum tempType = MessageTypeEnum.ban;
		text = text.TrimStart ( new char[]{ 'M', 'T'} );
		string s = text.TrimStart ( new char[]{':'} );
		if( s.EndsWith( "]" ) ){
			s = s.TrimEnd( new char[]{']'} );
		}		
		switch( s ){
		case "gift":
			tempType = MessageTypeEnum.gift;
			break;
		case "news":
			tempType = MessageTypeEnum.news;
			break;
		case "link":
			tempType = MessageTypeEnum.link;
			break;
		case "sale":
			tempType = MessageTypeEnum.sale;
			break;
		case "ban":
			tempType = MessageTypeEnum.ban;
			break;
		case "tournoment":
			tempType = MessageTypeEnum.tournoment;
			break;
		case "wait":
			tempType = MessageTypeEnum.wait;
			break;
		}
		return tempType;
	}
	
	public Sprite GetImageFromType( MessageTypeEnum type ){
		Sprite tempSprit = new Sprite();

		NewsInfo info = new NewsInfo ();
		for( int cnt = 0; cnt < newsInfos.Count; cnt++ ){
			if( newsInfos[cnt].name == type.ToString() ){
				info = newsInfos[cnt];
			}
		}

		switch( type ){
		case MessageTypeEnum.gift:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		case MessageTypeEnum.news:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		case MessageTypeEnum.link:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		case MessageTypeEnum.sale:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		case MessageTypeEnum.ban:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		case MessageTypeEnum.tournoment:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		case MessageTypeEnum.wait:
			tempSprit = info.image[GetImageIndex( info.image.Length )];
			break;
		}
		return tempSprit;
	}
	
	int GetImageIndex( int lastIndex ){
		int r = Random.Range ( 0,lastIndex );
		return r;
	}
	
	public void GetButtonNameFromType( MessageTypeEnum type, out string en_Name, out string fa_Name ){
		string tempEN = "";
		string tempFA = "";

		NewsInfo info = new NewsInfo ();
		for( int cnt = 0; cnt < newsInfos.Count; cnt++ ){
			if( newsInfos[cnt].name == type.ToString() ){
				info = newsInfos[cnt];
			}
		}

		switch( type ){
		case MessageTypeEnum.gift:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		case MessageTypeEnum.news:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		case MessageTypeEnum.link:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		case MessageTypeEnum.sale:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		case MessageTypeEnum.ban:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		case MessageTypeEnum.tournoment:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		case MessageTypeEnum.wait:
			tempEN = info.buttonNames_EN;
			tempFA = info.buttonNames_FA;
			break;
		}
		
		en_Name = tempEN;
		fa_Name = tempFA;
	}
	
	public int GetBillsFromText( string text ){
		text = text.TrimStart ( new char[]{ 'B','R' } );
		string m = text.TrimStart ( new char[]{ ':' } );
		if( m.EndsWith( "]" ) ){
			m = m.TrimEnd( new char[]{']'} );
		}
		int i = 0;
		int.TryParse ( m, out i );
		return i;
	}
	
	public int GetGoldFromText( string text ){
		text = text.TrimStart ( new char[]{ 'G','R' } );
		string m = text.TrimStart ( new char[]{ ':' } );
		if( m.EndsWith( "]" ) ){
			m = m.TrimEnd( new char[]{']'} );
		}
		int i = 0;
		int.TryParse ( m, out i );
		return i;
	}
	
	public string EN_GetMessageFromText( string text ){
		text = text.TrimStart ( new char[]{ 'M', 'E' } );
		string m = text.TrimStart ( new char[]{ ':' } );
		if( m.EndsWith( "]" ) ){
			m = m.TrimEnd( new char[]{']'} );
		}
		return m;
	}
	
	public string FA_GetMessageFromText( string text ){
		text = text.TrimStart ( new char[]{ 'M', 'F' } );
		string m = text.TrimStart ( new char[]{ ':' } );
		if( m.EndsWith( "]" ) ){
			m = m.TrimEnd( new char[]{']'} );
		}
		return m;
	}

	public string GetURLFromText( string text ){
		text = text.TrimStart ( new char[]{ 'U', 'R', 'L' } );
		string m = text.TrimStart ( new char[]{ ':' } );
		if( m.EndsWith( "]" ) ){
			m = m.TrimEnd( new char[]{']'} );
		}
		return m;
	}
}
[System.Serializable]
public class NewsInfo{
	public string name;
	public Sprite[] image;
	public string buttonNames_EN;
	public string buttonNames_FA;
}

[System.Serializable]
public class NewsData
{
	public string buttonTextEn;
	public string buttonTextFa;
	public string messageTextEn;
	public string messageTextFa;
	public int goldReward;
	public int billsReward;
	public string URL;
	public Sprite Image;
	public MessageTypeEnum messageType;
	public NewsTypeEnum newsType;
}

public enum NewsTypeEnum
{
	_event, _Inbox
}
public enum MessageTypeEnum
{
	gift,news,link,sale,ban,tournoment,wait
}
public enum MessageIDEnum
{
	gift = 0,news = 1,link = 2,sale = 3,ban = 4,tournoment = 5,wait = 6
}