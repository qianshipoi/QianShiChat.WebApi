namespace QianShiChat.Domain.Options;

/// <summary>
/// JWT选项
/// </summary>
public class JwtOptions
{
    public const string OptionsKey = "Authentication:Jwt";

    /// <summary>
    /// 加密key
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// 颁发者
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// 接受者
    /// </summary>
    public string Audience { get; set; } = null!;
    /// <summary>
    /// 过期时间 秒
    /// </summary>
    public long Expires { get; set; }
}