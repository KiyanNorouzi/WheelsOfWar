using UnityEngine;
using System.Collections;

public class SceneManager 
{
    static Scenes previousSceneType;
    static Scenes currentSceneType = Scenes.Login;
    static Maps currentMap;

    static Scenes initialScene = Scenes.MainMenu;
    //static Scenes initialScene = Scenes.IranAppsTest;


	//LoginSceneController  loginSceneController; 



    public static Scenes CurrentSceneType
    {
        get { return currentSceneType; }
    }

    public static void LoadScene(Scenes scene)
    {
        Debug.Log("load scene " + scene);

        if (CommonUI.Instance == null)
        {
            if (currentSceneType != previousSceneType)
                previousSceneType = currentSceneType;

            currentSceneType = scene;

            Application.LoadLevel(sceneNames[(int)scene]);
        }
        else
        {
            if (currentSceneType != previousSceneType)
                previousSceneType = currentSceneType;

            currentSceneType = scene;
            //Application.LoadLevel(sceneNames[(int)scene]);

            if (previousSceneType == Scenes.AdPlay || previousSceneType == Scenes.Loading)
                CommonUI.Instance.blackScreen.ToBlack(ActuallyLoadScene, false);
            else
                CommonUI.Instance.blackScreen.ToBlack(ActuallyLoadScene);
        }
    }

    public static void LoadAdScene(Scenes comebackScene, bool forcedAd)
    {
        AdPlaySceneManager.comebackScene = comebackScene;
        AdPlaySceneManager.forcedAd = forcedAd;

        LoadScene(Scenes.AdPlay);
    }

    private static void LoadLoadingScene()
    {
        Application.LoadLevel("Loading");
    }

    private static void ActuallyLoadScene()
    {
        Application.LoadLevel(sceneNames[(int)currentSceneType]);
    }

    public static void SceneLoaded(int level)
    {
        if (level != 1 && CommonUI.Instance.blackScreen.IsActive)
            CommonUI.Instance.blackScreen.ToTransparent(null);

        if (level > 1 && CommonUI.Instance != null)
            CommonUI.Instance.DestroyInitialBlackScreen();
    }

    public static void LoadSceneAdditive(Scenes scene)
    {
        Application.LoadLevelAdditive(sceneNames[(int)scene]);
    }

    public static void LoadPreviousScene()
    {
        LoadScene(previousSceneType);
    }

    public static void LoadGame()
    {
        LoadGame(Scenes.Login);
    }

    public static void LoadGame(Scenes scene)
    {
        Debug.Log("load game from scene " + scene);

        initialScene = scene;
        Application.LoadLevel("Loading");
    }

    public static Maps defaultMap = Maps.TrainStation;

    public static void LoadInitialScene()
    {
        Debug.Log("loading initil scene=" + initialScene);

        if (initialScene == Scenes.Gameplay)
            LoadMap(defaultMap);
        else
            LoadScene(initialScene);
    }

    public static void LoadMap(Maps map)
    {
        currentMap = map;
        if (CommonUI.Instance == null)
            ActuallyLoadMap();
        else
            CommonUI.Instance.blackScreen.ToBlack(ActuallyLoadMap);
    }

    private static void ActuallyLoadMap()
    {
        Application.LoadLevel(mapSceneNames[(int)currentMap]);
    }




    public static string[] sceneNames = {
        "Loading",
        "MainMenu",
        "MapSelect",
        "Garage",
        "Gameplay",
        "LoginNew",
        "Store",
        "LoginTutorial",
        "Tutorial",
        "AdPlay",
        "IranAppsTest"
    };

    public static string[] mapSceneNames = {
        "Tuterial",
        "Junkyard_Final",
        "TrainStationFinal",
    };
}

public enum Scenes
{
    Loading,
    MainMenu,
    MapSelect,
    Garage,
    Gameplay,
    Login,
    Store,
    LoginToturial,
    TutorialStartScene,
    AdPlay,
    IranAppsTest,
}

public enum Maps
{
    Tutorial,
    Junkyard,
    TrainStation,
}