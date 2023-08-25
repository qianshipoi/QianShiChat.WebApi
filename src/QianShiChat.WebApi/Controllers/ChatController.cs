namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// chat api
/// </summary>
[ApiController]
[Authorize]
public class ChatController : BaseController
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    private readonly IChatMessageService _chatMessageService;
    private readonly IAttachmentRepository _attachmentRepository;

    /// <summary>
    /// chat api
    /// </summary>
    public ChatController(
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper,
        IChatMessageService chatMessageService,
        IAttachmentRepository attachmentRepository)
    {
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _chatMessageService = chatMessageService;
        _attachmentRepository = attachmentRepository;
    }

    /// <summary>
    /// get file by filename
    /// </summary>
    /// <param name="filename">filename</param>
    /// <returns></returns>
    [HttpGet("{filename}")]
    public IActionResult GetFile(string filename)
    {
        var wwwroot = _webHostEnvironment.WebRootPath;

        var filePath = Path.Combine(wwwroot, filename);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        new FileExtensionContentTypeProvider()
            .Mappings.TryGetValue(Path.GetExtension(filePath), out var contenttype);

        return PhysicalFile(filePath, contenttype ?? "application/octet-stream");
    }

    public static Dictionary<ChatMessageSendType, Func<int, int, string>> SendTypeMapper = new()
        {
            { ChatMessageSendType.Personal, AppConsts.GetPrivateChatSessionId },
            { ChatMessageSendType.Group, AppConsts.GetGroupChatSessionId },
        };

    private string GetSesionId(int fromId, int toId, ChatMessageSendType type)
    {
        if (SendTypeMapper.ContainsKey(type))
        {
            throw new NotSupportedException();
        }
        return SendTypeMapper[type].Invoke(fromId, toId);
    }

    /// <summary>
    /// send text message.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("text")]
    public async Task<IActionResult> Send([FromBody] PrivateChatMessageRequest request, CancellationToken cancellationToken = default)
    {
        var now = Timestamp.Now;

        var chatMessage = new ChatMessage()
        {
            Id = YitIdHelper.NextId(),
            Content = request.Message,
            CreateTime = now,
            SessionId = GetSesionId(CurrentUserId, request.ToId, request.SendType),
            FromId = CurrentUserId,
            ToId = request.ToId,
            UpdateTime = now,
            MessageType = ChatMessageType.Text,
            SendType = request.SendType
        };

        var chatMessageDto = await _chatMessageService.SendMessageAsync(chatMessage);

        return Ok(chatMessageDto);
    }

    /// <summary>
    /// send file message.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="fileService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("file")]
    public async Task<IActionResult> SendFile(
        [FromBody] SendFileMessageRequest request,
        [FromServices] IFileService fileService,
        CancellationToken cancellationToken = default)
    {
        var now = Timestamp.Now;
        var attachment = await _attachmentRepository.FindByIdAsync(request.AttachmentId, cancellationToken);

        if (attachment is null)
        {
            return BadRequest("attachment not exists.");
        }

        var dto = _mapper.Map<AttachmentDto>(attachment);

        dto.RawPath = fileService.FormatPublicFile(dto.RawPath);
        if (!string.IsNullOrWhiteSpace(dto.PreviewPath))
        {
            dto.PreviewPath = fileService.FormatPublicFile(dto.PreviewPath);
        }

        var chatMessage = new ChatMessage()
        {
            Id = YitIdHelper.NextId(),
            Content = JsonSerializer.Serialize(dto),
            CreateTime = now,
            FromId = CurrentUserId,
            ToId = request.ToId,
            SessionId = GetSesionId(CurrentUserId, request.ToId, request.SendType),
            UpdateTime = now,
            MessageType = dto.ContentType.ToLower() switch
            {
                "image/png" => ChatMessageType.Image,
                "image/jpeg" => ChatMessageType.Image,
                "image/gif" => ChatMessageType.Image,
                "image/x-icon" => ChatMessageType.Image,
                "video/mpeg4" => ChatMessageType.Video,
                _ => ChatMessageType.OtherFile
            },
            SendType = request.SendType
        };

        var chatMessageDto = await _chatMessageService.SendMessageAsync(chatMessage);

        return Ok(chatMessageDto);
    }

    [HttpGet("{id:int}/history")]
    public Task<PagedList<ChatMessageDto>> GetUserHistory([FromRoute] int id, [FromQuery] QueryMessagesRequest request, CancellationToken cancellationToken = default)
    {
        return _chatMessageService.GetHistoryAsync(id, request, cancellationToken);
    }
}
