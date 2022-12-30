namespace QianShiChat.Application.Contracts;

public class GroupDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public int TotalUser { get; set; }
    public long CreateTime { get; set; }
}