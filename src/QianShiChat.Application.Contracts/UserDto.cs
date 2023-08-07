namespace QianShiChat.Application.Contracts;

public class UserDto
{
    public int Id { get; set; }

    public string Account { get; set; } = default!;

    public string Avatar { get; set; } = default!;

    public string NickName { get; set; } = default!;

    public long CreateTime { get; set; }
}

public class UserWithMessage : UserDto
{
    public List<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
}