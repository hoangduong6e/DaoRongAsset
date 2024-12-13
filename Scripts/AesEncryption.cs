using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime;

public class AesEncryption
{
    // 32 bytes cho AES-256 (256 bit)
    public static string Keyyy = "I2}/5fV8#012A45g7@90Z2"; // Đảm bảo Key có 32 bytes (256 bit)

    // 16 bytes cho AES (128 bit)
    public static string IVvvv = "1a34V6"; // Đảm bảo IV có 16 bytes (128 bit)



   

    public static string Encrypt(string plainText)
    {
        // Đảm bảo rằng Key và IV có đúng kích thước
        byte[] keyBytes = Encoding.UTF8.GetBytes(LoginFacebook.ins.keyAes);
        byte[] ivBytes = Encoding.UTF8.GetBytes(LoginFacebook.ins.IVAes);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes; // Khóa 32 byte (256 bit)
            aesAlg.IV = ivBytes;   // IV 16 byte (128 bit)

            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText); // Ghi chuỗi vào CryptoStream
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray()); // Trả về kết quả mã hóa dưới dạng Base64
                }
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        // Đảm bảo rằng Key và IV có đúng kích thước
        byte[] keyBytes = Encoding.UTF8.GetBytes(LoginFacebook.ins.keyAes);
        byte[] ivBytes = Encoding.UTF8.GetBytes(LoginFacebook.ins.IVAes);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = keyBytes; // Khóa 32 byte (256 bit)
            aesAlg.IV = ivBytes;   // IV 16 byte (128 bit)

            using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
            {
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText))) // Chuyển Base64 thành mảng byte
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd(); // Trả về chuỗi đã giải mã
                        }
                    }
                }
            }
        }
    }
}
