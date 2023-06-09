using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : Photon.MonoBehaviour 
{
    void OnGUI()
    {
        if (InputToEvent.goPointedAt != null)
        {
            PhotonView pv = InputToEvent.goPointedAt.GetPhotonView();
            if (pv != null)
            {
                GUI.Label(new Rect(Input.mousePosition.x + 5, Screen.height - Input.mousePosition.y - 15, 300, 30), string.Format("ViewID {0} InstID {1} Lvl {2} {3}", pv.viewID, pv.instantiationId, pv.prefix, (pv.isSceneView) ? "scene" : (pv.isMine) ? "mine" : "owner: " + pv.ownerId));
            }
        }
    }

}
