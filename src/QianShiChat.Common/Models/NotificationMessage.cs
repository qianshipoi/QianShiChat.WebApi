namespace QianShiChat.Models
{
    public class NotificationMessage
    {
        public NotificationType Type { get; set; }

        public string Message { get; set; }
    }

    public enum NotificationType
    {
        /// <summary>
        /// 好友上线
        /// </summary>
        FirendOnline,
        /// <summary>
        /// 好友下线
        /// </summary>
        FirendOffline,
        /// <summary>
        /// 好友申请
        /// </summary>
        FirendApply,
        /// <summary>
        /// 新好友
        /// </summary>
        NewFirend,
    }
}
