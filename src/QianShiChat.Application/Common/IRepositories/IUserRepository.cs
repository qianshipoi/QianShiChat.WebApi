
namespace QianShiChat.Application.Common.IRepositories;

public interface IUserRepository
{
    Task<bool> IsFriendAsync(int id1, int id2, CancellationToken cancellationToken = default);

    Task<string?> GetNicknameAsync(int userId, CancellationToken cancellationToken = default);

    Task<UserInfo?> FindByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> SetAliasAsync(int userId, int friendId, string? name, CancellationToken cancellationToken = default);
    Task<List<UserInfo>> FindByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
