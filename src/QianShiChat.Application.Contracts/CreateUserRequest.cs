namespace QianShiChat.Application.Contracts;

public class CreateUserRequest
{
    [Required, Range(1, int.MaxValue)]
    public int DefaultAvatarId { get; set; }
    [Required, MaxLength(32)]
    public string Account { get; set; } = null!;
    [Required, StringLength(32)]
    public string Password { get; set; } = null!;
    [Required, StringLength(10, MinimumLength = 2)]
    public string NickName { get; set; } = null!;
}