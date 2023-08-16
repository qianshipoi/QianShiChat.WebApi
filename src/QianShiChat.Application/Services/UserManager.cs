namespace QianShiChat.Application.Services;

public class UserManager : IUserManager, IScoped
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ChatDbContext _chatDbContext;
    private int _userId;
    private UserInfo? _currentUserInfo;

    public UserManager(IHttpContextAccessor contextAccessor, ChatDbContext chatDbContext)
    {
        _contextAccessor = contextAccessor;
        _chatDbContext = chatDbContext;
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
                    throw Oops.Bah("").StatusCode(System.Net.HttpStatusCode.Unauthorized);
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
