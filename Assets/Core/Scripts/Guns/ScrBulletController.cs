using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ScrBulletController : MonoBehaviour
{
    public delegate void hitEnemy(ScrCarController car);
    public event hitEnemy OnHitEnemy;


    public GameObject detail, rocketGameObject;
    public ExplotionClass explotion;
    public float speedRotate;
    
    public float deathTime = 3.0f;
    public DamageSystemStruct DamageSystem;
    public ParticleSystem trailParticleSystem;

    
    float Speed
    {
        get
        {
            if (DamageSystem.isRocket)
                return GameplayDefaultSettings.Instance.RocketSpeed;
            else
                return GameplayDefaultSettings.Instance.BulletSpeed;
        }
    }


    Transform myTransform, targetTransform;
    Vector3 targetPosition;

    Vector3 lastPos;
    AudioSource myAudioSource;
    Collider myCollider;
    ScrGunController gun;
    Transform parentTransform;
    bool isDead;
    public bool IsDead
    {
        get { return isDead; }
    }
    

    
    [ContextMenu("collect info")]
    void collectinfo()
    {
        detail = gameObject;
    }



    [System.Serializable]
    public class ExplotionClass
    {
        public ParticleSystem ps;
        public AudioClip ac;
    }

    [System.Serializable]
    public struct DamageSystemStruct
    {
        public float distance;
        public bool isRocket;
        public LayerMask hitLayer;
    }

    public void Init(ScrGunController parent)
    {
        this.parentTransform = myTransform.parent;
        this.gun = parent;

        Kill();

        if (parent.Nv.isMine)
        {
            if (DamageSystem.isRocket)
                this.name = "My Rocket [" + parent.parent.Username + "]";
            else
                this.name = "My Bullet [" + parent.parent.Username + "]";
        }
        else
        {
            if (DamageSystem.isRocket)
                this.name = "Rocket [" + parent.parent.Username + "]";
            else
                this.name = "Bullet [" + parent.parent.Username + "]";
        }
    }

    public void Born(Vector3 bulletStartPosition, Vector3 bulletStartEulerAngle, Vector3 target, bool sendAll)
    {
        myTransform.position = bulletStartPosition;
        myTransform.eulerAngles = bulletStartEulerAngle;
        myTransform.parent = null;

        isDead = false;
        myCollider.enabled = true;


        _SetTrailParticleEnable(true);

        if (gun.Nv.isMine)
        {
            if (DamageSystem.isRocket)
                Accounting.Instance.currentUser.statistics.AddRockets();
            else
            {
                Accounting.Instance.currentUser.statistics.AddBullets();
                QuestManager.Instance.FiredBullets();
            }
                
        }
        

        this.targetPosition = target;
        //Invoke("Dead", deathTime);


        if (rocketGameObject)
            rocketGameObject.SetActive(true);

        if (detail)
            detail.SetActive(true);
    }

    void _SetTrailParticleEnable(bool p)
    {
        if (trailParticleSystem)
            trailParticleSystem.gameObject.SetActive(p);
    }

    public void Born(Vector3 bulletStartPosition, Vector3 bulletStartEulerAngle, Transform target, bool sendAll)
    {
        myTransform.position = bulletStartPosition;
        myTransform.eulerAngles = bulletStartEulerAngle;

        myTransform.parent = null;
        isDead = false;

        
        myCollider.enabled = true;

        this.targetTransform = target;

        _SetTrailParticleEnable(true);

        if (rocketGameObject)
            rocketGameObject.SetActive(true);

        if (detail)
            detail.SetActive(true);

        if (gun.Nv.isMine)
        {
            if (DamageSystem.isRocket)
                Accounting.Instance.currentUser.statistics.AddRockets();
            else
            {
                Accounting.Instance.currentUser.statistics.AddBullets();
                QuestManager.Instance.FiredBullets();
            }

        }
    }

    

    void Awake()
    {
        myTransform = transform;
        myCollider = collider;

        myAudioSource=GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gun == null || isDead)
            return;

        Movement();

        /*if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctBulletPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctBulletRot, Time.deltaTime * 10);
        }
        else
            Movement();*/
    }



    void Dead()
    {
        if (isDead)
            return;

        if (explotion.ps)
        {
            explotion.ps.Play();
            if (DamageSystem.isRocket)
            {
                myAudioSource.clip = explotion.ac;
                myAudioSource.loop = false;
                myAudioSource.Play();
            }
            else
                EnvironmentController.Instance.PlayBulletDeacl(transform.position);
        }

        if (rocketGameObject)
            rocketGameObject.SetActive(false);

        targetTransform = null;

        Damage();
        Kill();
    }

    public void Kill()
    {
        isDead = true;
        myCollider.enabled = false;

        if (gun)
            myTransform.parent = gun.transform;


        _SetTrailParticleEnable(false);

        if (detail!= null && detail.activeSelf)
        {
            if (DamageSystem.isRocket)
                StartCoroutine(_DeactivateAfter(0.5f));
            else
                StartCoroutine(_DeactivateAfter(0.1f));
        }
    }

    public void KillInstantly()
    {
        isDead = true;
        myCollider.enabled = false;

        if (gun)
            myTransform.parent = gun.transform;


        _SetTrailParticleEnable(false);
        detail.SetActive(false);
    }

    IEnumerator _DeactivateAfter(float p)
    {
        yield return new WaitForSeconds(p);
        detail.SetActive(false);
    }

    void Rotation(Vector3 tar)
    {
        Quaternion rot = Quaternion.LookRotation(tar + new Vector3(0, 0.5f, 0) - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, speedRotate * Time.deltaTime);
    }

    void Damage()
    {
        if (!GameplayUI.IsTutorial && !gun.parent.nv.isMine)
            return;

        float radius = DamageSystem.isRocket ? gun.GunInfo.explosionArea : 1;
        Collider[] cols = Physics.OverlapSphere(myTransform.position, radius, DamageSystem.hitLayer);

        for (int i = 0; i < cols.Length; i++)
        {
            if (GameplayUI.IsTutorial)
            {
                TutorialHitTaker hitTaker = cols[i].GetComponent<TutorialHitTaker>();


                float damage = DamageSystem.isRocket ? Random.Range(gun.GunInfo.damageMin, gun.GunInfo.damageMax) : Random.Range(gun.MachineGunInfo.damageMin, gun.MachineGunInfo.damageMax);

                int killMethod = DamageSystem.isRocket ? ((int)KillMethod.Rocket) : ((int)KillMethod.MachineGun);
                //TutorialSceneManager.Instance.RocketDidntHitToObject(damage);

                if (hitTaker == null)
                    TutorialSceneManager.Instance.RocketDidntHitToObject(damage);
                else
                    hitTaker.AddDamage(killMethod, damage);
            }
            else
            {
                ScrCarController enemyCar = cols[i].GetComponent<ScrCarController>();
                if (enemyCar == null)
                    continue;

                if (enemyCar == gun.parent)
                    continue;

                if (enemyCar.IsInvincible)
                    continue;

//				Debug.Log( "EnemyTag : " + enemyCar.Owner.GetTeam() );
//				Debug.Log( "My Tag : " + ScrCarController.Instance.nv.owner.GetTeam() );
				if( gun.parent.nv.owner.GetTeam() != PunTeams.Team.none && GameplayDefaultSettings.Instance.isTeamMatch ){
					if( enemyCar.Owner.GetTeam() == gun.parent.nv.owner.GetTeam()  ){
						continue;
					}
				}

                float damage = DamageSystem.isRocket ? Random.Range(gun.GunInfo.damageMin, gun.GunInfo.damageMax) : Random.Range(gun.MachineGunInfo.damageMin, gun.MachineGunInfo.damageMax);

                int killMethod = DamageSystem.isRocket ? ((int)KillMethod.Rocket) : ((int)KillMethod.MachineGun);
                enemyCar.nv.RPC("ApplyDamage", PhotonTargets.All, damage, gun.parent.photonView.viewID, killMethod, enemyCar.LifeIndex);

                if (DamageSystem.isRocket)
                {
                    Accounting.Instance.currentUser.statistics.AddSuccessfullRockets();
                    QuestManager.Instance.HitRocket();
                }
                else
                    Accounting.Instance.currentUser.statistics.AddSuccessfullBullets();

                ScrController.Instance.CallDataHit(gun.parent, enemyCar, DamageSystem.isRocket ? KillMethod.Rocket : KillMethod.MachineGun, transform.position);
                if (OnHitEnemy != null)
                    OnHitEnemy(enemyCar);
            }
        }
    }

    void Movement()
    {
        Vector3 t;

        if (targetTransform)
        {
            t = targetTransform.position;
            myTransform.position += myTransform.forward *  Speed * Time.deltaTime;
            Rotation(targetTransform.position);
        }
        else
        {
            t = targetPosition;
            myTransform.position = Vector3.MoveTowards(myTransform.position, t, Speed * Time.deltaTime);
        }
        

        if (Vector3.Distance(myTransform.position, t) < 1)
            Dead();
    }

    void OnTriggerEnter(Collider col)
    {
        if (isDead)
            return;

        if (gun == null || gun.parent == null)
            return;

        if (col.tag != "Player" && col.tag != "Bullet")
        {
            if (col.tag == "Enemy" && col.name == gun.parent.name)
                return;

            Dead();
        }
    }

}
