namespace QianShiChat.Application.Services;

/// <summary>
/// firend apply service.
/// </summary>
public class FriendApplyService : IFriendApplyService, ITransient
{
    private readonly ILogger<FriendApplyService> _logger;
    private readonly ChatDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    /// <summary>
    /// firend apply service.
    /// </summary>
    public FriendApplyService(ILogger<FriendApplyService> logger, ChatDbContext context, IMapper mapper, IFileService fileService)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
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

    public async Task<FriendApplyDto> ApplyAsync(int userId, CreateFriendApplyDto dto, CancellationToken cancellationToken = default)
    {
        var apply = _mapper.Map<FriendApply>(dto);
        apply.UserId = userId;
        await _context.AddAsync(apply, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<FriendApplyDto>(apply);
    }

    public async Task<FriendApplyDto> UpdateApplyAsync(int userId, CreateFriendApplyDto dto, CancellationToken cancellationToken = default)
    {
        var apply = await _context.FriendApplies
            .Where(x => x.UserId == userId && x.FriendId == dto.UserId && x.Status == ApplyStatus.Applied)
            .SingleAsync(cancellationToken);

        apply.Remark = dto.Remark ?? "";
        apply.UpdateTime = Timestamp.Now;
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

    public async Task<long> GetPendingListCountByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.FriendApplies
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UpdateTime)
            .CountAsync(cancellationToken);
    }

    public async Task<FriendApplyDto> ApprovalAsync(int userId, int id, ApplyStatus status, CancellationToken cancellationToken = default)
    {
        var apply = await _context.FriendApplies
            .Include(x => x.User)
            .Include(x => x.Friend)
            .Where(x => x.UserId == userId && x.Id == id && x.Status == ApplyStatus.Applied)
            .FirstAsync(cancellationToken);

        apply.Status = status;
        apply.UpdateTime = Timestamp.Now;

        if (status == ApplyStatus.Passed
            && !await _context.UserRealtions.AnyAsync(x => x.UserId == apply.UserId && x.FriendId == apply.FriendId, cancellationToken))
        {
            _context.UserRealtions.Add(new UserRealtion
            {
                UserId = apply.FriendId,
                FriendId = apply.UserId,
                CreateTime = apply.UpdateTime
            });

            _context.UserRealtions.Add(new UserRealtion
            {
                UserId = apply.UserId,
                FriendId = apply.FriendId,
                CreateTime = apply.UpdateTime
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FriendApplyDto>(apply);
    }
}