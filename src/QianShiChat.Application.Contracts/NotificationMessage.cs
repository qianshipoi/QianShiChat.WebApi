﻿namespace QianShiChat.Application.Contracts;

public class PrivateChatMessageRequest
{
    [Required]
    [Range(1,int.MaxValue)]
    public int ToId { get; set; }

    [Required, MaxLength(255)]
    public string Message { get; set; } = default!;

    [Required]
    [EnumDataType(typeof(ChatMessageSendType))]
    public ChatMessageSendType SendType { get; set; }
}

public class SendFileMessageRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ToId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int AttachmentId { get; set; }
    [Required]
    [EnumDataType(typeof(ChatMessageSendType))]
    public ChatMessageSendType SendType { get; set; }
}


public record PrivateChatMessage(long Id, int UserId, string Message);

public record NotificationMessage(NotificationType Type, string Message);

public enum NotificationType
{
    /// <summary>
    /// 好友上线
    /// </summary>
    FriendOnline,

    /// <summary>
    /// 好友下线
    /// </summary>
    FriendOffline,

    /// <summary>
    /// 好友申请
    /// </summary>
    FriendApply,

    /// <summary>
    /// 新好友
    /// </summary>
    NewFriend,
}