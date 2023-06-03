using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrGunController : ScrGunsBaseController
{
    //public event hitEnemy OnHitEnemy;

    public ScrBulletController[] bulletsPool;
    public static ScrCarController[] cars;

    public AudioSource fireSound;




    public bool singleShot;
    public bool isRocketLauncher;
    public MuzzleFlashStruct muzzleFlash;
    public stateFireEnum stateFire;



    PhotonView nv;
    public PhotonView Nv
    {
        get { return nv; }
    }

    bool isShooting;
    int currentBulletIndex;
    float lastFireTime;


    
    void Start()
    {
        cars = GameObject.FindObjectsOfType<ScrCarController>();
        nv = GetComponent<PhotonView>();

        InitBullet();
    }

    float fireDelay;
    public override void SetSettings()
    {
        if (!isRocketLauncher)
            fireDelay = 1f / MachineGunInfo.fireRate;

    }

    void Update()
    {
        if (!parent.nv.isMine)
            return;

        if (isShooting)
        {
            if (fireDelay + lastFireTime <= Time.time)
            {
                parent.PlayMachineGunSound();

                Fire();
                lastFireTime = Time.time;
            }
        }
    }

    void FixedUpdate()
    {
        if (!parent.nv.isMine)
            return;

        //CheckTarget();
    }

    public override void InitBullets()
    {
        for (int i = 0; i < bulletsPool.Length; i++)
        {
            bulletsPool[i].transform.localPosition = Vector3.zero;
            bulletsPool[i].DamageSystem.isRocket = isRocketLauncher;
        }
    }

    public override void DeactivateAllBullets()
    {
        for (int i = 0; i < bulletsPool.Length; i++)
            bulletsPool[i].KillInstantly();
    }


    public override void SetFireState(bool v)
    {
        if (singleShot)
            Fire();
        else
            isShooting = v;
    }


    IEnumerator MuzzleFlashApply()
    {
        if (muzzleFlash.muzzle)
        {
            //Vector3 rot = muzzleFlash.muzzle.transform.eulerAngles;
            //rot.y = Random.Range(0, 360);
            //muzzleFlash.muzzle.transform.eulerAngles = rot;

            muzzleFlash.muzzle.SetBool("Fire",true);
            //Debug.Log("sdfdsdsd");
            //yield return new WaitForSeconds(muzzleFlash.timeShow);
            yield return new WaitForEndOfFrame();


            muzzleFlash.muzzle.SetBool("Fire", false);
        }

        yield return new WaitForEndOfFrame();
    }

    /*
    void CheckTarget()
    {
        if (cars.Length == 0)
            return;

        float disToTarget = 10000;
        for (int i = 0; i < cars.Length; i++)
        {
            if (!cars[i] || cars[i].tag=="Player")
            {
                continue;
            }

            //Debug.Log("cars " + i + " : " + cars[i].name);

            float dis = Vector3.Distance(parent.transform.position,cars[i].transform.position);
            target = null;
            if (dis < disToTarget && cars[i].mr.isVisible)
            {
                target = cars[i].transform;
                disToTarget = dis;
            }
        }

        //if (target) Debug.Log("name Target : " + target.name);
    }*/

    public ScrCarController GetInSightCar()
    {
        if (cars.Length <= 1)
            return null;

        float[] distances = new float[cars.Length];
        float[] angles = new float[cars.Length];

        Vector3 forward = parent.transform.forward;
        Vector3 myPosition = parent.transform.position;

        float degrees = MathHelper.ToDegrees(Mathf.Atan2(forward.z, forward.x));


        float disToTarget = (isRocketLauncher) ? GunInfo.Range : MachineGunInfo.range;


        for (int i = 0; i < cars.Length; i++)
        {
            if (!cars[i] || cars[i].tag == "Player" )
            {
                distances[i] = -1;
                continue;
            }

            distances[i] = Vector3.Distance(parent.transform.position, cars[i].transform.position);
            if (distances[i] < disToTarget && cars[i].mr.isVisible)
            {
                Vector3 diff = cars[i].transform.position - myPosition;
                float diffDegrees = MathHelper.ToDegrees(Mathf.Atan2(diff.z, diff.x));
                angles[i] = diffDegrees - degrees;
            }
            else
                distances[i] = -1;
        }


        float shortestDistance = float.MaxValue;
        int shortestIndex = -1;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] != -1 && angles[i] < shortestDistance)
            {
                shortestIndex = i;
                shortestDistance = angles[i];
            }
        }



        if (shortestIndex != -1) {
			if( ScrCarController.Instance.nv.owner.GetTeam ()!= PunTeams.Team.none && GameplayDefaultSettings.Instance.isTeamMatch )
				if (cars [shortestIndex].nv.owner.GetTeam () == ScrCarController.Instance.nv.owner.GetTeam ())
					return null;
		
			return cars [shortestIndex];
		}
        else
            return null;
    }

    void InitBullet()
    {
        for (int i = 0; i < bulletsPool.Length; i++)
            bulletsPool[i].Init(this);
    }

    public override void Fire()
    {
        switch (stateFire)
        {
            case stateFireEnum.Normal:
                //bulletsPool[bulletNow].Born(BulletPos.position, BulletPos.rotation.eulerAngles, parent.TargetPoint, true);
                _FireBullet(BulletPos.position, BulletPos.rotation.eulerAngles, null);
                break;

            case stateFireEnum.Target:
                if (GameplayUI.IsTutorial)
                {
                    _FireBulletTutorial(BulletPos.position, BulletPos.eulerAngles, TutorialSceneManager.Instance.RocketTargetTransform);
                }
                else
                {
                    ScrCarController targetCar = GetInSightCar();
                    _FireBullet(BulletPos.position, BulletPos.eulerAngles, targetCar);
                }

                /*if (targetCar != null)
                    bulletsPool[bulletNow].Born(BulletPos, targetCar.transform, true);
                else
                    bulletsPool[bulletNow].Born(BulletPos, parent.TargetPoint, true);*/
                break;
        }
        
        

        if (singleShot)
            Bullets -= 1;
    }

    void _FireBulletTutorial(Vector3 bulletPosition, Vector3 bulletRotation, Transform targetTransform)
    {
        Vector3 targetPosition = Vector3.zero;

        if (currentBulletIndex < bulletsPool.Length - 1)
            currentBulletIndex++;
        else
            currentBulletIndex = 0;


        bulletsPool[currentBulletIndex].Born(bulletPosition, bulletRotation, targetTransform, true);
        if (fireSound != null)
            fireSound.Play();

        if (muzzleFlash.muzzle)
            StartCoroutine(MuzzleFlashApply());
    }
    
    void _FireBullet(Vector3 bulletPosition, Vector3 bulletRotation, ScrCarController targetCar)
    {
        Vector3 targetPosition = Vector3.zero;
        int targetCarID = -1;


        if (currentBulletIndex < bulletsPool.Length - 1)
            currentBulletIndex++;
        else
            currentBulletIndex = 0;


        
        if (!isRocketLauncher)
        {
            float tolerance = MachineGunInfo.tolerance;
            bulletRotation.x += Random.Range(-tolerance, tolerance);
            bulletRotation.y += Random.Range(-tolerance, tolerance);
            bulletRotation.z += Random.Range(-tolerance, tolerance);
        }

        if (targetCar != null)
        {
            if (targetCar == ScrCarController.Instance)
                GameplayUI.Instance.rocketLockSign.Activate(bulletsPool[currentBulletIndex]);

            bulletsPool[currentBulletIndex].Born(bulletPosition, bulletRotation, targetCar.transform, true);

            targetCarID = targetCar.nv.viewID;
        }
        else
        {
            bulletsPool[currentBulletIndex].Born(bulletPosition, bulletRotation, parent.TargetPoint, true);
            targetPosition = parent.TargetPoint;
        }


        if (fireSound != null)
            fireSound.Play();

        if (muzzleFlash.muzzle) 
            StartCoroutine(MuzzleFlashApply());

        nv.RPC("_FireBulletRPC", PhotonTargets.Others, bulletPosition, bulletRotation, targetPosition, targetCarID);
    }

    [RPC]
    void _FireBulletRPC(Vector3 bulletPosition, Vector3 bulletRotation, Vector3 targetPosition, int carID)
    {
        /*if (Application.isEditor)
            Debug.Break();*/

        if (currentBulletIndex < bulletsPool.Length - 1)
            currentBulletIndex++;
        else
            currentBulletIndex = 0;

        if (carID != -1)
        {
            ScrCarController targetCar = ScrController.Instance.FindCar(carID);

			bulletsPool[currentBulletIndex].Born(bulletPosition, bulletRotation, targetCar.transform, true);

            if (targetCar == ScrCarController.Instance)
                GameplayUI.Instance.rocketLockSign.Activate(bulletsPool[currentBulletIndex]);
        }
        else
        {
            bulletsPool[currentBulletIndex].Born(bulletPosition, bulletRotation, targetPosition, true);
        }

        if (fireSound != null)
            fireSound.Play();

        if (muzzleFlash.muzzle)
            StartCoroutine(MuzzleFlashApply());
    }



    public void SetRedFire()
    {
        for (int i = 0; i < bulletsPool.Length; i++)
            bulletsPool[i].renderer.sharedMaterial = GameplayUI.Instance.RedfireMaterial;
    }
}


[System.Serializable]
public class MuzzleFlashStruct
{
    public Animator muzzle;
    public float timeShow;
}

public enum stateFireEnum
{
    Normal = 0,
    Target = 1,
}