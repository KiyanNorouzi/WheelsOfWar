using UnityEngine;
using System.Collections;

public class ScrBombController : Photon.MonoBehaviour {

    public float timeExplotion;
    private float startRunForFire;
    private bool isTrigger;
    public MeshRenderer[] MR;

    public ParticleSystem ps;

    private AudioSource AS;

    [System.Serializable]
    public class AudiosStruct
    {
        public AudioClip alive;
        public AudioClip explotion;
    }

    public AudiosStruct audios;

    [System.Serializable]
    public struct DamageSystemStruct
    {
        public float radious;
        public float distance;
        public float damageValue;
        public LayerMask hitLayer;
    }

    public DamageSystemStruct DamageSystem;

    private Transform tr;

    void Awake()
    {
        tr = transform;

        AS=GetComponent<AudioSource>();

        Kill();
    }

    public void Born(Vector3 vec,Vector3 rot)
    {

        this.tr.position = vec;
        this.tr.eulerAngles = rot;

        for (int i = 0; i < MR.Length; i++)
            MR[i].enabled = true;

            startRunForFire = Time.time;

            AS.Play();

        CounterFire();

        SoundPlay(audios.alive);
    }

    public void Kill()
    {
        for (int i = 0; i < MR.Length; i++)
            MR[i].enabled = false;

        CancelInvoke();
    }

    void CounterFire()
    {
        if (startRunForFire + timeExplotion < Time.time)
        {
            ReadyExplotion();
        }
        else Invoke("CounterFire",0.0f);
    }


    void ReadyExplotion()
    {
            isTrigger = true;
    }

    void Damage()
    {
        RaycastHit[] hit = Physics.SphereCastAll(tr.position, DamageSystem.radious, tr.forward, DamageSystem.distance, DamageSystem.hitLayer);

        for (int i = 0; i < hit.Length; i++)
        {
            print("name : " + hit[i].collider.name);
            ScrCarController car = hit[i].collider.GetComponent<ScrCarController>();
            car.nv.RPC("ApplyDamage", PhotonTargets.All, DamageSystem.damageValue, car.LifeIndex);// ApplyDamage(DamageSystem.damageValue);
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

        if(col.tag=="Player" || col.tag=="Enemy" || col.tag=="Bullet")
        {
            Kill();
            Damage();

            isTrigger = false;

            if (ps) ps.Play();

            SoundPlay(audios.explotion);
        }
    }
}
