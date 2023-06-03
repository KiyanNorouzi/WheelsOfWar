using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageBox : Window
{
    public Text messageTextEN, messageTextFA;
    public GameObject questionLayout, messageLayout;
    public string[] messagesFA, messagesEN;
    public Animator textAnimator;


    public MessageBox copy;


    [ContextMenu("copy")]
    void contextmenu()
    {
        messagesEN = new string[copy.messagesEN.Length];
        for (int i = 0; i < messagesEN.Length; i++)
            messagesEN[i] = copy.messagesEN[i];

        messagesFA = new string[copy.messagesFA.Length];
        for (int i = 0; i < messagesFA.Length; i++)
            messagesFA[i] = copy.messagesFA[i];
    }



    System.Action yesMethod, noMethod, okMethod;


    public void NoButton_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        Deactivate();

        if (noMethod != null)
            noMethod();
    }

    public void YesButton_Clicked()
    {
        CommonUI.Instance.PlayButtonClick();
        Deactivate();

        if (yesMethod != null)
            yesMethod();
    }


    public void ShowMessage(Messages message, System.Action okMethod, bool vibrate, params string[] p)
    {
        this.noMethod = okMethod;
        this.yesMethod = okMethod;

        _SetMessage(message, p);

        messageLayout.SetActive(true);
        questionLayout.SetActive(false);


        textAnimator.enabled = vibrate;
        base.Activate();
    }

    public void Ask(Messages message, System.Action yesMethod, System.Action noMethod, bool vibrate, params string[] p)
    {
        this.yesMethod = yesMethod;
        this.noMethod = noMethod;

        _SetMessage(message, p);


        messageLayout.SetActive(false);
        questionLayout.SetActive(true);

        textAnimator.enabled = vibrate;
        base.Activate();
    }

    private void _SetMessage(Messages message, params string[] p)
    {
        string messageString = "";

        messageTextFA.text = messageTextEN.text = "";
        Text thisLanguageText = null;

        if (SettingData.LanguageIndex == (int)LanguageName.Persian)
        {
            messageString = messagesFA[(int)message];
            thisLanguageText = messageTextFA;
        }
        else if (SettingData.LanguageIndex == (int)LanguageName.English)
        {
            messageString = messagesEN[(int)message];
            thisLanguageText = messageTextEN;
        }

        if (p != null && p.Length > 0)
            messageString = string.Format(messageString, p);

        thisLanguageText.text = messageString.Replace("<br>", System.Environment.NewLine);
    }

    /*public void Ask(int money, generalDelegate yesMethod, generalDelegate noMethod, params string[] p)
    {
        this.yesMethod = yesMethod;
        this.noMethod = noMethod;

        //question = string.Format(question, p);
        questionText.text = string.Format(buyTextFormat, money);

        messageLayout.SetActive(false);
        questionLayout.SetActive(true);

        myGameObject.SetActive(true);
    }*/

}

public enum Messages
{
    NotEnoughGolds,
    CarIsCoolingDown,
    CarIsNotAvailable,
    Quit,
    DisconnectedFromServer,
    BackToMainMenu,
    ConfirmBuyItem,
    SignOut,
    PasswordWillBeSent,
    NoSuchUsername,
    PleaseUpdate,
    YouDisconnected,
    RoomClosedForNotFindEnoughPlayers,
    MapNotAvailableYet,
    ServersUnderMaintenance,
    CouldnotEstablishConnectionToServer,
    RegisterFailed,
    JoinRoomFailed,
    HackDetected,
    ThanksForFacebook,
    ThanksForInstagram,
    ThanksForVideoAd,
    ErrorInVideoAd,
    AdNotAvailable,
    YouHavetoBeLevelXToOpenThisSlot,
    SureToQuitTutorial,
    WrongPassword,
    TestMessage,
    YouHaveToBeLevelXToBuyThisCar,
    MapLocked,
    NotEnoughGas,
    YouMustHaveVIPPackageToUseThisSlot,
    NotEnoughBills,
    YouHaveAnotherUpgradeInProgress
}