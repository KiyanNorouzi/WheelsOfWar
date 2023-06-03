using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour
{
    #region Singleton

    static Server _instance;
    public static Server Instance
    {
        get { return Server._instance; }
    }

    void Awake()
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.Disconnect();

        _instance = this;
    }

    void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }


    #endregion

    float serverTime;
    int serverTimeForShow = -1, gameStartTime;


    public int GameStartTime
    {
        get { return gameStartTime; }
        set 
        { 
            gameStartTime = value;
        }
    }

    public int ServerTimeForShow
    {
        get { return serverTimeForShow; }
        set { serverTimeForShow = value; }
    }

    public float ServerTime
    {
        get { return serverTime; }
        set 
        { 
            serverTime = value;
        }
    }

    public const int maxPlayerPerRoom = 6;


    //public static RoomInfo hostDataNow;
    //public static string roomName = "127.0.0.1";
    //public static bool createRoom = true;
    public PhotonView nv;
    public delegate void serverStateChanged(ServerState currentState);
    public event serverStateChanged OnServerStateChanged;
    public event Data.generalDelegate OnLeftTheRoom;

    public string uniqueGameName = "WarMachine"; //You have to change this!
    public float refreshTime = 10; //After which time will the server list be refreshed ?
  



    ScrServerController serverController;
    public ScrServerController ServerController
    {
        get { return serverController; }
    }

    RoomInfo[] rooms;


    ServerState state;
    public ServerState State
    {
        get { return state; }
        private set
        {
            state = value;
            if (state != ServerState.None && OnServerStateChanged != null)
                OnServerStateChanged(state);
        }
    }

    public int RemainingTime
    {
        get
        {
            if (GameStartTime == -1)
                return -1;
            else
            {
                int time = ServerTimeForShow - GameStartTime;
                return GameplayDefaultSettings.Instance.Settings.GameTime - time;
            }
        }
    }



    public float GameTime
    {
        get
        {
            if (GameStartTime == -1)
                return -1;
            else
            {
                return ServerTimeForShow - GameStartTime;
            }
        }
    }

    GameSessionState gameSessionState;
    public GameSessionState GameSessionState
    {
        get { return gameSessionState; }
    }

    public bool IsGameStartedYet
    {
        get { return gameSessionState == global::GameSessionState.GamePlaying; } // gameStartTime != -1; }
    }

    public bool IsRoomStillOpen
    {
        get { return PhotonNetwork.room.visible; }
    }

    bool isLevel1Game;
    public bool IsLevel1Game
    {
        get { return isLevel1Game; }
    }



    void Start()
    {
        //serverTime = ScoreSettings.Instance.GameTime;
        ServerTime = 0;
        ServerTimeForShow = -1;
        GameStartTime = -1;
        gameSessionState = global::GameSessionState.FreeRide;


        //MoneyTablet.Instance.State = MoneyTabletViewState.Off;
        State = ServerState.None;
        //nv = GetComponent<PhotonView>();
    }

    public void StartGameIfItIsntAlready()
    {
        if (gameSessionState == global::GameSessionState.FreeRide) // gameStartTime == -1)
        {
            int startGameTime = ServerTimeForShow + GameplayDefaultSettings.Instance.TimeForStartGameAfterLastPlayerJoined;
            //_StartGame(startGameTime);
            nv.RPC("_StartGame", PhotonTargets.AllBuffered, startGameTime);
        }
    }

    [RPC]
    void _StartGame(int time)
    {
        GameStartTime = time;
        gameSessionState = global::GameSessionState.GameStarting;

        if (ServerTimeForShow == -1) // time isn't synced yet
        {
            ServerTimeForShow = -2; // when time synced for the first time, start this method again
        }
        else if (GameStartTime > ServerTimeForShow) // the game is in waiting state
        {
            Invoke("_ReallyStartGame", GameStartTime - ServerTimeForShow);
        }
        else // the game has already begun
        {
            _ReallyStartGame();
        }
    }


    void _ReallyStartGame()
    {
        gameSessionState = global::GameSessionState.GamePlaying;

        GameplayUI.Instance.GameStarted();
        EnvironmentController.Instance.GameStarted();
    }

    public void SealTheRoom()
    {
        PhotonNetwork.room.visible = false;
    }

    public void EndGameSession()
    {
        gameSessionState = global::GameSessionState.Finished;
        
        PhotonNetwork.room.visible = false;
        int cars = ScrController.Instance.GetCarsCount();
		Debug.Log ( "Did" );
        for (int i = 0; i < cars; i++) {
			ScrController.Instance.GetCar (i).nv.RPC ("EndGame", PhotonTargets.AllBuffered);
		}
    }

    [RPC]
    private void EndGameSessionForNotFindingEnoughPlayers()
    {

        gameSessionState = global::GameSessionState.Finished;
        PhotonNetwork.Disconnect();
        CommonUI.Instance.messageBox.ShowMessage(Messages.RoomClosedForNotFindEnoughPlayers, _CloseTheRoom, true);
    }

    void _CloseTheRoom()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void ConnectToServer()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.ConnectUsingSettings("1.0v");

        serverController = new ScrServerController();

        if (PhotonNetwork.isMasterClient || PhotonNetwork.isNonMasterClientInRoom)
            State = ServerState.InLobby;
        else
        {
            PhotonNetwork.JoinLobby();
            State = ServerState.Connecting;
        }
    }

    public void CreateRoom(string roomName, bool isVisible = true)
    {
        //isLevel1Game = (roomName.StartsWith("1-"));

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = maxPlayerPerRoom;
        roomOptions.isVisible = isVisible;

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        State = ServerState.CreatingRoom;
    }


    public void JoinToRoom(string room)
    {
        isLevel1Game = (room.StartsWith("1-"));

        PhotonNetwork.JoinRoom(room);
        rooms = PhotonNetwork.GetRoomList();
        State = ServerState.JoiningTheRoom;
    }

    public RoomInfo[] GetRooms()
    {
        rooms = PhotonNetwork.GetRoomList();
        return rooms;
    }

    void OnJoinedLobby()
    {
        State = ServerState.InLobby;
    }

    /*void OnLeftLobby()
    {
        Debug.Log(" OnLeftLobby ");
    }*/

    void OnLeftRoom()
    {
        State = ServerState.InLobby;

        if (OnLeftTheRoom != null)
            OnLeftTheRoom();
    }

    void OnJoinedRoom()
    {
        State = ServerState.InRoom;

        if (GameplayUI.IsTutorial)
            GameplayUI.Instance.AccelerometerButton_Click();
        else
        {
            if (CommonUI.Instance.IsTutorial && EnvironmentController.Instance.map != Maps.Tutorial)
                GameplayUI.Instance.AccelerometerButton_Click();
            else
            {
                //GameplayUI.Instance.AccelerometerButton_Click();
                GameplayUI.Instance.boosterPanel.Activate();
            }
                
        }
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("photon join room failed");
        CommonUI.Instance.messageBox.ShowMessage(Messages.JoinRoomFailed, null, true);
        State = ServerState.InLobby;
    }

    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("photon create room failed");
        if (!GameplayUI.IsTutorial)
            CommonUI.Instance.messageBox.ShowMessage(Messages.JoinRoomFailed, null, true);
        State = ServerState.InLobby;
    }
    

    float roomsRefreshTime;
    void Update()
    {
        if (PhotonNetwork.insideLobby)
        {
            roomsRefreshTime += Time.deltaTime;
            if (roomsRefreshTime>= refreshTime)
            {
                rooms = PhotonNetwork.GetRoomList();
                roomsRefreshTime = 0;
            }
        }
        

        switch (State)
        {
            case ServerState.InLobby:
                if (PhotonNetwork.isMasterClient || PhotonNetwork.isNonMasterClientInRoom)
                {
                    if (CommonUI.Instance.IsTutorial && EnvironmentController.Instance.map != Maps.Tutorial)
                        GameplayUI.Instance.AccelerometerButton_Click();
                    //else
                        //GameplayUI.Instance.controlMethodSelectionGameObject.SetActive(true);

                    State = ServerState.InRoom;
                    enabled = false;
                }
                break;
        }

        if (PhotonNetwork.connected && PhotonNetwork.isMasterClient && ServerTimeForShow != -1)
        {
            if (IsRoomStillOpen && GameTime >= GameplayDefaultSettings.Instance.SealRoomAfterTime)
                SealTheRoom();
            
            //EndGameSessionForNotFindingEnoughPlayers();
            ServerTimeProcess();
        }

        if (!GameplayUI.IsTutorial &&  PhotonNetwork.connected && PhotonNetwork.isMasterClient && GameSessionState == global::GameSessionState.FreeRide && Time.timeSinceLevelLoad >= GameplayDefaultSettings.Instance.CloseGameAfter)
            nv.RPC("EndGameSessionForNotFindingEnoughPlayers", PhotonTargets.All);
    }


    void ServerTimeProcess()
    {
        ServerTime += Time.deltaTime;

        if (ServerTimeForShow != (int)ServerTime)
        {
            if (ServerTimeForShow == -2)
            {
                ServerTimeForShow = (int)ServerTime;
                _StartGame(GameStartTime);
            }
            else
                ServerTimeForShow = (int)ServerTime;

            nv.RPC("ServerTimeMethod", PhotonTargets.Others, ServerTimeForShow, gameStartTime);
        }
    }

    [RPC]
    void ServerTimeMethod(int TimeNow, int gameStartTime)
    {
        if (ServerTimeForShow == -2)
        {
            serverTime = ServerTimeForShow = TimeNow;
            //_StartGame(GameStartTime);
            _StartGame(GameStartTime);
        }
        else
        {
            serverTime = ServerTimeForShow = TimeNow;
            if (gameSessionState == global::GameSessionState.FreeRide && gameStartTime != -1)
                _StartGame(gameStartTime);
        }
    }

    //[RPC]
    //void UpdatePlayerCount(bool AddtoCount)
    //{ 
    //    if (AddtoCount)
    //    {
    //      //  PlayersCount++;
    //        PlayerCountScript.PlayerCounts++;

    //    }
    //    else
    //    {
    //       // PlayersCount--;
    //        PlayerCountScript.PlayerCounts--;

    //    }
    
    //}
}



public enum ServerState
{
    None,
    Connecting,
    InLobby,
    CreatingRoom,
    JoiningTheRoom,
    InRoom
}

public enum GameSessionState
{
    FreeRide,
    GameStarting,
    GamePlaying,
    Finished
}