using UnityEngine;
using System.Collections;

public class NewsLine : MonoBehaviour 
{
    public GameObject myGameObject;
    public RectTransform myTransform;
    public Animator myAnimator;
    public UnityEngine.UI.Text newsText;


    public bool IsActive
    {
        get { return myGameObject.activeInHierarchy; }
    }


	public void Activate(string text)
    {
        newsText.text = text;
        time = 2;
        myGameObject.SetActive(true);
    }

	public void Activate(string text, int pplayer)
	{
		newsText.text = text;

		ScrCarController car = ScrController.Instance.FindCar(pplayer);

		if (car.Owner.GetTeam () == PunTeams.Team.blue) {
			newsText.color = Color.blue;
		}
		else if (car.Owner.GetTeam () == PunTeams.Team.red) {
			newsText.color = Color.red;
		}

		time = 2;
		myGameObject.SetActive(true);
	}

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }

    float time;
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
            myAnimator.SetTrigger("fade");
    }
}