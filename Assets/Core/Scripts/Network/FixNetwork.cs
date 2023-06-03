using UnityEngine;
using System.Collections;

public class FixNetwork : Photon.MonoBehaviour
{

    void Awake()
    {
        Debug.Log("fix network");
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        PhotonNetwork.RemoveRPCs(player);
        PhotonNetwork.DestroyPlayerObjects(player);
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (PhotonNetwork.isMasterClient)
            Debug.Log("Local server connection disconnected");
        else
            if (info == NetworkDisconnection.LostConnection)
                Debug.Log("Lost connection to the server");
            else
                Debug.Log("Successfully diconnected from the server");

        Application.LoadLevel("MainMenu");
    }

}
