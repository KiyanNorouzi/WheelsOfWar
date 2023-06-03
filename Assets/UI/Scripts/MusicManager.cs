using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{
    public Music[] musics;
    public float transitionDuration;
	
    void Start()
    {
        for (int i = 0; i < musics.Length; i++)
            musics[i].Initialize();
	}
	
	void Update()
    {
        for (int i = 0; i < musics.Length; i++)
            musics[i].Update();
	}



    public void Play(int index)
    {
        musics[index].Play();
    }

    public void Stop(int index)
    {
        musics[index].Stop();
    }

    public void SetVolume(int index, float volume)
    {
        musics[index].SetVolume(volume);
    }

    public void Play(int index, float duration)
    {
        musics[index].Play(duration);
    }

    public void Stop(int index, float duration)
    {
        musics[index].Stop(duration);
    }

    public void SetVolume(int index, float volume, float duration)
    {
        musics[index].SetVolume(volume, duration);
    }

    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 90, 40), "Play"))
            musics[0].Play();

        if (GUI.Button(new Rect(110, 10, 90, 40), "Stop"))
            musics[0].Stop();

        if (GUI.Button(new Rect(210, 10, 90, 40), "vol to 0.5"))
            musics[0].SetVolume(0.5f);

        if (GUI.Button(new Rect(310, 10, 90, 40), "vol to 1"))
            musics[0].SetVolume(1);
    }*/
}

[System.Serializable]
public class Music
{
    public AudioSource music;
    public float transitionDuration;


    float defaultVolume;
    float time, duration, flow;

    bool changingVolume;
    float cTime, cDuration, cFlow;

    float startVolume, targetVolume;


    MusicState state = MusicState.Stop;
    public MusicState State
    {
        get { return state; }
    }

    public void SetVolume(float vol)
    {
        SetVolume(vol, transitionDuration);
    }

    public void SetVolume(float vol, float duration)
    {
        if (state == MusicState.Playing)
        {
            startVolume = defaultVolume;
            targetVolume = vol;

            cDuration = duration;
            cTime = 0;
            changingVolume = true;
        }
        else
        {
            this.defaultVolume = vol;
            music.volume = Mathf.Clamp(music.volume, 0, defaultVolume);
        }
    }


    public void Initialize()
    {
        defaultVolume = music.volume;
    }

    public void Play()
    {
        Play(transitionDuration);
    }

    public void Play(float duration)
    {
        time = 0;
        this.duration = duration;

        state = MusicState.GoingToPlay;

        music.volume = 0;

        if (!music.isPlaying)
            music.Play();
    }

    public void Stop()
    {
        Stop(transitionDuration);
    }

    public void Stop(float duration)
    {
        time = 0;
        this.duration = duration;

        state = MusicState.GoingToStop;
    }


    public void Update()
    {
        switch (state)
        {
            case MusicState.GoingToPlay:
                time += Time.deltaTime;
                flow = time / duration;

                if (flow >= 1)
                {
                    music.volume = defaultVolume;
                    state = MusicState.Playing;
                }
                else
                    music.volume = flow * defaultVolume;
                break;

            case MusicState.GoingToStop:
                time += Time.deltaTime;
                flow = time / duration;
                if (flow >= 1)
                {
                    music.Stop();
                    state = MusicState.Stop;
                }
                else
                    music.volume = (1 - flow) * defaultVolume;
                break;
        }

        if (changingVolume)
        {
            cTime += Time.deltaTime;
            cFlow = cTime / cDuration;

            if (cFlow >= 1)
            {
                defaultVolume = targetVolume;
                changingVolume = false;
            }
            else
                defaultVolume = startVolume + (targetVolume - startVolume) * cFlow;

            music.volume = defaultVolume;
        }
    }
}

public enum MusicState
{
    Stop,
    GoingToPlay,
    Playing,
    GoingToStop
}