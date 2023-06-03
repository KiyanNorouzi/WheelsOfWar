using UnityEngine;
using System.Collections;

public class Data
{
    BitwiseFlag flags;
    public delegate void generalDelegate();


    public static bool IsRated
    {
        get { return PlayerData.GetString("israted", "") == CommonUI.Instance.appVersion; }
        set { PlayerData.SetString("israted", value ? CommonUI.Instance.appVersion : ""); }
    }



    

    const float F = 10000;

    
    /*public static int Level
    {
        get
        {
            float a = (8f * TotalScore) / F;
            float a2 = 1 + a;
            float l = (float)((1 + Mathf.Sqrt(a2)) / 2f);

            return (int)l;
        }
    }*/


    public static string UserName;
    public static string Email;

    public static string LastLoginedUsername
    {
        get { return PlayerPrefs.GetString("lastuser", ""); }
        set { PlayerPrefs.SetString("lastuser", value); }
    }

    public static string LastLoginedUsernamePassword
    {
        get { return Encryption.DecryptString(PlayerPrefs.GetString("lastuser_p", "")); }
        set { PlayerPrefs.SetString("lastuser_p", Encryption.EncryptString(value)); }
    }


    /*public static string UserName
    {
        get { return PlayerPrefs.GetString("_username", ""); }
        set { PlayerData.SetString("_username", value); }
    }*/

    /*
    public static int SelectedCarIndex
    {
        get
        {
            int index = PlayerData.GetInt("_car", 0);
            /*if (IsCarLocked(Information.Instance.carInfo[index].carTag))
            {
                SelectedCarIndex = 0;
                return 0;
            }
            else
            Debug.Log("car get " + index);
            return index;
        }

        set 
        {
            Debug.Log("car set to " + value);
            PlayerData.SetInt("_car", value);
        }
    }*/



    public static bool IsCarLocked(string carTag)
    {
        int carLevel = PlayerData.GetInt(string.Concat("_carlevel_", carTag), -1);
        return carLevel == -1;
    }

    public static int GetCarLevel(string carTag)
    {
        return PlayerData.GetInt(string.Concat("_carlevel_", carTag), -1);
    }

    public static void SetCarLevel(string carTag, int level)
    {
        PlayerData.SetInt(string.Concat("_carlevel_", carTag), level);
    }

    public static int GetCarLevel(int carIndex)
    {
        string carTag = Information.Instance.carInfo[carIndex].carTag;
        return GetCarLevel(carTag);
    }

    public static void SetCarLevel(int carIndex, int level)
    {
        string carTag = Information.Instance.carInfo[carIndex].carTag;
        SetCarLevel(carTag, level);
    }

    public static int GetCarUseCount(string carTag)
    {
        return PlayerData.GetInt(string.Concat("_carUseCount_", carTag), 0);
    }

    public static void SetCarUseCount(string carTag, int uses)
    {
        PlayerData.SetInt(string.Concat("_carUseCount_", carTag), uses);
    }

    public static int GetCarUseCount(int carIndex)
    {
        string carTag = Information.Instance.carInfo[carIndex].carTag;
        return GetCarUseCount(carTag);
    }

    public static void SetCarUseCount(int carIndex, int uses)
    {
        string carTag = Information.Instance.carInfo[carIndex].carTag;
        SetCarUseCount(carTag, uses);
    }



    public static int GetCarUseCountSinceUpgrade(string carTag)
    {
        return PlayerData.GetInt(string.Concat("_carUseCountsu_", carTag), 0);
    }

    public static void SetCarUseCountSinceUpgrade(string carTag, int uses)
    {
        PlayerData.SetInt(string.Concat("_carUseCountsu_", carTag), uses);
    }

    public static int GetCarUseCountSinceUpgrade(int carIndex)
    {
        string carTag = Information.Instance.carInfo[carIndex].carTag;
        return GetCarUseCountSinceUpgrade(carTag);
    }

    public static void SetCarUseCountSinceUpgrade(int carIndex, int uses)
    {
        string carTag = Information.Instance.carInfo[carIndex].carTag;
        SetCarUseCountSinceUpgrade(carTag, uses);
    }




    /*public static void SetCoolDownForCar(int carIndex)
    {
        int nowInSeconds = (int)(System.DateTime.Now.Ticks / 10000000);
        int value = nowInSeconds + Information.Instance.carInfo[carIndex].levels[0].coolDownTime;

        //Debug.Log("[SET] now=" + nowInSeconds + ", time=" + value + ", diff=" + (value - nowInSeconds));
        PlayerData.SetString(string.Format("_c{0}hl", carIndex), Encryption.Encrypt(value));
        NotificationManager.Instance.SetRepairNotificationForCar(carIndex, Information.Instance.carInfo[carIndex].levels[0].coolDownTime);
    }*/

    public static int GetCarEndCoolDownTime(int carIndex)
    {
        string value = PlayerData.GetString(string.Format("_c{0}hl", carIndex), "0");
        if (value == "0")
            return 0;
        else
        {
            int time = Encryption.Decrypt(value);
            int nowInSeconds = (int)(System.DateTime.Now.Ticks / 10000000);
            //Debug.Log("[GET] now in seconds=" + MathHelper.GetStringWithComma(nowInSeconds) + ", time=" + time + ", diff=" + (time - nowInSeconds));

            return time - nowInSeconds;
            //return Mathf.Max(0, nowInSeconds - time);
        }
    }

    public static void CancelCoolDownForCar(int carIndex)
    {
        PlayerData.SetString(string.Format("_c{0}hl", carIndex), "0");
    }

    public static int InputMethod
    {
        get
        {
            return PlayerData.GetInt("_inputm", 0);
        }
        set 
        {
            PlayerData.SetInt("_inputm", value);
        }
    }
}