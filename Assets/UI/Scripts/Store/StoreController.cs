using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreController : MonoBehaviour
{
    const int SHOPS_COUNT = 2;

    public bool commingSoon;

    public Animator parentCamAnimator, camAnimator;
    public Button leftButton, rightButton;
    public BoosterShop boosterShop;
    public GameObject commingSoonOverlayGameObject, storesOpenGameObject;
    public GameObject selectButtonGameObject, commingSoonTextGameObject, selectionObjects;
    public GameObject[] titles;
    public GameObject backToSelectionButton, saleBannerGameObject;
    public GameObject[] shopsUIGameObjects, shopsGameObjects;

    bool isZoomedIn;


    int currentPageIndex = -1;
    public int CurrentPageIndex
    {
        get { return currentPageIndex; }
        set
        {
            int nextPageIndex = Mathf.Clamp(value, 0, SHOPS_COUNT);

            if (nextPageIndex != currentPageIndex)
            {
                currentPageIndex = nextPageIndex;
                parentCamAnimator.SetInteger("pos", currentPageIndex);
            }

            for (int i = 0; i < titles.Length; i++)
                titles[i].SetActive(i == currentPageIndex);


            //if (currentPageIndex == 0 || currentPageIndex == 1)
            //{
            //    CosmeticController.Instance.startButtonInCosmeticSection.SetActive(false);
            //    CosmeticController.Instance.lockOverlayOnMachineGameObject.SetActive(false);
            //}


            //if (currentPageIndex == 2)
            //{
            //    if (CosmeticController.Instance.machineSelectionStateInStore == MachineSelectionStateInStore.LOCKED)
            //    {
            //        CosmeticController.Instance.lockOverlayOnMachineGameObject.SetActive(true);
            //        CosmeticController.Instance.startButtonInCosmeticSection.SetActive(false);
            //    }

            //    if (CosmeticController.Instance.machineSelectionStateInStore == MachineSelectionStateInStore.AVAILABLE)
            //    {
            //        CosmeticController.Instance.lockOverlayOnMachineGameObject.SetActive(false);
            //        CosmeticController.Instance.startButtonInCosmeticSection.SetActive(true);
            //    }

            //}

            leftButton.interactable = (currentPageIndex > 0);
            rightButton.interactable = (currentPageIndex < SHOPS_COUNT);

            SetSelectButton();
        }
    }


    void Awake()
    {
        if (CommonUI.Instance == null)
            SceneManager.LoadGame(Scenes.Store);
    }

    void Start()
    {
        saleBannerGameObject.SetActive(Prices.Instance.BillsMultiplyer != 1 || Prices.Instance.PriceMultiplyer != 1);
        //MoneyTablet.Instance.State = MoneyTabletViewState.JustMoney;

        storesOpenGameObject.SetActive(true);



        Inventory.Instance.Enable();
        Inventory.Instance.SetOpenButtonEnable(false);


        Inventory.Instance.SetOpenButtonEnable(false);


        CurrentPageIndex = 1;

        for (int i = 0; i < shopsUIGameObjects.Length; i++)
            shopsUIGameObjects[i].SetActive(false);

        backToSelectionButton.SetActive(false);
    }


    public void LeftButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        if (isZoomedIn)
            boosterShop.Prev();
        else
            CurrentPageIndex--;
    }

    public void RightButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        if (isZoomedIn)
            boosterShop.Next();
        else
            CurrentPageIndex++;
    }

    public void SelectButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        isZoomedIn = true;
        if (CurrentPageIndex == 0)
        {
            camAnimator.SetBool("zoomiap", isZoomedIn);
        }

        if (CurrentPageIndex == 2)
        {
            camAnimator.SetBool("zoomArmory", isZoomedIn);
            Debug.Log("In Cosmetic");
            //CosmeticController.Instance.lockOverlayOnMachineGameObject.SetActive(false);
        }

        else
        {
            camAnimator.SetBool("zoom", isZoomedIn);

            //if (CosmeticController.Instance.machineSelectionStateInStore == MachineSelectionStateInStore.LOCKED)
            //{
            //    CosmeticController.Instance.lockOverlayOnMachineGameObject.SetActive(true);
            //    Debug.Log("Out of Cosmetic");
            //}

            //if (CosmeticController.Instance.machineSelectionStateInStore == MachineSelectionStateInStore.AVAILABLE)
            //{
            //    CosmeticController.Instance.lockOverlayOnMachineGameObject.SetActive(false);
            //}

        }



        leftButton.interactable = rightButton.interactable = true;

        selectionObjects.SetActive(false);

        shopsUIGameObjects[currentPageIndex].SetActive(true);

        for (int i = 0; i < shopsUIGameObjects.Length; i++)
            shopsUIGameObjects[i].SetActive(i == currentPageIndex);

        for (int i = 0; i < shopsGameObjects.Length; i++)
            shopsGameObjects[i].SetActive(i == currentPageIndex);


        for (int i = 0; i < titles.Length; i++)
            titles[i].SetActive(false);

        backToSelectionButton.SetActive(true);

        if (currentPageIndex == 1)
            Inventory.Instance.Open();

        if (currentPageIndex == 0)
            CommonUI.Instance.buyCoinsMenu.Activate();

    }

    public void BackButton_Click()
    {
        CommonUI.Instance.PlayButtonClick();

        if (!isZoomedIn)
        {
            SceneManager.LoadScene(Scenes.MainMenu);
        }
        else
        {
            isZoomedIn = false;

            if (CurrentPageIndex == 0)
                camAnimator.SetBool("zoomiap", isZoomedIn);
            if (CurrentPageIndex == 2)
            {
                camAnimator.SetBool("zoomArmory", isZoomedIn);
                //CosmeticController.Instance.editButton.SetActive(false);

                //CosmeticController.Instance.cameraLeftButtonObject.SetActive(false);
                //CosmeticController.Instance.cameraLeftButtonObject.SetActive(false);

                Debug.Log("Out of Cosmetic");


                //if (CosmeticController.Instance.CurrentCameraIndex == 1 || CosmeticController.Instance.CurrentCameraIndex == 2 ||
                //    CosmeticController.Instance.CurrentCameraIndex == 3 || CosmeticController.Instance.CurrentCameraIndex == 4 ||
                //    CosmeticController.Instance.CurrentCameraIndex == 5 || CosmeticController.Instance.CurrentCameraIndex == 6 ||
                //    CosmeticController.Instance.CurrentCameraIndex == 7)
                //{
                //    camAnimator.SetBool("CosmeticIn", isZoomedIn);
                //}
            }
            else
            {
                camAnimator.SetBool("zoom", isZoomedIn);
            }


            CurrentPageIndex = currentPageIndex;




            for (int i = 0; i < shopsUIGameObjects.Length; i++)
                shopsUIGameObjects[i].SetActive(false);

            for (int i = 0; i < shopsGameObjects.Length; i++)
                shopsGameObjects[i].SetActive(false);

            shopsGameObjects[2].SetActive(true);  //Remember that!!


            backToSelectionButton.SetActive(false);
            selectionObjects.SetActive(true);

            if (currentPageIndex == 1)
            {
                Inventory.Instance.Close();
                //Accounting.Instance.SetPlayerBoosters(Data.UserName, Data.Money, _Done);
            }

            if (currentPageIndex == 0)
                CommonUI.Instance.buyCoinsMenu.Deactivate();

            SetSelectButton();
        }
    }

    void SetSelectButton()
    {
        if (currentPageIndex == 0 || currentPageIndex == 1 || currentPageIndex == 2) // available sections
        {
            commingSoonTextGameObject.SetActive(false);
            selectButtonGameObject.SetActive(true);
        }
        else
        {
            commingSoonTextGameObject.SetActive(true);
            selectButtonGameObject.SetActive(false);
        }
    }

    void _Done(string decodedText)
    {
        Debug.Log("set player boosters done=" + decodedText);
    }
}


