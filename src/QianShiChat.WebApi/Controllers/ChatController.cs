using QianShiChat.Application.Services.IServices;

namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// chat api
/// </summary>
[ApiController]
[Authorize]
public class ChatController : BaseController
{
    private readonly IChatMessageService _chatMessageService;

    /// <summary>
    /// chat api
    /// </summary>
    public ChatController(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    /// <summary>
    /// send text message.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("text")]
    public Task<ChatMessageDto> SendText([FromBody] SendTextMessageRequest request, CancellationToken cancellationToken = default)
    {
        return _chatMessageService.SendTextMessageAsync(request, cancellationToken);
    }

    /// <summary>
    /// send file message.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("file")]
    public async Task<ChatMessageDto> SendFile([FromBody] SendFileMessageRequest request, CancellationToken cancellationToken = default)
    {
        return await _chatMessageService.SendFileMessageAsync(request, cancellationToken);
    }

    /// <summary>
    /// send files message.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("files")]
    public async Task<ChatMessageDto> SendFiles([FromBody] SendFilesMessageRequest request, CancellationToken cancellationToken = default)
    {
        return await _chatMessageService.SendFilesAsync(request, cancellationToken);
    }

    /// <summary>
    /// get room history.
    /// </summary>
    /// <param name="roomId">room id</param>
    /// <param name="request">paged query</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{roomId}/history")]
    public Task<PagedList<ChatMessageDto>> GetHistory([FromRoute] string roomId, [FromQuery] QueryMessageRequest request, CancellationToken cancellationToken = default)
    {
        return _chatMessageService.GetHistoryAsync(roomId, request, cancellationToken);
    }
}
