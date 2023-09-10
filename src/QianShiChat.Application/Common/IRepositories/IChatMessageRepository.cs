namespace QianShiChat.Application.Common.IRepositories;

public interface IChatMessageRepository
{
    Task<ChatMessage?> GetLastMessageAsync(string roomId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string roomId, long position, CancellationToken cancellationToken = default);
}
