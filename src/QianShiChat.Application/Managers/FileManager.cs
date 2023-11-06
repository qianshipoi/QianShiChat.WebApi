using System.Security.Cryptography;

namespace QianShiChat.Application.Managers;

public class FileManager : IFileManager, ISingleton
{
    private string GenerateFileSignature(string fileName, string contentType, string fileHash, string fileLength, string fileSecret)
    {
        var data = EncryptStringToBytes_Aes($"{fileName}\n{contentType}\n{fileHash}\n{fileLength}", Encoding.UTF8.GetBytes(fileSecret));
        var key = Encoding.UTF8.GetBytes(fileSecret);
        var signature = ComputeHmacSha256(data, key);
        return Convert.ToBase64String(signature);
    }
    private bool VerifyFileSignature(string fileName, string contentType, string fileHash, string fileLength, string fileSecret, string signature)
    {
        var data = EncryptStringToBytes_Aes($"{fileName}\n{contentType}\n{fileHash}\n{fileLength}", Encoding.UTF8.GetBytes(fileSecret));
        var key = Encoding.UTF8.GetBytes(fileSecret);
        var signatureBytes = Convert.FromBase64String(signature);
        return VerifyHmacSha256(data, signatureBytes, key);
    }
    static byte[] EncryptStringToBytes_Aes(string plainText, byte[] key)
    {
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.GenerateIV();
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }
    static byte[] ComputeHmacSha256(byte[] data, byte[] key)
    {
        using (var hmac = new HMACSHA256(key))
        {
            return hmac.ComputeHash(data);
        }
    }
    static bool VerifyHmacSha256(byte[] data, byte[] signature, byte[] key)
    {
        byte[] computedSignature = ComputeHmacSha256(data, key);

        if (computedSignature.Length != signature.Length)
        {
            return false;
        }

        for (int i = 0; i < computedSignature.Length; i++)
        {
            if (computedSignature[i] != signature[i])
            {
                return false;
            }
        }

        return true;
    }
}
