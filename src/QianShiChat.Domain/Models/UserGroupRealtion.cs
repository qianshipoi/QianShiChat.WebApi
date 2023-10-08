namespace QianShiChat.Domain.Models;

public class UserGroupRealtion
{
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public long CreateTime { get; set; }
    public string? Alias { get; set; }
    public GroupRole Role { get; set; }
    public virtual UserInfo? User { get; set; }
    public virtual Group? Group { get; set; }
}

public enum GroupRole
{
    Creator = 0,
    Admin = 1,
    Member = 2
}