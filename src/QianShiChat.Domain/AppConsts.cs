namespace QianShiChat.Domain;

/// <summary>
/// app consts.
/// </summary>
public static class AppConsts
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

    /// <summary>
    /// 客户端类型
    /// </summary>
    public const string ClientType = "Client-Type";

    /// <summary>
    /// get privatie chat cache key.
    /// </summary>
    /// <param name="id1"></param>
    /// <param name="id2"></param>
    /// <returns></returns>
    public static string GetPrivateChatCacheKey(int id1, int id2)
    {
        return id1 > id2 ? $"{id2}-{id1}" : $"{id1}-{id2}";
    }

    public static string GetPrivateChatRoomId(int id1, int id2) => $"personal-{GetPrivateChatCacheKey(id1, id2)}";

    public static string GetGroupChatRoomId(int userId, int groupId) => $"group-{groupId}";

    /// <summary>
    /// get group chat cache key.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public static string GetGroupChatCacheKey(int groupId)
    {
        return $"{GroupChatMessageCacheKey}:{groupId}";
    }

    /// <summary>
    /// get message curosr cache key.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GetMessageCursorCacheKey(int userId)
    {
        return $"{MessageCursorCacheKey}:{userId}";
    }

    public static string GetAuthorizeCacheKey(string clientType, string id) => $"{clientType}:{id}";

    public static long MaxAvatarLength = 1024 * 1024 * 4;
    public static string[] AllowAvatarExts = new string[] { ".png", ".jpg", ".jpeg", ".gif" };
    public const string AvatarPrefix = "Raw/Avatar";

    /// <summary>
    /// trim / or \
    /// </summary>
    public static char[] TrimChars = new[] { '/', '\\' };

    public const string GroupTokenKey = "359378D07D64404C93EA4658DB4A0AB0";

}