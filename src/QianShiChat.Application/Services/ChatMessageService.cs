namespace QianShiChat.Application.Services;

/// <summary>
/// chat message service
/// </summary>
public class ChatMessageService : IChatMessageService, ITransient
{
    private readonly ILogger<ChatMessageService> _logger;
    private readonly ChatDbContext _context;
    private readonly IRedisCachingProvider _redisCachingProvider;
    private readonly IMapper _mapper;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IFileService _fileService;

    /// <summary>
    /// chat message service
    /// </summary>
    public ChatMessageService(
        IRedisCachingProvider redisCachingProvider,
        ChatDbContext context,
        ILogger<ChatMessageService> logger,
        IMapper mapper,
        IHubContext<ChatHub, IChatClient> hubContext,
        IFileService fileService)
    {
        _redisCachingProvider = redisCachingProvider;
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _hubContext = hubContext;
        _fileService = fileService;
    }

    public async Task<List<ChatMessageDto>> GetNewMessageAndCacheAsync(
        int userId1,
        int userId2,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = AppConsts.GetPrivateChatCacheKey(userId1, userId2);

        var cacheVal = await _redisCachingProvider.HGetAllAsync(cacheKey);
        if (cacheVal is null) cacheVal = new Dictionary<string, string>();

        var messages = new List<ChatMessage>();
        foreach (var item in cacheVal)
        {
            var message = JsonSerializer.Deserialize<ChatMessage>(item.Value);
            if (message is null)
            {
                await _redisCachingProvider.HDelAsync(cacheKey, new List<string> { item.Key });
                continue;
            }
            messages.Add(message);
        }

        if (messages.Count < 10)
        {
            var minId = messages.Count > 0 ? messages.Min(x => x.Id) : long.MaxValue;

            var data = await _context.ChatMessages.AsNoTracking()
                 .Where(x => x.SendType == ChatMessageSendType.Personal)
                 .Where(x => x.ToId == userId1 && x.FromId == userId2 || x.ToId == userId2 && x.FromId == userId1)
                 .Where(x => x.Id < minId)
                 .OrderByDescending(x => x.Id)
                 .Take(10 - messages.Count)
                 .ToListAsync(cancellationToken);

            if (data.Count > 0)
            {
                messages.AddRange(data);

                foreach (var item in data)
                {
                    await _redisCachingProvider.HSetAsync(cacheKey, item.Id.ToString(), JsonSerializer.Serialize(item));
                }
            }
        }

        foreach (var message in messages)
        {
            if (message.MessageType != ChatMessageType.Text)
            {
                message.Content = _fileService.FormatWwwRootFile(message.Content);
            }
        }

        return _mapper.Map<List<ChatMessageDto>>(messages);
    }

    public async Task UpdateMessageCursor(
        int userId,
        UpdateCursorRequest request,
        CancellationToken cancellationToken = default)
    {
        var cursorInfo = await _context.MessageCursors
            .FindAsync(new object[] { userId }, cancellationToken);

        if (cursorInfo == null)
        {
            cursorInfo = _mapper.Map<MessageCursor>(request);
            cursorInfo.UserId = userId;
        }
        else
        {
            _mapper.Map(request, cursorInfo);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ChatMessageDto> SendMessageAsync(ChatMessage chatMessage)
    {
        var chatMessageDto = _mapper.Map<ChatMessageDto>(chatMessage);

        if (chatMessageDto.MessageType != ChatMessageType.Text)
        {
            chatMessageDto.Content = _fileService.FormatWwwRootFile(_fileService.FormatWwwRootFile(chatMessageDto.Content));
        }

        // send message.
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

        // cache message max 30 times;
        string? cacheKey;
        if (chatMessageDto.SendType == ChatMessageSendType.Personal)
        {
            cacheKey = AppConsts.GetPrivateChatCacheKey(chatMessageDto.ToId, chatMessageDto.FromId);
        }
        else
        {
            cacheKey = AppConsts.GetGroupChatCacheKey(chatMessageDto.ToId);
        }
        var cacheValue = await _redisCachingProvider.HGetAllAsync(cacheKey);
        if (cacheValue == null) cacheValue = new Dictionary<string, string>();

        cacheValue.Add(chatMessageDto.Id.ToString(), JsonSerializer.Serialize(chatMessageDto));

        if (cacheValue.Count > 30)
        {
            cacheValue.Remove(cacheValue.Keys.Min());
        }

        await _redisCachingProvider.HMSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        // cache save message queue.
        await _redisCachingProvider.HSetAsync(
            AppConsts.ChatMessageCacheKey,
            chatMessageDto.Id.ToString(),
            JsonSerializer.Serialize(chatMessage));

        // cache update message cursor
        await _redisCachingProvider.HSetAsync(
            AppConsts.MessageCursorCacheKey,
            chatMessageDto.FromId.ToString(),
            chatMessageDto.Id.ToString());

        return chatMessageDto;
    }
}