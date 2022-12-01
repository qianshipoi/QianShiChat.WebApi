using System.Security.Cryptography;
using System.Text;

namespace QianShiChat.Common.Extensions
{
    public static class StreamExtensions
    {
        public static string ToMd5(this Stream stream, bool toUpper = false)
        {
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                var md5 = MD5.Create();
                byte[] retVal = md5.ComputeHash(stream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                stream.Seek(0, SeekOrigin.Begin);
                return toUpper ? sb.ToString().ToUpper() : sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("ToMd5() fail,error:" + ex.Message);
            }
        }
    }
}