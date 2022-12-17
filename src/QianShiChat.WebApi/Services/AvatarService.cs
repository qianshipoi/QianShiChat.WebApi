namespace QianShiChat.WebApi.Services;

/// <summary>
/// user avatar service.
/// </summary>
public class AvatarService : IAvatarService, ITransient
{
    readonly ILogger<AvatarService> _logger;
    readonly IMapper _mapper;
    readonly ChatDbContext _context;
    readonly string _avatarPath;
    readonly IWebHostEnvironment _webHostEnvironment;

    const long AvatarMaxLenght = 1024 * 1024 * 4;
    const string AvatarPrefix = "/Raw/Avatar";

    static string[] AllowAvatarExts = new string[] { ".png", ".jpg", ".jpeg", ".gif" };

    public AvatarService(
        ChatDbContext context,
        ILogger<AvatarService> logger,
        IMapper mapper,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
        _avatarPath = Path.Combine(webHostEnvironment.WebRootPath, AvatarPrefix.Trim(new char[] { '/', '\\' }));
        if (!Directory.Exists(_avatarPath))
        {
            Directory.CreateDirectory(_avatarPath);
        }
    }

    public async Task<PagedList<AvatarDto>> GetUserAvatarsAsync(int userId, QueryUserAvatar query, CancellationToken cancellationToken = default)
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

        return PagedList.Create(data, query.Count);
    }

    public async Task<PagedList<AvatarDto>> GetDefaultAvatarsAsync(QueryUserAvatar query, CancellationToken cancellationToken = default)
    {
        var data = await _context.DefaultAvatars
           .AsNoTracking()
           .Where(x => !x.IsDeleted)
           .IfWhere(() => query.BeforeId.HasValue && query.BeforeId > 0, x => x.Id < query.BeforeId!.Value)
           .OrderBy(x => x.CreateTime)
           .Take(query.Count + 1)
           .ProjectTo<AvatarDto>(_mapper.ConfigurationProvider)
           .ToListAsync(cancellationToken);

        return PagedList.Create(data, query.Count);
    }

    public async Task<(bool success, string urlOrErrorMsg)> UploadAvatarAsync(int userId, IFormFile file, CancellationToken cancellationToken = default)
    {
        var uuid = YitIdHelper.NextId();
        if (file == null || file.Length < 1)
        {
            return (false, "file error.");
        }
        if (file.Length > AvatarMaxLenght)
        {
            return (false, string.Format("avatar max lenght [{0}]MB.", AvatarMaxLenght / 1024 / 1024));
        }

        var ext = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(ext)
            || !AllowAvatarExts.Any(x => string.Compare(x, ext, true) == 0))
        {
            return (false, string.Format("The avatars are in [{0}] format only", string.Join(',', AllowAvatarExts)));
        }

        var user = await _context.UserInfos.FindAsync(new object[] { userId }, cancellationToken);
        if (user == null)
        {
            return (false, "user not found.");
        }

        // save avatar.
        var newFilePath = Path.Combine(AvatarPrefix.Trim(new char[] { '/', '\\' }), uuid + ext);
        var saveFilePath = Path.Combine(_webHostEnvironment.WebRootPath, newFilePath);
        using var stream = file.OpenReadStream();
        using var fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.Write);
        stream.CopyTo(fileStream);
        await fileStream.FlushAsync();

        var avatar = new UserAvatar()
        {
            CreateTime = Timestamp.Now,
            UserId = userId,
            Path = '/' + newFilePath.Replace('\\', '/'),
            Size = (ulong)file.Length,
        };

        try
        {
            user.UserAvatars.Add(avatar);
            user.Avatar = avatar.Path;
            await _context.SaveChangesAsync(cancellationToken);
            return (true, user.Avatar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            if (File.Exists(saveFilePath)) { File.Delete(saveFilePath); }
            return (true, "server error.");
        }
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