namespace QianShiChat.Domain.Models;

public class DefaultAvatar : ISoftDelete, IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("avatar id.")]
    public long Id { get; set; }

    [Required]
    [Comment("create time.")]
    public long CreateTime { get; set; }

    [Required]
    [MaxLength(255)]
    [Comment(comment: "file path.")]
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

    [Required]
    [Comment("update time.")]
    public long UpdateTime { get; set; }
}