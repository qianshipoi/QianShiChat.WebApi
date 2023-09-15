namespace QianShiChat.Application.Contracts;

public class SendTextMessageRequest
{
    [Required]
    [Range(1, int.MaxValue)]
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

public class SendFilesMessageRequest : IValidatableObject
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ToId { get; set; }
    [MaxLength(255)]
    public string Message { get; set; } = string.Empty;
    [Required]
    public string Ids { get; set; } = string.Empty;
    [Required]
    [EnumDataType(typeof(ChatMessageSendType))]
    public ChatMessageSendType SendType { get; set; }
    public List<int> AttachmentIds { get; private set; } = new List<int>();
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var idStr in Ids.Split(','))
        {
            if (!int.TryParse(idStr, out var id))
            {
                yield return new ValidationResult("ids format error.", new[] { nameof(Ids) });
                break;
            }
        }
    }
}

public record PrivateChatMessage(long Id, int UserId, string Message);

public record NotificationMessage(NotificationType Type, object Message);
