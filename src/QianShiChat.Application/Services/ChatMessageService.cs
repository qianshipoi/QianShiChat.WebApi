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
    private readonly IFileService _fileService;
    private readonly IUserManager _userManager;

    /// <summary>
    /// chat message service
    /// </summary>
    public ChatMessageService(
        IRedisCachingProvider redisCachingProvider,
        ChatDbContext context,
        ILogger<ChatMessageService> logger,
        IMapper mapper,
        IHubContext<ChatHub, IChatClient> hubContext,
        IFileService fileService,
        IUserManager userManager)
    {
        _redisCachingProvider = redisCachingProvider;
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _hubContext = hubContext;
        _fileService = fileService;
        _userManager = userManager;
    }

    private async Task<List<ChatMessage>> GetCacheMessageAsync(int userId1, int userId2)
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

        return messages;
    }

    public async Task<List<ChatMessageDto>> GetNewMessageAndCacheAsync(
        int userId1,
        int userId2,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = AppConsts.GetPrivateChatCacheKey(userId1, userId2);

        var messages = await GetCacheMessageAsync(userId1, userId2);

        if (messages.Count < 10)
        {
            var minId = messages.Count > 0 ? messages.Min(x => x.Id) : long.MaxValue;

            var data = await _context.ChatMessages.AsNoTracking()
                 .Where(x => x.SendType == ChatMessageSendType.Personal)
                 .Where(x => (x.ToId == userId1 && x.FromId == userId2) || (x.ToId == userId2 && x.FromId == userId1))
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
                message.Content = _fileService.FormatPublicFile(message.Content);
            }
        }

        return _mapper.Map<List<ChatMessageDto>>(messages);
    }

    public async Task<PagedList<ChatMessageDto>> GetHistoryAsync(int userId, QueryMessagesRequest request, CancellationToken cancellationToken = default)
    {
        var sessionId = AppConsts.GetPrivateChatCacheKey(_userManager.CurrentUserId, userId);

        var cacheMessages = await GetCacheMessageAsync(_userManager.CurrentUserId, userId);
        var minId = cacheMessages.Count > 0 ? cacheMessages.Min(x => x.Id) : long.MaxValue;
        var skipCount = (request.Page - 1) * request.Size;
        var takeCount = 0;

        var message = new List<ChatMessage>();

        if (cacheMessages.Count >= skipCount + request.Size)
        {
            // all in cache.
            message.AddRange(cacheMessages.Skip(skipCount).Take(request.Size));
        }
        else if (cacheMessages.Count > skipCount)
        {
            // some record in cache.
            message.AddRange(cacheMessages.Skip(skipCount));
            takeCount = request.Size - (cacheMessages.Count - skipCount);
            skipCount = cacheMessages.Count;
        }
        else
        {
            // all out cache.
            takeCount = request.Size;
        }

        if (takeCount > 0)
        {
            var databaseMessages = await _context.ChatMessages.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Where(x => x.Id < minId)
                .Where(x=>x.SessionId == sessionId)
                .Where(x => x.SendType == ChatMessageSendType.Personal)
                .Skip(skipCount)
                .Take(takeCount)
                .ToListAsync(cancellationToken);
            if (databaseMessages is not null && databaseMessages.Any())
            {
                message.AddRange(databaseMessages);
            }
        }

        var databaseCount = await _context.ChatMessages
            .Where(x => x.Id < minId)
            .Where(x => x.SendType == ChatMessageSendType.Personal)
            .Where(x => (x.ToId == _userManager.CurrentUserId && x.FromId == userId) || (x.ToId == userId && x.FromId == _userManager.CurrentUserId))
            .CountAsync(cancellationToken);

        var total = cacheMessages.Count + databaseCount;

        return PagedList.Create(_mapper.Map<List<ChatMessageDto>>(message.OrderByDescending(x => x.CreateTime)), total, request.Size);
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