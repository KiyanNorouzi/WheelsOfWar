using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CosmeticPerCars : MonoBehaviour
{

    #region Singleton

    private static CosmeticPerCars _instance;
    public static CosmeticPerCars Instance
    {
        get { return _instance; }
    }

    public static void SetInstance(CosmeticPerCars m_instance)
    {
        _instance = m_instance;
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion

    #region Variables


    [Header("Cosmetic Item Editor")]
    public SideBack[] sideBack;
    public SideFront[] sideFront;
    public SideLeft[] sideLeft;
    public SideRight[] sideRight;
    public SideTop[] sideTop;
    public TireObjects[] tireObjects;

    [Header("Color Editor")]
    public ColorMaterials[] colorMaterials;
    public GameObject machineBody;


	//CarType carType;
	void Start()
	{
		/*ColorIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.COLOR_SIDE);
		SideBackIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.BACK_SIDE);
		SideFrontIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.FRONT_SIDE);
		SideLeftIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.LEFT_SIDE);
		SideRightIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RIGHT_SIDE);
		SideTopIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.TOP_SIDE);
		TireIndex = Accounting.Instance.currentUser.GetSelectedCosmetic(carType, SideStates.RING_SIDE);*/
	}



    #endregion



    public void SetCosmetic(SideStates side, int _index)
    {
        switch (side)
        {
            case SideStates.COLOR_SIDE:
                ColorIndex = _index;
                break;
            case SideStates.FRONT_SIDE:
                SideFrontIndex = _index;
                break;
            case SideStates.LEFT_SIDE:
                SideLeftIndex = _index;
                break;
            case SideStates.BACK_SIDE:
                SideBackIndex = _index;
                break;
            case SideStates.RIGHT_SIDE:
                SideRightIndex = _index;
                break;
            case SideStates.TOP_SIDE:
                SideTopIndex = _index;
                break;
            case SideStates.RING_SIDE:
                TireIndex = _index;
                break;
        }
    }


    int colorIndex;
    public int ColorIndex
    {
        get
        {
            return colorIndex;
        }
        set
        {
			colorIndex = value;
            machineBody.renderer.material = colorMaterials[colorIndex].material;
        }
    }

    int sideBackIndex;
    public int SideBackIndex
    {
        get
		{
            //Debug.Log("get" + sideBackIndex);
            return sideBackIndex;
        }
        set{
			//Debug.Log( "Back " + value );
            sideBackIndex = value;

            for (int i = 0; i < sideBack.Length; i++)
                sideBack[i].sideBackObject.SetActive(i == sideBackIndex);
        }
    }

    int sideFrontIndex;
    public int SideFrontIndex
    {
        get
        {
            return sideFrontIndex;
        }
        set
        {
			sideFrontIndex = value;

            for (int i = 0; i < sideFront.Length; i++)
                sideFront[i].sideFrontObject.SetActive(i == sideFrontIndex);
        }
    }

    int sideLeftIndex;
    public int SideLeftIndex
    {
        get
        {
            return sideLeftIndex;
        }
        set
        {
			sideLeftIndex = value;
            for (int i = 0; i < sideLeft.Length; i++)
            {
                sideLeft[i].sideLeftObject.SetActive(i == sideLeftIndex);
            }
                
        }
    }

    int sideRightIndex;
    public int SideRightIndex
    {
        get
        {
            return sideRightIndex;
        }
        set
        {
			sideRightIndex = value;
            for (int i = 0; i < sideRight.Length; i++)
                sideRight[i].sideRightObject.SetActive(i == sideRightIndex);
        }
    }

    int sideTopIndex;
    public int SideTopIndex
    {
        get
        {
            return sideTopIndex;
        }
        set
        {
			sideTopIndex = value;
            for (int i = 0; i < sideTop.Length; i++)
                sideTop[i].sideTopObject.SetActive(i == sideTopIndex);
        }
    }

    int tireIndex;
    public int TireIndex
    {
        get
        {
            return tireIndex;
        }
        set
        {
			tireIndex = value;
            for (int i = 0; i < tireObjects.Length; i++)
            {
                tireObjects[i].tireBL_Object.SetActive(i == tireIndex);
                tireObjects[i].tireBR_Object.SetActive(i == tireIndex);
                tireObjects[i].tireFL_Object.SetActive(i == tireIndex);
                tireObjects[i].tireFR_Object.SetActive(i == tireIndex);
            }

        }
    }


    public void Finalize()
    {
        int selectedItemIndex;


        Debug.Log("---------------------------");
        Debug.Log("finalizing");
        Debug.Log("---------------------------");


        selectedItemIndex = TireIndex;
        for (int i = 0; i < tireObjects.Length; i++)
        {
            if (i != selectedItemIndex)
            {
                Destroy(tireObjects[i].tireBL_Object);
                Destroy(tireObjects[i].tireBR_Object);
                Destroy(tireObjects[i].tireFL_Object);
                Destroy(tireObjects[i].tireFR_Object);
            }
        }


        Debug.Log("tire index=" + TireIndex);


        selectedItemIndex = SideTopIndex;
        for (int i = 0; i < sideTop.Length; i++)
        {
            if (i != selectedItemIndex)
                Destroy(sideTop[i].sideTopObject);
        }

        sideTop[0].sideTopObject = sideTop[sideTopIndex].sideTopObject;
       


        Debug.Log("top index=" + TireIndex);

        selectedItemIndex = SideLeftIndex;
        for (int i = 0; i < sideLeft.Length; i++)
        {
            if (i != selectedItemIndex)
                Destroy(sideLeft[i].sideLeftObject);
        }

        Debug.Log("left index=" + TireIndex);


        selectedItemIndex = SideRightIndex;
        for (int i = 0; i < sideRight.Length; i++)
        {
            if (i != selectedItemIndex)
                Destroy(sideRight[i].sideRightObject);
        }

        Debug.Log("right index=" + TireIndex);

        selectedItemIndex = SideBackIndex;
        for (int i = 0; i < sideBack.Length; i++)
        {
            if (i != selectedItemIndex)
                Destroy(sideBack[i].sideBackObject);
        }

        Debug.Log("back Back =" + SideBackIndex);


        selectedItemIndex = SideFrontIndex;
        for (int i = 0; i < sideFront.Length; i++)
        {
            if (i != selectedItemIndex)
                Destroy(sideFront[i].sideFrontObject);
        }

        Debug.Log("front Back =" + SideBackIndex);

        Debug.Log("---------------------------");
        Debug.Log("---------------------------");
        Debug.Log("---------------------------");

        //Destroy(this);
    }


}

#region Cosmetic Item Classes

[System.Serializable]
public class ColorMaterials
{
    public string ColorNameEN;
    public Material material;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}

[System.Serializable]
public class TireObjects
{
    public string tireNameEN;
    public GameObject tireFL_Object;
    public GameObject tireFR_Object;
    public GameObject tireBL_Object;
    public GameObject tireBR_Object;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}

[System.Serializable]
public class SideBack
{
    public string sideBackNameEN;
    public GameObject sideBackObject;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}

[System.Serializable]
public class SideLeft
{
    public string sideLeftNameEN;
    public GameObject sideLeftObject;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}

[System.Serializable]
public class SideRight
{
    public string sideRightNameEN;
    public GameObject sideRightObject;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}

[System.Serializable]
public class SideFront
{
    public string sideFrontNameEN;
    public GameObject sideFrontObject;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}

[System.Serializable]
public class SideTop
{
    public string sideTopNameEN;
    public GameObject sideTopObject;

    //[HideInInspector]
    public bool used = false;
    public bool saled = false;
}


#endregion