namespace QianShiChat.Domain.Models;

/// <summary>
/// 加群申请
/// </summary>
public class GroupApply : ApplyBase
{
    /// <summary>
    /// 群编号
    /// </summary>
    public int GroupId { get; set; }
    /// <summary>
    /// 群组
    /// </summary>
    public Group Group { get; set; } = default!;
}