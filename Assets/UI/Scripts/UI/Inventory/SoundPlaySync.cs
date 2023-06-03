using UnityEngine;
using System.Collections;

public class SoundPlaySync : MonoBehaviour 
{
    public AudioSource audioLoop;


    void Start()
    {
        audioLoop.playOnAwake = audioLoop.loop = false;
    }

    void _Play()
    {
        audioLoop.Play();
    }
}