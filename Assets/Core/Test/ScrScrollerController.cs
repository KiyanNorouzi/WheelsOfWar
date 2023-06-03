using UnityEngine;
using System.Collections;

public class ScrScrollerController : MonoBehaviour {


#region DefineVariables

    public const string SelectItemTrigger = "Select";
    public const string CenterItemTrigger = "Center";
    public const string UnCenterItemTrigger = "UnCenter";


    public const string ClickMouseAxes = "Fire1";


    [SerializeField] private float betweenSize = 100;
    [SerializeField] private GameObject[] items;
    private Animator[] itemsAnimator;

    // Touch
    private float startTouchX;
    [SerializeField] private float disTouchX;

    // Movement
    [Header(" Movement ")]
    [SerializeField] private float moveSpeed = 3;
    private float targetMove;
    [SerializeField]private int indexNow;
    private Vector2 movementTemp;

    private Transform tr;

#endregion

#region MonoFunc

    protected void Awake()
    {
        tr = transform;

        targetMove = tr.localPosition.x;

        InitVisualItems();
    }

    protected void Update()
    {
        Movement();
        UpdateTouch();
        TestLogic();
    }
#endregion

#region TouchCalculate


    private void InitTouch()
    {
        startTouchX = Input.mousePosition.x;
    }

    private void UpdateTouch()
    {
        if (Input.GetButtonDown(ClickMouseAxes))
            InitTouch();

        if (Input.GetButton(ClickMouseAxes))
            ProccessTouch();


    }

    private void ProccessTouch()
    {
        if (DisFloat(startTouchX, Input.mousePosition.x) > disTouchX)
        {

            if(startTouchX < Input.mousePosition.x)
            {
                LeftMove();
            }
            else
            {
                RightMove();
            }


            InitTouch(); // Reset Calculate Touch
        }
    }


#endregion


#region Movement

    private void Movement()
    {
        movementTemp = tr.localPosition; // Init
        movementTemp.x = Mathf.Lerp(movementTemp.x, targetMove, moveSpeed * Time.deltaTime); // Move
        tr.localPosition = movementTemp; // Give Value
    }

    private void LeftMove()
    {
        if (indexNow > 0)
        {
            targetMove += betweenSize;
            indexNow -= 1;
            ChangeCenterItems(indexNow, indexNow + 1);
        }
    }

    private void RightMove()
    {
        if (indexNow < items.Length - 1)
        {
            targetMove -= betweenSize;
            indexNow += 1;
            ChangeCenterItems(indexNow, indexNow - 1);
        }
    }


#endregion

#region VisualCenter

    private void InitVisualItems()
    {
        itemsAnimator=new Animator[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i].transform.localPosition = new Vector2(i * betweenSize,0);
            itemsAnimator[i] = items[i].GetComponent<Animator>();
        }
        itemsAnimator[0].SetTrigger(CenterItemTrigger);
    }
    private void ChangeCenterItems(int index,int lastIndex) // In ProccessTouch
    {
        itemsAnimator[index].ResetTrigger(CenterItemTrigger);
        itemsAnimator[index].ResetTrigger(UnCenterItemTrigger);

        itemsAnimator[lastIndex].ResetTrigger(CenterItemTrigger);
        itemsAnimator[lastIndex].ResetTrigger(UnCenterItemTrigger);

        itemsAnimator[index].SetTrigger(CenterItemTrigger);
        itemsAnimator[lastIndex].SetTrigger(UnCenterItemTrigger);
    }

    private void SelectItem()
    {
        itemsAnimator[indexNow].SetTrigger(SelectItemTrigger);
    }
#endregion


#region Tools
    public static float DisFloat(float a, float b)
    {
        return Mathf.Abs(a - b);
    }

    private void TestLogic()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            LeftMove();

        if (Input.GetKeyDown(KeyCode.RightArrow))
            RightMove();

        if (Input.GetKeyDown(KeyCode.Space))
            SelectItem();
    }
#endregion

}
