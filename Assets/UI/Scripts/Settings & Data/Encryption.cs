using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;

public class Encryption
{
    static string pass = "ASDZXCASDWQWEQWE45XCZXC45QWE4CZXCZ45ASD";
    static bool initialized;


    public static void Initialize()
    {
        if (initialized)
            return;

        string temp = "";
        for (int i = 0; i < pass.Length; i++)
            temp += ((char)(((int)pass[i]) + 15)).ToString();

        pass = temp;

        initialized = true;
    }


    public static string Encrypt(int n)
    {
        if (!initialized)
            Initialize();

        return StringCipher.Encrypt(n.ToString(), pass);
    }

    public static int Decrypt(string s)
    {
        if (!initialized)
            Initialize();

        return Decrypt(s, 0);
    }

    public static int Decrypt(string s, int defaultValue)
    {
        if (!initialized)
            Initialize();

        if (string.IsNullOrEmpty(s))
            return defaultValue;

        string decrypted = StringCipher.Decrypt(s, pass);


        int n;
        if (int.TryParse(decrypted, out n))
            return n;
        else
            return defaultValue;
    }

    public static string EncryptString(string n)
    {
        if (!initialized)
            Initialize();

        return StringCipher.Encrypt(n, pass);
    }

    public static string DecryptString(string s, string defaultValue="")
    {
        if (!initialized)
            Initialize();

        if (string.IsNullOrEmpty(s))
            return defaultValue;

        return StringCipher.Decrypt(s, pass);
    }
}


public static class StringCipher
{
    // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
    // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
    // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
    private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

    // This constant is used to determine the keysize of the encryption algorithm.
    private const int keysize = 256;

    public static string Encrypt(string plainText, string passPhrase)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);


        PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        {
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            byte[] cipherTextBytes = memoryStream.ToArray();

                            password = null;
                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
            }
        }
    }

    public static string Decrypt(string cipherText, string passPhrase)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        {
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                            password = null;
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }
    }
}
