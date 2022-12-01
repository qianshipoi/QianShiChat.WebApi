namespace QianShiChat.WebApi.Services;

public class FriendService : IFriendService, ITransient
{
    private readonly ChatDbContext _context;
    private readonly ILogger<FriendService> _logger;
    private readonly IMapper _mapper;
    private readonly IRedisCachingProvider _redisCachingProvider;

    public FriendService(ChatDbContext context, ILogger<FriendService> logger, IMapper mapper, IRedisCachingProvider redisCachingProvider)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _redisCachingProvider = redisCachingProvider;
    }

    public async Task<bool> IsFriendAsync(int userId, int friendId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRealtions
            .AnyAsync(x => x.UserId == userId && x.FriendId == friendId, cancellationToken);
    }

    public async Task<List<int>> GetFriendIdsAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRealtions.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.FriendId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserDto>> GetFriendsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var friends = await _context.UserRealtions
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(x => x.Friend)
            .Select(x => x.Friend)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<UserDto>>(friends);
    }

    public async Task<List<UserWithMessage>> GetNewMessageFriendsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var cursor = await _context.MessageCursors
             .Where(x => x.UserId == userId)
             .Select(x => x.Postiton)
             .FirstOrDefaultAsync(cancellationToken);

        // get cache messages
        var cacheValue = await _redisCachingProvider.HGetAllAsync(AppConsts.ChatMessageCacheKey);

        var cacheMessages = new List<ChatMessage>();
        foreach (var item in cacheValue)
        {
            if (long.Parse(item.Key) < cursor)
            {
                continue;
            }
            var message = JsonSerializer.Deserialize<ChatMessage>(item.Value);
            if (message != null && message.ToId == userId)
                cacheMessages.Add(message);
        }

        // get all unread messages in database
        var messages = _context.ChatMessages
            .AsNoTracking()
             .Where(x => x.ToId == userId)
             .Where(x => x.Id > cursor)
             .Where(x => x.SendType == ChatMessageSendType.Personal);

        var sorted = messages.OrderByDescending(x => x.Id);

        var allMsg = await messages.Select(x => x.FromId)
             .Distinct()
             .SelectMany(x => sorted.Where(y => y.FromId == x).Take(10))
             .ToListAsync(cancellationToken);

        allMsg.AddRange(cacheMessages);

        allMsg = allMsg.DistinctBy(x => x.Id).ToList();

        var friendIds = allMsg.Select(x => x.FromId).Distinct();

        var friends = await _context.UserInfos
              .AsNoTracking()
              .Where(x => friendIds.Contains(x.Id))
              .ToListAsync(cancellationToken);

        var uwm = _mapper.Map<List<UserWithMessage>>(friends);
        var msg = _mapper.Map<List<ChatMessageDto>>(allMsg);

        uwm.ForEach(item =>
        {
            item.Messages = msg.Where(x => x.FromId == item.Id).ToList();
        });

        return uwm;
    }
}