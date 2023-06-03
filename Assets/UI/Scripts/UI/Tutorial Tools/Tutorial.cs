using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
    public delegate void tutorialFramePassed(string frameTag);
    public event tutorialFramePassed OnFramePassed;

    public GameObject myGameObject;
    public GameObject barbodGameObject, dialogGameObject;
    public RectTransform myTransform, barbodTransform, dialogTransform, baloonPointerTransform;
    public HighlightRect rect;
    public Text tutorialText;
    public Text textEn;
    public GameObject[] backgroundButtons;
    public TutorialFrameInfo[] info;
    public float minimumSkipWaitTime;


    [HideInInspector]
    public int editingFrameIndex;


    float activeTime;
    TutorialFrameInfo currentFrame;
    bool stayAside;


    void Start()
    {
        float originalAspect = 16f /  9f;
        float currentAspect = 0;
        
        if (Screen.width > Screen.height)
            currentAspect = ((float)Screen.width) / ((float)Screen.height);
        else
            currentAspect = ((float)Screen.height) / ((float)Screen.width);

        float f = currentAspect / originalAspect;
        //Debug.Log("orig=" + originalAspect + ", curre=" + currentAspect + ", cam=" + Camera.main.aspect + ", f=" + f);

        myTransform.localScale = new Vector3(f, 1, 1);

        Deactivate();

        rect.OnClick += rect_OnClick;
    }

    void rect_OnClick()
    {
        _SkipFrame();    
    }

    public void NextButton_Click()
    {
        _SkipFrame();
    }

    void _SkipFrame()
    {
        if (Time.time - activeTime < minimumSkipWaitTime)
            return;

        Deactivate();
        if (OnFramePassed != null)
            OnFramePassed(currentFrame.tag);
    }


    
    public void Deactivate()
    {
        barbodGameObject.SetActive(false);
        dialogGameObject.SetActive(false);
        _SetBackgroundButtons(false);
        rect.Deactivate();

        myGameObject.SetActive(false);
    }

    void _SetBackgroundButtons(bool enabled)
    {
        for (int i = 0; i < backgroundButtons.Length; i++)
            backgroundButtons[i].SetActive(enabled);
    }


    public void LoadFrameASide(string tag, float stayDuration)
    {
        stayAside = true;

        tag = tag.ToLower();
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].tag.ToLower().Equals(tag))
            {
                currentFrame = info[i];
                LoadFrameToScene(info[i]);
                break;
            }
        }
        
        activeTime = Time.time + stayDuration;
        rect.Deactivate();
        _SetBackgroundButtons(false);

        if (!myGameObject.activeSelf)
            myGameObject.SetActive(true);
    }

    public void LoadFrame(string tag)
    {
        stayAside = false;

        tag = tag.ToLower();
        for (int i = 0; i < info.Length; i++)
        {
            if (info[i].tag.ToLower().Equals(tag))
            {
                currentFrame = info[i];
                LoadFrameToScene(info[i]);
                activeTime = Time.time;
                return;
            }
        }

        
        currentFrame = null;
        Debug.LogError("[" + tag + "] frame not found");

        if (!myGameObject.activeSelf)
            myGameObject.SetActive(true);
    }



    public void LoadFrameToScene(TutorialFrameInfo info)
    {
        barbodGameObject.SetActive(info.IsBarbodActive);

        if (info.IsBaloonActive)
        {
            barbodTransform.anchoredPosition = info.barbodPosition;
            barbodTransform.localRotation = info.barbodRotation;
            barbodTransform.localScale = info.barbodScale;
        }


        dialogGameObject.SetActive(info.IsBaloonActive);
        if (info.IsBaloonActive)
        {
            tutorialText.text = info.text;
            textEn.text = info.text_En;
           

            dialogTransform.anchoredPosition = info.baloonPosition;
            dialogTransform.localRotation = info.baloonRotation;
            dialogTransform.localScale = info.baloonScale;

            baloonPointerTransform.localPosition = info.baloonPointerPos;
            baloonPointerTransform.localRotation = info.baloonPointerRotation;
            baloonPointerTransform.localScale = info.baloonPointerScale;
        }
        else
        {
            tutorialText.text = "";
            textEn.text = "";
        }


        
        if (info.IsHighlightRectActive)
        {
            rect.Activate(info.RectPosition, info.RectSize, info.AcceptClickOnlyInHighlightArea);
        }
        else
        {
            rect.Deactivate();
        }

        if (info.IsHighlightRectActive && !info.AcceptClickOnlyInHighlightArea)
        {
            backgroundButtons[0].SetActive(true);
            backgroundButtons[1].SetActive(false);
        }
        else
            _SetBackgroundButtons(!info.IsHighlightRectActive);

        if (!myGameObject.activeSelf)
            myGameObject.SetActive(true);
    }

    public TutorialFrameInfo GetFrameInfoFromScene()
    {
        TutorialFrameInfo info = new TutorialFrameInfo();
        info.IsBarbodActive = barbodGameObject.activeSelf;
        info.barbodPosition = barbodTransform.anchoredPosition;
        info.barbodRotation = barbodTransform.localRotation;
        info.barbodScale = barbodTransform.localScale;

        info.IsBaloonActive = dialogGameObject.activeSelf;
        info.text = tutorialText.text;
        info.text_En = textEn.text;
        info.baloonPosition = dialogTransform.anchoredPosition;
        info.baloonRotation = dialogTransform.localRotation;
        info.baloonScale = dialogTransform.localScale;

        info.IsHighlightRectActive = rect.IsActive;
        info.RectPosition = rect.myTransform.anchoredPosition;
        info.RectSize = new Vector2(rect.myTransform.rect.width, rect.myTransform.rect.height);

        info.baloonPointerPos = baloonPointerTransform.localPosition;
        info.baloonPointerRotation = baloonPointerTransform.localRotation;
        info.baloonPointerScale = baloonPointerTransform.localScale;



        //Debug.Log("baloon pointer pos=" + baloonPointerTransform.)

        return info;
    }




    void Update()
    {
        if (stayAside && Time.time >= activeTime)
            Deactivate();
    }
}

[System.Serializable]
public class TutorialFrameInfo
{
    public string tag;


    public bool IsBarbodActive;
    public Vector2 barbodPosition;
    public Quaternion barbodRotation;
    public Vector3 barbodScale;

    public bool IsBaloonActive;
    public string text;
    public string text_En;
    public Vector2 baloonPosition;
    public Quaternion baloonRotation;
    public Vector3 baloonScale;

    public Vector3 baloonPointerPos;
    public Quaternion baloonPointerRotation;
    public Vector3 baloonPointerScale;

    public bool IsHighlightRectActive;
    public Vector2 RectPosition;
    public Vector2 RectSize;
    public bool AcceptClickOnlyInHighlightArea;




    public override string ToString()
    {
        string str = "barbod active=";
        str += IsBaloonActive.ToString();
        str += ";";
        str += "barbod pos=" + barbodPosition.ToString();
        str += ";";

        return str;
    }
}