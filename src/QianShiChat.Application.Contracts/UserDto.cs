﻿namespace QianShiChat.Application.Contracts;

public class UserDto : IRoomToObject
{
    public int Id { get; set; }
    public string Account { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string NickName { get; set; } = string.Empty;
    public long CreateTime { get; set; }
    public bool IsOnline { get; set; }
}

public class UserWithMessage : UserDto
{
    public List<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
}