using UnityEngine;
using System.Collections;

public class ScrBombManager : Photon.MonoBehaviour {
    
    public int numberBombs;
    public GameObject BombPrefab;

    public static ScrBombController[] bombs;
    private static int bombsNow;

    public static ScrBombManager bombManager;

    void Awake()
    {
        bombManager = this;
        InitBombs();
    }

    void InitBombs()
    { 
        bombs=new ScrBombController[numberBombs];
        for (int i = 0; i < numberBombs; i++)
        {
            GameObject Bomb = (GameObject)Instantiate(BombPrefab, transform.position, Quaternion.identity);
            bombs[i]=Bomb.GetComponent<ScrBombController>();
            bombs[i].transform.SetParent(transform);
            bombs[i].Kill();
        }
    }

    public static void CreateBomb(Transform tr)
    {
        bombManager.photonView.RPC("ApplyCreateBomb", PhotonTargets.All, tr.position, tr.eulerAngles);
    }

    [RPC]
    private void ApplyCreateBomb(Vector3 vec , Vector3 rot)
    {
        bombs[bombsNow].Born(vec,rot);

        if (bombsNow < bombs.Length)
            bombsNow++;
        else bombsNow = 0;
    }
}
