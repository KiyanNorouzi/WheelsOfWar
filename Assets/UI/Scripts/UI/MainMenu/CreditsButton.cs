using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour {

	public GameObject myGameObject;
	public OptionContentPanel aboutOptionPanel;
	
	public virtual void Activate()
	{
		myGameObject.SetActive(true);
		aboutOptionPanel.aboutText.SetActive (false);
		
	}
	
	public virtual void Deactivate()
	{
		myGameObject.SetActive(false);
	}
}
