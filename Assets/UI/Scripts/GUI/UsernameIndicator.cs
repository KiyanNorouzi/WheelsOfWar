using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UsernameIndicator : MonoBehaviour
{
    public GameObject myGameObject;
    public RectTransform myTransform, arrowTransform;
    public Image arrowImage, invincibleImage, healthRegenerationImage, fireBulletImage;
    public Text usernameText;
	public Image teamImageColor;
    public Color[] colors;
    public Color armorColor;



    ScrCarController car;

    float halfWidth;
    float screenWidth;
    float eachColorChance;




    bool isActive;
    public bool IsActive
    {
        get { return isActive; }
    }

    void Start()
    {
        halfWidth = arrowImage.rectTransform.rect.width / 2f;
        screenWidth = Screen.width - halfWidth;

        eachColorChance = 1f / colors.Length;
        invincibleImage.enabled = false;
    }

    public void Activate(ScrCarController car)
    {
        this.car = car;
        usernameText.text = car.Username;
		SetUsername (car.Username);


        RefreshColor();

        healthRegenerationImage.enabled = car.HasHealthRegeneration;
        fireBulletImage.enabled = car.HasFireBullet;

        isActive = true;
        myGameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        myGameObject.SetActive(false);
    }


    public void ShowInvincible()
    {
        invincibleImage.enabled = true;
    }

    public void HideInvincible()
    {
        invincibleImage.enabled = false;
    }


    void Update()
    {
        if (car == null)
        {
            //Deactivate();
            return;
        }

        Vector3 indicatorPos = EnvironmentController.Instance.followCam.camera.WorldToScreenPoint(car.usernameTransform.position);


		SetUsername ( car.Username );
        bool outOfScreen = false;
        if (indicatorPos.x < halfWidth)
        {
            outOfScreen = true;
            indicatorPos.x = halfWidth;
            arrowTransform.localRotation = Quaternion.AngleAxis(-90, Vector3.forward);
        }

        if (indicatorPos.x > screenWidth)
        {
            outOfScreen = true;
            indicatorPos.x = screenWidth;
            arrowTransform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);
        }

        if (indicatorPos.y < 0)
        {
            outOfScreen = true;
            indicatorPos.y = 0;
            arrowTransform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
        }

        if (indicatorPos.z < 0)
        {
            outOfScreen = true;
            indicatorPos.y = 0;
            indicatorPos.x = Screen.width - indicatorPos.x;

            arrowTransform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
        }


        RefreshColor();

        if (!outOfScreen)
            arrowTransform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);

        usernameText.enabled = !outOfScreen;
        myTransform.position = indicatorPos;

        /*Vector2 angleP = myTransform.anchoredPosition;
        angleP.y += Screen.height / 2f;

        float angle = MathHelper.ToDegrees(Mathf.Atan2(angleP.y, angleP.x));
        myTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Debug.Log("pos=" + angleP + ", angle=" + angle);*/
        


        /*if (!test)
        {


            Vector3 position = ScrCarController.Instance.transform.position - ScrController.Instance.GetCar(1).transform.position;
            float angle = MathHelper.ToDegrees(Mathf.Atan2(position.x, position.z));

            Debug.Log("angle=" + angle);

            Vector2 indicatorPos = new Vector2();
            indicatorPos.x = Mathf.Cos(MathHelper.ToRadians(angle)) * 200;
            indicatorPos.y = Mathf.Sin(MathHelper.ToRadians(angle)) * 200;

            myTransform.anchoredPosition = indicatorPos;
        }*/
            
    }

    public void SetUsername(string username)
    {
        usernameText.text = username;
        RefreshColor();
    }

    public void RefreshColor()
    {
        if (car == null)
            return;

		if (GameplayDefaultSettings.Instance.isTeamMatch) {
			
			teamImageColor.gameObject.SetActive( true );	
			if( car.nv.owner.GetTeam() == PunTeams.Team.blue ){
				teamImageColor.color = Color.blue;
			}
			else{
				teamImageColor.color = Color.red;
			}
		}

        //float v = Mathf.Clamp01((car.healthManager.CurrentHealth + car.healthManager.CurrentShiled) / 100f);
		float h = (car.healthManager.CurrentHealth) / Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex].levels[0].health;
		float v = car.healthManager.CurrentShiled;
        if (v > 1)
            arrowImage.color = usernameText.color = armorColor;
        else
        {
            int colorIndex = Mathf.Clamp((int)(h / eachColorChance), 0, colors.Length - 1);
            arrowImage.color = usernameText.color = colors[colorIndex];
        }
    }

    public void Refresh()
    {
        healthRegenerationImage.enabled = car.HasHealthRegeneration;
        fireBulletImage.enabled = car.HasFireBullet;
    }
}