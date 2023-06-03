using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewsSection : MonoBehaviour 
{
    public GameObject newsPanel, newsIndicator, inboxIndiacator;
    public NewsSlot[] newsSlot;
    public RectTransform contentTransform;
    public GameObject eventBut;
    public GameObject inboxBut;
    public Color activColor;
    public Color deactiveColor;
    public GameObject inboxPanel;
    public GameObject eventPanel;
    


    int maxID;

    void Start()
    {
        EventView();
    }

    void OnEnable()
    {
        News.Instance.OnNewsFetched += Instance_OnNewsFetched;
    }

    void OnDisable()
    {
        News.Instance.OnNewsFetched -= Instance_OnNewsFetched;
    }



    void Instance_OnNewsFetched()
    {
        CheckForNews();
    }
    public void InboxView()
    {
        inboxPanel.SetActive(true);
        eventPanel.SetActive(false);
		inboxIndiacator.SetActive (false);
        inboxBut.GetComponent<Image>().color = activColor;
        eventBut.GetComponent<Image>().color = deactiveColor;
    }
    public void EventView()
    {
        inboxPanel.SetActive(false);
        eventPanel.SetActive(true);
        inboxBut.GetComponent<Image>().color = deactiveColor;
        eventBut.GetComponent<Image>().color = activColor;
    }
    
    public void CheckForNews()
    {
        maxID = int.MinValue;

        int activeNewsSlotCount = 0;
        for (int i = 0; i < newsSlot.Length; i++)
        {
            if (i < News.Instance.newsItems.Length)
            {
                if (News.Instance.newsItems[i] != null && News.Instance.newsItems[i].ID != -1)
                {
                    newsSlot[i].Activate(News.Instance.newsItems[i]);
                    maxID = Mathf.Max(maxID, News.Instance.newsItems[i].ID);

                    activeNewsSlotCount++;
                }
                else
                    newsSlot[i].Deactivate();
            }
            else
                newsSlot[i].Deactivate();
        }


        float height = Mathf.Max(activeNewsSlotCount * 70, 160);
        contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);


        if (CommonUI.Instance.IsTutorial || maxID <= PlayerData.GetInt("_lastNewsSeen"))
        {
            newsPanel.SetActive(false);
            newsIndicator.SetActive(false);
        }
        else
        {
            newsIndicator.SetActive(true);

            //PlayerData.SetInt("_lastNewsSeen", maxID);
            newsPanel.SetActive(true);
            PlayerData.SetInt("_lastNewsSeen", maxID);
        }
    }

    public void OpenNews_Click()
    {
        if (newsPanel.activeInHierarchy)
        {
            //myAnimator.SetTrigger("out");
            _DeactivateAfter();
        }
        else
        {
            newsIndicator.SetActive(false);

//            CheckForNews();
            newsPanel.SetActive(true);
        }
    }
    
    void _DeactivateAfter()
    {
        newsPanel.SetActive(false);
    }

    public void OpenNews_Close()
    {
        //myAnimator.SetTrigger("out");
        _DeactivateAfter();
        //newsPanel.SetActive(false);
    }

    public void ContactUs_Click()
    {
        Application.OpenURL(string.Format("mailto:{0}?Subject={1}", GeneralSettings.Instance.contactEmail, GeneralSettings.Instance.contactEmailSubject));
    }
}
