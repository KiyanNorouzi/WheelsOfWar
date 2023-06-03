using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapButton : MonoBehaviour 
{
    public Text unlockText, gasText;
    public GameObject unlockOverlayGameObject;
    public Animator animator;

	public void SetLock(bool locked, int level, int consumeGas)
    {
        unlockOverlayGameObject.SetActive(locked);
        unlockText.text = string.Concat("UNLOCK AT LEVEL ", level);
        gasText.text = consumeGas.ToString();
    }
}
