using UnityEngine;
using System.Collections;

public class BilboardText : MonoBehaviour 
{
    public Transform myTransform;
    public float textSizeMultiplyer;
    public TextMesh textMesh;
    public SpriteRenderer triangleSprite;

    ScrCarController carController;
    Transform camTransform;


    public void SetCameraTransform(Transform cameraTransform, ScrCarController car)
    {
        this.camTransform = cameraTransform;
        this.carController = car;

        RefreshColor();
    }

    void Update()
    {
        if (camTransform == null)
            return;

        Vector3 rot = camTransform.rotation.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        myTransform.rotation = Quaternion.Euler(rot);

        float distance = (camTransform.position - myTransform.position).magnitude * textSizeMultiplyer;
        myTransform.localScale = new Vector3(distance, distance, 1);
        //textMesh.characterSize = distance * textSizeMultiplyer;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string username)
    {
        textMesh.text = username;
    }

    public void RefreshColor()
    {
        if (carController != null)
        {
            Color c = Color.Lerp(Color.red, Color.green, carController.healthManager.CurrentHealth / 100f);
            textMesh.color = triangleSprite.color = c;

            //Debug.Log(transform.parent.name + ", refresh color, health=" + carController.healthManager.currentHealth);
        }
        /*else
            Debug.Log("refresh color on car controller null");*/
    }
}