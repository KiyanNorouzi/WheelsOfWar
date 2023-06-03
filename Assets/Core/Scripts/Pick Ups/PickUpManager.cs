using UnityEngine;
using System.Collections;
using System;

public class PickUpManager : MonoBehaviour 
{
    public GameObject[] PickUpPrefabs;
    public int[] counts;
    public PickUpSpawnPointStruct[] spawnPoints;
    public PhotonView nv;


    ScrPickUpController[][] PickUps;
    int currentPickUpIndex;


    [ContextMenu("collect points")]
    void collectpoints()
    {
        ScrPickUpController[] pickups = GetComponentsInChildren<ScrPickUpController>();
        spawnPoints = new PickUpSpawnPointStruct[pickups.Length];

        for (int i = 0; i < pickups.Length; i++)
        {
            GameObject n = new GameObject();
            n.transform.SetParent(pickups[i].transform.parent);
            n.transform.localPosition = pickups[i].transform.localPosition;
            n.name = string.Format("Pickup Spawn Point {0} [{1}]", i, pickups[i].type);

            spawnPoints[i] = new PickUpSpawnPointStruct();
            spawnPoints[i].pointTransform = n.transform;
            spawnPoints[i].type = pickups[i].type;
        }
    }
    

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        /*if (!PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
        {
            Invoke("InitPickUps", 5.0f);
            return;
        }*/

        PickUps = new ScrPickUpController[PickUpPrefabs.Length][];

        bool[] fullHouses = new bool[spawnPoints.Length];
        for (int i = 0; i < PickUps.Length; i++)
        {
            PickUps[i] = new ScrPickUpController[counts[i]];
            for (int j = 0; j < PickUps[i].Length; j++)
            {
                //GameObject PickUp = PhotonNetwork.Instantiate(PickUpPrefabs[i].name, transform.position, Quaternion.identity, 0);
                GameObject PickUp = (GameObject)Instantiate(PickUpPrefabs[i], transform.position, Quaternion.identity);

                PickUps[i][j] = PickUp.GetComponent<ScrPickUpController>();
                PickUps[i][j].transform.SetParent(transform);
                PickUps[i][j].i = i;
                PickUps[i][j].j = j;

                //if (PhotonNetwork.isMasterClient)
                    PickUps[i][j].OnCollected += PickUpManager_OnCollected;

                CollectibleType t = (CollectibleType)i;
                for (int k = 0; k < spawnPoints.Length; k++)
                {
                    if (spawnPoints[k].type == t && !fullHouses[k])
                    {
                        fullHouses[k] = true;
                        PickUps[i][j].transform.localPosition = spawnPoints[k].pointTransform.localPosition;
                        //PickUps[i][j].transform.SetParent(spawnPoints[k].pointTransform);
                        break;
                    }
                }
                
                PickUps[i][j].name = string.Concat((CollectibleType)i, j);
                PickUps[i][j].Kill();
            }
        }

        //nv.RPC("InitPickUpsRPC", PhotonTargets.OthersBuffered);
    }

    void PickUpManager_OnCollected(int i, int j)
    {
        nv.RPC("DeactivatePickUp", PhotonTargets.All, i, j);
    }


    float lastPickUpActivateTime;


    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < PickUps.Length; i++)
            {
                for (int j = 0; j < PickUps[i].Length; j++)
                {
                    if (Time.time - lastPickUpActivateTime >= 0.5f && !PickUps[i][j].IsActive && PickUps[i][j].IsReadyToRespawn())
                    {
                        nv.RPC("ActivatePickUp", PhotonTargets.All, i, j);
                        lastPickUpActivateTime = Time.time;
                        return;
                    }
                }
            }
                
        }
    }


    [RPC]
    void ActivatePickUp(int i, int j)
    {
        PickUps[i][j].Respawn();
    }

    [RPC]
    void DeactivatePickUp(int i, int j)
    {
        PickUps[i][j].Kill();
    }



    /*
    public static void SetPickUp(CollectibleType type, Vector3 position)
    {
        currentPickUpIndex++;
        if (currentPickUpIndex >= PickUps.Length)
            currentPickUpIndex = 0;

        PickUps[(int)type][currentPickUpIndex].Respawn(position);
    }*/
}


[Serializable]
public class PickUpSpawnPointStruct
{
    public Transform pointTransform;
    public CollectibleType type;
}
