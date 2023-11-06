namespace QianShiChat.Application.Services;

/// <summary>
/// user avatar service.
/// </summary>
public class AvatarService : IAvatarService, ITransient
{
    private readonly ILogger<AvatarService> _logger;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;

    public AvatarService(
        IApplicationDbContext context,
        ILogger<AvatarService> logger,
        IMapper mapper,
        IFileService fileService)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<PagedList<AvatarDto>> GetUserAvatarsAsync(int userId, QueryUserAvatarRequest query, CancellationToken cancellationToken = default)
    {
        var data = await _context.UserAvatars
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Where(x => x.UserId == userId)
            .IfWhere(() => query.BeforeId.HasValue && query.BeforeId > 0, x => x.Id < query.BeforeId!.Value)
            .OrderByDescending(x => x.CreateTime)
            .Take(query.Count + 1)
            .ProjectTo<AvatarDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        data.ForEach(item =>
        {
            item.Path = _fileService.FormatPublicFile(item.Path);
        });

        return PagedList.Create(data, query.Count);
    }

    public async Task<PagedList<AvatarDto>> GetDefaultAvatarsAsync(QueryUserAvatarRequest query, CancellationToken cancellationToken = default)
    {
        var data = await _context.DefaultAvatars
           .AsNoTracking()
           .Where(x => !x.IsDeleted)
           .IfWhere(() => query.BeforeId.HasValue && query.BeforeId > 0, x => x.Id < query.BeforeId!.Value)
           .OrderBy(x => x.CreateTime)
           .Take(query.Count + 1)
           .ProjectTo<AvatarDto>(_mapper.ConfigurationProvider)
           .ToListAsync(cancellationToken);

        data.ForEach(item =>
        {
            item.Path = _fileService.FormatPublicFile(item.Path);
        });

        return PagedList.Create(data, query.Count);
    }

    public Task<string?> GetDefaultAvatarByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.DefaultAvatars
            .Where(x => !x.IsDeleted)
            .Where(x => x.Id == id)
            .Select(x => x.Path)
            .FirstOrDefaultAsync(cancellationToken);
    }
}