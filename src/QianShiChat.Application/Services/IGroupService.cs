namespace QianShiChat.Application.Services;

public interface IGroupService
{
    Task ApplyJoin(int userId, int id, JoinGroupRequest request, CancellationToken cancellationToken = default);
    Task<GroupDto> Create(int userId, string name, CancellationToken cancellationToken = default);
    Task<GroupDto> CreateByFriendAsync(int userId, CreateGroupRequest request, CancellationToken cancellationToken = default);
    Task Delete(int userId, int id, CancellationToken cancellationToken = default);
    Task<List<GroupDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task Leave(int userId, int id, CancellationToken cancellationToken = default);
}