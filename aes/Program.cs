using System.Security.Cryptography;

string plainText = "Hello, AES Encryption!";

using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();
            Console.WriteLine($"Key: {Convert.ToBase64String(aes.Key)}");
            Console.WriteLine($"IV: {Convert.ToBase64String(aes.IV)}");

            // Encrypt the message
            byte[] encrypted = aes.Encrypt(plainText, aes.Key, aes.IV);
            Console.WriteLine($"Encrypted: {Convert.ToBase64String(encrypted)}");

            // Decrypt the message
            string decrypted = aes.Decrypt(encrypted, aes.Key, aes.IV);
            Console.WriteLine($"Decrypted: {decrypted}");
        }

public static class AesHelper
{
public static byte[] Encrypt(this Aes aes, string plainText, byte[] key, byte[] iv) {
    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    using (MemoryStream ms = new MemoryStream())
    {
        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (StreamWriter sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return ms.ToArray();
    }
}

public static string Decrypt(this Aes aes, byte[] cipherText, byte[] key, byte[] iv)
{
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream(cipherText))
        {
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

}

