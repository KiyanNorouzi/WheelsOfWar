using UnityEngine;
using System.Collections;

public class ScrMineManager : MonoBehaviour 
{    
    public int minesCount;
    public GameObject minePrefab;
    public PhotonView nv;

    public ScrMineController[] mines;
    private int currentMineIndex;

    void Awake()
    {
        InitMines();
    }

    void InitMines()
    {
        mines = new ScrMineController[minesCount];
        for (int i = 0; i < minesCount; i++)
        {
            mines[i] = ((GameObject)Instantiate(minePrefab, transform.position, Quaternion.identity)).GetComponent<ScrMineController>();
            mines[i].myTransform.SetParent(transform);
            mines[i].Kill();
        }
    }

    public void SetupMine(Vector3 position, Vector3 rotation, ScrCarController car)
    {
        //Debug.Log("setup mine");
        _SetUpMine(position, rotation, car);
        //nv.RPC("_SetupMineRPC", PhotonTargets.Others, position, rotation, car.nv.viewID);
    }

    [RPC]
    void _SetupMineRPC(Vector3 position, Vector3 rotation, int carViewID)
    {
        //Debug.Log("setup mine rpc");

        ScrCarController car = ScrController.Instance.FindCar(carViewID);
        _SetUpMine(position, rotation, car);
    }

    private void _SetUpMine(Vector3 position, Vector3 rotation, ScrCarController car)
    {
        currentMineIndex++;
        if (currentMineIndex >= mines.Length)
            currentMineIndex = 0;

        mines[currentMineIndex].Born(position, rotation, car);

        if (GameplayUI.IsTutorial)
            TutorialSceneManager.Instance.CarSetMine(mines[currentMineIndex]);
    }

    public void DeactivateAllMines()
    {
        for (int i = 0; i < minesCount; i++)
        {
            mines[i].myTransform.SetParent(transform);
            mines[i].Kill();
        }
    }
}