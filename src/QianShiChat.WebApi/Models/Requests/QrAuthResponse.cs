namespace QianShiChat.WebApi.Models.Requests;

public class QrAuthResponse
{
    /// <summary>
    /// 800 二维码不存在或已过期 801 等待扫码 802 授权中 200 授权成功
    /// </summary>
    public int Code { get; set; }

    public string Message { get; set; }
}

public class QrKeyResponse
{
    public int Code { get; set; }

    public string Key { get; set; }
}

public class QrCreateRequset
{
    [Required, MaxLength(32)]
    public string Key { get; set; }

    public bool Qrimg { get; set; }
}

public class QrCreateResponse
{
    public string? Url { get; set; }

    public string? Image { get; set; }
}

public class QrCheckResponse
{
    /// <summary>
    /// 800 二维码不存在或已过期 801 等待扫码 802 授权中 803 授权成功
    /// </summary>
    public int Code { get; set; }

    public string Message { get; set; }

    /// <summary>
    /// 802 有值
    /// </summary>
    public UserDto? User { get; set; }

    /// <summary>
    /// 803 有值
    /// </summary>
    public string? AccessToken { get; set; }
}