using UnityEngine;
using System.Collections;

public class ScrEnemyTestController : ScrCarController 
{
    public TextMesh healthShow;

    void Awake()
    {
        Invoke("BornEnemy",0.1f);
    }

   void BornEnemy()
   {
       this.name = "Enemy";
       healthManager.CurrentHealth = GameplayDefaultSettings.Instance.defaultHealth;
       healthManager.CurrentShiled = GameplayDefaultSettings.Instance.defaultShield;

       _RefreshHealthAndArmorUI();


       ScrController.Instance.AddCar(this, photonView.isMine);
       //ScrGunController.AddCar(this);
   }

   void LateUpdate() { healthShow.text = healthManager.CurrentHealth.ToString(); }

    void Update() { }

    void FixedUpdate() { }

    void OnTriggerEnter(Collider col) { }
}
