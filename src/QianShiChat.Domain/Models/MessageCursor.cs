namespace QianShiChat.Domain.Models;

[Table(nameof(MessageCursor))]
public class MessageCursor
{
    [Key]
    [Required]
    [Comment("user id")]
    public int UserId { get; set; }

    [Required]
    [Comment("message position")]
    [DefaultValue(0L)]
    public long Postiton { get; set; }

    [Required]
    public long LastUpdateTime { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserInfo User { get; set; }
}