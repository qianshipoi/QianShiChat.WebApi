namespace QianShiChat.Application.Services;

/// <summary>
/// firend apply service.
/// </summary>
public class FriendApplyService : IFriendApplyService, ITransient
{
    private readonly ILogger<FriendApplyService> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IUserManager _userManager;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IFriendGroupRepository _friendGroupRepository;
    private readonly IChatService _chatService;

    /// <summary>
    /// firend apply service.
    /// </summary>
    public FriendApplyService(
        ILogger<FriendApplyService> logger,
        IApplicationDbContext context,
        IMapper mapper,
        IFileService fileService,
        IUserManager userManager,
        IHubContext<ChatHub, IChatClient> hubContext,
        IFriendGroupRepository friendGroupRepository,
        IChatService chatService)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
        _userManager = userManager;
        _hubContext = hubContext;
        _friendGroupRepository = friendGroupRepository;
        _chatService = chatService;
    }

    public async Task<bool> IsApplyAsync(int userId, int friendId, CancellationToken cancellationToken = default)
    {
        return await _context.FriendApplies
            .Where(x => x.UserId == userId && x.FriendId == friendId && x.Status == ApplyStatus.Applied)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsApprovalAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.FriendApplies
            .Where(x => x.Id == id && x.Status != ApplyStatus.Applied)
            .AnyAsync(cancellationToken);
    }

    public async Task<FriendApplyDto> ApplyAsync(int userId, CreateFriendApplyRequest dto, CancellationToken cancellationToken = default)
    {
        var apply = _mapper.Map<FriendApply>(dto);
        apply.UserId = userId;
        await _context.FriendApplies.AddAsync(apply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<FriendApplyDto>(apply);
    }

    public async Task<FriendApplyDto> UpdateApplyAsync(int userId, CreateFriendApplyRequest dto, CancellationToken cancellationToken = default)
    {
        var apply = await _context.FriendApplies
            .Where(x => x.UserId == userId && x.FriendId == dto.UserId && x.Status == ApplyStatus.Applied)
            .SingleAsync(cancellationToken);

        apply.Remark = dto.Remark ?? "";
        apply.UpdateTime = Timestamp.Now;
        apply.FriendGroupId = dto.FriendGroupId;
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FriendApplyDto>(apply);
    }

    public async Task<List<FriendApplyDto>> GetPendingListByUserAsync(int size, int userId, long beforeTime = 0, CancellationToken cancellationToken = default)
    {
        var data = await _context.FriendApplies
              .AsNoTracking()
              .Where(x => x.UserId == userId)
              .Where(x => x.CreateTime > beforeTime)
              .Include(x => x.User)
              .Include(x => x.Friend)
              .OrderByDescending(x => x.UpdateTime)
              .ThenByDescending(x => x.Id)
              .Take(size)
              .ProjectTo<FriendApplyDto>(_mapper.ConfigurationProvider)
              .ToListAsync(cancellationToken);
        data.ForEach(item => {
            if (item.User != null)
                item.User.Avatar = _fileService.FormatPublicFile(item.User.Avatar);
            if (item.Friend != null)
                item.Friend.Avatar = _fileService.FormatPublicFile(item.Friend.Avatar);
        });

        return data;
    }

    public async Task ClearApplyAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        var applies = await _context.FriendApplies
            .Where(x => x.UserId == _userManager.CurrentUserId)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        _context.FriendApplies.RemoveRange(applies);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearAllApplyAsync(CancellationToken cancellationToken = default)
    {
        var applies = await _context.FriendApplies
            .Where(x => x.UserId == _userManager.CurrentUserId)
            .ToListAsync(cancellationToken);
        _context.FriendApplies.RemoveRange(applies);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var apply = await _context.FriendApplies
            .Where(x => x.Id == id)
            .Where(x => x.UserId == _userManager.CurrentUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (apply is null) return;

        _context.FriendApplies.RemoveRange(apply);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<long> GetPendingListCountByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.FriendApplies
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UpdateTime)
            .CountAsync(cancellationToken);
    }

    private async Task<FriendGroup> GetUserFriendGroupAsync(int userId, CancellationToken cancellationToken = default)
    {
        var group = await _friendGroupRepository.FindUserDefaultGroupByIdAsync(userId, cancellationToken);

        if (group == null)
        {
            group = new FriendGroup
            {
                IsDefault = true,
                Name = "好友",
                UserId = userId,
                Sort = 9999,
                CreateTime = Timestamp.Now,
            };

            _context.FriendGroups.Add(group);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return group;
    }

    private async Task<int> GetFriendGroupId(int userId, int? friendGroupId)
    {
        if (friendGroupId.HasValue && friendGroupId.Value > 0)
        {
            var result = await _friendGroupRepository.ExistsAsync(userId, friendGroupId.Value);

            if (result)
                return friendGroupId.Value;
        }
        var group = GetUserFriendGroupAsync(userId);
        return group.Id;
    }

    public async Task<FriendApplyDto> ApprovalAsync(int userId, ApprovalRequest request, CancellationToken cancellationToken = default)
    {
        var apply = await _context.FriendApplies
            .Include(x => x.User)
            .Include(x => x.Friend)
            .Where(x => x.UserId == userId && x.Id == request.Id && x.Status == ApplyStatus.Applied)
            .FirstAsync(cancellationToken);

        apply.Status = request.Status;
        apply.UpdateTime = Timestamp.Now;

        if (request.Status == ApplyStatus.Passed
            && !await _context.UserRealtions.AnyAsync(x => x.UserId == apply.UserId && x.FriendId == apply.FriendId, cancellationToken))
        {
            var friendGroupId = await GetFriendGroupId(apply.FriendId, apply.FriendGroupId);
            _context.UserRealtions.Add(new UserRealtion
            {
                UserId = apply.FriendId,
                FriendId = apply.UserId,
                CreateTime = apply.UpdateTime,
                FriendGroupId = friendGroupId
            });
            var currentGroupId = await GetFriendGroupId(apply.UserId, request.FriendGroupId);
            _context.UserRealtions.Add(new UserRealtion
            {
                UserId = apply.UserId,
                FriendId = apply.FriendId,
                CreateTime = apply.UpdateTime,
                FriendGroupId = currentGroupId
            });
        }

        if (request.Status == ApplyStatus.Passed)
        {
            var roomId = AppConsts.GetPrivateChatRoomId(apply.UserId, apply.FriendId);
            var messagePosition = YitIdHelper.NextId();

            if (!await _context.Rooms.AnyAsync(x => x.Id == roomId && x.FromId == apply.UserId))
            {
                _context.Rooms.Add(new Room
                {
                    CreateTime = apply.UpdateTime,
                    FromId = apply.UserId,
                    ToId = apply.FriendId,
                    Type = ChatMessageSendType.Personal,
                    Id = roomId,
                    MessagePosition = messagePosition,
                    UpdateTime = apply.UpdateTime
                });
            }
            if (!await _context.Rooms.AnyAsync(x => x.Id == roomId && x.FromId == apply.FriendId))
            {
                _context.Rooms.Add(new Room
                {
                    CreateTime = apply.UpdateTime,
                    FromId = apply.FriendId,
                    ToId = apply.UserId,
                    Type = ChatMessageSendType.Personal,
                    Id = roomId,
                    MessagePosition = messagePosition,
                    UpdateTime = apply.UpdateTime
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (request.Status == ApplyStatus.Passed)
        {
            // send new friend notification
            var userDto = _mapper.Map<UserDto>(apply.User);
            var friendDto = _mapper.Map<UserDto>(apply.Friend);
            _fileService.FormatAvatar(userDto);
            _fileService.FormatAvatar(friendDto);

            _ = _chatService.SendNewFriendNotifyAsync(new int[] { userDto.Id, friendDto.Id });
        }

        return _mapper.Map<FriendApplyDto>(apply);
    }
}