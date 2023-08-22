namespace QianShiChat.Application.Contracts;

public class ChatMessageDto
{
    public long Id { get; set; }

    public int FromId { get; set; }

    public int ToId { get; set; }

    public string SessionId { get; set; } = default!;

    public ChatMessageSendType SendType { get; set; }

    public ChatMessageType MessageType { get; set; }

    public string Content { get; set; } = null!;

    public long CreateTime { get; set; }

    public UserDto? FromUser { get; set; }
}