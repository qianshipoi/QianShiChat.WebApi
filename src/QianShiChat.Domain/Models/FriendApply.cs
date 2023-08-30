namespace QianShiChat.Domain.Models;

/// <summary>
/// 好友申请
/// </summary>
[Table(nameof(FriendApply))]
public class FriendApply : ApplyBase
{
    /// <summary>
    /// 目标人
    /// </summary>
    [Required]
    public int FriendId { get; set; }

    /// <summary>
    /// 目标
    /// </summary>
    [ForeignKey(nameof(FriendId))]
    public UserInfo Friend { get; set; } = default!;
}

/// <summary>
/// 加群申请
/// </summary>
public class GroupApply : ApplyBase
{
    /// <summary>
    /// 群编号
    /// </summary>
    [Required]
    public int GroupId { get; set; }

    /// <summary>
    /// 群组
    /// </summary>
    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; } = default!;
}