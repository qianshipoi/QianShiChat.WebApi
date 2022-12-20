namespace QianShiChat.Domain.Models;

public class UserAvatar : ISoftDelete, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("avatar id.")]
    public long Id { get; set; }

    [Required]
    [Comment("user id.")]
    public int UserId { get; set; }

    [Required]
    [Comment("create time.")]
    public long CreateTime { get; set; }

    [Required]
    [Comment("update time.")]
    public long UpdateTime { get; set; }

    [Required]
    [MaxLength(255)]
    [Comment("file path.")]
    public string Path { get; set; }

    [Required]
    [Comment("file size.")]
    public ulong Size { get; set; }

    [Required]
    [DefaultValue(false)]
    [Comment("is deleted.")]
    public bool IsDeleted { get; set; }

    [Comment("delete time.")]
    public long DeleteTime { get; set; }

    public UserInfo User { get; set; }
}