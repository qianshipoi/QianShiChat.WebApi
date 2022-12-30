namespace QianShiChat.Application.Services;

/// <summary>
/// JWT Service
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// 生成Token
    /// </summary>
    /// <param name="claims">用户信息</param>
    /// <returns></returns>
    string CreateToken(IEnumerable<Claim> claims);

    /// <summary>
    /// 校验Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    bool Validate(string token);

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    IEnumerable<Claim> GetClaims(string token);
}