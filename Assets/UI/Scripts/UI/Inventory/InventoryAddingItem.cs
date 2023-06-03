using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryAddingItem : MonoBehaviour 
{
    public delegate void addingItemReached(int slotIndex, int count, bool justForShow);
    public event addingItemReached OnReached;

    public GameObject myGameObject;
    public Image myImage;
    public RectTransform myTrasnform;
    public AnimationCurve moveCurve;
    public float duration;
    public AudioStruct audioStruct;

    [System.Serializable]
    public class AudioStruct : AudioStructBase
    {
        public AudioClip buyItem;
    }

    float time;
    Vector3 sourcePos, destPos;
    bool justForShow;


    int addingSlotIndex, count;

    

    public BoosterPackageType Type
    {
        get { return (BoosterPackageType)addingSlotIndex; }
    }

    public int CarryingItemCount
    {
        get { return count; }
        set { count = value; }
    }

    public bool IsActive
    {
        get { return myGameObject.activeSelf; }
    }


    public void Activate(Vector3 sourcePos, Vector3 destPos, Sprite sprite, int slotIndex, int count, bool justForShow)
    {
        this.justForShow = justForShow;
        this.addingSlotIndex = slotIndex;
        this.count = count;

        this.sourcePos = RectTransformUtility.WorldToScreenPoint(Camera.main, sourcePos);
        this.destPos = destPos;

        myImage.sprite = sprite;
        myTrasnform.position =  sourcePos;

        time = 0;
        myGameObject.SetActive(true);

        if (GameplayUI.Instance != null)
        {
            if (count < 0)
                audioStruct.Play(audioStruct.buyItem);
        }
        else
            audioStruct.Play(audioStruct.buyItem);
    }

    public void Deactivate()
    {
        myGameObject.SetActive(false);
    }
	
	void Update()
    {
        time += Time.deltaTime;
        float flow = time / duration;
        flow = moveCurve.Evaluate(flow);

        if (flow >= 1)
        {
            Deactivate();

            if (OnReached != null)
                OnReached(addingSlotIndex, count, justForShow);
        }
        else
        {
            myTrasnform.position = sourcePos + (destPos - sourcePos) * flow;
        }
	}
}