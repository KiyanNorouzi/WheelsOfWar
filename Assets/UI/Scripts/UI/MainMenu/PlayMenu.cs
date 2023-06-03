using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayMenu : MonoBehaviour 
{
    public GameObject myGameObject, loadingOverlayGameObject;
    public GameObject[] pages;

    public Button backButton;
    public Text serverName, loadingMessageText;


    public ServerInfoSlot sample;
    public GameObject serverInfoPrefab;
    public int count;
    public RectTransform contentTransform;
    public GameObject noServersGameObject;
    public GameObject nullServerNameErrorGameObject, offensiveServernameGameObject, englishLettersPleaseErrorGameObject;
    public GameObject filterRoomsGameObject, createTestRoomGameObject;
    public Toggle[] filterRoomsToggles;
    public Toggle createTestToggle, isPrivateRoomToggle;
    public PasswordPanel passwordPanel;

    ServerInfoSlot[] serverSlots;

    bool autoConnect;


    int pageIndex;
    public int PageIndex
    {

        get { return pageIndex; }
        private set 
        {
            pageIndex = value;
            //backButton.interactable = (pageIndex != 0);

            if (pageIndex == -1)
                CloseButton_Clicked();
            else
            {
                for (int i = 0; i < pages.Length; i++)
                    pages[i].SetActive(pageIndex == i);

                if (pageIndex == 2)
                {
                    RefreshServersList();
                }

                if (pageIndex == 3)
                {
                    englishLettersPleaseErrorGameObject.SetActive(false);
                    nullServerNameErrorGameObject.SetActive(false);
                    offensiveServernameGameObject.SetActive(false);
                }
            }
        }
    }

    int serversFound;
    public int ServersFound
    {
        get { return serversFound; }
    }



    bool dontRefreshList;
    void Start()
    {

        dontRefreshList = true;

        if (Debug.isDebugBuild)
        {
            filterRoomsGameObject.SetActive(true);
            createTestRoomGameObject.SetActive(true);

            createTestToggle.isOn = true;

            filterRoomsToggles[0].isOn = false;
            filterRoomsToggles[1].isOn = false;
            filterRoomsToggles[2].isOn = true;
        }
        else
        {
            filterRoomsGameObject.SetActive(false);
            createTestRoomGameObject.SetActive(false);

            createTestToggle.isOn = false;

            filterRoomsToggles[0].isOn = false;
            filterRoomsToggles[1].isOn = true;
            filterRoomsToggles[2].isOn = false;
        }

        dontRefreshList = false;
		
        if (count > 0)
        {
            serverSlots = new ServerInfoSlot[count];
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                    serverSlots[i] = sample;
                else
                {
                    serverSlots[i] = ((GameObject)Instantiate(serverInfoPrefab)).GetComponent<ServerInfoSlot>();
                    serverSlots[i].name = serverSlots[0].name + i.ToString();
                    serverSlots[i].myTransform.SetParent(serverSlots[0].myTransform.parent);
                    serverSlots[i].myTransform.anchoredPosition = serverSlots[0].myTransform.anchoredPosition + new Vector2(0, i * -100);
                    serverSlots[i].myTransform.localScale = Vector3.one;
                }

                serverSlots[i].OnServerSelect += ServerSelected;
                serverSlots[i].Deactivate();
            }
        }

        
    }

    void OnEnable()
    {
        Server.Instance.OnServerStateChanged += Instance_OnServerStateChanged;
    }

    void OnDisable()
    {
        Server.Instance.OnServerStateChanged -= Instance_OnServerStateChanged;
    }


    void Instance_OnServerStateChanged(ServerState currentState)
    {
		switch (currentState)
        {
            case ServerState.Connecting:
                ShowPage(ServerMenuPages.ConnectingPage);
			break;
            case ServerState.InLobby:
				if (autoConnect)
                {
                    autoConnectTries = 0;
                    AutoConnectToARoom();
                }
                else
                {
                    EndShowLoading();
                    ShowPage(ServerMenuPages.StartJoinServerPage);
                }
                
                break;
            case ServerState.CreatingRoom:
                //GameplayUI.Instance.newsWall.SubmitText("Creating room [" + roomName + "]..."); // + roomName + "]...");
                break;
            case ServerState.JoiningTheRoom:
                //GameplayUI.Instance.newsWall.SubmitText("Joining room [" + roomName + "]..."); // + roomsCount.ToString());
                break;

            case ServerState.InRoom:
                //GameplayUI.Instance.newsWall.SubmitText("Done. Ready to spawn!");
                Deactivate();
                break;
        }
    }

    int autoConnectTries;
    private void AutoConnectToARoom()
    {
        RoomInfo[] rooms = Server.Instance.GetRooms();
        Debug.Log("Rooms Count : " + rooms.Length);

        if (rooms.Length == 0)
        {
            autoConnectTries++;
            if (autoConnectTries < 5)
            {
                Invoke("AutoConnectToARoom", 1);
                return;
            }
        }


        bool foundRoom = false;

        for (int j = 0; j < 2; j++)
        {
            int levelDiff = (j == 0) ? 2 : 5;
            for (int i = 0; i < rooms.Length; i++)
            {
                if (ShouldShowThisRoom(rooms[i].name, levelDiff) && rooms[i].playerCount < 6)
                {
                    //Debug.Log ("On server6");
                    _JoinRoom(rooms[i].name, rooms[i].name.Substring(rooms[i].name.IndexOf("-")));
                    foundRoom = true;
                    break;
                }
            }

            if (foundRoom)
                break;
        }

        if (!foundRoom)
            _CreateServer(string.Concat("Room", Random.Range(10000, 99999)), false);
    }

    public void ShowPage(ServerMenuPages page)
    {
        PageIndex = (int)page;
    }


    public void Activate()
    {
        PageIndex = 0;

        loadingOverlayGameObject.SetActive(false);
        myGameObject.SetActive(true);

        passwordPanel.Deactivate();
        isPrivateRoomToggle.isOn = false;

        Server.Instance.ConnectToServer();
        autoConnect = !Debug.isDebugBuild;
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

	
	public void CloseButton_Clicked()
    {
        myGameObject.SetActive(false);
    }

    public void BackButton_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        switch (PageIndex)
        {
            case 0:
            case 1:
                CommonUI.Instance.messageBox.Ask(Messages.BackToMainMenu, _BackToMainMenu, null, true);
                break;
            case 2:
            case 3:
                PageIndex = 1;
                break;
        }
    }

    void _BackToMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    public void NextButton_Clicked()
    {
        Debug.Log("next");
    }


    /*
    int waitState = 0;
    void Update()
    {
        switch (waitState)
        {
            case 1:
                if (CommonUI.Instance.IsServerReady)
                {
                    loadingOverlayGameObject.SetActive(false);
                    waitState = 0;

                    PageIndex = 1;
                }
                break;
        }
    }
     */

    public void ShowLoadingAndMessage(string message)
    {
        loadingMessageText.text = message;
        loadingOverlayGameObject.SetActive(true);
    }

    public void EndShowLoading()
    {
        loadingOverlayGameObject.SetActive(false);
    }


    #region Page 1 [Select Map]

    public void MapSelected(int mapIndex)
    {
        PageIndex = 1;
    }

    #endregion

    #region Page 2 [Start/Join Server]

    public void CreateServer()
    {
        PageIndex = 3;
        CommonUI.Instance.PlayButtonClick();
    }

    public void JoinServer()
    {
        PageIndex = 2;
        CommonUI.Instance.PlayButtonClick();
    }

    #endregion

    #region Page 3 [Servers List]


    public void Filter_Changed(int index)
    {
        if (!dontRefreshList && filterRoomsToggles[index].isOn)
            RefreshServersList();
    }

    ServerInfoSlot selectedSlot;
    void ServerSelected(ServerInfoSlot slot)
    {
        CommonUI.Instance.PlayButtonClick();
        
        if (slot.IsPasswordProtected)
        {
            selectedSlot = slot;
            passwordPanel.Activate(_PasswordCollectedForJoin, null);
        }
        else
        {
            _JoinRoom(slot.PhysicalServerName, slot.ServerName);
        }
    }


    void _PasswordCollectedForJoin()
    {
        if (selectedSlot.Password == passwordPanel.Password)
            _JoinRoom(selectedSlot.PhysicalServerName, selectedSlot.ServerName);
        else
            CommonUI.Instance.messageBox.ShowMessage(Messages.WrongPassword, null, true);
    }

    void _JoinRoom(string realName, string displayName)
    {
        Server.Instance.JoinToRoom(realName);
        ShowLoadingAndMessage("Joining room [" + displayName + "]...");
    }

    public void RefreshServersList()
    {
        serversFound = 0;
        CommonUI.Instance.PlayButtonClick();

        RoomInfo[] rooms = Server.Instance.GetRooms(); // PhotonNetwork.GetRoomList();

        int lastActiveServerSlotIndex = -1;
        int roomIndex = 0;
        for (int i = 0; i < serverSlots.Length; i++)
        {
            if (roomIndex < rooms.Length)
            {
                //Debug.Log("room " + i + "=" + rooms[roomIndex].name);

                //Debug.Log("should show room " + rooms[roomIndex].name);
                if (!ShouldShowThisRoom(rooms[roomIndex].name))
                {
                    //Debug.Log("--- shouldnot show");
                    i--;
                }
                /*if (!rooms[roomIndex].name.EndsWith(EnvironmentController.Instance.map.ToString()))
                {
                    i--;
                }*/
                else
                {
                    serverSlots[i].Activate(rooms[roomIndex]);
                    lastActiveServerSlotIndex = i;

                    serversFound++;

                    

                    /*
                    if (Accounting.Instance.currentUser.Level == 1)
                    {
                        if (rooms[roomIndex].name.StartsWith("1-") && rooms[roomIndex].playerCount < rooms[i].maxPlayers)
                        {
                            serverSlots[i].Activate(rooms[roomIndex]);
                            lastActiveServerSlotIndex = i;
                        }
                        else
                            i--;

                        serversFound++;
                    }
                    else
                    {
                        if (rooms[roomIndex].name.StartsWith("2-") && rooms[roomIndex].playerCount < rooms[i].maxPlayers)
                        {
                            serverSlots[i].Activate(rooms[roomIndex]);
                            lastActiveServerSlotIndex = i;
                        }
                        else
                            i--;

                        serversFound++;
                    }*/
                }
            }
            else
                serverSlots[i].Deactivate();

            roomIndex++;
        }

        if (lastActiveServerSlotIndex == -1)
        {
            noServersGameObject.SetActive(true);
            contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        }
        else
        {
            noServersGameObject.SetActive(false);

            int lastOne = lastActiveServerSlotIndex;
            contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(serverSlots[lastOne].myTransform.anchoredPosition.y) + 150);
        }
    }


    private bool ShouldShowThisRoom(string roomName, int levelDifference = 5)
    {
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
            isTeamMatch = (properties[5] == "team");
        }
        else
        {
            int arrowIndex = roomName.IndexOf(">") + 1;

            mapName = roomName.Substring(arrowIndex, roomName.IndexOf("-", arrowIndex) - arrowIndex);
            roomLevel = int.Parse(roomName.Substring(0, 1));
        }


        if (mapName != EnvironmentController.Instance.map.ToString())
            return false;

        if (isTeamMatch != GameplayDefaultSettings.Instance.isTeamMatch)
            return false;

        if (Accounting.Instance.currentUser.Level == 1)
        {
            if (roomLevel != 1)
                return false;
        }
        else
        {
            if (roomLevel == 1)
                return false;

            int diff = Mathf.Abs(Accounting.Instance.currentUser.Level - roomLevel);
            if (diff > levelDifference)
                return false;
        }


        if (filterRoomsToggles[2].isOn) // all
			return true;
        else if (filterRoomsToggles[0].isOn) // only development rooms
            return idDevRoom;
        else
            return !idDevRoom;
    }

    #endregion

    #region Page 4 [Start Server]

    public void ServerCreate()
    {
        CommonUI.Instance.PlayButtonClick();

        if (string.IsNullOrEmpty(serverName.text))
        {
            englishLettersPleaseErrorGameObject.SetActive(false);
            offensiveServernameGameObject.SetActive(false);
            nullServerNameErrorGameObject.SetActive(true);
            return;
        }

        NameValidationError err = GeneralSettings.ValidateName(serverName.text);
        if (err != NameValidationError.None)
        {
            offensiveServernameGameObject.SetActive(false);
            nullServerNameErrorGameObject.SetActive(false);
            englishLettersPleaseErrorGameObject.SetActive(true);
            return;
        }

        if (GeneralSettings.IsOffensive(serverName.text))
        {
            englishLettersPleaseErrorGameObject.SetActive(false);
            nullServerNameErrorGameObject.SetActive(false);
            offensiveServernameGameObject.SetActive(true);
            return;
        }



        if (isPrivateRoomToggle.isOn)
            passwordPanel.Activate(_PasswordCollectedForCreate, null);
        else
            _CreateServer(serverName.text, createTestToggle.isOn);
    }

    private void _CreateServer(string roomName, bool isTestRoom, bool isPasswordProtected = false, string password = "")
    {
        System.Text.StringBuilder serverName = new System.Text.StringBuilder();


        serverName.Append(roomName);
        serverName.Append("-");
        serverName.Append(EnvironmentController.Instance.map);
        serverName.Append("-");
        serverName.Append(createTestToggle.isOn ? "dev" : "user");
        serverName.Append("-");
        serverName.Append(isPasswordProtected ? "private" : "public");
        serverName.Append("-");
        serverName.Append("lvl");
        serverName.Append(Accounting.Instance.currentUser.Level);
        serverName.Append("-");
        serverName.Append(GameplayDefaultSettings.Instance.isTeamMatch ? "team" : "ffa");
        
        Server.Instance.CreateRoom (serverName.ToString());
        //Server.Instance.CreateRoom("roomdefault");
        ShowLoadingAndMessage("Creating room [" + roomName + "]...");
    }

    void _PasswordCollectedForCreate()
    {
        _CreateServer(serverName.text, createTestToggle.isOn, true, passwordPanel.Password);
    }
    

    #endregion
}

public enum ServerMenuPages
{
    ConnectingPage,
    StartJoinServerPage,
    ServersListPage,
    StartServerPage
}