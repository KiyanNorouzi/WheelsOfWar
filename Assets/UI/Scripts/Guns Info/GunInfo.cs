[System.Serializable]
public class GunInfo
{
    public float DPS
    {
        get
        {
            return ((damageMin + damageMax) / 2) * coolDownTime;
        }
    }

    public string nameEN, nameFA;
    public float damageMin, damageMax;
    public float coolDownTime;
    public float explosionArea;
    public float Range; 

}



[System.Serializable]
public class MachineGunInfo
{

    public float DPS
    {
        get
        {
            return (damageMin + damageMax) / 2;
        }
    }

    public string nameEN, nameFA;
    public float damageMin, damageMax;
    public float fireRate;
    public float tolerance;
    public float range;

}