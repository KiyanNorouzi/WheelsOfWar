using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageTypeUIMenu : MonoBehaviour {
	
	private static MessageTypeUIMenu instance;
	public static MessageTypeUIMenu Instance{
		get{ return instance; }
	}
	void Awake(){
		instance = this;
	}

	public GameObject messageObj,pixelObj;
	public Transform messageEventParent;
	public Transform messageInboxParent;
	public Transform eventPixlesParent, inboxPixlesParent;

	public List<NewsMenu> eventNewsList = new List<NewsMenu> ();
	public List<NewsMenu> inboxNewsList = new List<NewsMenu> ();
	public List<GameObject> eventPixlesList = new List<GameObject> ();
	public List<GameObject> inboxPixlesList = new List<GameObject> ();
	public ScrollRectsnap eventScrollSnap;
    public ScrollRectsnap inboxScrollSnap;

	public GameObject eventAlertObject;
	public GameObject inboxAlertObject;
	
	public Color pixleOnColro = Color.yellow;
	
	void Start(){		
		StartCoroutine (w ());
		CheckTheAlerts();
	}

	public void	SetupNewsMessage( NewsData news ){
		switch( news.newsType ){
		case NewsTypeEnum._event:
			SetupEventMessage( news );
			break;
		case NewsTypeEnum._Inbox:
			SetupInboxMessage( news );
			break;
		}
	}

	IEnumerator w(){
		yield return new WaitForSeconds ( 0 );
		for( int cnt =0 ; cnt < NewsDataManager.Instance.newsEventList.Count; cnt++ ){
			SetupNewsMessage( NewsDataManager.Instance.newsEventList[cnt] );
		}
		
		for( int cnt =0 ; cnt < NewsDataManager.Instance.newsInboxList.Count; cnt++ ){
			SetupNewsMessage( NewsDataManager.Instance.newsInboxList[cnt] );
		}
	}

	void SetupEventMessage( NewsData news ){
		NewsMenu newsGo = ((GameObject)Instantiate( messageObj )).GetComponent<NewsMenu>();
		GameObject newspixle = ((GameObject)Instantiate( pixelObj ));
		newsGo.transform.SetParent ( messageEventParent );
		newspixle.transform.SetParent ( eventPixlesParent );
		RectTransform rect = newsGo.GetComponent< RectTransform >();
		RectTransform pixRect = newspixle.GetComponent<RectTransform>();
		rect.anchoredPosition = new Vector2 ( 220,0 );
		pixRect.anchoredPosition = new Vector2 ( 0,0 );
		rect.localScale = new Vector3 ( 1,1,1 );

		if (eventNewsList.Count > 0) {
            rect.anchoredPosition = eventNewsList[eventNewsList.Count - 1].GetComponent<RectTransform>().anchoredPosition + new Vector2(436,0 );
			pixRect.anchoredPosition = eventPixlesList[eventPixlesList.Count - 1].GetComponent<RectTransform>().anchoredPosition + new Vector2( 10,0 );
            messageEventParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(eventNewsList[eventNewsList.Count - 1].transform.GetComponent<RectTransform>().anchoredPosition.x) + 655);

			messageEventParent.GetComponent<RectTransform>().anchoredPosition += eventNewsList[0].transform.GetComponent<RectTransform>().anchoredPosition;

			for( int cnt = 0; cnt < eventPixlesList.Count; cnt++ ){
				eventPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition = eventPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition + new Vector2( -10, 0 );
			}
        }
		eventPixlesList.Add ( newspixle );
		newsGo.Active ( news );
		eventNewsList.Add ( newsGo );
		eventScrollSnap.Activeted( NewsDataManager.Instance.newsEventList.Count);
	}

	void SetupInboxMessage( NewsData news ){
		NewsMenu newsGo = ((GameObject)Instantiate( messageObj )).GetComponent<NewsMenu>();
		GameObject newspixle = ((GameObject)Instantiate( pixelObj ));
		newsGo.transform.SetParent ( messageInboxParent );
		newspixle.transform.SetParent ( inboxPixlesParent );
		RectTransform rect = newsGo.GetComponent< RectTransform >();
		RectTransform pixRect = newspixle.GetComponent<RectTransform>();
		rect.anchoredPosition = new Vector2 ( 220,0 );
		pixRect.anchoredPosition = new Vector2 ( 0,0 );
		rect.localScale = new Vector3 ( 1,1,1 );
		if (inboxNewsList.Count > 0) {
			rect.anchoredPosition = inboxNewsList[inboxNewsList.Count - 1].GetComponent<RectTransform>().anchoredPosition + new Vector2(436,0 );
			pixRect.anchoredPosition = inboxPixlesList[inboxPixlesList.Count - 1].GetComponent<RectTransform>().anchoredPosition + new Vector2( 10,0 );
			messageInboxParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(inboxNewsList[inboxNewsList.Count - 1].transform.GetComponent<RectTransform>().anchoredPosition.x) + 655);

			messageInboxParent.GetComponent<RectTransform>().anchoredPosition += inboxNewsList[0].transform.GetComponent<RectTransform>().anchoredPosition;

			for( int cnt = 0; cnt < inboxPixlesList.Count; cnt++ ){
				inboxPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition = inboxPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition + new Vector2( -10, 0 );
			}

		}
		inboxPixlesList.Add ( newspixle );
		newsGo.Active ( news );
		inboxNewsList.Add ( newsGo );
		inboxScrollSnap.Activeted( NewsDataManager.Instance.newsInboxList.Count);
	}

	public void RemoveSomeMessage( NewsMenu news, NewsTypeEnum nType ){
		switch( nType ){
		case NewsTypeEnum._event:
			eventNewsList.Remove(news);
			Destroy( eventPixlesList[0].gameObject);
			eventPixlesList.Remove( eventPixlesList[0] );
			for( int cnt = 0; cnt < eventNewsList.Count; cnt++ ){
				if( cnt == 0){
					eventNewsList[cnt].GetComponent< RectTransform >().anchoredPosition = new Vector2 (220,0);
				}
				else{
					eventNewsList[cnt].GetComponent< RectTransform >().anchoredPosition = eventNewsList[cnt - 1].GetComponent< RectTransform >().anchoredPosition + new Vector2(436,0);
				}
			}
			
			for( int cnt = 0; cnt < eventPixlesList.Count; cnt++ ){
				eventPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition = eventPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition + new Vector2( -10, 0 );
			}
			messageEventParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(eventNewsList[eventNewsList.Count - 1].transform.GetComponent<RectTransform>().anchoredPosition.x) + 655 - 436);
			messageEventParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,0);
			eventScrollSnap.SetPosZero();
			eventScrollSnap.Activeted( eventNewsList.Count);
			break;
		case NewsTypeEnum._Inbox:
			inboxNewsList.Remove(news);
			Destroy( inboxPixlesList[0].gameObject);
			inboxPixlesList.Remove( inboxPixlesList[0] );
			for( int cnt = 0; cnt < inboxNewsList.Count; cnt++ ){
				if( cnt == 0){
					inboxNewsList[cnt].GetComponent< RectTransform >().anchoredPosition = new Vector2 (220,0);
				}
				else{
					inboxNewsList[cnt].GetComponent< RectTransform >().anchoredPosition = inboxNewsList[cnt - 1].GetComponent< RectTransform >().anchoredPosition + new Vector2(436,0);
				}
			}
			
			for( int cnt = 0; cnt < inboxPixlesList.Count; cnt++ ){
				inboxPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition = inboxPixlesList[cnt].GetComponent<RectTransform>().anchoredPosition + new Vector2( -10, 0 );
			}
			if( inboxNewsList.Count > 0 ){
				messageInboxParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(inboxNewsList[inboxNewsList.Count - 1].transform.GetComponent<RectTransform>().anchoredPosition.x) + 655 - 436);
				messageInboxParent.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0,0);
				inboxScrollSnap.SetPosZero();
			}
			inboxScrollSnap.Activeted( inboxNewsList.Count);
			break;
		}
	}

	public void ContactUs_Click()
	{
		Application.OpenURL(string.Format("mailto:{0}?Subject={1}", GeneralSettings.Instance.contactEmail, GeneralSettings.Instance.contactEmailSubject));
	}
	

	public void CheckTheAlerts(){
		if (NewsDataManager.Instance.newsEventList.Count > 0) {
			if (!eventAlertObject.activeSelf)
				eventAlertObject.SetActive (true);
		}
		else
			if (eventAlertObject.activeSelf)
				eventAlertObject.SetActive (false);

		if (NewsDataManager.Instance.newsInboxList.Count > 0)
			inboxAlertObject.SetActive (true);
		else
			inboxAlertObject.SetActive (false);
	}
}
