using UnityEngine;
using System.Collections;

public class ScrServerController
{
    private RoomInfo[] datas;


    private string gameName = "ServerDreamRain";

    //You have to change this!
    public string uniqueGameName = "ClashOfBrains";

    //After which time will the server list be refreshed ?
    public float refreshTime = 2;

    public static RoomInfo roomConnected;

    public bool isServerExistForConnect;

    private MonoBehaviour parent;



    public ScrServerController()
    {
        //this.parent = parent;
        JoinToLobby();
    }

    public void JoinToLobby()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.ConnectUsingSettings("1.0v");
        //roomConnected = PhotonNetwork.room;
    }

    public void GetRoomList()
    {
        datas = PhotonNetwork.GetRoomList();
        isServerExistForConnect = datas.Length > 0 ? true : false;
    }

    public void CreateRoom(string gameName="DreamRainServer")
    {
        ResetDataLastRoom();

        PhotonNetwork.CreateRoom(gameName);
        roomConnected = PhotonNetwork.room;
    }

    public bool Connecting()
    {
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].playerCount < 5)
            {
                ClientConnect(i);
                return false;
            }
        }

        CreateRoom();
        return true;
    }

    public void PlayerLeft(PhotonPlayer player)
    {
        ScrCarController[] cars = ScrController.Instance.GetCars();
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].IsOnLine && cars[i].nv.owner == null)
            {
                cars[i].Deactivate();

                if (SettingData.LanguageIndex == 0) // english
                    GameplayUI.Instance.newsWall.SubmitText(string.Concat(cars[i].Username, " *"), ExtraSigns.Left, true);
                else if (SettingData.LanguageIndex == 1) // persian
                    GameplayUI.Instance.newsWall.SubmitText(string.Concat("* ", cars[i].Username), ExtraSigns.Left, true);
                //GameplayUI.Instance.newsWall.SubmitText("* " + cars[i].Username, ExtraSigns.Left, true);
                break;
            }
        }
    }

    public bool ClientConnect(int num = 0)
    {
        if (datas.Length == 0)
            return false;

        ResetDataLastRoom();
        PhotonNetwork.JoinRoom(datas[num].name);

        return true;
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient || PhotonNetwork.isNonMasterClientInRoom)
        {
            roomConnected = datas[0];
        }

        if (PhotonNetwork.room.playerCount == 2)
            PhotonNetwork.room.visible = false;
    }

    void OnConnectedToMaster()
    {
        //Debug.Log("connected to master");
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        //Debug.Log("player " + otherPlayer.ID + " dropped");
    }

    private void ResetDataLastRoom()
    {
        if (PhotonNetwork.isMasterClient || PhotonNetwork.isNonMasterClientInRoom)
        {
            /*
            PhotonNetwork.RemoveRPCs(ScrPlayerController.player.nv);
            PhotonNetwork.RemoveRPCs(ScrPlayerController.rival.nv);
            
            if (ScrPlayerController.player)
                Object.Destroy(ScrPlayerController.player.gameObject);
            if (ScrPlayerController.rival)
                Object.Destroy(ScrPlayerController.rival.gameObject);
             */
            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        roomConnected = null;
    }
}
