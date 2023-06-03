using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewsMenu : MonoBehaviour {

	public Image imageIcon;
	public Button actionButton;
	public Text buttonText;
	public Text messageText;

	private MessageTypeEnum messageType;
	private NewsTypeEnum newsType;
	private string message_EN;
	private string message_FA;
	private string url;
	private int gold;
	private int bills;
	private int id;

	void Start(){
		actionButton.onClick.RemoveAllListeners ();
		actionButton.onClick.AddListener ( OnButtonClick );
	}

	public void Active( NewsData news ){
		Active ( news.Image, news.messageTextEn, news.messageTextFa, news.buttonTextEn, news.buttonTextFa, news.newsType, news.messageType, news.billsReward, news.goldReward, news.URL );
	}

	public void Active( Sprite icon, string en_message, string fa_message ,string en_ButtonName, string fa_ButtonName, NewsTypeEnum nType, MessageTypeEnum mType, int bills, int golds, string url  ){
		if (SettingData.LanguageIndex == 0) { // english
			buttonText.text = en_ButtonName;
			message_EN = en_message;
			messageText.text = message_EN;
		}
		else {
			buttonText.text = fa_ButtonName;
			message_FA = fa_message;
			messageText.text = message_FA;
		}
		this.messageType = mType;
		this.newsType = nType;
		this.imageIcon.sprite = icon;
		this.gold = golds;
		this.bills = bills;
		this.url = url;

		if( messageType == MessageTypeEnum.news ){
			actionButton.gameObject.SetActive( false );
		}
	}

	public void OnButtonClick(){
		switch (messageType) {
		case MessageTypeEnum.gift:
			GiftAction( this.gold, this.bills );
			break;
		case MessageTypeEnum.news:
			NewsAction();
			break;
		case MessageTypeEnum.link:
			LinkAction(this.url);
			break;
		case MessageTypeEnum.sale:
			SaleAction();
			break;
		case MessageTypeEnum.ban:
			BanAction();
			break;
		case MessageTypeEnum.tournoment:
			TournomentAction( this.gold );
			break;
		case MessageTypeEnum.wait:
			WaitAction();
			break;
		}
	}

	void GiftAction( int gold, int bills ){
		Debug.Log ( "Gift Action" );
		this.gold = gold;
		this.bills = bills;
		this.id = (int)MessageIDEnum.gift;
		Accounting.Instance.DeletingMessages ( Accounting.Instance.currentUser.Id, this.id, GetHimPrices, null  );
	}
	void GetHimPrices(){
		if( this.bills > 0 )
			Accounting.Instance.currentUser.Bills += bills;
		if( this.gold > 0 )
			Accounting.Instance.currentUser.Golds += gold;
		RemoveAndDestroyMe ();
	}

	void NewsAction(){
		Debug.Log ( "News Action" );
		actionButton.gameObject.SetActive (false);
	}
	void LinkAction( string url ){
		Debug.Log ( "Link Action" );
		Application.OpenURL ( url );
	}
	void SaleAction(){
		Debug.Log ( "Sale Action" );
		CommonUI.Instance.messageBox.Ask(Messages.NotEnoughBills, _BuyBills, null, false);
		CommonUI.Instance.messageBox.YesButton_Clicked ();
	}
	void _BuyBills()
	{
		CommonUI.Instance.buyCoinsMenu.Activate(BuyCurrencySections.Bills);
	}

	void BanAction(){
		Debug.Log ( "Ban Action" );
		SendEmail ( "Email", "subject","Boddy" );
	}

	public void SendEmail (string email,string subject,string body)
	{
		subject = MyEscapeURL(subject);
		body = MyEscapeURL(body);
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
	public string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}


	void TournomentAction( int gold ){
		this.gold = gold;
		Debug.Log ( "Tornuments Action" );
		Accounting.Instance.DeletingMessages ( Accounting.Instance.currentUser.Id, this.id, GetHimGolds, null  );

	}
	void GetHimGolds(){
		Accounting.Instance.currentUser.Golds += gold;
		if( this.gold > 0 )
			Accounting.Instance.currentUser.Golds += gold;
		RemoveAndDestroyMe ();
	}


	void WaitAction(){
		Debug.Log ( "Wait Action" );
		actionButton.gameObject.SetActive (false);
	}

	void RemoveAndDestroyMe(){
		MessageTypeUIMenu.Instance.RemoveSomeMessage( this, newsType );
		Destroy ( gameObject );
	}
}
