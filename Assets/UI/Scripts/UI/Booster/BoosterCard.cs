using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class BoosterCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static BoosterCard draggingCard;
    public RectTransform myTransform;
    public Animator myAnimator;
    public CanvasGroup canvasGroup;
    public BoosterPackageType type;
    public Text priceText, titleText, explaneText;
    public int index;


    Transform defaultParent;
    Vector2 defaultPostition;
    bool isMoving, isScaling;
    Vector2 startPos, targetPos;
    float moveTime, moveDuration;
    Vector3 startScale, targetScale;
    float scaleTime, scaleDuration;

    bool isInDeck = true;



    [ContextMenu("Collect Info")]
    void collectinfo()
    {
        myTransform = GetComponent<RectTransform>();
        myAnimator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    void Start()
    {
        defaultPostition = myTransform.anchoredPosition;
        defaultParent = myTransform.parent;


        _RefreshCards();
    }

    void OnEnable()
    {
        Accounting.Instance.currentUser.OnVIPPackagesChanged += currentUser_OnVIPPackagesChanged;
    }

    void OnDisable()
    {
        Accounting.Instance.currentUser.OnVIPPackagesChanged -= currentUser_OnVIPPackagesChanged;
    }

    void currentUser_OnVIPPackagesChanged()
    {
        _RefreshCards();
    }

    void _RefreshCards()
    {
        float boosterPriceMultiplyer = Accounting.Instance.currentUser.GetVIPActionMultiplyer(VIPActionType.BoosterPrice);

        for (int cnt = 0; cnt < StoreData.Instance.packages.Length; cnt++)
        {
            if (StoreData.Instance.packages[cnt].type == type)
            {
                priceText.text = Mathf.RoundToInt(StoreData.Instance.packages[cnt].price.Bills * boosterPriceMultiplyer).ToString();
                titleText.text = StoreData.Instance.packages[cnt].packageNameEN;
                explaneText.text = StoreData.Instance.packages[cnt].DescEN;
                index = StoreData.Instance.packages[cnt].index;
            }
        }
    }



    #region Interface Methods

    bool isDragging;
    Vector2 dragStartPos, myStartPos;


    BoosterSlot mySlot;
    public void PickedUp(BoosterSlot slot)
    {
        isInDeck = false;
        mySlot = slot;
    }

    public void BackInDeck()
    {
        isInDeck = true;
        mySlot = null;

        myTransform.SetParent(defaultParent);

        myAnimator.SetBool("big", false);
        MoveTo(defaultPostition, 0.25f);
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown()
    {
        myAnimator.SetBool("big", true);
        myTransform.SetSiblingIndex(6);
    }

    public void OnPointerUp()
    {
        Invoke("CheckForComeBack", 0.1f);
    }

    void CheckForComeBack()
    {
        if (isInDeck)
        {
            myAnimator.SetBool("big", false);
            if (isDragging)
                OnEndDrag(null);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("on start drag=" + eventData.position);

        draggingCard = this;

        isDragging = true;
        dragStartPos = eventData.position;
        myStartPos = myTransform.anchoredPosition;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        

        if (isDragging)
        {
            Vector2 diff = eventData.position - dragStartPos;
            diff *= GameplayUI.Instance.boosterPanel.ScreenWidthK;
            myTransform.anchoredPosition = myStartPos + diff;

            Debug.Log("on drag=" + eventData.position + ", is draggin=" + isDragging + ", diff=" + diff);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("on end drag=" + eventData.position);
        isDragging = false;

        if (isInDeck)
        {
            MoveTo(defaultPostition, 0.25f);
            canvasGroup.blocksRaycasts = true;
        }

        draggingCard = null;
    }

    #endregion


    public void MoveTo(Vector2 target, float duration)
    {
        this.startPos = myTransform.anchoredPosition;
        this.targetPos = target;

        this.moveTime = 0;
        this.moveDuration = duration;

        isMoving = true;
    }

    public void ScaleTo(Vector3 targetScale, float duration)
    {
        this.startScale = myTransform.localScale;
        this.targetScale = targetScale;

        this.scaleTime = 0;
        this.scaleDuration = duration;

        isScaling = true;
    }

    void Update()
    {
        if (isMoving)
        {
            moveTime += Time.deltaTime;
            float flow = moveTime / moveDuration;
            if (flow >= 1)
            {
                myTransform.anchoredPosition = targetPos;
                isMoving = false;
            }
            else
                myTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, flow);
        }

        if (isScaling)
        {
            scaleTime += Time.deltaTime;
            float flow = scaleTime / scaleDuration;
            if (flow >= 1)
            {
                myTransform.localScale = targetScale;
                isScaling = false;
            }
            else
                myTransform.localScale = Vector3.Lerp(startScale, targetScale, flow);
        }
    }
}
