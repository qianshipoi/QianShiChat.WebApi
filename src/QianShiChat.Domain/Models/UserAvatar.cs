namespace QianShiChat.Domain.Models;

public class UserAvatar : ISoftDelete, IAuditable
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public string Path { get; set; } = default!;
    public ulong Size { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    public UserInfo User { get; set; } = default!;
}