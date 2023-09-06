namespace QianShiChat.Domain.Models;

public class Attachment : ISoftDelete, IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string RawPath { get; set; } = default!;
    public string? PreviewPath { get; set; }
    public string Hash { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
    public int UserId { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }

    public UserInfo User { get; set; } = default!;
}


