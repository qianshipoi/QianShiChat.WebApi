﻿namespace QianShiChat.Application.Contracts;

public class GroupDto : IRoomToObject
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalUser { get; set; }
    public long CreateTime { get; set; }
    public string Avatar { get; set; } = string.Empty;
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}