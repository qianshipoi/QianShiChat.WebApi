namespace QianShiChat.Application.Contracts;

public class ChatMessageDto
{
    public long Id { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public string RoomId { get; set; } = string.Empty;
    public ChatMessageSendType SendType { get; set; }
    public ChatMessageType MessageType { get; set; }
    public object Content { get; set; } = string.Empty;
    public long CreateTime { get; set; }
    public UserDto? FromUser { get; set; }

    public List<AttachmentDto> Attachments { get; set; } = new List<AttachmentDto>();
}