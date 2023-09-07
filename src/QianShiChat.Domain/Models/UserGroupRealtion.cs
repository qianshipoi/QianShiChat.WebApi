namespace QianShiChat.Domain.Models;

public class UserGroupRealtion
{
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public long CreateTime { get; set; }
    public virtual UserInfo? User { get; set; }
    public virtual Group? Group { get; set; }
}