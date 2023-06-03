using UnityEngine;
using System.Collections;
using System;

public class TutorialPreparingMenu : MonoBehaviour 
{
    public GameObject myGameObject;





	void Start()
    {
        
    }

    void OnEnable()
    {
        Server.Instance.OnServerStateChanged += Instance_OnServerStateChanged;
    }

    void OnDisable()
    {
        Server.Instance.OnServerStateChanged -= Instance_OnServerStateChanged;
    }

    int trys;
    void Instance_OnServerStateChanged(ServerState currentState)
    {
        switch (currentState)
        {
            case ServerState.Connecting:
                break;
            case ServerState.InLobby:
                trys++;
                if (trys >= 10)
                {
                    CommonUI.Instance.messageBox.ShowMessage(Messages.ServersUnderMaintenance, _ServerDown, true);
                }
                else
                {
                    if (trys == 1)
                        Server.Instance.CreateRoom("0-Tutorial" + Guid.NewGuid(), false);
                    else
                    {
                        Debug.Log("create room failed, retry #" + trys);
                        StartCoroutine(ConnectToServerIn(1));
                    }
                }
                break;
            case ServerState.CreatingRoom:
                break;
            case ServerState.JoiningTheRoom:
                break;
            case ServerState.InRoom:
                Deactivate();
                break;
        }
    }

    IEnumerator ConnectToServerIn(float wait)
    {
        yield return new WaitForSeconds(wait);
        Server.Instance.CreateRoom("0-Tutorial" + Guid.NewGuid(), false);
    }

    private void _ServerDown()
    {
        TutorialStartScene.ServerError = true;
        SceneManager.LoadScene(Scenes.TutorialStartScene);
    }
	
	public void Activate()
    {
        myGameObject.SetActive(true);
        Server.Instance.ConnectToServer();
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}
