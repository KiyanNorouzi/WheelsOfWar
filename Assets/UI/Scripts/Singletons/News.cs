using UnityEngine;
using System.Collections;

public class News : MonoBehaviour 
{
    #region Singleton

    static News _instance;

    public static News Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    #endregion 

	
    public event Data.generalDelegate OnNewsFetched;


    public NewsItem[] newsItems;
    
    
    bool isNewsLoaded;
    public bool IsNewsLoaded
    {
        get { return isNewsLoaded; }
    }

    void _NewsFetched(string decodedText)
    {
        newsItems = new NewsItem[GeneralSettings.Instance.NewsCount];

        if (decodedText.StartsWith("systemerror"))
        {
            Debug.Log("error loading news: " + decodedText);
            return;
        }

        //string data = decodedText.Substring(2);

        string data = "";
        if (decodedText.ToLower().StartsWith("ok"))
            data = decodedText.Substring(2);
        else
        {
            Debug.Log("error news data=" + decodedText);
            return;
        }


        //Debug.Log("news data=" + data);

        int semiColonIndex = 0;
        for (int i = 0; i < newsItems.Length; i++)
        {
            newsItems[i] = new NewsItem();

            semiColonIndex = data.IndexOf(";");

            //Debug.Log("i=" + i + ", semi colon index=" + semiColonIndex + ", substring=" + data.Substring(0, semiColonIndex));

            newsItems[i].ID = int.Parse(data.Substring(0, semiColonIndex));
            data = data.Substring(semiColonIndex + 1);

            semiColonIndex = data.IndexOf(";");
            newsItems[i].textEN = data.Substring(0, semiColonIndex);
            data = data.Substring(semiColonIndex + 1);

            semiColonIndex = data.IndexOf(";");
            newsItems[i].textFA = data.Substring(0, semiColonIndex);
            data = data.Substring(semiColonIndex + 1);

            semiColonIndex = data.IndexOf(">");
            newsItems[i].link = data.Substring(0, semiColonIndex);
            data = data.Substring(semiColonIndex + 1);

            if (semiColonIndex >= data.Length - 1)
                break;
        }

        if (OnNewsFetched != null)
            OnNewsFetched();

        isNewsLoaded = true;
    }
}

[System.Serializable]
public class NewsItem
{
    public int ID = -1;
    public string textFA, textEN, link;
    
}