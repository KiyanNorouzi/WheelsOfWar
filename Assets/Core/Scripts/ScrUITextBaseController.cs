using UnityEngine;
using System.Collections;

public class ScrUITextBaseController : MonoBehaviour {

    void Start()
    {
        transform.parent = null;
        Destroy(this);
    }
}
