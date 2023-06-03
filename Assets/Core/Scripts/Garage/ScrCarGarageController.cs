using UnityEngine;
using System.Collections;

public class ScrCarGarageController : MonoBehaviour {

    public float speedDown;

    void Awake()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, -speedDown, 0);
    }

}
