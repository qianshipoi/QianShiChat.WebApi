namespace QianShiChat.WebApi.Services;

public interface IAvatarService
{
    Task<string?> GetDefaultAvatarByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedList<AvatarDto>> GetDefaultAvatarsAsync(QueryUserAvatar query, CancellationToken cancellationToken = default);

    Task<PagedList<AvatarDto>> GetUserAvatarsAsync(int userId, QueryUserAvatar query, CancellationToken cancellationToken = default);

    Task<(bool success, string urlOrErrorMsg)> UploadAvatarAsync(int userId, IFormFile file, CancellationToken cancellationToken = default);
}