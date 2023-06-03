using UnityEngine;
using System.Collections;

public class EulaButton : MonoBehaviour
{

    public OptionContentPanel aboutOptionPanel;

    public string URL;

    public virtual void Activate()
    {
        if (URL == string.Empty)
            return;

        Application.OpenURL(URL);
    }

}
