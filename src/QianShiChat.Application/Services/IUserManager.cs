namespace QianShiChat.Application.Services;

public interface IUserManager
{
    int CurrentUserId { get; }

    UserInfo GetCurrentUser();
}
