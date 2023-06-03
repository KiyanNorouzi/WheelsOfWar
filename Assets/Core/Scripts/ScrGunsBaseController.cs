using UnityEngine;
using System.Collections;

public class ScrGunsBaseController : MonoBehaviour 
{

    public delegate void bulletCountChanged(int currentNumber);
    public event bulletCountChanged OnBulletCountChanged;

    public Transform BulletPos;
    //public float damage;
    public ScrCarController parent;


    private int bullets;




    GunInfo gunInfo;
    public GunInfo GunInfo
    {
        get { return gunInfo; }
        set 
        { 
            gunInfo = value;
            SetSettings();
        }
    }

    MachineGunInfo machineGunInfo;
    public MachineGunInfo MachineGunInfo
    {
        get { return machineGunInfo; }
        set 
        { 
            machineGunInfo = value;
            SetSettings();
        }
    }



    public int Bullets
    {
        set
        {
            bullets = value;

            if (OnBulletCountChanged != null)
                OnBulletCountChanged(bullets);
        }
        get
        {
            return bullets;
        }
    }

    public virtual void SetSettings()
    { }

    public virtual void Fire()
    { 
    
    }

    public virtual void SetFireState(bool v)
    {
    }

    public virtual void InitBullets()
    { 
    
    }


    public virtual void DeactivateAllBullets()
    {
    }
}
