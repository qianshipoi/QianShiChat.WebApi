namespace QianShiChat.Application.Common.IRepositories;

public interface IGroupRepository
{
    Task TotalUserIncrementAsync(int groupId, CancellationToken cancellationToken = default);

    Task TotalUserDecrementAsync(int groupId, CancellationToken cancellationToken = default);
    Task<Group?> FindByIdAsync(int groupId, CancellationToken cancellationToken = default);
    Task<int> UpdateGroupNameAsync(int groupId, string name, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int userId, int groupId, CancellationToken cancellationToken = default);
    Task<bool> IsJoinGroupAsync(int userId, int groupId, CancellationToken cancellationToken = default);
    Task<int> SetAliasAsync(int userId, int groupId, string? name, CancellationToken cancellationToken = default);
    Task<int> UpdateGroupAvatarAsync(int groupId, string avatarPath, CancellationToken cancellationToken = default);
}
