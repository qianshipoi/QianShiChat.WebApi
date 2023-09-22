namespace QianShiChat.Application.Services;

public interface IUserManager
{
    int CurrentUserId { get; }
    bool IsLogin { get; }

    UserInfo GetCurrentUser();
}
