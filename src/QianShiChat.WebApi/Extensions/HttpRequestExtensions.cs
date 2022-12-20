namespace QianShiChat.WebApi.Extensions;

public static class HttpRequestExtensions
{
    /// <summary>
    /// 获取 Scheme://Host:Port
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string GetBaseUrl(this HttpRequest request)
    {
        return new StringBuilder()
            .Append(request.IsHttps ? "https" : "http")
            .Append("://")
            .Append(request.Host)
            .Append(request.PathBase)
            .ToString();
    }
}