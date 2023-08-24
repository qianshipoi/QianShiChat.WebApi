namespace QianShiChat.Application.Repositories;

public interface IChatMessageRepository
{
    Task<int> GetUnreadCountAsync(string sessionId, long position, CancellationToken cancellationToken = default);
}
