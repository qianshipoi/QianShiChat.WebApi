namespace QianShiChat.Application.Contracts;

public class FriendGroupDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Sort { get; set; }
    public bool IsDefault { get; set; }
}
