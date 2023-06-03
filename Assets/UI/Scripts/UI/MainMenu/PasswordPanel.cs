using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PasswordPanel : MonoBehaviour 
{
    public Data.generalDelegate doneMethod, cancelMethod;

    public GameObject myGameObject;
    public InputField passwordField;
    public GameObject nullErrorGameObject;



    string password;
    public string Password
    {
        get { return password; }
    }


    public void Activate(Data.generalDelegate doneMethod, Data.generalDelegate cancelMethod)
    {
        this.doneMethod = doneMethod;
        this.cancelMethod = cancelMethod;

        passwordField.text = "";
        myGameObject.SetActive(true);
    }


    public void OK_Click()
    {
        if (string.IsNullOrEmpty(passwordField.text))
        {
            nullErrorGameObject.SetActive(true);
            return;
        }

        this.password = passwordField.text;
        if (doneMethod != null)
            doneMethod();

        Deactivate();
    }

    public void OnClose_Click()
    {
        if (cancelMethod != null)
            cancelMethod();

        Deactivate();
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}