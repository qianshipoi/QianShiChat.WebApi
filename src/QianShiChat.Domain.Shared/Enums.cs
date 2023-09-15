namespace QianShiChat.Domain.Shared;

public enum ApplyStatus : sbyte
{
    Applied = 1,
    Passed,
    Rejected,
    Ignored
}

public enum ChatMessageType : sbyte
{
    Text = 1,
    Image = 2,
    Video = 3,
    Audio = 4,
    OtherFile = 127
}

public enum ChatMessageSendType : sbyte
{
    Personal = 1,
    Group
}

public enum NotificationType
{
    /// <summary> 好友上线 </summary>
    FriendOnline,
    /// <summary> 好友下线 </summary>
    FriendOffline,
    /// <summary> 好友申请 </summary>
    FriendApply,
    /// <summary> 新好友 </summary>
    NewFriend,
    /// <summary> 强制注销（其他地方登录） </summary>
    Signed,
    /// <summary> 询问在线文件传输 </summary>
    OnlineTransmissionConfirm,
    /// <summary> 确认在线传呼文件 </summary>
    OnlineTransmissionPassed,
    /// <summary> 取消在线传呼文件 </summary>
    OnlineTransmissionCancel,
}