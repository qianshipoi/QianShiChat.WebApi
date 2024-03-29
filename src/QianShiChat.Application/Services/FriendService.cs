﻿namespace QianShiChat.Application.Services;

public class FriendService : IFriendService, ITransient
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<FriendService> _logger;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IOnlineManager _onlineManager;
    private readonly IUserRepository _userRepository;

    public FriendService(
        IApplicationDbContext context,
        ILogger<FriendService> logger,
        IMapper mapper,
        IFileService fileService,
        IOnlineManager onlineManager,
        IUserRepository userRepository)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _fileService = fileService;
        _onlineManager = onlineManager;
        _userRepository = userRepository;
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

    public async Task DeleteAsync(int userId, int friendId, CancellationToken cancellationToken = default)
    {
        var realtions = await _context.UserRealtions.Where(x => (x.UserId == userId && x.FriendId == friendId) || (x.FriendId == userId && x.UserId == friendId)).ToListAsync(cancellationToken);

        _context.UserRealtions.RemoveRange(realtions);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<FriendDto>> GetFriendsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var friends = await _context.UserRealtions
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(x => x.Friend)
            .ProjectTo<FriendDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        friends.ForEach(item => {
            item.Avatar = _fileService.FormatPublicFile(item.Avatar);
            item.IsOnline = _onlineManager.CheckOnline(item.Id);
        });

        return friends;
    }

    public async Task SetAliasAsync(int userId, int friendId, string? name, CancellationToken cancellationToken = default)
    {
        var isFriend = await _userRepository.IsFriendAsync(userId, friendId, cancellationToken);
        if (!isFriend) throw Oops.Bah("").StatusCode(HttpStatusCode.Forbidden);
        await _userRepository.SetAliasAsync(userId, friendId, name, cancellationToken);
    }
}
