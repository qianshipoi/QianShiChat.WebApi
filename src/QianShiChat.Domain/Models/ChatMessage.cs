namespace QianShiChat.Domain.Models;

public class ChatMessage : IAuditable
{
    public long Id { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public ChatMessageSendType SendType { get; set; }
    public ChatMessageType MessageType { get; set; }
    public string Content { get; set; } = string.Empty;
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
    public bool IsDeleted { get; set; }
    public virtual UserInfo? FromUser { get; set; }
}