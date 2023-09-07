namespace QianShiChat.Domain.Models;

/// <summary>
/// 用户关系表
/// </summary>
public class UserRealtion
{
    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 用户编号
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 朋友编号
    /// </summary>
    public int FriendId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public long CreateTime { get; set; }
    public virtual UserInfo User { get; set; } = default!;
    public virtual UserInfo Friend { get; set; } = default!;
}