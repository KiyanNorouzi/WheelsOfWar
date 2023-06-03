//using UnityEngine;
//using ExitGames.Client.Photon;
//using Hashtable = ExitGames.Client.Photon.Hashtable;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using System.Collections;
//
//public class NetworkManager : Photon.MonoBehaviour {
//
//	#region Singleton
//
//	static NetworkManager _instance;
//
//	public static NetworkManager Instance
//	{
//		get{return _instance;}
//	}
//
//	void Awake()
//	{
//		_instance = this;
//	}
//
//	#endregion
//
//
//	public List<GameObject> playerList = new List<GameObject>();
//
//	public int StartCounter;
//
//	public Text startCounterText;
//	public Button playerButton;
//	public Button fightButton;
//	public Text ConnectionText;
//
//	public GameObject hostCharacterPanel;
//	public Image hostCharacterIconImage;
//	public Text hostCharacterLevelText;
//	public Image hostCharacterFactionImage;
//	public Text hostCharacterFactionText;
//	public Image hostCharacterTierImage;
//	public Text hostCharacterTierText;
//
//	public GameObject oponentCharacterPanel;
//	public Image oponentCharacterIconImage;
//	public Text oponentCharacterLevelText;
//	public Image oponentCharacterFactionImage;
//	public Text oponentCharacterFactionText;
//	public Image oponentCharacterTierImage;
//	public Text oponentCharacterTierText;
//
//
//	private Transform hostCharacterSpawnPos;
//	private Transform oponentCharacterSpawnPos;
//
//	private string roomName = "Room01";
//	private string roomStates = "";
//
//	private int maxPlayer = 1;
//	private string maxPlayerString;
//	private int indexLevelPlus;
//
//	private Room[] game;
//
//	private Vector2 scrollPos;
//	
//	int selectedLevelIndex = 0;
//
//	public PlayerData player;
//	public CharacterStats hostCharacter, oponentCharacter ;
//
//	public PhotonView myPhotonView;
//	// Use this for initialization
//	void Start () {
//		fightButton.gameObject.SetActive(false);
//		hostCharacterPanel.SetActive(false);
//		oponentCharacterPanel.SetActive(false);
//		player = GameObject.Find( "PlayerData" ).GetComponent<PlayerData>();
//		PhotonNetwork.ConnectUsingSettings("0.1");
//		hostCharacterSpawnPos = GameObject.FindGameObjectWithTag ("PlayerPos").transform;
//		oponentCharacterSpawnPos = GameObject.FindGameObjectWithTag ("OponentPos").transform;
//
//	}
//
//	void OnJoinedLobby(){
//		playerButton.interactable = true;
//		if( photonView.isMine ){
//			player = GameObject.Find( "PlayerData" ).GetComponent<PlayerData>();
//		}
//	}
//
//	void Update(){
//	}
//	
//	void OnJoinedRoom(){
//		playerButton.transform.FindChild("Text").GetComponent<Text>().text = "Diss";
//		playerButton.onClick.RemoveAllListeners();
//		playerButton.onClick.AddListener( DisconnectMe );
//
//
//		if( PhotonNetwork.isMasterClient == true ){
//			player = GameObject.Find( "PlayerData" ).GetComponent<PlayerData>();
//			Destroy(player.curCharacter);
//			player.curCharacter = PhotonNetwork.Instantiate( "Characters/Tire"+ player.characterTier + "/" + player.characterName, hostCharacterSpawnPos.transform.position, hostCharacterSpawnPos.transform.rotation, 0 )as GameObject;
////			player.curCharacter = PhotonNetwork.Instantiate( "Characters/Tire"+ PlayerPrefs.GetInt( "Tire" ) + "/" +"P2" , hostCharacterSpawnPos.transform.position, hostCharacterSpawnPos.transform.rotation, 0 )as GameObject;
//			hostCharacter = player.curCharacter.GetComponent<CharacterStats>();
//		}
//		else{
//			player = GameObject.Find( "PlayerData" ).GetComponent<PlayerData>();
//
//			Destroy(player.curCharacter);
//			player.curCharacter = PhotonNetwork.Instantiate( "Characters/Tire"+ player.characterTier + "/" + player.characterName,oponentCharacterSpawnPos.transform.position, oponentCharacterSpawnPos.transform.rotation, 0 )as GameObject;
////			player.curCharacter = PhotonNetwork.Instantiate( "Characters/Tire"+ PlayerPrefs.GetInt( "Tire" ) + "/" +"P4", oponentCharacterSpawnPos.transform.position, oponentCharacterSpawnPos.transform.rotation, 0 )as GameObject;
//
//			oponentCharacter = player.curCharacter.GetComponent<CharacterStats>();
//		}
//	}
//
//	//[PunRPC]
//	public void SetHostAtts(CharacterStats hostCharacter){
//		if(!hostCharacterPanel)
//			return;
//
//		hostCharacterPanel.SetActive(true);
//		
//		hostCharacterIconImage.sprite = hostCharacter.characterImage;
//		hostCharacterLevelText.text = hostCharacter.characterLevel.ToString();
//		hostCharacterFactionImage.sprite = hostCharacter.factionIcon;
//		hostCharacterFactionText.text = hostCharacter.faction.ToString();
//		hostCharacterTierImage.sprite = hostCharacter.tierIcon;
//		hostCharacterTierText.text = hostCharacter.health.ToString();
//
//	}
//
//	//[PunRPC]
//	public void SetOponentAtts(CharacterStats oponentCharacter){
//		if(!oponentCharacterPanel)
//			return;
//
//		oponentCharacterPanel.SetActive(true);
//
//		oponentCharacterIconImage.sprite = oponentCharacter.characterImage;
//		oponentCharacterLevelText.text = oponentCharacter.characterLevel.ToString();
//		oponentCharacterFactionImage.sprite = oponentCharacter.factionIcon;
//		oponentCharacterFactionText.text = oponentCharacter.faction.ToString();
//		oponentCharacterTierImage.sprite = oponentCharacter.tierIcon;
//		oponentCharacterTierText.text = oponentCharacter.health.ToString();
//	}
//
//	public void StartFight(){
//		photonView.RPC( "StartFightRPC", PhotonTargets.AllViaServer );
//	}
//
//	[PunRPC]
//	public void StartFightRPC(){
//		if( PhotonNetwork.playerList.Length == 2 ){
//			StartCoroutine( PlayerData.Instance.RPC_SceneChanger( "OnlineFightRound" ));
//		}
//	}
//
//	IEnumerator StartCountingToStart(){
//		photonView.RPC( "SetCounter", PhotonTargets.AllBuffered );
//		for(;;){
//			yield return new WaitForSeconds(1);
//			Client_CountDown();
//		}
//	} 
//
//	[PunRPC]
//	void SetCounter(){
//		StartCounter = 60;
//		fightButton.gameObject.SetActive(true);
//		if( PhotonNetwork.isMasterClient ){
//			fightButton.interactable = true;
//		}
//		else{
//			fightButton.interactable = false;
//		}
//	}
//	
//	void Client_CountDown(){
//		photonView.RPC("Server_CountDown", PhotonTargets.AllViaServer);
//	}
//
//	[PunRPC]
//	void Server_CountDown(){
//		if( StartCounter <= 50 ){
//			StartFight();
//		}
//		if( StartCounter < 0 ){
//			StartCounter = 0;
//		}
//		StartCounter --;
//		startCounterText.text = StartCounter.ToString();
//	}
//
//	
//	void OnPhotonRandomJoinFailed(){
//		indexLevelPlus += 1;
//		if( indexLevelPlus < 10 ){
//			StartJoin( indexLevelPlus );
//		}
//		else{
//			CreateAMatch();
//		}
//	}
//
//	public void OnPhotonPlayerConnected(){
//		StartCoroutine(StartCountingToStart());
//	}
//
//	public void OnPhotonPlayerDisconnected()
//	{    
//		fightButton.gameObject.SetActive(false);
//		
//		if( PhotonNetwork.isMasterClient ){
//			if( photonView.isMine ){
//				oponentCharacterIconImage.sprite = null;
//				oponentCharacterLevelText.text = null;
//				oponentCharacterFactionImage.sprite = null;
//				oponentCharacterFactionText.text = null;
//				oponentCharacterTierImage.sprite = null;
//				oponentCharacterTierText.text =null;
//				oponentCharacterPanel.SetActive(false);
//
//				player.curCharacter.transform.position = hostCharacterSpawnPos.position;
//				player.curCharacter.transform.rotation = hostCharacterSpawnPos.rotation;
//				SetHostAtts(player.curCharacter.GetComponent<CharacterStats>());
//
//			}
//		}
//	}
//
//	public void DisconnectMe(){
//		PhotonNetwork.Disconnect();
//		Application.LoadLevel("OnlineScene");
//	}
//
//	public void StartJoin( int cnt ){
//		TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);    // same as above
//		int a = 1;
//		string sqlLobbyFilter = "C0 =" + ( player.curCharacter.GetComponent<CharacterStats>().characterLevel + cnt);   // find a game with mode 0
//		PhotonNetwork.JoinRandomRoom(null, 4, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
//	}
//
//	public void CreateAMatch(){
//
//		RoomOptions newRoomOptions = new RoomOptions();
//		newRoomOptions.isOpen = true;
//		newRoomOptions.isVisible = true;
//		newRoomOptions.maxPlayers = 2;
//		// in this example, C0 might be 0 or 1 for the two (fictional) game modes
//		newRoomOptions.customRoomProperties = new ExitGames.Client.Photon.Hashtable() {
//			{ "C0", player.curCharacter.GetComponent<CharacterStats>().characterLevel }
//		};
//		newRoomOptions.customRoomPropertiesForLobby = new string[] { "C0" }; // this makes "C0" available in the lobby
//		
//		// let's create this room in SqlLobby "myLobby" explicitly
//		int asd = Random.Range(0,100000);
//		TypedLobby sqlLobby = new TypedLobby("myLobby", LobbyType.SqlLobby);
//		PhotonNetwork.CreateRoom(asd.ToString(), newRoomOptions, sqlLobby);
//		ConnectionText.text = newRoomOptions.customRoomProperties.ToString();
//	}
//}