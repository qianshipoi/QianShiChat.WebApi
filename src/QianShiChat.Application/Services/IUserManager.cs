namespace QianShiChat.Application.Services;

public interface IUserManager
{
    int CurrentUserId { get; }
    string CurrentClientType { get; }
    bool IsLogin { get; }

    Task ChangePasswordAsync(string oldPasswrod, string newPassword, CancellationToken cancellationToken = default);
    UserInfo GetCurrentUser();
    Task SetDescriptionAsync(string? description, CancellationToken cancellationToken = default);
    Task SetNicknameAsync(string name, CancellationToken cancellationToken = default);
}
