using UnityEngine;
using System.Collections;

public class VIPWindow : Window
{
    public VIPSlot[] slots;
    public UnityEngine.UI.Text[] texts;
    public Color selectedColor, normalColor;



    public override void Activate()
    {
        _Refresh();

        Accounting.Instance.currentUser.OnVIPPackagesChanged += OnVIPPackagesChanged;
        base.Activate();
    }

    private void _Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ActivateLocked();
            texts[i].color = normalColor;
        }

        for (int i = 0; i < Accounting.Instance.currentUser.vipPackages.Count; i++)
        {
            slots[Accounting.Instance.currentUser.vipPackages[i].TypeID].ActivateAvailable(Accounting.Instance.currentUser.vipPackages[i].TimeRemaining);
            texts[Accounting.Instance.currentUser.vipPackages[i].TypeID].color = selectedColor;
        }
    }

    public override void Deactivate()
    {
        Accounting.Instance.currentUser.OnVIPPackagesChanged -= OnVIPPackagesChanged;
        base.Deactivate();
    }

    void OnVIPPackagesChanged()
    {
        _Refresh();
    }


    int slotIndex;
    public void Slot_Click(int slotIndex)
    {
        this.slotIndex = slotIndex;
        Accounting.Instance.currentUser.Buy(slots[slotIndex].price, _BoughtVIPPackage);
    }

    private void _BoughtVIPPackage()
    {
        slots[slotIndex].ActivateAvailable(259200); // 259,200 seconds = 3 days
        Accounting.Instance.currentUser.BuyVIPPackage(slotIndex);
    }
}