namespace QianShiChat.Domain.Models;

public class Room : IAuditable, ISoftDelete
{
    public string Id { get; set; } = default!;
    public ChatMessageSendType Type { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public long MessagePosition { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
    public virtual UserInfo? FromUser { get; set; }
}
