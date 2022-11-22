using QianShiChat.Models;
using QianShiChat.WebApi.Models.Entity;

namespace QianShiChat.WebApi.Services
{
    public interface IChatMessageService
    {
        Task<List<ChatMessageDto>> GetNewMessageAndCacheAsync(int userId1, int userId2, CancellationToken cancellationToken = default);

        /// <summary>
        /// send message.
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        Task<ChatMessageDto> SendMessageAsync(ChatMessage chatMessage);
        Task UpdateMessageCursor(int userId, UpdateCursorRequest request, CancellationToken cancellationToken = default);
    }
}