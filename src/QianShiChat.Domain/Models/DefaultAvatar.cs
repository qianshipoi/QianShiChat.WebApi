namespace QianShiChat.Domain.Models;

public class DefaultAvatar : ISoftDelete, IAuditable
{
    public long Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public ulong Size { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
}