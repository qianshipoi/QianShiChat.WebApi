namespace QianShiChat.Application.Services.IServices;

public interface IFriendGroupService
{
    Task<FriendGroupDto> AddAsync(int userId, string? name, CancellationToken cancellationToken = default);
    Task DeleteAsync(int userId, int groupId, CancellationToken cancellationToken = default);
    Task<List<FriendGroupDto>> GetGroupsAsync(int userId, CancellationToken cancellationToken = default);
    Task MoveToAsync(int userId, int friendId, MoveToGroupRequest request, CancellationToken cancellationToken = default);
    Task SortAsync(int userId, SortRequest request, CancellationToken cancellationToken = default);
    Task<FriendGroupDto> UpdateAsync(int userId, int groupId, string? name, CancellationToken cancellationToken = default);
}