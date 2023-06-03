using UnityEngine;
using System.Collections;

public class UpgradePerCars : MonoBehaviour
{
    public void SetItem(UpgradeItem item, int index)
    {
        /*
        switch (item)
        {
            case UpgradeItem.RIFEL_UPGRADE:
                CurrentMachineGunUpgradeLevel = index;
                machinegunUpgradeState = state;
                break;
            case UpgradeItem.ROCKET_UPGRADE:
                CurrentRocketUpgradeLevel = index;
                rocketUpgradeState = state;
                break;
            case UpgradeItem.MINE_UPGRADE:
                CurrentMineUpgradeLevel = index;
                mineUpgradeState = state;
                break;
            case UpgradeItem.CHASIS_UPGRADE:
                CurrentChassisUpgradeLevel = index;
                chassisUpgradeState = state;
                break;
            case UpgradeItem.ENGINE_UPGRADE:
                CurrentEngineUpgradeLevel = index;
                engineUpgradeState = state;
                break;
            case UpgradeItem.GUARD_UPGRADE:
                CurrentGaurdUpgradeLevel = index;
                guardUpgradeState = state;
                break;
        }*/
    }

    public int GetItem(UpgradeParts part)
    {
        CarUpgrade upgrade = Accounting.Instance.currentUser.GetCarUpgrade(Information.Instance.carInfo[MainGarageCarController.Instance.CurrentCarIndex].carTag, (int)part);
        if (upgrade == null)
            return 0;
        else
            return upgrade.Level;
    }

    public int GetItem(UpgradeItem item)
    {
        return GetItem(ConvertUpgradeItemToPart(item));
    }

    public static UpgradeItem ConvertUpgradePartToItem(UpgradeParts part)
    {
        UpgradeItem item;
        switch (part)
        {
            default:
            case UpgradeParts.Riffle:
                item = UpgradeItem.RIFEL_UPGRADE;
                break;
            case UpgradeParts.Mine:
                item = UpgradeItem.MINE_UPGRADE;
                break;
            case UpgradeParts.Rocket:
                item = UpgradeItem.ROCKET_UPGRADE;
                break;
            case UpgradeParts.Engine:
                item = UpgradeItem.ENGINE_UPGRADE;
                break;
            case UpgradeParts.Armor:
                item = UpgradeItem.GUARD_UPGRADE;
                break;
            case UpgradeParts.Chasis:
                item = UpgradeItem.CHASIS_UPGRADE;
                break;
        }

        return item;
    }

    public static UpgradeParts ConvertUpgradeItemToPart(UpgradeItem item)
    {
        switch (item)
        {
            default:
            case UpgradeItem.RIFEL_UPGRADE: return UpgradeParts.Riffle;
            case UpgradeItem.ROCKET_UPGRADE: return UpgradeParts.Rocket;
            case UpgradeItem.MINE_UPGRADE: return UpgradeParts.Mine;
            case UpgradeItem.CHASIS_UPGRADE: return UpgradeParts.Chasis;
            case UpgradeItem.ENGINE_UPGRADE: return UpgradeParts.Engine;
            case UpgradeItem.GUARD_UPGRADE: return UpgradeParts.Armor;
        }       
    }
}