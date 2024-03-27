using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public static class AesCrypt
    {
        private static readonly byte[] key = 
            [161, 194, 160, 6, 26, 0, 85, 117, 96, 104, 177, 240, 76, 239, 81, 114, 144, 
                164, 156, 134, 224, 251, 199, 72, 48, 192, 223, 120, 191, 139, 67, 165];//GenerateRandomBytes(32);
        private static readonly byte[] iv = [23, 116, 130, 34, 40, 217,
            89, 24, 209, 228, 52, 15, 81, 192, 216, 234];

        public static byte[] Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new("plainText");

            //byte[] encrypted;

            //using Aes aesAlg = Aes.Create();
            //aesAlg.Key = key;
            //aesAlg.IV = iv;

            //ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            //using MemoryStream msEncrypt = new();
            //using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            //using StreamWriter swEncrypt = new(csEncrypt);
            //swEncrypt.Write(plainText);
            //encrypted = msEncrypt.ToArray();

            //return encrypted;
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                encrypted = msEncrypt.ToArray();
            }

            return encrypted;
        }

        public static string Decrypt(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new("cipherText");

            string? plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(cipherText);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                plaintext = srDecrypt.ReadToEnd();
            }
            return plaintext;
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            Random random = new();
            byte[] buffer = new byte[length];
            random.NextBytes(buffer);
            return buffer;
        }
    }
}