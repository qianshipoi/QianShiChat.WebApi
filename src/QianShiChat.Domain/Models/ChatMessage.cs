namespace QianShiChat.Domain.Models;

[Table(nameof(ChatMessage))]
public class ChatMessage : IAuditable
{
    [Key]
    [Required]
    public long Id { get; set; }

    [Required]
    public int FromId { get; set; }

    [Required]
    public int ToId { get; set; }

    [Required]
    [MaxLength(32)]
    public string SessionId { get; set; } = default!;

    [Required]
    public ChatMessageSendType SendType { get; set; }

    [Required]
    public ChatMessageType MessageType { get; set; }

    [Required]
    public string Content { get; set; } = default!;

    [Required]
    public long CreateTime { get; set; }

    [Required]
    public long UpdateTime { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(FromId))]
    public UserInfo? FromUser { get; set; }

    [NotMapped]
    public UserInfo? ToUser { get; set; }
}