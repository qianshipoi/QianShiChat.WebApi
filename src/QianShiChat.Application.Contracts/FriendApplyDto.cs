﻿namespace QianShiChat.Application.Contracts;

public class FriendApplyDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FriendId { get; set; }
    public long CreateTime { get; set; }
    public ApplyStatus Status { get; set; }
    public string? Remark { get; set; }
    public UserDto? User { get; set; }
    public UserDto? Friend { get; set; }
}
