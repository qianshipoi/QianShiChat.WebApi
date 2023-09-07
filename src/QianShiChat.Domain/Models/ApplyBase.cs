namespace QianShiChat.Domain.Models;

public class ApplyBase : IAuditable, ISoftDelete
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
    /// 修改时间
    /// </summary>
    public long UpdateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    /// <summary>
    /// 目标
    /// </summary>
    public virtual UserInfo? User { get; set; }
}