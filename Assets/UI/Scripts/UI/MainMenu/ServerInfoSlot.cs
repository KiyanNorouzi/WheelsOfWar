using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerInfoSlot : MonoBehaviour 
{
    public delegate void serverSelected(ServerInfoSlot slot);
    public event serverSelected OnServerSelect;

    public GameObject myGameObject, passwordProtectedGameObject;
    public RectTransform myTransform;
    public Text serverNameText, playersText;
    public Image image;
    public Sprite[] mapSprites;


    bool isPasswordProtected;
    public bool IsPasswordProtected
    {
        get { return isPasswordProtected; }
    }

    string password;
    public string Password
    {
        get { return password; }
    }

    string realServerName;


    public string ServerName
    {
        get
        {
            return serverNameText.text;
        }
    }

    public string PhysicalServerName
    {
        get { return realServerName; }
    }


    public void Activate(RoomInfo roomInfo)
    {
        Activate(roomInfo.name, roomInfo.playerCount, roomInfo.maxPlayers);
    }

    public void Activate(string roomName, int currentPlayers, int maxPlayers)
    {
        int index = (int)EnvironmentController.Instance.map;
        if (index > 0)
            index--;



        string[] properties = roomName.Split('-'); /* 0: room name, 1: map, 2: dev/user, 3: private/public, 4: level, 5: team/ffa*/

        bool isTeamMatch = false;
        bool idDevRoom = false;
        string mapName = "";
        int roomLevel = -1;


        if (properties.Length >= 6)
        {
            mapName = properties[1];
            idDevRoom = (properties[2] == "dev");
            roomLevel = int.Parse(properties[4].Substring(3));
            isPasswordProtected = properties[3] == "private";
            isTeamMatch = (properties[5] == "team");
        }
        else
        {
            int arrowIndex = roomName.IndexOf(">") + 1;

            mapName = roomName.Substring(arrowIndex, roomName.IndexOf("-", arrowIndex) - arrowIndex);
            roomLevel = int.Parse(roomName.Substring(0, 1));
            isPasswordProtected = false;
        }





        image.sprite = mapSprites[index];

        realServerName = roomName;

        serverNameText.text = properties[0];
        playersText.text = string.Format("Players: {0}/{1}", currentPlayers, maxPlayers);

        password = "";

        passwordProtectedGameObject.SetActive(isPasswordProtected);
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    public void Clicked()
    {
        if (OnServerSelect != null)
            OnServerSelect(this);
        else
            Debug.Log("null");
    }
}
