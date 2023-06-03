using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LogPanel : MonoBehaviour 
{
    public GameObject myGameObject;
    public Animator myAnimator;
    public Text text;
    public float stayDuration;
    public string[] formats, formatsEN;
    public Color blueTeam ;
    public Color redTeam ;
    public string waitMessageFa, waitMessageEn;
    public string pingMessageFa, pingMessageEn;

    List<string> queue = new List<string>();


    public void SubmitText(string username, LogPanelMessages message)
    {

		if (GameplayDefaultSettings.Instance.isTeamMatch) {
			switch (ScrCarController.Instance.Owner.GetTeam ()) {
			case PunTeams.Team.none:
				break;
			case PunTeams.Team.red:
				this.text.color = redTeam;
				break;
			case PunTeams.Team.blue:
				this.text.color = blueTeam;
				break;
			}
		}

        string text = "";
        if (SettingData.LanguageIndex == 0) // english
            text = formatsEN[(int)message].Replace("*", username);
        else
            text = formats[(int)message].Replace("*", username);

        SubmitText(text);
    }

    public void SubmitText(string text)
    {
        if (myGameObject.activeSelf)
            queue.Add(text);
        else
        {
            this.text.text = text;
            myGameObject.SetActive(true);

            if (myGameObject.activeInHierarchy)
            {
                time = stayDuration;
                enabled = true;
            }
        }
    }

    public void ShowWaitingMessage()
    {
        if (SettingData.LanguageIndex == 0)
            SubmitText(waitMessageEn);
        else
            SubmitText(waitMessageFa);
    }

    float time;
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            enabled = false;
            Close();
        }
    }

    public void Close()
    {
        myAnimator.SetTrigger("deactivate");
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);

        if (queue.Count > 0)
        {
            SubmitText(queue[0]);
            queue.RemoveAt(0);
        }
    }

    public void DeactivateAndClearQueue()
    {
        queue.Clear();
        myGameObject.SetActive(false);
    }

    public void SubmitPingAlarm()
    {
        if (SettingData.LanguageIndex == 0)
            SubmitText(pingMessageEn);
        else
            SubmitText(pingMessageFa);
    }
}

public enum LogPanelMessages
{
    RocketHit,
    MineHit,
    TrainCome,
    WaitingForOtherPlayers,
    YouKilled,
    WormCome,
	YouAreInBlueTeam,
	YouAreInRedTeam
}