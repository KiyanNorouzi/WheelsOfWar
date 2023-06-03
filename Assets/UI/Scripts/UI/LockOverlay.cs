using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LockOverlay : MonoBehaviour 
{
    public GameObject myGameObject;
    public Text[] texts;
    public string[] messagesFA, messagesEN;





    public bool IsActive
    {
        get
        {
            return myGameObject.activeSelf;
        }
    }


    public void Activate(LockOverlayMessages message)
    {
        Activate(messagesEN[(int)message], messagesFA[(int)message]);
    }

    public void Activate(string messageEN, string messageFA)
    {
        texts[0].text = messageEN;
        texts[1].text = messageFA;

        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}

public enum LockOverlayMessages
{
    ConnectingToStore,
    ConnectingToServer,
    WaitingForVideoAd,
}