namespace QianShiChat.Application.Contracts;

public class SessionDto
{
    public string Id { get; set; } = string.Empty;
    public ChatMessageSendType Type { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public int UnreadCount { get; set; }
    public long LastMessageTime { get; set; }
    public object? LastMessageContent { get; set; }
    public UserDto? FromUser { get; set; }
    public object? ToObject { get; set; }
}
