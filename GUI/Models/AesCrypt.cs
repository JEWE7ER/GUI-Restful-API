//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//public static class AesCrypt
//{
//    private static readonly byte[] key = GenerateRandomBytes(64);
//    //private static readonly byte[] iv = { /* здесь нужно указать свой вектор инициализации */ };

//    public static byte[] Encrypt(string plainText)
//    {
//        if (plainText == null || plainText.Length <= 0)
//            throw new("plainText");
//        if (key == null || key.Length <= 0)
//            throw new("Key");

//        byte[] encrypted;

//        using Aes aesAlg = Aes.Create();
//        aesAlg.Key = key;
//        aesAlg.GenerateIV();

//        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

//        using MemoryStream msEncrypt = new();
//        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
//        using StreamWriter swEncrypt = new(csEncrypt);
//        swEncrypt.Write(plainText);
//        encrypted = msEncrypt.ToArray();

//        return encrypted;
//    }

//    public static string Decrypt(byte[] cipherText)
//    {
//        if (cipherText == null || cipherText.Length <= 0)
//            throw new("cipherText");
//        if (key == null || key.Length <= 0)
//            throw new("Key");

//        string? plaintext = null;

//        using Aes aesAlg = Aes.Create();
//        aesAlg.Key = key;
//        aesAlg.GenerateIV();

//        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

//        using MemoryStream msDecrypt = new(cipherText);
//        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
//        using StreamReader srDecrypt = new(csDecrypt);
//        plaintext = srDecrypt.ReadToEnd();
//        return plaintext;
//    }

//    private static byte[] GenerateRandomBytes(int length)
//    {
//        Random random = new();
//        byte[] buffer = new byte[length];
//        random.NextBytes(buffer);
//        return buffer;
//    }
//}
