namespace QianShiChat.Domain.Models;

/// <summary>
/// 用户关系表
/// </summary>
public class UserRealtion
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FriendId { get; set; }
    public string? Alias { get; set; }
    public long CreateTime { get; set; }
    public virtual UserInfo? User { get; set; }
    public virtual UserInfo? Friend { get; set; }
}