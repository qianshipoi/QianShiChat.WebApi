namespace QianShiChat.Application.Services;

/// <summary>
/// user service.
/// </summary>
public class UserService : IUserService, ITransient
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly ChatDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;

    private const string AvatarPrefix = "/Raw/Avatar";

    public UserService(
        ChatDbContext context,
        IMapper mapper,
        ILogger<UserService> logger,
        IWebHostEnvironment webHostEnvironment,
        IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        var avatarPath = Path.Combine(webHostEnvironment.WebRootPath, AvatarPrefix.Trim(new char[] { '/', '\\' }));
        if (!Directory.Exists(avatarPath))
        {
            Directory.CreateDirectory(avatarPath);
        }
        _fileService = fileService;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.UserInfos
            .AsNoTracking()
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user == null) return user;

        user.Avatar = _fileService.FormatWwwRootFile(user.Avatar);

        return user;
    }

    public async Task<UserInfo?> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default)
    {
        var user = await _context.UserInfos
            .Where(x => x.Account == account)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null) return user;

        user.Avatar = _fileService.FormatWwwRootFile(user.Avatar!);

        return user;
    }

    public async Task<bool> AccountExistsAsync(string account, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos
             .AsNoTracking()
             .AnyAsync(x => x.Account.Equals(account), cancellationToken);
    }

    public async Task<UserDto> AddAsync(
        CreateUserDto dto,
        string avatarPath,
        CancellationToken cancellationToken = default)
    {
        var uuid = YitIdHelper.NextId();
        var user = _mapper.Map<UserInfo>(dto);
        user.Password = user.Password.ToMd5();

        // save avatar.
        var defaultAvatarPath = Path.Combine(_webHostEnvironment.WebRootPath, avatarPath.Trim('/').Trim('\\'));
        var newPath = Path.Combine(AvatarPrefix, uuid + Path.GetExtension(avatarPath));
        var newAvatarPath = Path.Combine(_webHostEnvironment.WebRootPath, newPath.Trim('/').Trim('\\'));
        try
        {
            File.Copy(defaultAvatarPath, newAvatarPath);
            user.Avatar = newPath.Replace('\\', '/');
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            user.Avatar = _fileService.FormatWwwRootFile(user.Avatar);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            if (File.Exists(newAvatarPath)) File.Delete(newAvatarPath);
            throw Oops.Oh("create user error.");
        }
    }

    public async Task<List<UserDto>> GetUserByAccontAsync(string account, CancellationToken cancellationToken = default)
    {
        var users = await _context.UserInfos.AsNoTracking()
            .Where(x => x.Account == account)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        users.ForEach(item =>
        {
            item.Avatar = _fileService.FormatWwwRootFile(item.Avatar);
        });
        return users;
    }

    public async Task<List<UserDto>> GetUserByNickNameAsync(int page, int size, string nickName, CancellationToken cancellationToken = default)
    {
        var users = await _context.UserInfos.AsNoTracking()
            .Where(x => EF.Functions.Like(x.NickName, $"%{nickName}%"))
            .OrderBy(x => x.CreateTime)
            .Skip((page - 1) * size)
            .Take(size)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        users.ForEach(item =>
        {
            item.Avatar = _fileService.FormatWwwRootFile(item.Avatar);
        });
        return users;
    }

    public async Task<long> GetUserCountByNickNameAsync(string nickName, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos.AsNoTracking()
                .Where(x => EF.Functions.Like(x.NickName, $"%{nickName}%"))
                .CountAsync(cancellationToken);
    }

    public async Task<string?> GetNickNameByIdAsync(int userId, CancellationToken cancellationToken= default)
    {
        return await _context.UserInfos.Where(x => x.Id == userId).Select(x => x.NickName).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsFriendAsync(int userId, int friendId,CancellationToken cancellationToken= default)
    {
        return await _context.UserRealtions.AnyAsync(x => x.UserId == userId && x.FriendId == friendId, cancellationToken);
    }
}