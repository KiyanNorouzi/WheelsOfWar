using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class ScrMineController : MonoBehaviour 
{
    public Transform myTransform;
	public GameObject[] mineAoeRenderers;
    public GameObject myGameObject;
    public GameObject[] infoGameObjects;
    public float timeExplotion;
    public float timeReadyExplotion;
    private float startRunForFire;
    private bool isTrigger;
    public MeshRenderer[] allRenderers;
    public TextMesh secondsText, usernameText;



    [ContextMenu("Collect All Renderers")]
    void collectAllRenderers()
    {
        allRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    [ContextMenu("Collect All Sprites")]
    void collectAllSprites()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }



    public SpriteRenderer[] sprites;
    public TextMesh[] texts;
    ScrCarController mineCreatedBy;

    public ParticleSystem ps;

    private AudioSource AS;

    //private PhotonView nv;

    void TriggerActivate()
    {
        isTrigger = true;
    }


    [System.Serializable]
    public class AudioStruct: AudioStructBase
    {
        public AudioClip beep;
        public AudioClip explosion;
    }

    public AudioStruct audioPlayer;

    [System.Serializable]
    public struct DamageSystemStruct
    {
        public float distance;
        public LayerMask hitLayer;
    }

    public DamageSystemStruct DamageSystem;

    
     
    void Awake()
    {
        AS = GetComponent<AudioSource>();
        //Kill();
    }

    public void Born(Vector3 vec, Vector3 rot, ScrCarController mineCreatedBy)
    {
        this.mineCreatedBy = mineCreatedBy;
        if (mineCreatedBy != null && mineCreatedBy.nv.isMine)
             Accounting.Instance.currentUser.statistics.AddMines();

        this.myTransform.position = vec;
        this.myTransform.eulerAngles = rot;

        for (int i = 0; i < allRenderers.Length; i++)
            allRenderers[i].enabled = true;

        startRunForFire = Time.time;

		Color color = new Color ();

		if (!GameplayDefaultSettings.Instance.isTeamMatch) {
			color = ((mineCreatedBy == ScrCarController.Instance) ? GameplayDefaultSettings.Instance.MyMineColor : GameplayDefaultSettings.Instance.EnemyMineColor);
		}
		else {
			color = ((mineCreatedBy.Owner.GetTeam() == ScrCarController.Instance.Owner.GetTeam()) ? GameplayDefaultSettings.Instance.MyMineColor : GameplayDefaultSettings.Instance.EnemyMineColor);
		}

        for (int i = 0; i < sprites.Length; i++)
        {
            Color c = color;
            c.a = sprites[i].color.a;
            sprites[i].color = c;
        }

		for( int cnt = 0; cnt < mineAoeRenderers.Length; cnt++ ){
			if( mineAoeRenderers[cnt].GetComponent<Renderer>() ){
				mineAoeRenderers[cnt].GetComponent<Renderer>().material.color = color;
			}
		}

        for (int i = 0; i < texts.Length; i++)
            texts[i].color = color;


        if (mineCreatedBy != null)
            usernameText.text = mineCreatedBy.Username;
        else
            usernameText.text = "";

        Invoke("TriggerActivate", timeReadyExplotion);
        AS.Play();

        if (!GameplayUI.IsTutorial)
            CounterFire();


        audioPlayer.Play(audioPlayer.beep);
        _SetInfoGameObject(true);
        myGameObject.SetActive(true);
    }

    void _SetInfoGameObject(bool enabled)
    {
        for (int i = 0; i < infoGameObjects.Length; i++)
            infoGameObjects[i].SetActive(enabled);

		for( int cnt = 0; cnt < mineAoeRenderers.Length; cnt++ ){
			mineAoeRenderers[cnt].gameObject.SetActive(enabled	);
		}
    }

    public void Kill()
    {
        for (int i = 0; i < allRenderers.Length; i++)
        {
            if (allRenderers[i] != null)
                allRenderers[i].enabled = false;
        }


        isTrigger = false;
        _SetInfoGameObject(false);

        Invoke("_deactivate", .5f);
    }

    void _deactivate()
    {
        myGameObject.SetActive(false);
    }

    void CounterFire()
    {
        if (startRunForFire + timeExplotion < Time.time)
            Explotion();
        else
        {
            int timeRemaining = (int)((startRunForFire + timeExplotion) - Time.time);
            secondsText.text = timeRemaining.ToString("0");

            if (timeRemaining <= 5)
            {
                Invoke("CounterFire2", 0.5f);
                audioPlayer.Play(audioPlayer.beep);
            }
            else
                audioPlayer.Play(audioPlayer.beep);

            Invoke("CounterFire", 1);
        }
    }

    void CounterFire2()
    {
        audioPlayer.Play(audioPlayer.beep);
    }

    public void Explode()
    {
        Kill();

        if (ps)
            ps.Play();

        audioPlayer.Play(audioPlayer.explosion);
        _SetInfoGameObject(false);
    }

    [RPC]
    void Explotion()
    {
        Kill();

        if (mineCreatedBy != null)
            Damage();

        if (ps) 
            ps.Play();

        audioPlayer.Play(audioPlayer.explosion);
        _SetInfoGameObject(false);
    }

    void Damage()
    {
        //if (mineCreatedBy != null && !mineCreatedBy.nv.isMine)
            //return;

        float radius = mineCreatedBy.miner.GunInfo.Range;
        RaycastHit[] hit = Physics.SphereCastAll(myTransform.position - myTransform.up * 10.0f, radius, myTransform.up, DamageSystem.distance, DamageSystem.hitLayer);

        for (int i = 0; i < hit.Length; i++)
        {
            ScrCarController enemy = hit[i].collider.GetComponent<ScrCarController>();
            if (enemy.nv.viewID == mineCreatedBy.nv.viewID)
                continue;

            if (enemy.IsInvincible)
                continue;

//			Debug.Log( "EnemyTag : " + enemy.Owner.GetTeam() );
//			Debug.Log( "My Tag : " + ScrCarController.Instance.nv.owner.GetTeam() );
			if(  mineCreatedBy.nv.owner.GetTeam() != PunTeams.Team.none && GameplayDefaultSettings.Instance.isTeamMatch ){
				if( enemy.Owner.GetTeam() == mineCreatedBy.nv.owner.GetTeam() ){
					continue;
				}
			}


            float damage = Random.Range(mineCreatedBy.miner.GunInfo.damageMin, mineCreatedBy.miner.GunInfo.damageMin);
            //enemy.nv.RPC("ApplyDamage", PhotonTargets.All, damage, mineCreatedBy.photonView.viewID, (int)KillMethod.Mine, enemy.LifeIndex);// ApplyDamage(DamageSystem.damageValue);
            //if (enemy == ScrCarController.Instance)
                //enemy.ApplyDamage(damage, mineCreatedBy, KillMethod.Mine);

            if (enemy == ScrCarController.Instance)
                enemy.ApplyDamage(damage, mineCreatedBy, KillMethod.Mine);
                //enemy.nv.RPC("ApplyDamage", PhotonTargets.All, damage, mineCreatedBy.photonView.viewID, (int)KillMethod.Mine, enemy.LifeIndex);// ApplyDamage(DamageSystem.damageValue);

            ScrController.Instance.CallDataHit(mineCreatedBy, enemy, KillMethod.Mine, transform.position);
        }

        if (mineCreatedBy != null && !mineCreatedBy.nv.isMine)
        {
            Accounting.Instance.currentUser.statistics.AddSuccessfullMines();
            QuestManager.Instance.HitMine();
        }
    }

    void SoundPlay(AudioClip ac)
    {
        AS.clip = ac;
        AS.Play();
    }


    void OnTriggerEnter(Collider col)
    {
        if (!isTrigger)
            return;


        if (mineCreatedBy == null)
        {
            Explotion();

            return;
        }
            
        if ((col.tag == "Enemy" || col.tag == "Player") && col.name != mineCreatedBy.name)
        {
			if( mineCreatedBy.nv.owner.GetTeam() != PunTeams.Team.none && GameplayDefaultSettings.Instance.isTeamMatch )
			{
				if( col.GetComponent<ScrCarController>().nv.owner.GetTeam() != mineCreatedBy.nv.owner.GetTeam() )
					Explotion();
			}
			else{
				Explotion();
			}
        }
    }
}
