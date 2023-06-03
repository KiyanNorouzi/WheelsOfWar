using UnityEngine;
using System.Collections;

public class OptionContentPanel : MonoBehaviour 
{
    public GameObject myGameObject;

	public CreditsButton creditButton;
	public PrivacyButton privacyButton;
	public EulaButton eULAButton;

	public GameObject aboutText;

	public virtual void Activate()
    {
        myGameObject.SetActive(true);
		if( creditButton != null && privacyButton != null && eULAButton != null ){
			creditButton.Deactivate ();


			aboutText.SetActive(true);
		}
    }

    public virtual void Deactivate()
    {
        myGameObject.SetActive(false);
    }
}
