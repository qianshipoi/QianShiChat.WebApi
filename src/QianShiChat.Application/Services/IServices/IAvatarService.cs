namespace QianShiChat.Application.Services.IServices;

public interface IAvatarService
{
    Task<string?> GetDefaultAvatarByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PagedList<AvatarDto>> GetDefaultAvatarsAsync(QueryUserAvatarRequest query, CancellationToken cancellationToken = default);

    Task<PagedList<AvatarDto>> GetUserAvatarsAsync(int userId, QueryUserAvatarRequest query, CancellationToken cancellationToken = default);

    Task<(bool success, string urlOrErrorMsg)> UploadAvatarAsync(int userId, IFormFile file, CancellationToken cancellationToken = default);
}