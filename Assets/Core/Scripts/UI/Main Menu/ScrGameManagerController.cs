using UnityEngine;
using System.Collections;
using Ini;
using System.IO;

public class ScrGameManagerController : MonoBehaviour {


    private static bool initGameManager;

    public static ScrGameManagerController instance;

	void Awake () 
    {

		DontDestroyOnLoad(this);

        if (initGameManager)
            return;

        instance  = this;

        initGameManager = true;

        InitData();
	}

	public int stageIndex;
	public bool stage;
	public int overallIndex;

    [System.Serializable]
    public class dataGameStruct
    {
        public int totalScore=0;
        public bool soundFx;
        public bool music;
    }

    public static dataGameStruct dataGame;
    public static int numberSectionNow;

    public static void InitData()
    {
        string path = Application.persistentDataPath + "/dataGame.WarMachineSaveFile";

        if (File.Exists(path))
            return;

        IniFile f = new IniFile(path);

        dataGame = new dataGameStruct();

        // Total
        f.IniWriteValue("DataGame", "totalScore", "0");
        f.IniWriteValue("DataGame", "soundFx", "false");
        f.IniWriteValue("DataGame", "music", "false");
    }

    public static void Save()
    {
        //Debug.Log("Save");
        IniFile f = new IniFile(Application.persistentDataPath + "/dataGame.WarMachineSaveFile");

        // Total

        f.IniWriteValue("DataGame", "totalScore", dataGame.totalScore.ToString());
        f.IniWriteValue("DataGame", "soundFx", dataGame.soundFx.ToString());
        f.IniWriteValue("DataGame", "music", dataGame.music.ToString());
    }

    public static void Load()
    {
        //Debug.Log("Load");
        InitData();

        IniFile f = new IniFile(Application.persistentDataPath + "/dataGame.WarMachineSaveFile");

        if (dataGame == null)
              dataGame = new dataGameStruct();

        // Total
        dataGame.totalScore = int.Parse(f.IniReadValue("DataGame", "totalScore"));

        dataGame.soundFx = (f.IniReadValue("DataGame", "soundFx") == "true") ? true : false;
        dataGame.music = (f.IniReadValue("DataGame", "music") == "true") ? true : false;
    }

    public static void ResetData()
    {
        IniFile f = new IniFile(Application.persistentDataPath + "/dataGame.WarMachineSaveFile");

        // Total
        f.IniWriteValue("DataGame", "totalScore", "0");
        f.IniWriteValue("DataGame", "soundFx", "false");
        f.IniWriteValue("DataGame", "music", "false");
    }


}
