namespace QianShiChat.Domain.Models;

public class FriendGroup : ISoftDelete
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int Sort { get; set; }
    public bool IsDefault { get; set; }
    public long CreateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    public virtual UserInfo? User { get; set; }
}
