using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreRow : MonoBehaviour 
{
    public GameObject myGameObject;
    public Text rankText, usernameText, killsText, deathsText, scoreText, cheatsText;
    public Image highlightLine;
    public Image droppedImage;


    public bool IsActive
    {
        get { return myGameObject.activeInHierarchy; }
    }

	public void Activate(int rank, string username, int kills, int deaths, int score, int cheats, bool isDropped)
    {
        Activate(rank, username, kills, deaths, score, cheats, isDropped, false, Color.black);
    }

    public void Activate(int rank, string username, int kills, int deaths, int score, int cheats, bool isDropped, bool isMine, Color color)
    {
        //rankText.text = rank.ToString();

        usernameText.text = username;
        killsText.text = kills.ToString();
        deathsText.text = deaths.ToString();
        scoreText.text = score.ToString();
        cheatsText.text = cheats.ToString();

        droppedImage.enabled = isDropped;

		if (!GameplayDefaultSettings.Instance.isTeamMatch) {
			if (isMine) {
				highlightLine.enabled = true;
				highlightLine.color = color;
			} else {
				highlightLine.enabled = false;
			}
		}
		else {
			if (isMine) {
				highlightLine.enabled = true;
			} else {
				highlightLine.enabled = false;
			}
		}

        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}