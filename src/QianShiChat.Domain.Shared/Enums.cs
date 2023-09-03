namespace QianShiChat.Domain.Shared;

/// <summary>
/// 申请状态
/// </summary>
public enum ApplyStatus : sbyte
{
    /// <summary>
    /// 已申请
    /// </summary>
    Applied = 1,

    /// <summary>
    /// 通过
    /// </summary>
    Passed,

    /// <summary>
    /// 驳回
    /// </summary>
    Rejected,

    /// <summary>
    /// 忽略
    /// </summary>
    Ignored
}

public enum ChatMessageType : sbyte
{
    /// <summary> 文字 </summary>
    Text = 1,

    /// <summary> 图片 </summary>
    Image,

    /// <summary> 视频 </summary>
    Video,

    /// <summary> 其他文件 </summary>
    OtherFile
}

public enum ChatMessageSendType : sbyte
{
    /// <summary> 个人 </summary>
    Personal = 1,

    /// <summary> 群组 </summary>
    Group
}

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

    /// <summary>
    /// 强制注销（其他地方登录）
    /// </summary>
    Signed,
}