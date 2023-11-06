namespace QianShiChat.Application.Services;

/// <summary>
/// user service.
/// </summary>
public class UserService : IUserService, ITransient
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly string _avatarPath;

    public UserService(
        IApplicationDbContext context,
        IMapper mapper,
        ILogger<UserService> logger,
        IWebHostEnvironment webHostEnvironment,
        IFileService fileService,
        IAttachmentRepository attachmentRepository,
        IUserRepository userRepository)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _fileService = fileService;
        _attachmentRepository = attachmentRepository;
        _avatarPath = Path.Combine(webHostEnvironment.WebRootPath, AppConsts.AvatarPrefix);
        if (!Directory.Exists(_avatarPath)) Directory.CreateDirectory(_avatarPath);
        _userRepository = userRepository;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _context.UserInfos
            .AsNoTracking()
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (user == null) return user;

        user.Avatar = _fileService.FormatPublicFile(user.Avatar);

        return user;
    }

    public async Task<UserInfo?> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default)
    {
        var user = await _context.UserInfos
            .Where(x => x.Account == account)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null) return user;

        user.Avatar = _fileService.FormatPublicFile(user.Avatar!);

        return user;
    }

    public async Task<bool> AccountExistsAsync(string account, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos
             .AsNoTracking()
             .AnyAsync(x => x.Account.Equals(account), cancellationToken);
    }

    public async Task<UserDto> AddAsync(
        CreateUserRequest dto,
        string avatarPath,
        CancellationToken cancellationToken = default)
    {
        var uuid = YitIdHelper.NextId();
        var user = _mapper.Map<UserInfo>(dto);
        user.Password = user.Password.ToMd5();

        // save avatar.
        var defaultAvatarPath = Path.Combine(_webHostEnvironment.WebRootPath, avatarPath.Trim('/').Trim('\\'));
        var newPath = Path.Combine(AppConsts.AvatarPrefix, uuid + Path.GetExtension(avatarPath));
        var newAvatarPath = Path.Combine(_webHostEnvironment.WebRootPath, newPath.Trim('/').Trim('\\'));
        try
        {
            File.Copy(defaultAvatarPath, newAvatarPath);
            user.Avatar = newPath.Replace('\\', '/');
            // add default friend group;
            var friendGroup = new FriendGroup
            {
                Name = "My friend",
                CreateTime = Timestamp.Now,
                IsDefault = true,
                Sort = 999,
            };
            user.FriendGroups.Add(friendGroup);
            await _context.UserInfos.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            user.Avatar = _fileService.FormatPublicFile(user.Avatar);
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

        users.ForEach(item => {
            item.Avatar = _fileService.FormatPublicFile(item.Avatar);
        });
        return users;
    }

    public async Task<PagedList<UserDto>> SearchUsersAsync(UserSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.UserInfos
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x => EF.Functions.Like(x.NickName, $"%{request.Search}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.CreateTime)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        items.ForEach(_fileService.FormatAvatar);
        return PagedList.Create(items, totalCount, request.Size);
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
        users.ForEach(_fileService.FormatAvatar);
        return users;
    }

    public async Task<long> GetUserCountByNickNameAsync(string nickName, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos.AsNoTracking()
                .Where(x => EF.Functions.Like(x.NickName, $"%{nickName}%"))
                .CountAsync(cancellationToken);
    }

    public async Task<string?> GetNickNameByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserInfos.Where(x => x.Id == userId).Select(x => x.NickName).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsFriendAsync(int userId, int friendId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRealtions.AnyAsync(x => x.UserId == userId && x.FriendId == friendId, cancellationToken);
    }

    public async Task ChangeAvatarAsync(int userId, int attachmentId, CancellationToken cancellationToken = default)
    {
        var uuid = YitIdHelper.NextId();

        var attachment = await _attachmentRepository.FindByIdAsync(attachmentId, cancellationToken);
        if (attachment is null) throw Oops.Bah("attachment not found.");

        if (attachment.Size > AppConsts.MaxAvatarLength)
        {
            throw Oops.Bah(string.Format("avatar max lenght [{0}]MB.", AppConsts.MaxAvatarLength / 1024 / 1024));
        }

        var ext = Path.GetExtension(attachment.Name);
        if (string.IsNullOrEmpty(ext)
            || !AppConsts.AllowAvatarExts.Any(x => string.Compare(x, ext, true) == 0))
        {
            throw Oops.Bah(string.Format("The avatars are in [{0}] format only", string.Join(',', AppConsts.AllowAvatarExts)));
        }

        var user = await _userRepository.FindByIdAsync(userId, cancellationToken)!;

        // save avatar.
        var newFilePath = Path.Combine(AppConsts.AvatarPrefix, uuid + ext);
        var saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, newFilePath);
        var attachmentPath = Path.Combine(_webHostEnvironment.WebRootPath, attachment.RawPath.Trim('/').Trim('\\'));

        File.Copy(attachmentPath, destFileName: saveFilePath);

        var avatar = new UserAvatar()
        {
            CreateTime = Timestamp.Now,
            UserId = userId,
            Path = '/' + newFilePath.Replace('\\', '/'),
            Size = (ulong)attachment.Size,
        };

        try
        {
            user!.UserAvatars.Add(avatar);
            user.Avatar = avatar.Path;
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            if (File.Exists(saveFilePath)) { File.Delete(saveFilePath); }
            throw Oops.Oh("server error.");
        }
    }
}