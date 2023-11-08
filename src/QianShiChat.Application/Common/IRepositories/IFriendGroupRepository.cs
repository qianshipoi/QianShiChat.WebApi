namespace QianShiChat.Application.Common.IRepositories;

public interface IFriendGroupRepository
{
    Task<FriendGroup?> FindUserDefaultGroupByIdAsync(int userId, CancellationToken cancellationToken = default);
}
