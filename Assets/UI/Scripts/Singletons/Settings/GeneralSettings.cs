using UnityEngine;
using System.Collections;

public class GeneralSettings : MonoBehaviour 
{
    #region Singleton

    static GeneralSettings _instance;

    public static GeneralSettings Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    #endregion 

    public string facebookUrl, instagramUrl, championsUrl;
    public string contactEmail;
    public string contactEmailSubject;
    public string LoginProblemsEmailSubject;
    public string[] gunCharacters, singCharacters;
    public float ScoreMoneyConvertRate;
    public float DefaultSoundVolume = 1;
    public float RemoteWheelRotationRate = 3;
    public float LoadingMinimumTime;
    public float rateOfferTime;
    public int NewsCount;
    public bool AllowCheat;
    public bool AllowKeyboardControl;
    public float ProgressNeededToOpenUpgradeCar;
    public int GasRegenerateTime;

    public LanguageName DefaultLanguage;







    static char[] allowedCharacter = { '_', '.', ',', '-', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '/', '\\', ' ' };
    public static NameValidationError ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return NameValidationError.CannotBeNull;

        name = name.ToUpper();
        for (int i = 0; i < name.Length; i++)
        {
            char ch = name[i];
            int b = (int)ch;
            bool isLetter = (b >= 65 && b <= 90);
            if (isLetter)
                continue;

            bool isNumber = (b >= 48 && b <= 57);
            if (isNumber)
                continue;

            bool isAllowedCharacter = false;
            for (int j = 0; j < allowedCharacter.Length; j++)
            {
                if (ch.Equals(allowedCharacter[j]))
                {
                    isAllowedCharacter = true;
                    continue;
                }
            }

            if (!isLetter && !isNumber && !isAllowedCharacter)
            {
                return NameValidationError.UnallowedCharacters;
            }
                
        }

        return NameValidationError.None;
    }


    static char[] emailAllowedCharacter = { '_', '.', ',', '-', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '/', '\\' };
    public static EmailValidationError ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return EmailValidationError.CannotBeNull;

        email = email.ToUpper();
        int atSignIndex = -1;
        int dotIndex = -1;

        for (int i = 0; i < email.Length; i++)
        {
            char ch = email[i];
            int b = (int)ch;
            bool isLetter = (b >= 65 && b <= 90);
            if (isLetter)
                continue;

            bool isNumber = (b >= 48 && b <= 57);
            if (isNumber)
                continue;

            bool isAllowedCharacter = false;
            for (int j = 0; j < emailAllowedCharacter.Length; j++)
            {
                if (ch.Equals(emailAllowedCharacter[j]))
                //if (ch == emailAllowedCharacter[i])
                {
                    isAllowedCharacter = true;

                    if (ch == '@')
                    {
                        if (atSignIndex == -1)
                            atSignIndex = i;
                        else
                            return EmailValidationError.UnallowedCharacters;
                    }

                    if (atSignIndex != -1 && ch == '.')
                    {
                        dotIndex = i;
                    }

                    break;
                }
            }

            if (isAllowedCharacter)
                continue;

            if (!isLetter && !isNumber && !isAllowedCharacter)
                return EmailValidationError.UnallowedCharacters;
        }

        if (atSignIndex == -1)
            return EmailValidationError.NoAtSign;

        if (dotIndex == -1)
            return EmailValidationError.NoDot;

        if (dotIndex == email.Length - 1 || dotIndex == atSignIndex + 1)
            return EmailValidationError.BadFormat;


        return EmailValidationError.None;
    }



    static string[] OffensiveWords = new string[] {
        "kos",
        "kir",
        "koon",
        "ass",
        "sex",
        "fuck",
        "dick",
        "suck",
        "jende",
        "vulva",
        "vagina",
        "cock",
        "wtf",
        "boob",
        "pestoon",
        "gay",
        "mame",
        "bitch",
        "koos",
        "kus",
        "jagh",
        "masterbate",
        "tokhm",
        "anal",
        "blow",
        "horn",
        "kown",
        "gaid",
        "harlot",
        "fornicate",
        "siktir",
        "got",
        "dool"
    };

    public static bool IsOffensive(string text)
    {
        for (int i = 0; i < OffensiveWords.Length; i++)
        {
            if (text.ToLower().Contains(OffensiveWords[i]))
                return true;
        }

        return false;
    }



}


public enum NameValidationError
{
    None,
    CannotBeNull,
    UnallowedCharacters
}

public enum EmailValidationError
{
    None,
    CannotBeNull,
    UnallowedCharacters,
    NoAtSign,
    NoDot,
    BadFormat
}