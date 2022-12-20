using System.Security.Cryptography;
using System.Text;

namespace QianShiChat.Common.Extensions;

public static class StringExtensions
{
    public static string ToMd5(this string input)
    {
        StringBuilder hash = new StringBuilder();
        var md5provider = MD5.Create();
        byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));
        for (int i = 0; i < bytes.Length; i++)
        {
            hash.Append(bytes[i].ToString("x2"));
        }
        return hash.ToString();
    }
}