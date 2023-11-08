namespace QianShiChat.Application.Managers;

public class UserManager : IUserManager, IScoped
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IApplicationDbContext _chatDbContext;
    private int _userId;
    private UserInfo? _currentUserInfo;

    public UserManager(IHttpContextAccessor contextAccessor, IApplicationDbContext chatDbContext)
    {
        _contextAccessor = contextAccessor;
        _chatDbContext = chatDbContext;
    }

    public async Task SetDescriptionAsync(string? description, CancellationToken cancellationToken = default)
    {
        var user = GetCurrentUser();
        user.Description = description;
        await _chatDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SetNicknameAsync(string name, CancellationToken cancellationToken = default)
    {
        var user = GetCurrentUser();
        user.NickName = name;
        await _chatDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangePasswordAsync(string oldPasswrod, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = GetCurrentUser();
        if (user.Password != oldPasswrod)
        {
            throw Oops.Bah("old password is wrong.");
        }
        user.Password = newPassword;
        await _chatDbContext.SaveChangesAsync(cancellationToken);
    }

    public string CurrentClientType
    {
        get
        {
            var val = _contextAccessor.HttpContext!.User.Claims?.FirstOrDefault(x => x.Type == CustomClaim.ClientType)?.Value;
            if (string.IsNullOrWhiteSpace(val))
            {
                throw Oops.Bah(string.Empty).StatusCode(HttpStatusCode.Unauthorized);
            }
            return val;
        }
    }

    public int CurrentUserId
    {
        get
        {
            if (_userId < 1)
            {
                var val = _contextAccessor.HttpContext!.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(val))
                {
                    throw Oops.Bah(string.Empty).StatusCode(HttpStatusCode.Unauthorized);
                }
                _userId = int.Parse(val);
            }

            return _userId;
        }
    }

    public bool IsLogin => _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public UserInfo GetCurrentUser()
    {
        if (_currentUserInfo is null)
        {
            var user = _chatDbContext.UserInfos.Find(CurrentUserId);
            if (user is null)
            {
                throw Oops.Bah("").StatusCode(HttpStatusCode.Unauthorized);
            }
            _currentUserInfo = user;
        }
        return _currentUserInfo;
    }
}
