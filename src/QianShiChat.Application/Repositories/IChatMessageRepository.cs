namespace QianShiChat.Application.Repositories;

public interface IChatMessageRepository
{
    Task<ChatMessage?> GetLastMessageAsync(string sessionId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string sessionId, long position, CancellationToken cancellationToken = default);
}
