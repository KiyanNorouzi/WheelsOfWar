using UnityEngine;
using System.Collections;

public class ScrMineGunController : ScrGunsBaseController
{
    public override void SetFireState(bool v)
    {
        if (v)
        {
            EnvironmentController.Instance.mineManager.SetupMine(BulletPos.position, BulletPos.eulerAngles, parent);
            Bullets -= 1;
        }
    }
}