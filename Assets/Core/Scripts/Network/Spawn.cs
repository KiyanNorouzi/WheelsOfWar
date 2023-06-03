using UnityEngine;
using System.Collections;

public class Spawn : Photon.MonoBehaviour
{
    #region Singleton

    static Spawn _instance;
    public static Spawn Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion


    ScrDeadBodyController[] burnedInstances;
    

    [System.Serializable]
    public class AudiosStruct
    {
        public AudioClip Create;
        [HideInInspector]
        public AudioSource AS;

        public void Init(GameObject go)
        {
            go.AddComponent<AudioSource>();
            AS = go.GetComponent<AudioSource>();
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
    public AudiosStruct sound;


    void Start()
    {
        sound.Init(gameObject);
        burnedInstances = new ScrDeadBodyController[Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].burnedInstanceCount];
    }

    public void SpawnCar()
    {


        Transform randomPoint = EnvironmentController.Instance.GetSafestSpawnPoint();
        PhotonNetwork.Instantiate(Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].carPrefabName, randomPoint.position, randomPoint.rotation, 0);

        //Debug.Log("car ubdex=" + Accounting.Instance.currentUser.SelectedCarIndex);

        EnvironmentController.Instance.DisableFreeLookCam();
        GameplayUI.Instance.SetHUDActive(true);

        sound.Play(sound.Create);
    }

    public GameObject SpawnBurnedCar(CarType carType, Vector3 position, Quaternion rotation)
    {
        int carIndex = (int)carType;

        bool shouldInstantiate = false;
        int mostSuitableInstanceForReinstanciating = 0;
        float bestTime = float.MaxValue;

        for (int i = 0; i < burnedInstances.Length; i++)
        {
            if (burnedInstances[i] != null && burnedInstances[i].carType == carType)
            {
                if (burnedInstances[i].LastActivateTime < bestTime)
                {
                    bestTime = burnedInstances[i].LastActivateTime;
                    mostSuitableInstanceForReinstanciating = i;
                }

                if (!burnedInstances[i].IsActive)
                {
                    burnedInstances[i].Activate(position, rotation);
                    return burnedInstances[i].myGameObject;
                }
            }
            else
                shouldInstantiate = true;
        }

        if (shouldInstantiate)
        {
            for (int i = 0; i < burnedInstances.Length; i++)
            {
                if (burnedInstances[i] == null)
                {
                    //burnedInstances[i] = ((GameObject)Instantiate(Information.Instance.carInfo[Accounting.Instance.currentUser.SelectedCarIndex].carBurnedPrefab, position, rotation)).GetComponent<ScrDeadBodyController>();
                    burnedInstances[i] = ((GameObject)Instantiate(Information.Instance.carInfo[carIndex].carBurnedPrefab, position, rotation)).GetComponent<ScrDeadBodyController>();
                    return burnedInstances[i].myGameObject;
                }
            }

            burnedInstances[0] = ((GameObject)Instantiate(Information.Instance.carInfo[carIndex].carBurnedPrefab, position, rotation)).GetComponent<ScrDeadBodyController>();
            return burnedInstances[0].myGameObject;
        }
        else
        {
            burnedInstances[mostSuitableInstanceForReinstanciating].Deactivate();
            burnedInstances[mostSuitableInstanceForReinstanciating].Activate(position, rotation);

            return burnedInstances[mostSuitableInstanceForReinstanciating].myGameObject;
        }
    }


    public void Die()
    {
        EnvironmentController.Instance.EnableFreeLookCam();
    }
}
