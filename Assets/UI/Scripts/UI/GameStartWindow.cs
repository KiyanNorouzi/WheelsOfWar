using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartWindow : MonoBehaviour 
{
    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip beep, lastBeep;
    }


    public AudioStruct audioPlayer;
    public GameObject myGameObject;
    public Text secondsText;

    int seconds;

	public void Activate()
    {
        if (ScrCarController.Instance == null)
            return;

        CommonUI.Instance.menuMusicManager.Stop(1, 0.25f);


        if (Time.time - ScrCarController.Instance.LastResoawnTime > 3)
        {
            int myID = ScrController.Instance.GetMyCarID();
            ScrCarController.Instance.Respawn(myID);

            ScrCarController.Instance.rocketLauncher.Bullets = GameplayDefaultSettings.Instance.Settings.defaultRocketsCount;
            ScrCarController.Instance.miner.Bullets = GameplayDefaultSettings.Instance.Settings.defaultMinesCount;
            
            if (!GameplayUI.IsTutorial)
                GameplayUI.Instance.AddBoosters();
        }
            
        /*else
            ScrCarController.Instance.Init();*/

        seconds = 3;
        secondsText.text = "3";
        myGameObject.SetActive(true);

        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        GameplayUI.Instance.SetHUDActive(false);

        audioPlayer.Play(audioPlayer.beep);

        for (int i = 2; i >= 1; i--)
        {
            yield return new WaitForSeconds(1);
            seconds--;
            secondsText.text = seconds.ToString();

            if (i == 1)
                audioPlayer.Play(audioPlayer.lastBeep);
            else
                audioPlayer.Play(audioPlayer.beep);
        }

        yield return new WaitForSeconds(1);

        myGameObject.SetActive(false);
        GameplayUI.Instance.SetHUDActive(true);

        CommonUI.Instance.menuMusicManager.musics[1].SetVolume(1);
        CommonUI.Instance.menuMusicManager.Play(1);
    }
}