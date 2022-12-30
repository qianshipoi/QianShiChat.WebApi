namespace QianShiChat.Application.Services;

/// <summary>
/// chat message service.
/// </summary>
public interface IChatMessageService
{
    /// <summary>
    /// get new messages.
    /// </summary>
    /// <param name="userId1"></param>
    /// <param name="userId2"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ChatMessageDto>> GetNewMessageAndCacheAsync(int userId1, int userId2, CancellationToken cancellationToken = default);

    /// <summary>
    /// send message.
    /// </summary>
    /// <param name="chatMessage"></param>
    /// <returns></returns>
    Task<ChatMessageDto> SendMessageAsync(ChatMessage chatMessage);

    /// <summary>
    /// update message cursor.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateMessageCursor(int userId, UpdateCursorRequest request, CancellationToken cancellationToken = default);
}