using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankMedal : MonoBehaviour 
{
    public Text rankText;
    public Animator myAnimator;
    public AudioStruct audioStruct;



    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip increaseLevel, decreaseLevel;
    }


    void Start()
    {
        playerRank = -1;
        rankText.text = "-";
    }

    public void GameStarted()
    {
        Refresh();
    }

    int playersNumber;
    ScrCarController[] cars;

    public void Refresh()
    {
        cars = ScrController.Instance.GetCars();
        playersNumber = cars.Length;


        int[] scores = new int[playersNumber];
        int[] ranks = new int[playersNumber];

        int newRank = 1;
        int myIndex = 0;

        for (int i = 0; i < playersNumber; i++)
        {
            if (!cars[i].nv.isMine && cars[i].IsDropped)
                scores[i] = -1; // cars[i].Score;
            else
                scores[i] = cars[i].Score;

            ranks[i] = i;
            if (cars[i] == ScrCarController.Instance)
                myIndex = i;
        }

        /*
        int temp;
        for (int i = 0; i < scores.Length; i++)
        {
            for (int j = 0; j < scores.Length - 1; j++)
            {
                if (scores[j] < scores[j + 1])
                {
                    temp = scores[j + 1];
                    scores[j + 1] = scores[j];
                    scores[j] = temp;

                    temp = ranks[j + 1];
                    ranks[j + 1] = ranks[j];
                    ranks[j] = temp;

                    if (j == newRank)
                        newRank = j + 1;

                    if (j + 1 == newRank)
                        newRank = j;
                }
            }
        }*/

        for (int i = 0; i < scores.Length; i++)
        {
            if (i != myIndex)
            {
                if (scores[i] > scores[myIndex])
                    newRank++;
            }
        }


        //Debug.Log("last rank=" + playerRank + ", new rank=" + newRank);


        if (playerRank == -1 || playerRank == newRank)
        {
            playerRank = newRank;
            rankText.text = playerRank.ToString();
        }
        else
        {
            if (newRank < playerRank)
            {
                audioStruct.Play(audioStruct.increaseLevel);
                myAnimator.SetTrigger("increase");
            }

            else
            {
                audioStruct.Play(audioStruct.decreaseLevel);
                myAnimator.SetTrigger("decrease");
            }

            playerRank = newRank;
        }
    }


    public int playerRank;
    void AnimDone()
    {
        rankText.text = playerRank.ToString();
    }
}