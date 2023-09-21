namespace QianShiChat.Application.Services;

public interface IRoomService
{
    Task AddOrUpdatePositionAsync(int fromId, int toId, ChatMessageSendType type, long position = 0, CancellationToken cancellationToken = default);
    Task<RoomDto?> GetRoomAsync(int userId, string roomId, CancellationToken cancellationToken = default);
    Task<List<string>> GetRoomIdsAsync(int userId, CancellationToken cancellationToken = default);
    IAsyncEnumerable<RoomDto> GetRoomsAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<RoomDto>> GetUserRoomAsync(int userId, CancellationToken cancellationToken = default);
    Task UpdateRoomPositionAsync(int userId, string roomId, long position, CancellationToken cancellationToken = default);
}
