namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// chat api
/// </summary>
[ApiController]
[Authorize]
public class ChatController : BaseController
{
    private readonly IRedisCachingProvider _redisCachingProvider;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    private readonly IChatMessageService _chatMessageService;
    private readonly IAttachmentRepository _attachmentRepository;

    /// <summary>
    /// chat api
    /// </summary>
    public ChatController(
        IRedisCachingProvider redisCachingProvider,
        IHubContext<ChatHub, IChatClient> hubContext,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper,
        IChatMessageService chatMessageService,
        IAttachmentRepository attachmentRepository)
    {
        _redisCachingProvider = redisCachingProvider;
        _hubContext = hubContext;
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

    ///// <summary>
    ///// send file message.
    ///// </summary>
    ///// <param name="request"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    //[HttpPost("file")]
    //public async Task<IActionResult> SendFile(
    //    [FromForm] SendFileRequest request,
    //    CancellationToken cancellationToken = default)
    //{
    //    // save to wwwroot
    //    var wwwroot = _webHostEnvironment.WebRootPath;

    //    var fileExt = Path.GetExtension(request.File.FileName);
    //    using var stream = request.File.OpenReadStream();
    //    var md5 = stream.ToMd5();
    //    var newFilePath = Path.Combine("Raw", "Chat", md5 + fileExt);

    //    var saveFilePath = Path.Combine(wwwroot, newFilePath);
    //    var dirPath = Path.GetDirectoryName(saveFilePath);

    //    if (!Directory.Exists(dirPath))
    //    {
    //        Directory.CreateDirectory(dirPath!);
    //    }

    //    using var fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.Write);
    //    stream.CopyTo(fileStream);
    //    await fileStream.FlushAsync();

    //    // send message
    //    var now = Timestamp.Now;

    //    var messageType = ChatMessageType.OtherFile;
    //    var isImage = FileHelper.IsAllowImages(request.File.FileName);
    //    if (isImage)
    //    {
    //        messageType = ChatMessageType.Image;
    //    }
    //    else
    //    {
    //        var isVideo = FileHelper.IsAllowVideos(request.File.FileName);
    //        if (isVideo) messageType = ChatMessageType.Video;
    //    }

    //    var chatMessage = new ChatMessage()
    //    {
    //        Id = YitIdHelper.NextId(),
    //        Content = newFilePath,
    //        CreateTime = now,
    //        FromId = CurrentUserId,
    //        ToId = request.ToId,
    //        UpdateTime = now,
    //        MessageType = messageType,
    //        SendType = request.SendType
    //    };

    //    var chatMessageDto = await _chatMessageService.SendMessageAsync(chatMessage);

    //    return CreatedAtAction(nameof(GetFile), new { filename = newFilePath }, chatMessageDto);
    //}

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

        if(attachment is null)
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

}
