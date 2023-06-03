using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextIndicator : MonoBehaviour 
{
    public Text myText;
    public Animator myAnimator;

	public void Show(int number)
    {
        
        if (number > 0)
        {
            myText.text = string.Concat("+", number);
            myAnimator.SetTrigger("up");
        }
        else
        {
            myText.text = number.ToString();
            myAnimator.SetTrigger("down");
        }
    }
}
