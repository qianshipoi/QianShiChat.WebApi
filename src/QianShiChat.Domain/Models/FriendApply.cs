namespace QianShiChat.Domain.Models;

public class FriendApply : ApplyBase
{
    public int FriendId { get; set; }
    public virtual UserInfo? Friend { get; set; }
    public int? FriendGroupId { get; set; }
}
