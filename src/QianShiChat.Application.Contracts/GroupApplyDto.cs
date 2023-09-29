namespace QianShiChat.Application.Contracts;

public class GroupApplyDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public long CreateTime { get; set; }
    public ApplyStatus Status { get; set; }
    public string? Remark { get; set; }
    public UserDto? User { get; set; }
    public GroupDto? Group { get; set; }
}