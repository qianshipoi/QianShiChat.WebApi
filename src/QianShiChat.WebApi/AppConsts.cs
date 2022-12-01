namespace QianShiChat.WebApi
{
    /// <summary>
    /// app consts.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// cors name.
        /// </summary>
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        /// <summary>
        /// online cache key.
        /// </summary>
        public const string OnlineCacheKey = "OnlineList";

        /// <summary>
        /// jwt claim userId key.
        /// </summary>
        public const string ClaimUserId = "UserId";

        /// <summary>
        /// chat message cache key.
        /// </summary>
        public const string ChatMessageCacheKey = "ChatMessages";

        /// <summary>
        /// message cursor cache key.
        /// </summary>
        public const string MessageCursorCacheKey = "MessageCursor";

        /// <summary>
        /// group chat message cache key.
        /// </summary>
        public const string GroupChatMessageCacheKey = "GroupMessages";

        public static string GetPrivateChatCacheKey(int id1, int id2)
        {
            return id1 > id2 ? $"{id2}-{id1}" : $"{id1}-{id2}";
        }

        public static string GetGroupChatCacheKey(int groupId)
        {
            return $"{GroupChatMessageCacheKey}:{groupId}";
        }

        public static string GetMessageCursorCacheKey(int userId)
        {
            return $"{MessageCursorCacheKey}:{userId}";
        }
    }
}