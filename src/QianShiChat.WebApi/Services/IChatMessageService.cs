using QianShiChat.Models;

namespace QianShiChat.WebApi.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessageDto>> GetNewMessageAndCacheAsync(int userId1, int userId2, CancellationToken cancellationToken = default);
        Task UpdateMessageCursor(int userId, UpdateCursorRequest request, CancellationToken cancellationToken = default);
    }
}