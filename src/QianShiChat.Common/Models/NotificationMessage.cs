namespace QianShiChat.Models
{
    public record PrivateChatMessage(int UserId, string Message);

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
}
