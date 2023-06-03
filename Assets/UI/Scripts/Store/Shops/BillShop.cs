using UnityEngine;
using System.Collections;

public class BillShop : MonoBehaviour
{

    public void Button_Click(int index)
    {
        InAppPurchases.Instance.Request(index, _PurchaseDone);
    }

    private void _PurchaseDone(int index, bool successful)
    {
        int price = 2000;
        if (Accounting.Instance.currentUser.Bills >= price)
        {
            // do the shopping
            Accounting.Instance.currentUser.Bills -= price;
        }
        else
        {
//            CommonUI.Instance.messageBox.ShowMessage(Messages.NotEnoughMoney, null, true);
            // show message short on money
        }



    }



}



