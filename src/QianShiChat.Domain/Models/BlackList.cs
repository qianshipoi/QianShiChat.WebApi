namespace QianShiChat.Domain.Models;

public class BlackList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BlackUserId { get; set; }
    public long CreateTime { get; set; }
    public virtual UserInfo? User { get; set; }
    public virtual UserInfo? BlackUser { get; set; }
}
