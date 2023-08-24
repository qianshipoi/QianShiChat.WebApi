namespace QianShiChat.Application.Services;

public interface ISessionService
{
    Task AddOrUpdatePositionAsync(int fromId, int toId, ChatMessageSendType type, long position = 0, CancellationToken cancellationToken = default);
    Task<List<SessionDto>> GetUserSessionAsync(int userId, CancellationToken cancellationToken = default);
}
