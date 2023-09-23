namespace QianShiChat.Application.Services;

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

    public UserInfo GetCurrentUser()
    {
        if (_currentUserInfo is null)
        {
            var user = _chatDbContext.UserInfos.Find(CurrentUserId);

            if (user is null)
            {
                throw Oops.Bah("").StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
            _currentUserInfo = user;
        }
        return _currentUserInfo;
    }
}
