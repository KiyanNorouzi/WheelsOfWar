using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TemporaryDataPanel : MonoBehaviour 
{
    public GameObject myGameObject;
    public InputField googlePlayIDInputField;
    public LoginSceneController loginSceneController;


	void Start()
    {
	
	}

	public void LoginButton_Click()
    {
        /*loginSceneController.isSignedInToGooglePlay = true;
        loginSceneController.googlePlayID = googlePlayIDInputField.text;

        myGameObject.SetActive(false);
        loginSceneController.gameObject.SetActive(true);*/
    }
}
