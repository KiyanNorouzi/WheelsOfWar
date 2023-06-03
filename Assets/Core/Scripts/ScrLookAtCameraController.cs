using UnityEngine;
using System.Collections;

public class ScrLookAtCameraController : MonoBehaviour {

    private Transform tr;

    public bool xFrezae;
    public bool yFrezae;
    public bool zFrezae;

    void Awake()
    {
        tr = transform;
    }

    void Update()
    {
        if (MainCameraMover.Instance)
        {
            Vector3 lastRot = tr.eulerAngles;

            tr.LookAt(MainCameraMover.Instance.transform.position);
            tr.eulerAngles = new Vector3(xFrezae ? lastRot.x : tr.eulerAngles.x, yFrezae ? lastRot.y : tr.eulerAngles.y, zFrezae ? lastRot.z : tr.eulerAngles.z);
        }
    }
}
