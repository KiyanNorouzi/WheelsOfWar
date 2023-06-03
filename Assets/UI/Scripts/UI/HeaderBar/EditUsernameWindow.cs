using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditUsernameWindow : Window 
{
    public InputField usernameInputField;
    public Text priceText;
    public PriceStructure changePrice;



    public override void Activate()
    {
        if (Accounting.Instance.currentUser.UsernameChangesCount == 0)
            priceText.text = "0";
        else
            priceText.text = "15";

        usernameInputField.text = Accounting.Instance.currentUser.DisplayName;
        base.Activate();
    }
    
    public void SetButton_Click()
    {
        if (usernameInputField.text.ToLower() == Accounting.Instance.currentUser.DisplayName)
        {
            Deactivate();
        }
        else
        {
            if (Accounting.Instance.currentUser.UsernameChangesCount == 0)
                _ChangeName();
            else
                Accounting.Instance.currentUser.Buy(changePrice, _ChangeName);
        }
    }

    void _ChangeName()
    {
        Accounting.Instance.currentUser.DisplayName = usernameInputField.text;
        Deactivate();
    }
}