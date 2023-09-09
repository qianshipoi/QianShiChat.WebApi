namespace QianShiChat.Application.Contracts;

public class SendTextMessageRequest
{
    [Required]
    [Range(1,int.MaxValue)]
    public int ToId { get; set; }
    [Required, MaxLength(255)]
    public string Message { get; set; } = default!;
    [Required]
    [EnumDataType(typeof(ChatMessageSendType))]
    public ChatMessageSendType SendType { get; set; }
}

public class SendFileMessageRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ToId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int AttachmentId { get; set; }
    [Required]
    [EnumDataType(typeof(ChatMessageSendType))]
    public ChatMessageSendType SendType { get; set; }
}

public record PrivateChatMessage(long Id, int UserId, string Message);

public record NotificationMessage(NotificationType Type, object Message);
