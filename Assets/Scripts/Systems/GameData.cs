using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Systems
{
    public static class GameDataSystem
    {
        private static readonly string KeyPath = Application.persistentDataPath + "/aes.key";
        private static readonly string IvPath = Application.persistentDataPath + "/aes.iv";

        public class GameData
        {
            public long Money;
            public int ClickForce;
            public int UpgradeLevel;
            public int ClickUpgradeLevel;
            public bool BeatrixBuying;
            public int StatueUpgradeLevel;
            public int GardenUpgradeLevel;
        }

        private static byte[] GetOrCreateKey()
        {
            if (File.Exists(KeyPath))
            {
                return File.ReadAllBytes(KeyPath);
            }
            else
            {
                using var aesAlg = Aes.Create();
                
                aesAlg.GenerateKey();
                File.WriteAllBytes(KeyPath, aesAlg.Key);
                
                return aesAlg.Key;
            }
        }

        private static byte[] GetOrCreateIv()
        {
            if (File.Exists(IvPath))
            {
                return File.ReadAllBytes(IvPath);
            }
            else
            {
                using var aesAlg = Aes.Create();
                
                aesAlg.GenerateIV();
                File.WriteAllBytes(IvPath, aesAlg.IV);
                
                return aesAlg.IV;
            }
        }

        private static readonly byte[] AesKey = GetOrCreateKey();
        private static readonly byte[] AesIv = GetOrCreateIv();

        public static void SaveData(Action<GameData> updateAction)
        {
            var data = LoadData() ?? new GameData();
            updateAction(data);
            string json = JsonUtility.ToJson(data);
            string encryptedJson = EncryptString(json, AesKey, AesIv);
            
            File.WriteAllText(Application.persistentDataPath + "/game.data", encryptedJson);
        }

        public static GameData LoadData()
        {
            if (!File.Exists(Application.persistentDataPath + "/game.data")) return null;
            
            string encryptedJson = File.ReadAllText(Application.persistentDataPath + "/game.data");
            string json = DecryptString(encryptedJson, AesKey, AesIv);
            
            return JsonUtility.FromJson<GameData>(json);
        }

        static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);
            
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(buffer);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }
    }
}