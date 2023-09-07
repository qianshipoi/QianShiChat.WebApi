namespace QianShiChat.Domain.Models;

public class Group : ISoftDelete
{
    public Group()
    {
        GroupApplies = new HashSet<GroupApply>();
        UserGroupRealtions = new HashSet<UserGroupRealtion>();
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public int TotalUser { get; set; }
    public long CreateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    public virtual UserInfo? User { get; set; }
    public virtual ICollection<GroupApply> GroupApplies { get; set; }
    public virtual ICollection<UserGroupRealtion> UserGroupRealtions { get; set; }
}
