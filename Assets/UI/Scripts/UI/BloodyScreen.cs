using UnityEngine;
using System.Collections;

public class BloodyScreen : MonoBehaviour 
{
    bool isActive;


    public BloodFrame[] frames;
    public AudioStruct audioPlayer;
    

    public void Activate(float shockPower)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            if (frames[i].IsActive)
                return;
        }

        if (shockPower < 5)
            return;

        GameplayUI.Instance.Viberate();


        if (shockPower < 15)
        {
            int random = Random.Range(0, 3);
            frames[random].Activate(0.5f);

            audioPlayer.PlaySound1();
        }
        else if (shockPower < 30)
        {
            int random1 = Random.Range(0, 3);
            frames[random1].Activate(1f);

            if (random1 != 3)
            {
                int random2 = Random.Range(0, 3);
                frames[random2].Activate(1f);
            }

            audioPlayer.PlaySound2();
        }
        else if (shockPower > 5 && shockPower < 1000)
        {
            for (int i = 0; i < 2; i++)
                frames[i].Activate(2f);

            if (Random.value < 0.5f)
                frames[2].Activate(2f);
            else
                frames[3].Activate(2f);

            audioPlayer.PlaySound3();
        }
    }

    public void Deactivate()
    {
        for (int i = 0; i < frames.Length; i++)
            frames[i].Deactivate();
    }



    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip sound1, sound2, sound3, sound4;
        public AudioSource player2;

        public void PlaySound1()
        {
            Play(sound1);
        }

        public void PlaySound2()
        {
            Play(sound2);
        }

        public void PlaySound3()
        {
            Play(sound3);

            player2.clip = sound4;
            player2.Play();
        }
    }
}