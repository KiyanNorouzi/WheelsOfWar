using UnityEngine;
using System.Collections;

public class ScrBombGunController : MonoBehaviour {

    public Transform BulletPos;
    //public ScrTouchPadController toch;

    public void Start()
    { 
       //toch=GameObject.FindGameObjectWithTag("Player").GetComponent<ScrTouchPadController>();
       //toch.miner = this;
    }

    public void Respawn()
    {
        ScrBombManager.CreateBomb(BulletPos);
    }

}
