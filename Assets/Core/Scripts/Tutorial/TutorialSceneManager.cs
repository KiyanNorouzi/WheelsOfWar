using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class TutorialSceneManager : MonoBehaviour
{
    #region Singleton

    static TutorialSceneManager _instance;
    public static TutorialSceneManager Instance
    {
        get { return TutorialSceneManager._instance; }
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


    public TutorialWayPoint[] points;
    public TutorialWayPoint[] shootingPoints;
    public GameObject[] otherItemsGameObjects;
    public GameObject shootingWalls, drivingWalls;
    public GameObject[] drivingGates, shootingGates;

    public Transform shootingTutorialStartPointTransform;
    public Color waypointNormalColor, waypointPassedColor;
    public GameObject drivingTutorialGameObject, shootingTutorialGameObject;
    public UnityEngine.UI.Image arrowImage;
    public RectTransform arrowTransform;
    public BlackScreen blackScreen;
    public Transform RocketTargetTransform, MineTransform;
    public Camera mineCamera;
    public TutorialDummyCar rocketTargetCar;
    public float DragDuration;

    public GameObject[] firstPathColliders;
    public int givenScore;
    public AudioSource waypointReachedAudio;

    int balloonShowedCount;
    float originalDamping;


    int level;
    public int Level
    {
        get { return level; }
        set 
        {
            balloonShowedCount = 0;
            level = value;
            levelChanged();
        }
    }

    void OnEnable()
    {
        CommonUI.Instance.tutorial.OnFramePassed += tutorial_OnFramePassed;
        Server.Instance.OnServerStateChanged += Instance_OnServerStateChanged;
    }

    void OnDisable()
    {
        CommonUI.Instance.tutorial.OnFramePassed -= tutorial_OnFramePassed;
        Server.Instance.OnServerStateChanged -= Instance_OnServerStateChanged;
    }





    IEnumerator Start()
    {
        mineCamera.enabled = false;


        for (int i = 0; i < points.Length; i++)
        {
            points[i].index = i;
            points[i].OnTriggerReached += DrivingPoint_OnTriggerReached;
            points[i].Deactivate();
        }

        for (int i = 0; i < shootingPoints.Length; i++)
        {
            shootingPoints[i].index = i;
            shootingPoints[i].OnTriggerReached += ShootingPoint_OnTriggerReached;
            shootingPoints[i].Deactivate();
        }


        shootingTutorialGameObject.SetActive(false);
        drivingTutorialGameObject.SetActive(true);

        shootingWalls.SetActive(false);
        drivingWalls.SetActive(true);

        for (int i = 0; i < otherItemsGameObjects.Length; i++)
            if (otherItemsGameObjects[i] != null)
                otherItemsGameObjects[i].SetActive(false);


        for (int i = 0; i < shootingGates.Length; i++)
            shootingGates[i].SetActive(false);

        yield return new WaitForSeconds(1);

        GameplayUI.Instance.hud.AccelerateEnabled = GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.TimerEnabled = 
            GameplayUI.Instance.hud.ScoreboardEnabled = GameplayUI.Instance.hud.WaitOtherPlayersPanelEnabled = GameplayUI.Instance.hud.MachineGunEnabled =
            GameplayUI.Instance.hud.RocketEnabled = GameplayUI.Instance.hud.MineEnabled = GameplayUI.Instance.hud.PauseEnabled = GameplayUI.Instance.hud.WaitingForOtherPlayersEnabled = false;

        GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;

        yield return new WaitForSeconds(1);
    }



    void Instance_OnServerStateChanged(ServerState currentState)
    {
        //Debug.Log("current state=" + currentState);
        if (currentState == ServerState.InRoom)
        {
            level = -1;
            //level = 17;

            GoToNextLevelIn(2.5f);
        }
        
    }

    void tutorial_OnFramePassed(string frameTag)
    {
        balloonShowedCount++;
        levelChanged();
    }


    void DrivingPoint_OnTriggerReached(int index)
    {
        Level++;
        points[index].DragCarIntoCenter();
    }

    void ShootingPoint_OnTriggerReached(int index)
    {
        Level++;
        shootingPoints[index].DragCarIntoCenter();
    }

	void levelChanged()
    {
        if (level >= 6 && level < 18)
        {
            if (level % 2 == 0)
            {
                if (level == 6)
                {
                    drivingGates[2].SetActive(false);
                }
                else if (level == 8)
                {
                    drivingGates[5].SetActive(false);
                    CommonUI.Instance.tutorial.LoadFrameASide("Phase_02_Step_11_Rail way", 2.5f);
                }
                else if (level == 10)
                    CommonUI.Instance.tutorial.LoadFrameASide("Phase_02_Step_12_Rail way", 2.5f);
                else if (level == 12)
                    CommonUI.Instance.tutorial.LoadFrameASide("Phase_02_Step_13_Rail way_Re", 2.5f);
                else if (level == 14)
                    drivingGates[4].SetActive(false);
                else if (level == 16)
                    drivingGates[3].SetActive(false);

                ShowWayPoint(level / 2);
            }
            else
            {
                points[level / 2].SetColor(waypointPassedColor);
                GoToNextLevelIn(0.25f);


                if (level == 9)
                    drivingGates[4].SetActive(true);

                if (level == 11)
                    drivingGates[3].SetActive(true);

                /*if (level == 15)
                    drivingGates[2].SetActive(true);*/
            }
            return;
        }




        switch (level)
        {
            case 0:
                if (balloonShowedCount == 0)
                {
                    GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;
                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_01_Rail way");
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    ShowWayPoint(0);
                    GameplayUI.Instance.hud.AccelerateEnabled = true;
                    StartCoroutine(deactivateAccIconIn(0.2f));

                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_02_Rail way");
                    return;
                }
                break;
            
            case 1:
                if (balloonShowedCount == 0)
                {
                    //GameplayUI.Instance.accelerometer.AccelerateButton_PointerUp();
                    GameplayUI.Instance.hud.AccelerateEnabled = false;
                    ScrCarController.Instance.Brake(true);
                    //ScrCarController.Instance.StopImmediately();
                    ScrCarController.Instance.StopIn(0.25f);

                    points[0].SetColor(waypointPassedColor);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_03_Rail way_Re");
                    return;
                }

                GoToNextLevelIn(0.5f);
                break;
            
            case 2:
                if (balloonShowedCount == 0)
                {
                    ShowWayPoint(1);
                    GameplayUI.Instance.hud.BrakeEnabled = true;

                    originalDamping = EnvironmentController.Instance.followCam.rotationDamping;
                    EnvironmentController.Instance.followCam.rotationDamping = 10;
                    EnvironmentController.Instance.followCam.isReverse = true;

                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_04_Rail way");
                    return;
                }
                break;

            case 3:

                //if (balloonShowedCount == 0)
                {
                    //GameplayUI.Instance.accelerometer.BrakeButton_PointerUp();
                    GameplayUI.Instance.hud.BrakeEnabled = false;
                    ScrCarController.Instance.Brake(true);

                    //ScrCarController.Instance.StopImmediately();
                    ScrCarController.Instance.StopIn(0.25f);

                    points[1].SetColor(waypointPassedColor);

                    //CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_05_Rail way");
                    //return;
                }


                
                GoToNextLevelIn(1.5f);
                StartCoroutine(ReverseCameraAfter(0.5f));

                break;

            case 4:

                if (balloonShowedCount == 0)
                {
                    drivingGates[0].SetActive(false);
                    drivingGates[1].SetActive(false);

                    GameplayUI.Instance.hud.BrakeEnabled = true;
                    GameplayUI.Instance.hud.AccelerateEnabled = true;
                    

                    ScrCarController.Instance.Brake(true);

                    ShowWayPoint(2);

                    for (int i = 0; i < firstPathColliders.Length; i++)
                        firstPathColliders[i].SetActive(false);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_05_Rail way");
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_06_Rail way");
                    GameplayUI.Instance.accelerometer.IsSteeringEnabled = true; 
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_07_Rail way");
                    return;
                }
                

                
                break;
            case 5:
                if (balloonShowedCount == 0)
                {
                    drivingGates[5].SetActive(true);

                    points[2].SetColor(waypointPassedColor);
                    ScrCarController.Instance.StopIn(0.25f);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_08_Rail way");
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    ShowWayPoint(3);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_09_Rail way");
                    return;
                }

                if (balloonShowedCount == 2)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_02_Step_10_Rail way");
                    return;
                }

                GoToNextLevelIn(0.25f);
                break;


            case 18:
                ScrCarController.Instance.StopIn(0.25f);
                GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = false;
                GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;
                blackScreen.ToBlack(NextLevel);
                break;

            case 19:
                ScrCarController.Instance.myTransform.position = shootingTutorialStartPointTransform.position;
                ScrCarController.Instance.myTransform.rotation = shootingTutorialStartPointTransform.rotation;

                ShowWayPoint(-1);
                drivingTutorialGameObject.SetActive(false);
                shootingTutorialGameObject.SetActive(true);

                shootingWalls.SetActive(true);
                drivingWalls.SetActive(false);
                //ShowShootingWayPoint(0);

                blackScreen.ToTransparent(NextLevel);
                break;

            case 20:
                

                //shootingPoints[0].SetColor(waypointPassedColor);
                GoToNextLevelIn(1);
                break;

            case 21:
                
                if (balloonShowedCount == 0)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_01_Rail way");
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    otherItemsGameObjects[0].SetActive(true);
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_02_Rail way");
                    return;
                }

                if (balloonShowedCount == 2)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_03_Rail way");
                    return;
                }

                if (balloonShowedCount == 3)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_04_Rail way");
                    return;
                }

                if (balloonShowedCount == 4)
                {
                    GameplayUI.Instance.hud.MachineGunEnabled = true;
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_05_Rail way");
                    return;
                }

                
                
                break;

            case 22:
                if (balloonShowedCount == 0)
                {
                    

                    GameplayUI.Instance.hud.MachineGunEnabled = false;
                    GameplayUI.Instance.MachineGun_Released();
                    otherItemsGameObjects[1].SetActive(true);
                    ShowShootingWayPoint(0);
                    shootingPoints[0].myCollider.enabled = false;



                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_06_Rail way");
                    return;
                }

                

                GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = true;
                GameplayUI.Instance.accelerometer.IsSteeringEnabled = true;
                GameplayUI.Instance.hud.RocketEnabled = true;
                break;

            case 23:
                if (balloonShowedCount == 0)
                {
                    shootingGates[1].SetActive(true);
                    shootingPoints[0].DragCarIntoCenter();
                    
                    GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = false;
                    GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;
                    //GameplayUI.Instance.accelerometer.BrakeButton_PointerUp();
                    //GameplayUI.Instance.accelerometer.AccelerateButton_PointerUp();

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_07_Rail way");

                    GameplayUI.Instance.hud.RocketEnabled = true;
                    ScrCarController.Instance.StopIn(0.25f);
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    otherItemsGameObjects[2].SetActive(true);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_08_Rail way");
                    return;
                }

                ShowShootingWayPoint(-1);


                break;

            case 24:
                if (balloonShowedCount == 0)
                {
                    otherItemsGameObjects[2].SetActive(false);
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_09_Rail way");
                    return;
                }


                if (balloonShowedCount == 1)
                {
                    otherItemsGameObjects[3].SetActive(true);
                    ShowShootingWayPoint(1);
                    shootingPoints[1].myCollider.enabled = false;

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_10_Rail way");
                    return;
                }

                GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = true;
                GameplayUI.Instance.accelerometer.IsSteeringEnabled = true;
                GameplayUI.Instance.hud.RocketEnabled = false;
                GameplayUI.Instance.hud.MineEnabled = true;
                //shootingPoints[1].myCollider.enabled = false;
                break;

            case 25:
                if (balloonShowedCount == 0)
                {
                    otherItemsGameObjects[3].SetActive(false);

                    shootingPoints[1].Deactivate();
                    shootingPoints[2].Activate();

                    ScrCarController.Instance.StopIn(0.25f);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_11_Rail way");
                    return;
                }
                
                break;
            case 26:

                if (balloonShowedCount == 0)
                {
                    shootingGates[0].SetActive(true);

                    ScrCarController.Instance.StopIn(0.25f);
                    GameplayUI.Instance.hud.MineEnabled = true;
                    GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = false;
                    GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;
                    //GameplayUI.Instance.accelerometer.BrakeButton_PointerUp();
                    //GameplayUI.Instance.accelerometer.AccelerateButton_PointerUp();

                    //ShowShootingWayPoint(-1);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_11.5_Rail way");
                    return;
                }

                ShowShootingWayPoint(-1);
                break;

            case 27:

                if (balloonShowedCount == 0)
                {
                    GameplayUI.Instance.hud.MineEnabled = false;
                    ShowShootingWayPoint(3);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_12_Rail way");
                    return;
                }

                GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = true;
                GameplayUI.Instance.accelerometer.IsSteeringEnabled = true;
                break;

            case 28:
                ScrCarController.Instance.StopIn(0.25f);
                GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = false;
                GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;
                ShowShootingWayPoint(-1);

                mineCamera.enabled = true;
                EnvironmentController.Instance.followCam.camera.enabled = false;

                otherItemsGameObjects[4].SetActive(true);
                GoToNextLevelIn(2);
                break;

            case 29:
                mine.Explode();
                //otherItemsGameObjects[5].SetActive(true);
                GoToNextLevelIn(1.5f);
                break;
            case 30:
                if (balloonShowedCount == 0)
                {
                    ScrCarController.Instance.StopIn(0.25f);
                    ShowShootingWayPoint(-1);
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_13_Rail way");
                    return;
                }

                NextLevel();
                break;

            case 31:
                ShowShootingWayPoint(4);
                mineCamera.enabled = false;
                EnvironmentController.Instance.followCam.camera.enabled = true;

                otherItemsGameObjects[4].SetActive(false);
                GoToNextLevelIn(0.5f);
                break;

            case 32:
                if (balloonShowedCount == 0)
                {
                    ScrCarController.Instance.StopIn(0.25f);
                    GameplayUI.Instance.hud.BrakeEnabled = GameplayUI.Instance.hud.AccelerateEnabled = true;
                    GameplayUI.Instance.accelerometer.IsSteeringEnabled = true;

                    GameplayUI.Instance.hud.ScoreboardEnabled = true;

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_14_Rail way");
                    return;
                }

                
                break;

            case 33:
                if (balloonShowedCount == 0)
                {
                    ScrCarController.Instance.StopIn(0.25f);
                    
                    EnvironmentController.Instance.mineManager.SetupMine(MineTransform.position, MineTransform.eulerAngles, null);

                    GameplayUI.Instance.Health = 0.7f;

                    ShowShootingWayPoint(-1);
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_15_Rail way");
                    return;
                }

                Level++;
                break;

            case 34:
                if (balloonShowedCount == 0)
                {
                    ShowShootingWayPoint(5);
                    otherItemsGameObjects[6].SetActive(true);

                    shootingPoints[5].myCollider.enabled = false;

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_16_Rail way");
                    return;
                }
                break;

            case 35:
                if (balloonShowedCount == 0)
                {
                    ScrCarController.Instance.StopIn(0.25f);
                    //ShowShootingWayPoint(-1);

                    otherItemsGameObjects[7].SetActive(true);
                    ShowShootingWayPoint(6);
                    shootingPoints[6].myCollider.enabled = false;

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_17_Rail way");
                    return;
                }

                Level++;
                break;

            case 36:
                otherItemsGameObjects[7].SetActive(true);
                ShowShootingWayPoint(6);
                break;

            case 37:
                ScrCarController.Instance.StopIn(0.75f);
                GameplayUI.Instance.hud.AccelerateEnabled = GameplayUI.Instance.hud.BrakeEnabled = false;
                GoToNextLevelIn(1);
                break;

            case 38:
                if (balloonShowedCount == 0)
                {
                    ScrCarController.Instance.StopIn(0.25f);
                    otherItemsGameObjects[7].SetActive(false);
                    ShowShootingWayPoint(-1);

                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_18_Rail way");
                    return;
                }

                if (balloonShowedCount == 1)
                {
                    CommonUI.Instance.tutorial.LoadFrame("Phase_03_Step_19_Rail way");
                    return;
                }

                GameplayUI.Instance.Score = givenScore;
                GameplayUI.Instance.gameOverMenu.Activate(false, 1, 8000, 100, 50, 10, 0, 0);

                //Game Analatic Eventl
                //GA.API.Design.NewEvent("Tutorial is Done");
                GameAnalytics.NewDesignEvent("Tutorial is Done",1f);

                break;
                
        }
    }

    void NextLevel()
    {
        Level++;
    }

    IEnumerator ReverseCameraAfter(float p)
    {
        yield return new WaitForSeconds(p);
        EnvironmentController.Instance.followCam.isReverse = false;

        yield return new WaitForSeconds(1);
        EnvironmentController.Instance.followCam.rotationDamping = originalDamping;
        ScrCarController.Instance.Accelerate(1);

        yield return new WaitForSeconds(0.25f);
        ScrCarController.Instance.StopAccelerate();
    }

    IEnumerator deactivateAccIconIn(float p)
    {
        yield return new WaitForSeconds(p);
        GameplayUI.Instance.accelerometer.IsSteeringEnabled = false;
    }

    void GoToNextLevelIn(float p)
    {
        StartCoroutine(_GoToLevelIn(Level + 1, p));
    }

    void GoToLevelIn(int level, float p)
    {
        _GoToLevelIn(level, p);
    }

    IEnumerator _GoToLevelIn(int targetLevel, float p)
    {
        yield return new WaitForSeconds(p);
        Level = targetLevel;
    }





    void ShowWayPoint(int index)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i == index)
                points[i].Activate(waypointNormalColor);
            else
                points[i].Deactivate();
        }
    }

    void ShowShootingWayPoint(int index)
    {
        for (int i = 0; i < shootingPoints.Length; i++)
        {
            if (i == index)
                shootingPoints[i].Activate(waypointNormalColor);
            else
                shootingPoints[i].Deactivate();
        }
    }

    public void CarCollectedItem(CollectibleType t, int n)
    {
        switch (t)
        {
            case CollectibleType.RocketLauncher:
                Level++;
                break;
            case CollectibleType.Miner:
                GameplayUI.Instance.hud.MineEnabled = false;
                Level++;
                break;
            case CollectibleType.Health:
                if (level == 34)
                    Level++;
                break;
            case CollectibleType.Armor:
                if (level == 36)
                    Level++;
                break;
            case CollectibleType.All:
                break;
        }
    }

    ScrMineController mine;
    public void CarSetMine(ScrMineController mine)
    {
        if (level == 26)
        {
            this.mine = mine;
            Level++;
        }
    }

    public void RocketDidntHitToObject(float supposedDamage)
    {
        rocketTargetCar.hitTaker.AddDamage(1, supposedDamage);
    }
}