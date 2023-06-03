using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NamePanel : MonoBehaviour 
{
    public event Data.generalDelegate OnClose;



    public InputField nameField;
    public GameObject myGameObject;
    public Text errorTextFA, errorTextEN;
    public Animator[] errorAnimators;
    public string[] errorMessagesFA, errorMessagesEN;

	
    public void Activate()
    {
        nameField.text = Data.UserName;

        errorTextEN.text = errorTextFA.text = "";
        myGameObject.SetActive(true);
    }

    public void OKButton_Click()
    {
        NameValidationError error = GeneralSettings.ValidateName(nameField.text);
        if (error != NameValidationError.None)
        {
            int index = ((int)error) - 1;
            errorTextFA.text = errorMessagesFA[index];
            errorTextEN.text = errorMessagesEN[index];

            for (int i = 0; i < errorAnimators.Length; i++)
                errorAnimators[i].SetTrigger("error");

            return;
        }

        Data.UserName = nameField.text;
        myGameObject.SetActive(false);


        if (OnClose != null)
            OnClose();
    }
}