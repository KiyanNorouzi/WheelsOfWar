using UnityEngine;
using System.Collections;

public class BlackScreen : MonoBehaviour 
{
    public GameObject myGameObject, loadingGameObject, hintsParentGameObject;
    public Animator myAnimator;
    public GameObject[] hintGameObjects;

    Data.generalDelegate doneMethod;
    float startLoadingTime;
    bool firstBlackScreen;


    void Start()
    {
    }

    public void ToBlack(Data.generalDelegate doneMethod, bool showLoading = true)
    {
        //Debug.Log("Black screen activate, show loading=" + showLoading);

        this.doneMethod = doneMethod;

        int hintIndex = -1;
        if (CommonUI.Instance.IsSignedIn)
            hintIndex = Random.Range(0, hintGameObjects.Length);

        for (int i = 0; i < hintGameObjects.Length; i++)
            hintGameObjects[i].SetActive(i == hintIndex);

        if (loadingGameObject != null)
            loadingGameObject.SetActive(showLoading);

        if (hintsParentGameObject != null)
            hintsParentGameObject.SetActive(showLoading);

        myGameObject.SetActive(true);

        StartCoroutine(Done(true, 0, 0.5f, doneMethod));

        if (SettingData.SoundInitialized)
            CommonUI.Instance.audioPlayer.Play(CommonUI.Instance.audioPlayer.loading);

        startLoadingTime = Time.time;
        
    }

    public void ToTransparent(Data.generalDelegate doneMethod)
    {
        this.doneMethod = doneMethod;

        float duration = Time.time - startLoadingTime;
        float diff = GeneralSettings.Instance.LoadingMinimumTime - duration;
        if (diff > 0 && loadingGameObject!=null && loadingGameObject.activeSelf)
            StartCoroutine(Done(false, diff,  0.5f, doneMethod));
        else
            StartCoroutine(Done(false, 0, 0.5f, doneMethod));
    }


    IEnumerator Done(bool turnOn, float delay, float wait, Data.generalDelegate doneMethod)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        if (!turnOn)
        {
            for (int i = 0; i < hintGameObjects.Length; i++)
                hintGameObjects[i].SetActive(false);

            myAnimator.SetTrigger("off");
        }
            

        yield return new WaitForSeconds(wait);

        if (!firstBlackScreen)
        {
            firstBlackScreen = true;
            //CommonUI.Instance.moneyTabletGameObject.SetActive(true);
        }

        if (doneMethod != null)
            doneMethod();

        if (!turnOn)
            myGameObject.SetActive(false);
    }

    public void ItIsBlack()
    {
    /*    Debug.Log("done");

        if (doneMethod != null)
            doneMethod();*/
    }

    public void ItIsDone()
    {
        /*Debug.Log("done");

        if (doneMethod != null)
            doneMethod();*/
    }

    public bool IsActive
    { get { return myGameObject.activeInHierarchy; } }
}