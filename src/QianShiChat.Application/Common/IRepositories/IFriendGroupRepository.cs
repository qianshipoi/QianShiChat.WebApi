namespace QianShiChat.Application.Common.IRepositories;

public interface IFriendGroupRepository
{
    Task<bool> ExistsAsync(int userId, int id, CancellationToken cancellationToken = default);
    Task<FriendGroup?> FindUserDefaultGroupByIdAsync(int userId, CancellationToken cancellationToken = default);
}
