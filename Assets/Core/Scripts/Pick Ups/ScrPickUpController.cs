using UnityEngine;
using System.Collections;

public class ScrPickUpController : MonoBehaviour 
{
    public delegate void pickupCollected(int i, int j);
    public event pickupCollected OnCollected;


    [ContextMenu("Collect Info")]
    void collectinfo()
    {
        myGameObject = rotatingTransform.gameObject;
    }



    public GameObject myGameObject;
    public Transform rotatingTransform;
    public CollectibleType type;
    public int number;
    public AllItemsStruct allItems;
    public float respawnTime;
    public int i,j;
    



    float lastTime;



    public bool IsActive
    {
        get { return myGameObject.activeInHierarchy; }
    }



    [System.Serializable]
    public struct AllItemsStruct
    {
        public int rocket;
        public int mine;
		public int health;
		public int armor;

//		void Start(){
//			health = Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health / healthPricent;
//			armor = Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].armor / armorPricent;
//		}
    }



    [HideInInspector]
    public PhotonView nv;

    public bool rotate;
    public float speedRotate;

    void Awake()
    {
        /*if (!PhotonNetwork.isMasterClient)
            transform.SetParent(EnvironmentController.Instance.pickupManager.transform);*/
    }

    void RotateTheDetail()
    {
        rotatingTransform.Rotate(Vector3.right * Time.deltaTime * speedRotate);
    }

    void Update()
    {
        if (IsActive)
            return;

        if (rotatingTransform && rotate)
            RotateTheDetail();
    }

    public bool IsReadyToRespawn()
    {
        return (respawnTime + lastTime < Time.time);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Player")
        {
            ScrCarController car = col.GetComponent<ScrCarController>();
            if (car != null)
                AddToCar(car);


            if (OnCollected!=null)
                OnCollected(i,j);

            Kill();
        }
            
    }


    [ContextMenu("activate")]
    public void activate()
    {
        collider.enabled = true;
        myGameObject.SetActive(true);
    }


    public void Respawn()
    {
        collider.enabled = true;
        myGameObject.SetActive(true);
    }

    public void Kill()
    {
        lastTime = Time.time;

        collider.enabled = false;
        myGameObject.SetActive(false);
    }

    void AddToCar(ScrCarController car)
    {
        if (car)
        {
            GameplayUI.Instance.PlayCollectingItemSound(type);
            Accounting.Instance.currentUser.statistics.PickUpCollected(type);
            QuestManager.Instance.PickedUp(type);

            if (type == CollectibleType.All)
				car.AddItems(allItems.rocket, allItems.mine,Information.Instance.carInfo[ Accounting.Instance.currentUser.SelectedCarIndex ].levels[0].health / allItems.health ,Information.Instance.carInfo [Accounting.Instance.currentUser.SelectedCarIndex].levels [0].health / allItems.armor);
            else 
                car.AddItems(number, type);
        }
    }
}