namespace QianShiChat.Application.Services;

/// <summary>
/// chat message service
/// </summary>
public class ChatMessageService : IChatMessageService, ITransient
{
    public static Dictionary<ChatMessageSendType, Func<int, int, string>> SendTypeMapper = new()
    {
        { ChatMessageSendType.Personal, AppConsts.GetPrivateChatSessionId },
        { ChatMessageSendType.Group, AppConsts.GetGroupChatSessionId },
    };

    private readonly ChatDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IFileService _fileService;
    private readonly IUserManager _userManager;
    private readonly ISessionService _sessionService;
    private readonly IAttachmentRepository _attachmentRepository;

    /// <summary>
    /// chat message service
    /// </summary>
    public ChatMessageService(
        ChatDbContext context,
        IMapper mapper,
        IHubContext<ChatHub, IChatClient> hubContext,
        IFileService fileService,
        IUserManager userManager,
        ISessionService sessionService,
        IAttachmentRepository attachmentRepository)
    {
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
        _fileService = fileService;
        _userManager = userManager;
        _sessionService = sessionService;
        _attachmentRepository = attachmentRepository;
    }

    public Task<PagedList<ChatMessageDto>> GetGroupHistoryAsync(int toId, QueryMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var sessionId = GetRoomId(_userManager.CurrentUserId, toId, ChatMessageSendType.Group);
        return GetHistoryAsync(sessionId, request, cancellationToken);
    }

    public async Task<PagedList<ChatMessageDto>> GetHistoryAsync(string roomId, QueryMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var queryable = _context.ChatMessages
            .AsNoTracking()
            .OrderByDescending(x => x.CreateTime)
            .Where(x => x.SessionId == roomId)
            .Where(x => x.SendType == ChatMessageSendType.Personal);

        var messages = await queryable
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var total = await queryable
            .CountAsync(cancellationToken);
        var messageDtos = _mapper.Map<List<ChatMessageDto>>(messages);

        Parallel.ForEach(messageDtos, message => {
            if (message.MessageType != ChatMessageType.Text
                && message.Content is not null
                && message.Content is string content
                && !string.IsNullOrWhiteSpace(content))
            {
                var attachment = JsonSerializer.Deserialize<Attachment>(content);
                if (attachment is not null)
                {
                    var attachmentDto = _mapper.Map<AttachmentDto>(attachment);
                    FormatAttachment(attachmentDto);
                    message.Content = attachmentDto;
                }
            }
        });

        return PagedList.Create(messageDtos, total, request.Size);
    }

    public Task<PagedList<ChatMessageDto>> GetUserHistoryAsync(int userId, QueryMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var sessionId = GetRoomId(_userManager.CurrentUserId, userId, ChatMessageSendType.Personal);
        return GetHistoryAsync(sessionId, request, cancellationToken);
    }

    public async Task<ChatMessageDto> SendMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default)
    {
        var chatMessageDto = _mapper.Map<ChatMessageDto>(chatMessage);
        if (chatMessageDto.MessageType is not ChatMessageType.Text
            && chatMessageDto.Content is not null
            && chatMessageDto.Content is string content
            && !string.IsNullOrWhiteSpace(content))
        {
            var attachment = JsonSerializer.Deserialize<Attachment>(content)!;
            var attachmentDto = _mapper.Map<AttachmentDto>(attachment);
            FormatAttachment(attachmentDto);
            chatMessageDto.Content = attachmentDto;
        }

        if (chatMessageDto.SendType == ChatMessageSendType.Personal)
        {
            await _hubContext.Clients
                .User(chatMessageDto.ToId.ToString())
                .PrivateChat(chatMessageDto);
        }
        else if (chatMessageDto.SendType == ChatMessageSendType.Group)
        {
            await _hubContext.Clients
                .Group(chatMessageDto.ToId.ToString())
                .PrivateChat(chatMessageDto);
        }
        else
        {
            throw new NotSupportedException();
        }

        await _sessionService.AddOrUpdatePositionAsync(
            chatMessage.FromId,
            chatMessage.ToId,
            chatMessage.SendType,
            chatMessage.Id);

        await _context.AddAsync(chatMessage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chatMessageDto;
    }

    public async Task<ChatMessageDto> SendTextMessageAsync(SendTextMessageRequest request, CancellationToken cancellationToken = default)
    {
        var now = Timestamp.Now;

        var chatMessage = new ChatMessage()
        {
            Id = YitIdHelper.NextId(),
            Content = request.Message,
            CreateTime = now,
            SessionId = GetRoomId(_userManager.CurrentUserId, request.ToId, request.SendType),
            FromId = _userManager.CurrentUserId,
            ToId = request.ToId,
            UpdateTime = now,
            MessageType = ChatMessageType.Text,
            SendType = request.SendType
        };

        return await SendMessageAsync(chatMessage);
    }

    public async Task<ChatMessageDto> SendFileMessageAsync(SendFileMessageRequest request, CancellationToken cancellationToken = default)
    {
        var now = Timestamp.Now;
        var attachment = await _attachmentRepository.FindByIdAsync(request.AttachmentId, cancellationToken);

        if (attachment is null)
        {
            throw Oops.Bah("attachment not exists.");
        }

        var dto = _mapper.Map<AttachmentDto>(attachment);
        FormatAttachment(dto);

        var chatMessage = new ChatMessage()
        {
            Id = YitIdHelper.NextId(),
            Content = JsonSerializer.Serialize(attachment),
            CreateTime = now,
            FromId = _userManager.CurrentUserId,
            ToId = request.ToId,
            SessionId = GetRoomId(_userManager.CurrentUserId, request.ToId, request.SendType),
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

        return await SendMessageAsync(chatMessage);
    }

    private void FormatAttachment(AttachmentDto dto)
    {
        dto.RawPath = _fileService.FormatPublicFile(dto.RawPath);
        if (!string.IsNullOrWhiteSpace(dto.PreviewPath))
        {
            dto.PreviewPath = _fileService.FormatPublicFile(dto.PreviewPath);
        }
    }

    private string GetRoomId(int fromId, int toId, ChatMessageSendType type)
    {
        if (!SendTypeMapper.ContainsKey(type))
        {
            throw new NotSupportedException();
        }
        return SendTypeMapper[type].Invoke(fromId, toId);
    }
}