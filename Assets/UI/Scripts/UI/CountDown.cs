using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour 
{
    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip beep;
        public AudioClip liveBeep;
    }


    public AudioStruct audioPlayer;
    public GameObject myGameObject, cancelButtonGameObject;
    public Text numberText, descText;

    int lastSecond;
    float time;
    Data.generalDelegate doneMethod, cancelMethod;

    public void Activate(int seconds, Data.generalDelegate doneMethod)
    {
        Activate(seconds, false, doneMethod, null);
    }

    public void Activate(int seconds, bool hasCancelButton, Data.generalDelegate doneMethod, Data.generalDelegate cancelMethod)
    {
        Activate(seconds, hasCancelButton, doneMethod, cancelMethod, "WAIT");
    }

    bool hasCancelButton;
    public void Activate(int seconds, bool hasCancelButton, Data.generalDelegate doneMethod, Data.generalDelegate cancelMethod, string text)
    {
        this.hasCancelButton = hasCancelButton;

        this.doneMethod = doneMethod;
        this.cancelMethod = cancelMethod;

        cancelButtonGameObject.SetActive(hasCancelButton);
        numberText.text = seconds.ToString();

        time = seconds;
        lastSecond = seconds;

        descText.text = text;

        myGameObject.SetActive(true);
        _PlayBeep();
    }

    void _PlayBeep()
    {
        if (hasCancelButton)
            audioPlayer.Play(audioPlayer.beep);
        else
            audioPlayer.Play(audioPlayer.liveBeep);
    }

    public void CancelButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        if (cancelMethod != null)
            cancelMethod();

        myGameObject.SetActive(false);
    }


    void Update()
    {
        time -= Time.deltaTime;

        int intTime = (int)time;
        if (intTime != lastSecond)
        {
            lastSecond = intTime;
            numberText.text = (lastSecond + 1).ToString();

            if (intTime >= 0)
                _PlayBeep();
        }

        if (time <= 0)
        {
            myGameObject.SetActive(false);

            if (doneMethod != null)
                doneMethod();
        }
    }



    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}