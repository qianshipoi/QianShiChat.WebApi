namespace QianShiChat.Application.Contracts;

public class FriendApplyDto
{
    /// <summary>
    /// 申请编号
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 申请人
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// 好友
    /// </summary>
    public int FriendId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public long CreateTime { get; set; }
    /// <summary>
    /// 申请状态
    /// </summary>
    public ApplyStatus Status { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 申请人
    /// </summary>
    public UserDto? User { get; set; }
    /// <summary>
    /// 被申请人
    /// </summary>
    public UserDto? Friend { get; set; }
}