namespace QianShiChat.Application.Services;

public interface IGroupService
{
    Task ApplyAsync(int id, int userId, GroupApplyRequest request, CancellationToken cancellationToken = default);
    Task ApprovalAync(int applyId, int userId, ApplyStatus status, CancellationToken cancellationToken = default);
    Task<GroupDto> CreateAsync(int userId, string name, CancellationToken cancellationToken = default);
    Task<GroupDto> CreateByFriendAsync(int userId, CreateGroupRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int userId, int id, CancellationToken cancellationToken = default);
    Task<GroupDto?> FindByIdAsync(int groupId, CancellationToken cancellationToken = default);
    Task<List<GroupDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<PagedList<GroupApplyDto>> GetApplyPendingAsync(int userId, QueryGroupApplyPendingRequest request, CancellationToken cancellationToken = default);
    Task<PagedList<UserDto>> GetMembersByGroupAsync(int groupId, GroupMemberQueryRequest request, CancellationToken cancellationToken = default);
    Task LeaveAsync(int userId, int id, CancellationToken cancellationToken = default);
    Task<PagedList<GroupDto>> SearchGroupAsync(GroupSearchRequest request, CancellationToken cancellationToken = default);
}