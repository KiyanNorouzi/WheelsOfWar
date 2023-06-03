using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
    public GameObject timerGameObject;
    public GameObject scoreboardGameObject;
    public GameObject waitOtherPlayersPanelGameObject;
    public GameObject machineGunGameObject, rocketButtonGameObject, mineButtonGameObject;
    public GameObject accelerateButtonGameObject, brakeButtonGameObject;
    public GameObject pauseButtonGameObject;
    public GameObject waitingSectionGameObject;


    public bool TimerEnabled
    {
        get { return timerGameObject.activeSelf; }
        set { timerGameObject.SetActive(value); }
    }

    public bool ScoreboardEnabled
    {
        get { return scoreboardGameObject.activeSelf; }
        set { scoreboardGameObject.SetActive(value); }
    }

    public bool WaitOtherPlayersPanelEnabled
    {
        get { return waitOtherPlayersPanelGameObject.activeSelf; }
        set { waitOtherPlayersPanelGameObject.SetActive(value);
        Debug.Log("wait for other=" + value);
        }
    }

    public bool MachineGunEnabled
    {
        get { return machineGunGameObject.activeSelf; }
        set { machineGunGameObject.SetActive(value); }
    }

    public bool RocketEnabled
    {
        get { return rocketButtonGameObject.activeSelf; }
        set { rocketButtonGameObject.SetActive(value); }
    }

    public bool MineEnabled
    {
        get { return mineButtonGameObject.activeSelf; }
        set { mineButtonGameObject.SetActive(value); }
    }

    public bool AccelerateEnabled
    {
        get { return accelerateButtonGameObject.activeSelf; }
        set 
        { 
            accelerateButtonGameObject.SetActive(value);
            GameplayUI.Instance.accelerometer.AccelerateButton_PointerUp();
        }
    }

    public bool BrakeEnabled
    {
        get { return brakeButtonGameObject.activeSelf; }
        set 
        { 
            brakeButtonGameObject.SetActive(value);
            GameplayUI.Instance.accelerometer.BrakeButton_PointerUp();
        }
    }

    public bool PauseEnabled
    {
        get { return pauseButtonGameObject.activeSelf; }
        set { pauseButtonGameObject.SetActive(value); }
    }

    public bool WaitingForOtherPlayersEnabled
    {
        get { return waitingSectionGameObject.activeSelf; }
        set { waitingSectionGameObject.SetActive(value); }
    }
}