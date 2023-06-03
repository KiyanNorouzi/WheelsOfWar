using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour 
{
    public Animator myAnimator;
    public GameObject myGameObject;
    public AudioStruct audioStruct;
    public float spawnTime, alarmTime, startDelayTime;


    float lastSpawnTime;
    bool alarmPlayed;
	
	
    void Start()
    {
    }

	void Update() 
    {
        if (Server.Instance.GameTime != -1)
        {
            if (lastSpawnTime + spawnTime + startDelayTime - alarmTime <= Server.Instance.GameTime)
            {
                startDelayTime = 0;

                lastSpawnTime = Server.Instance.GameTime;
                StartCoroutine(MoveAfterPlayingAlarm());
            }
        }
	}

    IEnumerator MoveAfterPlayingAlarm()
    {
        audioStruct.player.volume = audioStruct.alarmVol;
        audioStruct.Play(audioStruct.alarm);

        if (GameplayUI.Instance != null)
        {
            //GameplayUI.Instance.newsWall.SubmitText("*", ExtraSigns.Train);
            GameplayUI.Instance.logPanel.SubmitText("", LogPanelMessages.TrainCome);
        }

        yield return new WaitForSeconds(alarmTime);

        audioStruct.player.volume = audioStruct.loopVol;
        audioStruct.Play(audioStruct.loop, true);

        myAnimator.SetTrigger("move");
    }


    void _TrainReachedEndOfThePath()
    {
        audioStruct.player.Stop();
    }


    [System.Serializable]
    public class AudioStruct: AudioStructBase
    {
        public AudioClip alarm, loop;
        public float alarmVol, loopVol;
    }
}