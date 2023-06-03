using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitForOtherPlayersWindow : MonoBehaviour 
{
    public GameObject myGameObject, waitingGameObject, startingGameObject;
    public Text textEN, textFA, gameStartingTextEN, gameStartingTextFA;
    public string ENFormat, FAFormat, startFAFormat, startEnFormat;


    int part;
    int howManyMorePlayersNeeded;

	void Update () 
    {
        _CalculateRemainingPlayers();
	}

    void _CalculateRemainingPlayers()
    {
        if (ScrController.Instance == null || Server.Instance == null)
            return;

        if (part == 1)
        {
            howManyMorePlayersNeeded = GameplayDefaultSettings.Instance.MinimumPlayersToStartGame - ScrController.Instance.GetCarsCount();
            textEN.text = string.Format(ENFormat, howManyMorePlayersNeeded);
            textFA.text = string.Format(FAFormat, howManyMorePlayersNeeded);

            if (Server.Instance.GameSessionState == GameSessionState.GameStarting)
            {
                part = 2;

                waitingGameObject.SetActive(false);
                startingGameObject.SetActive(true);
            }
        }
        else
        {
            int timeRemaining = (int)(Server.Instance.GameStartTime - Server.Instance.ServerTimeForShow);
            gameStartingTextEN.text = string.Format(startEnFormat, timeRemaining);
            gameStartingTextFA.text = string.Format(startFAFormat, timeRemaining);
        }
    }



    public void Activate()
    {
        part = 1;
        waitingGameObject.SetActive(true);
        startingGameObject.SetActive(false);

        _CalculateRemainingPlayers();
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}