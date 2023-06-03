using UnityEngine;
using System.Collections;

public class ScrDeadBodyController : MonoBehaviour 
{
    [System.Serializable]
    public class AudiosStruct
    {
        public AudioSource AS;
        public AudioClip create;
        

        public void Init(GameObject go)
        {
            AS = go.AddComponent<AudioSource>();
        }

        public void Play(AudioClip ac)
        {
            AS.clip = ac;
            AS.Play();
        }

        public void PlayCheck(AudioClip ac)
        {
            if (!AS.isPlaying)
                AS.Play();
        }

        public void PlayCheckClip(AudioClip ac)
        {
            if (AS.clip != ac)
                AS.clip = ac;

            if (!AS.isPlaying)
                AS.Play();
        }
    }



    public GameObject myGameObject;
    public Animator myAnimator;
    public ParticleSystem explosionParticles;
    public Renderer carModelRenderer;
    public AudiosStruct sound;
    public CarType carType;
    //public PhotonView nv;
    public float stayTime;


    float spawnTime;


    public bool IsActive
    {
        get { return myGameObject.activeInHierarchy; }
    }

    public float LastActivateTime
    {
        get { return spawnTime; }
    }





    [ContextMenu("collect info")]
    void collectinfo()
    {
        explosionParticles = GetComponentInChildren<ParticleSystem>();
        explosionParticles.playOnAwake = false;
    }


    void Start()
    {
        sound.Init(gameObject);
        Activate(transform.position, transform.rotation);
    }

    
    [RPC]
    void _ActivateRPC()
    {
        myGameObject.SetActive(true);

        sound.Play(sound.create);
        explosionParticles.Play();

        StartCoroutine(PlayAnimationAfter(0.1f));
        spawnTime = Time.time;
        enabled = true;
    }


    public void Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        _ActivateRPC();
        //nv.RPC("_ActivateRPC", PhotonTargets.Others);
    }

    public void Deactivate()
    {
        enabled = false;
        myGameObject.SetActive(false);
    }

    void Update()
    {
        if (spawnTime + stayTime <= Time.time && !carModelRenderer.isVisible)
            Deactivate();
    }

    IEnumerator PlayAnimationAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        myAnimator.SetInteger("burnanim", Random.Range(1, 4));
    }
}