using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewsWall : Photon.MonoBehaviour 
{
    public NewsLine[] newsLines;
    //public string[] gunCharacters, singCharacters;
    public PhotonView nv;

    void Start()
    {
        for (int i = 0; i < newsLines.Length; i++)
            newsLines[i].Deactivate();
    }

    public void SubmitText(string text, bool isLocal)
    {
        if (isLocal)
        {
            _SubmitText(text);
        }
        else
        {
            if (nv == null || !PhotonNetwork.connected)
                _SubmitText(text);
            else
                nv.RPC("SubmitTextRPC", PhotonTargets.All, text);
        }
    }

	public void P_SubmitText(int pplayer,string text, bool isLocal)
	{
		if (isLocal)
		{
			P_SubmitText(pplayer,text);
		}
		else
		{
			if (nv == null || !PhotonNetwork.connected)
				P_SubmitText(pplayer ,text);
			else
				nv.RPC("P_SubmitTextRPC", PhotonTargets.All, pplayer ,text);
		}
	}


    void _SubmitText(string text)
    {
        for (int i = 0; i < newsLines.Length; i++)
        {
            if (newsLines[i].IsActive)
            {
                newsLines[i].myTransform.anchoredPosition -= new Vector2(0, 35f);
                if (newsLines[i].myTransform.anchoredPosition.y < -140f)
                    newsLines[i].Deactivate();
            }
        }

        for (int i = 0; i < newsLines.Length; i++)
        {
            if (!newsLines[i].IsActive)
            {
                newsLines[i].myTransform.anchoredPosition = new Vector2(0, 0);
                newsLines[i].Activate(text);
                break;
            }
        }
    }

	void P_SubmitText(int pplayer,string text)
	{
		for (int i = 0; i < newsLines.Length; i++)
		{
			if (newsLines[i].IsActive)
			{
				newsLines[i].myTransform.anchoredPosition -= new Vector2(0, 35f);
				if (newsLines[i].myTransform.anchoredPosition.y < -140f)
					newsLines[i].Deactivate();
			}
		}
		
		for (int i = 0; i < newsLines.Length; i++)
		{
			if (!newsLines[i].IsActive)
			{
				newsLines[i].myTransform.anchoredPosition = new Vector2(0, 0);
				newsLines[i].Activate(text, pplayer);
				break;
			}
		}
	}


    public void SubmitText(string text, KillMethod method, bool isLocal = false)
    {
        string gunCharacter = GeneralSettings.Instance.gunCharacters[(int)method];
        text = text.Replace("<*>", gunCharacter);

        SubmitText(text, isLocal);
    }
	public void SubmitText(int pplayer ,string text, KillMethod method, bool isLocal = false)
	{
		string gunCharacter = GeneralSettings.Instance.gunCharacters[(int)method];
		text = text.Replace("<*>", gunCharacter);
		
		P_SubmitText(pplayer,text, isLocal);
	}


    public void SubmitText(string text, ExtraSigns sign, bool isLocal = false)
    {
        string signCharacter = "";
        if (SettingData.LanguageIndex == 0) // english
        {
            if (sign == ExtraSigns.Left)
                signCharacter = "Left.";
            else if (sign == ExtraSigns.Joined)
                signCharacter = "Joined";
            else
                signCharacter = GeneralSettings.Instance.singCharacters[(int)sign];
        }
        else
            signCharacter = GeneralSettings.Instance.singCharacters[(int)sign];

        text = text.Replace("*", signCharacter);
        SubmitText(text, isLocal);
    }

	public void SubmitText( int pplayer ,string text, ExtraSigns sign, bool isLocal = false)
	{
		string signCharacter = "";
		if (SettingData.LanguageIndex == 0) // english
		{
			if (sign == ExtraSigns.Left)
				signCharacter = "Left.";
			else if (sign == ExtraSigns.Joined)
				signCharacter = "Joined";
			else
				signCharacter = GeneralSettings.Instance.singCharacters[(int)sign];
		}
		else
			signCharacter = GeneralSettings.Instance.singCharacters[(int)sign];
		
		text = text.Replace("*", signCharacter);
		P_SubmitText(pplayer,text, isLocal);
	}


    [RPC]
    public void SubmitTextRPC(string text)
    {
        _SubmitText(text);
    }

	[RPC]
	public void P_SubmitTextRPC(int pplayer, string text)
	{
		P_SubmitText(pplayer,text);
	}

    /*float tTime = 0;
    void Update()
    {
        tTime += Time.deltaTime;
        if (tTime >= time)
        {
            tTime = 0;
            _SubmitText("");
        }
    }*/

}

public enum KillMethod
{
    MachineGun,
    Rocket,
    Mine,
    Environment,
    Crash,
    SelfDestruction,
}

public enum ExtraSigns
{
    Joined,
    Left,
    GotTheLead,
    Train,
    Worm
}