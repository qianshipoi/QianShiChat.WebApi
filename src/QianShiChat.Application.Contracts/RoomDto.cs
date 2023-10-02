namespace QianShiChat.Application.Contracts;


public interface IRoomToObject
{

}

public class RoomDto
{
    public string Id { get; set; } = string.Empty;
    public ChatMessageSendType Type { get; set; }
    public int FromId { get; set; }
    public int ToId { get; set; }
    public int UnreadCount { get; set; }
    public long LastMessageTime { get; set; }
    public object? LastMessageContent { get; set; }
    public UserDto? FromUser { get; set; }
    public IRoomToObject? ToObject { get; set; }

    public static RoomDto CreateEmpty<T>(string roomId, UserDto user, T toObject) where T : IRoomToObject
    {
        if (toObject is UserDto toUser)
        {
            return new RoomDto
            {
                FromId = user.Id,
                FromUser = user,
                Id = roomId,
                LastMessageContent = string.Empty,
                LastMessageTime = 0,
                ToId = toUser.Id,
                ToObject = toObject,
                Type = ChatMessageSendType.Personal,
                UnreadCount = 0
            };
        }
        else if (toObject is GroupDto toGroup)
        {
            return new RoomDto
            {
                FromId = user.Id,
                FromUser = user,
                Id = roomId,
                LastMessageContent = string.Empty,
                LastMessageTime = 0,
                ToId = toGroup.Id,
                ToObject = toObject,
                Type = ChatMessageSendType.Group,
                UnreadCount = 0
            };
        }
        else
        {
            throw new NotSupportedException("to object not supported");
        }
    }
}
