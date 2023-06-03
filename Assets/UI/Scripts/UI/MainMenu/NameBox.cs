using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameBox : MonoBehaviour 
{
    public Button button;
    public Text buttonText, openButtonText;
    public InputField input;
    public Animator myAnimator;
    public GameObject errorGameObject;
    public Text emailText;

    
	void Start() 
    {
        Refresh();
	}

    public void Refresh()
    {
        emailText.text = Data.Email;

        string n = Data.UserName;
        if (string.IsNullOrEmpty(n) || n.Equals("???")) // never registered
        {
            buttonText.text = "Register";
            button.interactable = false;

            openButtonText.text = string.Concat("Hello Guest!"); //, System.Environment.NewLine, "[Register]");
        }
        else
        {
            input.text = n;
            buttonText.text = "Change";
            button.interactable = false;

            openButtonText.text = string.Concat("Hello, ", Data.UserName); //, System.Environment.NewLine, "[Change]");
        }
    }

    bool isIn;

    public void RegisterButton_Clicked()
    {
        isIn = !isIn;
        if (isIn)
            input.text = (string.IsNullOrEmpty(Data.UserName)) ? "Gues" : Data.UserName;
        else
            errorGameObject.SetActive(false);

        if (IsUsernameAcceptable(input.text))
        {
            button.interactable = false;
            if (isIn)
            {
                openButtonText.text = "Close [X]";
            }
            else
            {
                Data.UserName = input.text;
                if (string.IsNullOrEmpty(Data.UserName) || Data.UserName.Equals("???")) // never registered
                    openButtonText.text = string.Concat("Hello Guest!", System.Environment.NewLine, "[Register]");
                else
                    openButtonText.text = string.Concat("Hello, ", Data.UserName, System.Environment.NewLine, "[Change]");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(Data.UserName) || Data.UserName.Equals("???")) // never registered
                openButtonText.text = string.Concat("Hello Guest!", System.Environment.NewLine, "[Register]");
            else
                openButtonText.text = string.Concat("Hello, ", Data.UserName, System.Environment.NewLine, "[Change]");
        }

        myAnimator.SetBool("isin", isIn);
    }

	public void TextBox_TextChanged()
    {
        if (input.text.Equals(Data.UserName))
        {
            openButtonText.text = "Close [X]";
            button.interactable = false;
        }
        else if (!IsUsernameAcceptable(input.text))
        {
            errorGameObject.SetActive(true);
            openButtonText.text = "Cancel [X]";
            button.interactable = false;
        }
        else
        {
            errorGameObject.SetActive(false);
            openButtonText.text = "Save & Close [X]";
            button.interactable = true;
        }
            
    }



    bool IsUsernameAcceptable(string username)
    {
        NameValidationError error = GeneralSettings.ValidateName(username);
        if (error == NameValidationError.None)
            return true;
        else
            return false;
    }

    public void SignOutButton_Click()
    {
        CommonUI.Instance.messageBox.Ask(Messages.SignOut, CommonUI.Instance.SignOut, null, true);
    }
}