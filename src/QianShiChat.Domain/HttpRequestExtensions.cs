namespace QianShiChat.Domain.Shared;

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
            .Append(request.Scheme)
            .Append("://")
            .Append(request.Host)
            .Append(request.PathBase)
            .ToString();
    }

    public static bool TryGetHeaderFirstValue(this HttpContext httpContext, string header, out string? value)
    {
        value = string.Empty;
        if (!httpContext.Request.Headers.TryGetValue(header, out StringValues values))
        {
            return false;
        }
        value = values.FirstOrDefault();
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool TryGetAccessToken(this HttpContext httpContext, out string? accessToken)
    {
        if (!TryGetHeaderFirstValue(httpContext, "Authorization", out accessToken))
        {
            return false;
        }

        var vals = accessToken!.Split(' ');
        if (vals.Length == 2 && string.Compare(vals[0], "Bearer", true) == 0)
        {
            accessToken = vals[1].Trim();
            return true;
        }
        return false;
    }
}