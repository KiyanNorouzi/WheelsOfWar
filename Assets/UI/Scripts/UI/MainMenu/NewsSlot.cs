using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewsSlot : MonoBehaviour 
{
    public GameObject myGameObject;
    public Text newsTextFA;
	public Text newsTextEN;
    public Color normalNewsColor, linkNewsColor;
	public Image pic;
	public GameObject Button;
	public Text butTextEn;
	public Text butTextFa;


    int id;
    string newsFA, newsEN, link;


    void OnEnable()
    {
        SettingData.OnSwitchLanguage += SettingData_OnSwitchLanguage;
    }

    void OnDisable()
    {
        SettingData.OnSwitchLanguage -= SettingData_OnSwitchLanguage;
    }



    void SettingData_OnSwitchLanguage()
    {
        if (SettingData.LanguageIndex == 0) // english
			newsTextFA.text = newsEN;
        else
			newsTextFA.text = newsFA;

        if (string.IsNullOrEmpty(link))
			newsTextFA.color = normalNewsColor;
        else
        {
			newsTextFA.color = linkNewsColor;
			newsTextFA.text += System.Environment.NewLine + link;
        }
    }


    public void Activate(NewsItem news)
    {
        Activate(news.ID, news.textEN, news.textFA, news.link);
    }

    public void Activate(int id, string newsEN, string newsFA, string link)
    {
        this.id = id;
        this.newsEN = newsEN;
        this.newsFA = newsFA;
        this.link = link;

        if (SettingData.LanguageIndex == 0) // english
			newsTextFA.text = newsEN;
        else
			newsTextFA.text = newsFA;

        if (string.IsNullOrEmpty(link))
			newsTextFA.color = normalNewsColor;
        else
        {
			newsTextFA.color = linkNewsColor;
			newsTextFA.text += System.Environment.NewLine + link;
        }
        
        myGameObject.SetActive(true);
    }



    public void Slot_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        if (!string.IsNullOrEmpty(link))
            Application.OpenURL(link);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}
