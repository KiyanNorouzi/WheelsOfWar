using UnityEngine;
using System.Collections;

public class TutorialHitTaker : MonoBehaviour 
{
    public delegate void getDamage(int killmethod, float damage);
    public event getDamage OnGetDamage;

    public void AddDamage(int killMethod, float damage)
    {
        KillMethod method = (KillMethod)killMethod;
        Debug.Log(name + " get damage " + method + ", amount=" + damage);

        if (OnGetDamage != null)
            OnGetDamage(killMethod, damage);
    }
}
