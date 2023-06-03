using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
    public GameOverMenu gameOverMenu;

	IEnumerator Start () 
    {
        gameOverMenu.Deactivate();

        yield return new WaitForSeconds(1);
        gameOverMenu.Activate(false, 1, 8000, 100, 50, 10, 0, 0);
	}
}