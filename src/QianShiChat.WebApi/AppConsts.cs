using QianShiChat.Models;

namespace QianShiChat.WebApi
{
    public class AppConsts
    {
        public const string ClaimUserId = "UserId";

        public const string ChatMessageCacheKey = "ChatMessages";

        public const string MessageCursorCacheKey = "MessageCursor";

        public static string GetPrivateChatCacheKey(int id1, int id2)
        {
            return id1 > id2 ? $"{id2}-{id1}" : $"{id1}-{id2}";
        }

        public static string GetMessageCursorCacheKey(int userId)
        {
            return $"{MessageCursorCacheKey}:{userId}";
        }

    }
}
