namespace QianShiChat.Domain.Models;

public class Attachment : ISoftDelete, IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RawPath { get; set; } = string.Empty;
    public string? PreviewPath { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public int UserId { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public virtual UserInfo? User { get; set; } 
}


