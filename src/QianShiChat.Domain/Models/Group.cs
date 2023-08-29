namespace QianShiChat.Domain.Models;

public class Group : ISoftDelete
{
    public Group()
    {
        UserGroupRealtions = new HashSet<UserGroupRealtion>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required, MaxLength(128)]
    public string Name { get; set; } = null!;

    [MaxLength(512)]
    public string? Avatar { get; set; }

    [Required]
    public int TotalUser { get; set; }

    [Required]
    public long CreateTime { get; set; }

    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserInfo? User { get; set; }

    public virtual ICollection<UserGroupRealtion> UserGroupRealtions { get; set; }
}

[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class UserGroupRealtion
{
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public long CreateTime { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual UserInfo User { get; set; } = null!;

    [ForeignKey(nameof(GroupId))]
    public virtual Group Group { get; set; } = null!;
}