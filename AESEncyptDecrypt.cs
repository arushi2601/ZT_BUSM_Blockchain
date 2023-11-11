using System.Security.Cryptography;

public static class AESEncyptDecrypt
{
    private static readonly Aes _aesAlgorithm = Aes.Create();
    private static readonly string _key = Convert.ToBase64String(_aesAlgorithm.Key);
    private static readonly string _vector = Convert.ToBase64String(_aesAlgorithm.IV);

    public static string EncryptDataWithAes(string plainText)
    {
        ICryptoTransform encryptor = _aesAlgorithm.CreateEncryptor();

        byte[] encryptedData;

        //Encryption will be done in a memory stream through a CryptoStream object
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                encryptedData = ms.ToArray();
            }
        }

        return Convert.ToBase64String(encryptedData);
    }


    public static string DecryptDataWithAes(string cipherText)
    {
        ICryptoTransform decryptor = _aesAlgorithm.CreateDecryptor();

        byte[] cipher = Convert.FromBase64String(cipherText);

        //Decryption will be done in a memory stream through a CryptoStream object
        using (MemoryStream ms = new MemoryStream(cipher))
        {
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}