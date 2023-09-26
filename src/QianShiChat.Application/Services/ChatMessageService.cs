namespace QianShiChat.Application.Services;

/// <summary>
/// chat message service
/// </summary>
public class ChatMessageService : IChatMessageService, ITransient
{
    public static Dictionary<ChatMessageSendType, Func<int, int, string>> SendTypeMapper = new()
    {
        { ChatMessageSendType.Personal, AppConsts.GetPrivateChatRoomId },
        { ChatMessageSendType.Group, AppConsts.GetGroupChatRoomId },
    };

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IFileService _fileService;
    private readonly IUserManager _userManager;
    private readonly IRoomService _roomService;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IOnlineManager _onlineManager;

    /// <summary>
    /// chat message service
    /// </summary>
    public ChatMessageService(
        IApplicationDbContext context,
        IMapper mapper,
        IHubContext<ChatHub, IChatClient> hubContext,
        IFileService fileService,
        IUserManager userManager,
        IRoomService roomService,
        IAttachmentRepository attachmentRepository,
        IOnlineManager onlineManager)
    {
        _context = context;
        _mapper = mapper;
        _hubContext = hubContext;
        _fileService = fileService;
        _userManager = userManager;
        _roomService = roomService;
        _attachmentRepository = attachmentRepository;
        _onlineManager = onlineManager;
    }

    public Task<PagedList<ChatMessageDto>> GetGroupHistoryAsync(int toId, QueryMessageRequest request, CancellationToken cancellationToken = default)
    {
        var roomId = GetRoomId(_userManager.CurrentUserId, toId, ChatMessageSendType.Group);
        return GetHistoryAsync(roomId, request, cancellationToken);
    }

    public async Task<PagedList<ChatMessageDto>> GetHistoryAsync(string roomId, QueryMessageRequest request, CancellationToken cancellationToken = default)
    {
        var queryable = _context.ChatMessages
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Include(x => x.Attachments)
            .Where(x => x.RoomId == roomId);

        var messages = await queryable
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var total = await queryable
            .CountAsync(cancellationToken);
        var messageDtos = _mapper.Map<List<ChatMessageDto>>(messages);

        Parallel.ForEach(messageDtos, FormatAttachmentMessage);

        return PagedList.Create(messageDtos, total, request.Size);
    }

    private void FormatAttachmentMessage(ChatMessageDto message)
    {
        if (message.MessageType is not ChatMessageType.Text
            && message.Content is not null
            && message.Content is string content
            && !string.IsNullOrWhiteSpace(content))
        {
            if (content.StartsWith("["))
            {
                var attachments = JsonSerializer.Deserialize<List<Attachment>>(content)!;
                var attachmentDtos = _mapper.Map<List<AttachmentDto>>(attachments);
                attachmentDtos.ForEach(FormatAttachment);
                message.Content = attachmentDtos;
            }
            else
            {
                var attachment = JsonSerializer.Deserialize<Attachment>(content)!;
                var attachmentDto = _mapper.Map<AttachmentDto>(attachment);
                FormatAttachment(attachmentDto);
                message.Content = attachmentDto;
            }
        }
    }

    public Task<PagedList<ChatMessageDto>> GetUserHistoryAsync(int userId, QueryMessageRequest request, CancellationToken cancellationToken = default)
    {
        var roomId = GetRoomId(_userManager.CurrentUserId, userId, ChatMessageSendType.Personal);
        return GetHistoryAsync(roomId, request, cancellationToken);
    }

    public async Task<ChatMessageDto> SendMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default)
    {
        var chatMessageDto = _mapper.Map<ChatMessageDto>(chatMessage);
        FormatAttachmentMessage(chatMessageDto);

        var currentConnectionId = await _onlineManager.GetUserClientConnectionIdAsync(chatMessage.FromId, _userManager.CurrentClientType);

        if (chatMessageDto.SendType == ChatMessageSendType.Personal)
        {
            await _hubContext.Clients
                .User(chatMessageDto.ToId.ToString())
                .PrivateChat(chatMessageDto);

            var ownOtherClients = _onlineManager.GetConnections(chatMessage.FromId).Except(new string[] { currentConnectionId });
            if (ownOtherClients.Any())
            {
                await _hubContext.Clients
                     .Clients(ownOtherClients)
                     .OwnOtherClientMessage(chatMessageDto);
            }
        }
        else if (chatMessageDto.SendType == ChatMessageSendType.Group)
        {
            await _hubContext.Clients
                .GroupExcept(AppConsts.GetGroupChatRoomId(_userManager.CurrentUserId, chatMessageDto.ToId), currentConnectionId)
                .GroupMessage(chatMessageDto);
        }
        else
        {
            throw new NotSupportedException();
        }

        await _roomService.AddOrUpdatePositionAsync(
            chatMessage.FromId,
            chatMessage.ToId,
            chatMessage.SendType,
            chatMessage.Id);
        await _context.ChatMessages.AddAsync(chatMessage, cancellationToken);
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
            RoomId = GetRoomId(_userManager.CurrentUserId, request.ToId, request.SendType),
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
            RoomId = GetRoomId(_userManager.CurrentUserId, request.ToId, request.SendType),
            UpdateTime = now,
            MessageType = dto.ContentType.ToLower() switch
            {
                "image/png" => ChatMessageType.Image,
                "image/jpeg" => ChatMessageType.Image,
                "image/gif" => ChatMessageType.Image,
                "image/x-icon" => ChatMessageType.Image,
                "video/mpeg4" => ChatMessageType.Video,
                "audio/mpeg" => ChatMessageType.Audio,
                _ => ChatMessageType.OtherFile
            },
            SendType = request.SendType
        };
        chatMessage.Attachments.Add(attachment);

        return await SendMessageAsync(chatMessage);
    }

    public async Task<ChatMessageDto> SendFilesAsync(SendFilesMessageRequest request, CancellationToken cancellationToken = default)
    {
        var now = Timestamp.Now;
        var attachments = await _attachmentRepository.GetByIdsAsync(request.AttachmentIds, cancellationToken);

        if (!attachments.Any())
        {
            throw Oops.Bah("attachment not exists.");
        }

        var dtos = _mapper.Map<List<AttachmentDto>>(attachments);
        dtos.ForEach(FormatAttachment);
        ChatMessageType messageType = ChatMessageType.OtherFile;
        if (dtos.Count == 1)
        {
            messageType = dtos[0].ContentType.ToLower() switch
            {
                "image/png" => ChatMessageType.Image,
                "image/jpeg" => ChatMessageType.Image,
                "image/gif" => ChatMessageType.Image,
                "image/x-icon" => ChatMessageType.Image,
                "video/mpeg4" => ChatMessageType.Video,
                "audio/mpeg" => ChatMessageType.Audio,
                _ => ChatMessageType.OtherFile
            };
        }

        var chatMessage = new ChatMessage()
        {
            Id = YitIdHelper.NextId(),
            Content = JsonSerializer.Serialize(dtos),
            CreateTime = now,
            FromId = _userManager.CurrentUserId,
            ToId = request.ToId,
            RoomId = GetRoomId(_userManager.CurrentUserId, request.ToId, request.SendType),
            UpdateTime = now,
            MessageType = messageType,
            SendType = request.SendType
        };
        attachments.ForEach(chatMessage.Attachments.Add);

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