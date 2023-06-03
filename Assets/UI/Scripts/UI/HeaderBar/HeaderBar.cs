using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeaderBar : MonoBehaviour 
{
    public GameObject myGameObject;
    public Text usernameText, levelText, energyText, goldText, billsText, vipShortText;
    public Slider energySlider, scoreSlider;

    public OptionsWindow optionsWindow;
    public UserProfileWindow userProfileWindow;
    public BuyMoneyWindow buyCoinsMenu;
    public EditUsernameWindow editUsernameWindow;
    public VIPWindow vipWindow;
    public GasRefeelWindow gasWindow;

    public GasSectionInHeaderBar gasSection;
    public TextIndicator goldIndicator, billsIndicator;


	void Start()
    {
        optionsWindow.Deactivate();
        userProfileWindow.Deactivate();
        buyCoinsMenu.Deactivate();
        editUsernameWindow.Deactivate();
        vipWindow.Deactivate();
        gasWindow.Deactivate();


        OnDisplayNameChanged();
        OnBillsChanged();
        OnGoldChanged();
        OnLevelChanged();
        //OnEnergyChanged();
        OnVIPPackagesChanged();

        Accounting.Instance.currentUser.OnBillsChanged += OnBillsChanged;
        Accounting.Instance.currentUser.OnGoldChanged += OnGoldChanged;
        Accounting.Instance.currentUser.OnLevelChanged += OnLevelChanged;
        Accounting.Instance.currentUser.OnDisplayNameChanged+= OnDisplayNameChanged;
        Accounting.Instance.currentUser.OnVIPPackagesChanged += OnVIPPackagesChanged;
	}

	void Update(){
		if( Application.loadedLevelName != "Gameplay" ){
			OnLevelChanged ();
		}
	}

    int lastGold, lastBills;

    void OnDisplayNameChanged()
    {
        usernameText.text = Accounting.Instance.currentUser.DisplayName;
    }

    public void Disable()
    {
        myGameObject.SetActive(false);
    }

    public void Enable()
    {
        myGameObject.SetActive(true);
    }

    void OnVIPPackagesChanged()
    {
        bool[] houses = new bool[3];
        for (int i = 0; i < Accounting.Instance.currentUser.vipPackages.Count; i++)
            houses[Accounting.Instance.currentUser.vipPackages[i].TypeID] = true;

        string complete = "PRO";
        string text = "";
        for (int i = 0; i < houses.Length; i++)
            text += (houses[i]) ? complete[i].ToString() : "-";

        vipShortText.text = text;

        gasSection.Refresh();
        //OnEnergyChanged();
    }

    /*void OnEnergyChanged()
    {
        Debug.Log("gas=" + Accounting.Instance.currentUser.Gas + ", max gas=" + Accounting.Instance.currentUser.MaxGas);

        energyText.text = string.Format("{0}/{1}", Accounting.Instance.currentUser.Gas, Accounting.Instance.currentUser.MaxGas);

        energySlider.value = Accounting.Instance.currentUser.Gas;
        energySlider.maxValue = Accounting.Instance.currentUser.MaxGas;
    }*/

    void OnLevelChanged()
    {
		if( scoreSlider != null ){
			int levelMinScore = Leveling.Instance.GetScoreForLevel(Accounting.Instance.currentUser.Level);
			int nextLevelMinScore = Leveling.Instance.GetScoreForLevel(Accounting.Instance.currentUser.Level + 1);
			int scoreInLevel = Accounting.Instance.currentUser.TotalScore - levelMinScore;

			scoreSlider.minValue = 0;
			scoreSlider.maxValue = nextLevelMinScore;
			scoreSlider.value = scoreInLevel;
		}
        levelText.text = string.Concat("LVL ", Accounting.Instance.currentUser.Level);
    }

    void OnGoldChanged()
    {
        goldText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.Golds);
        int diff = Accounting.Instance.currentUser.Golds - lastGold;
        goldIndicator.Show(diff);

        lastGold = Accounting.Instance.currentUser.Golds;
    }

    void OnBillsChanged()
    {
        billsText.text = MathHelper.GetStringWithComma(Accounting.Instance.currentUser.Bills);
        int diff = Accounting.Instance.currentUser.Bills - lastBills;
        billsIndicator.Show(diff);

        lastBills = Accounting.Instance.currentUser.Bills;
    }





    public void SettingButton_Click()
    {
        optionsWindow.Activate();
    }

    public void UserProfileButton_Click()
    {
        userProfileWindow.Activate();
    }

    public void VIPButton_Click()
    {
        vipWindow.Activate();
    }

    public void GasButton_Click()
    {
        gasWindow.Activate();
    }

    public void BuyBillsButton_Click()
    {
        buyCoinsMenu.Activate(BuyCurrencySections.Bills);
    }

    public void BuyGoldsButton_Click()
    {
        buyCoinsMenu.Activate(BuyCurrencySections.Golds);
    }

    public void BackButton_Click()
    {
        if (Window.openWindows.Count > 0)
        {
            Window.openWindows[Window.openWindows.Count - 1].Deactivate();
        }
        else if (CommonUI.Instance.currentScene != null)
            CommonUI.Instance.currentScene.BackButton_Click();
    }
}