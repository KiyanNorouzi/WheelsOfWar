using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentController : MonoBehaviour
{
    #region Singleton

    static EnvironmentController _instance;
    public static EnvironmentController Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    public Maps map;
    public GameObject freeLookCameraGameObject;
    public Camera PlayerCamera;
    public ScrSmoothFlowController followCam;
    public Transform[] spawnPoints;
    public PickUpManager pickupManager;
    public ScrMineManager mineManager;
    public AudioClip[] bulletDecalAudioClips;
    public int bulletDecalSoundsCount;
    public GameObject Sun;
    public 
    

    AudioSource[] bulletDecalSounds;



    void Start()
    {
        //freeLookCameraGameObject.GetComponent<Camera>().cullingMask -= 1 << LayerMask.NameToLayer("Particle");
        //PlayerCamera.cullingMask -= 1 << LayerMask.NameToLayer("Particle");
        SceneManager.defaultMap = map;
        SceneManager.LoadSceneAdditive(Scenes.Gameplay);
        EnableFreeLookCam();
        Sun.SetActive(false);
        GameObject parentGameObject = new GameObject("Decal SFXs");
        parentGameObject.transform.SetParent(transform);

        int count  = bulletDecalAudioClips.Length * bulletDecalSoundsCount;
        bulletDecalSounds = new AudioSource[count];
        for (int i = 0; i < count; i++)
        {
            GameObject soundGameObject = new GameObject();
            soundGameObject.transform.SetParent(parentGameObject.transform);

            bulletDecalSounds[i] = soundGameObject.AddComponent<AudioSource>();
            bulletDecalSounds[i].playOnAwake = false;
            bulletDecalSounds[i].clip = bulletDecalAudioClips[i % bulletDecalAudioClips.Length];
            soundGameObject.name = string.Format("SFX [{0}]", bulletDecalSounds[i].clip.name);
        }

      
		if( SettingData.VFX )
        {
            //freeLookCameraGameObject.GetComponent<Camera>().cullingMask = 1;
            //PlayerCamera.cullingMask = 1;
            Sun.SetActive(true);
		}
    }

    public void PlayBulletDeacl(Vector3 position)
    {
        int count = 0;
        int index = -1;

        do
        {
            index = Random.Range(0, bulletDecalSounds.Length);
            if (++count >= 15)
                return;

        } while (bulletDecalSounds[index].isPlaying);

        bulletDecalSounds[index].transform.position = position;
        bulletDecalSounds[index].Play();
    }

    public void DisableFreeLookCam()
    {
        freeLookCameraGameObject.SetActive(false);
        followCam.Activate(ScrCarController.Instance.transform);
    }

    public void EnableFreeLookCam()
    {
        freeLookCameraGameObject.SetActive(true);
        followCam.Deactivate();
    }

    public Transform GetNearestSpawnPoint(Vector3 carPosition)
    {
        int nearestPointIndex = -1;

        float distance = float.MaxValue;
        float thisPointDistance = 0;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            thisPointDistance = (carPosition - spawnPoints[i].position).sqrMagnitude;
            if (thisPointDistance < distance)
            {
                distance = thisPointDistance;
                nearestPointIndex = i;
            }
        }

        return spawnPoints[nearestPointIndex];
    }

    public Transform GetRandomSpawnPoint()
    {
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomSpawnPointIndex];
    }

    public Transform GetSpawnPointByIndex(int respawnPointIndex)
    {
        return spawnPoints[respawnPointIndex];
    }

    public Transform GetMostFarSpawnPoint()
    {
        int loopCount = 0;
        float distance;
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
            distance = GetDistanceOfClosestCar(randomIndex);

            if (++loopCount > 15)
                break;

        } while (distance < 35);

        return spawnPoints[randomIndex];

        /*int mostFarPointIndex = -1;

        float distance = 0;
        float thisPointDistance = 0;

        int cars = ScrController.Instance.GetCarsCount();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            thisPointDistance = GetDistanceOfClosestCar(i); // (carPosition - spawnPoints[i].position).sqrMagnitude;
            if (thisPointDistance > distance)
            {
                distance = thisPointDistance;
                mostFarPointIndex = i;
            }
        }

        return spawnPoints[mostFarPointIndex];*/
    }

    float GetDistance(int index)
    {
        float d = 0;
        int cars = ScrController.Instance.GetCarsCount();
        for (int i = 0; i < cars; i++)
            d += (ScrController.Instance.GetCar(i).transform.position - spawnPoints[index].position).sqrMagnitude;

        return d;
    }

    float GetDistanceOfClosestCar(int spawnPointIndex)
    {
        float nearestDistance = float.MaxValue;
        int cars = ScrController.Instance.GetCarsCount();
        Vector3 position = spawnPoints[spawnPointIndex].position;
        

        for (int i = 0; i < cars; i++)
        {
            ScrCarController car = ScrController.Instance.GetCar(i);
            if (!car.IsOnLine)
                continue;

            Vector3 thisCarPosition = car.myTransform.position;
            position.y = thisCarPosition.y;

            float thisCarDistance = (thisCarPosition - spawnPoints[spawnPointIndex].position).sqrMagnitude;
            if (thisCarDistance < nearestDistance)
                nearestDistance = thisCarDistance;
        }

        return nearestDistance;
    }

    public void GameStarted()
    {
        mineManager.DeactivateAllMines();

        if (ScrCarController.Instance != null)
            ScrCarController.Instance.DeactivateAllBullets();
    }

    public Transform GetSafestSpawnPoint()
    {
        return spawnPoints[GetSafestSpawnPointIndex()];
    }

    List<OffLimitsSpawnPointStruct> offlimitsPoints;
    public int GetSafestSpawnPointIndex()
    {
        if (offlimitsPoints == null)
            offlimitsPoints = new List<OffLimitsSpawnPointStruct>();
        else
        {
            for (int i = 0; i < offlimitsPoints.Count; i++)
            {
				if( ScrController.Instance.IndexVild( offlimitsPoints[i].spawnPointIndex ) ){
                	if (offlimitsPoints[i].spawnTime < Time.time - 10)
                	{
                   		offlimitsPoints.RemoveAt(i);
                    	i--;
                    	continue;
                	}
				}
            }
        }


        int safestSpawnPointIndex = -1;
        float longestDistance = float.MinValue;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            bool skipThisPoint = false;
            for (int k = 0; k < offlimitsPoints.Count; k++)
            {
                if (i == offlimitsPoints[k].spawnPointIndex)
                {
                    skipThisPoint = true;
                    break;
                }
            }

            if (skipThisPoint)
                continue;

            float distance = GetDistanceOfClosestCar(i);
            if (distance > longestDistance)
            {
                longestDistance = distance;
                safestSpawnPointIndex = i;
            }
        }

        if (safestSpawnPointIndex == -1)
        {
            GameplayUI.Instance.newsWall.SubmitText("Random spawn points, off limit points=" + offlimitsPoints.Count, false);
            return Random.Range(0, spawnPoints.Length);
        }
        else
        {
            offlimitsPoints.Add(new OffLimitsSpawnPointStruct(safestSpawnPointIndex, Time.time));
            //lastSafestSpawnPoint = safestSpawnPointIndex;
            return safestSpawnPointIndex;
        }
    }



    
}


public struct OffLimitsSpawnPointStruct
{
    public int spawnPointIndex;
    public float spawnTime;

    public OffLimitsSpawnPointStruct(int index, float time)
    {
        this.spawnPointIndex = index;
        spawnTime = time;
    }
}