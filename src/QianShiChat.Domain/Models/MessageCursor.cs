namespace QianShiChat.Domain.Models;

public class MessageCursor
{
    public int UserId { get; set; }
    public long Postiton { get; set; }
    public long LastUpdateTime { get; set; }
    public UserInfo User { get; set; } = default!;
}