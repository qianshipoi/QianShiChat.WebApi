namespace QianShiChat.Domain.Models;

public class MessageAttachment
{
    public long MessageId { get; set; }
    public int AttachmentId { get; set; }
    public virtual ChatMessage Message { get; set; } = null!;
    public virtual Attachment Attachment { get; set; } = null!;
}
