using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class GameDataSystem
{
    private static readonly string KeyPath = Application.persistentDataPath + "/aes.key";
    private static readonly string IVPath = Application.persistentDataPath + "/aes.iv";

    public class GameData
    {
        public int money;
        public int clickForce;
    }

    private static byte[] GetOrCreateKey()
    {
        if (File.Exists(KeyPath))
        {
            return File.ReadAllBytes(KeyPath);
        }
        else
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                File.WriteAllBytes(KeyPath, aesAlg.Key);
                return aesAlg.Key;
            }
        }
    }

    private static byte[] GetOrCreateIV()
    {
        if (File.Exists(IVPath))
        {
            return File.ReadAllBytes(IVPath);
        }
        else
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                File.WriteAllBytes(IVPath, aesAlg.IV);
                return aesAlg.IV;
            }
        }
    }

    private static readonly byte[] aesKey = GetOrCreateKey();
    private static readonly byte[] aesIV = GetOrCreateIV();

    public static void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string encryptedJson = EncryptString(json, aesKey, aesIV);
        File.WriteAllText(Application.persistentDataPath + "/game.data", encryptedJson);
    }

    public static GameData LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/game.data"))
        {
            string encryptedJson = File.ReadAllText(Application.persistentDataPath + "/game.data");
            string json = DecryptString(encryptedJson, aesKey, aesIV);
            return JsonUtility.FromJson<GameData>(json);
        }

        return null;
    }

    static string EncryptString(string plainText, byte[] Key, byte[] IV)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Mode = CipherMode.CBC; 
            aesAlg.Padding = PaddingMode.PKCS7; 
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    static string DecryptString(string cipherText, byte[] Key, byte[] IV)
    {
        byte[] buffer = Convert.FromBase64String(cipherText);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Mode = CipherMode.CBC; 
            aesAlg.Padding = PaddingMode.PKCS7; 
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(buffer))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}