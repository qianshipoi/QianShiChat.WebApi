namespace QianShiChat.Application.Services;

public interface IUserManager
{
    int CurrentUserId { get; }
    string CurrentClientType { get; }

    UserInfo GetCurrentUser();
}
