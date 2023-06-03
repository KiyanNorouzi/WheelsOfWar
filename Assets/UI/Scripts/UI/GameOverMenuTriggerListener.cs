using UnityEngine;
using System.Collections;

public class GameOverMenuTriggerListener : MonoBehaviour 
{
    public AudioSource audio;

	public void PlaySound()
    {
        audio.Play();
    }
}