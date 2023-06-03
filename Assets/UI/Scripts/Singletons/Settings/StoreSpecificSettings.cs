using UnityEngine;
using System.Collections;

public class StoreSpecificSettings : MonoBehaviour 
{
    #region Singleton

    static StoreSpecificSettings _instance;

    public static StoreSpecificSettings Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    #endregion 

	
    public StoreSettings[] settings;
    public StoreSettings Setting
    {
        get 
        {
            if (index == -1)
                FindIndex();

            return settings[index]; 
        }
    }


    int index = -1;
    void FindIndex()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            if (settings[i].store == CommonUI.Instance.store)
            {
                index = i;
                break;
            }
        }
    }
}

[System.Serializable]
public class StoreSettings
{
    public Stores store;
    public string rateUsUrl;
    public string updateUrl;
}