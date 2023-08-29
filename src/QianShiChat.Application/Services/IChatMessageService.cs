namespace QianShiChat.Application.Services;

/// <summary>
/// chat message service.
/// </summary>
public interface IChatMessageService
{
    Task<PagedList<ChatMessageDto>> GetGroupHistoryAsync(int toId, QueryMessagesRequest request, CancellationToken cancellationToken = default);
    Task<PagedList<ChatMessageDto>> GetHistoryAsync(string roomId, QueryMessagesRequest request, CancellationToken cancellationToken = default);
    Task<PagedList<ChatMessageDto>> GetUserHistoryAsync(int userId, QueryMessagesRequest request, CancellationToken cancellationToken = default);
    Task<ChatMessageDto> SendFileMessageAsync(SendFileMessageRequest request, CancellationToken cancellationToken = default);
    Task<ChatMessageDto> SendMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default);
    Task<ChatMessageDto> SendTextMessageAsync(SendTextMessageRequest request, CancellationToken cancellationToken = default);
}