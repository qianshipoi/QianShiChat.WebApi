namespace QianShiChat.Domain.Models;

public class ApplyBase : IAuditable, ISoftDelete
{
    /// <summary>
    /// 申请编号
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// 申请人
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    public long CreateTime { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    [Required]
    public ApplyStatus Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(255)]
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
    [ForeignKey(nameof(UserId))]
    public UserInfo User { get; set; } = default!;
}