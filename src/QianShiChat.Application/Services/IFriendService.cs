namespace QianShiChat.Application.Services;

/// <summary>
/// friend service.
/// </summary>
public interface IFriendService
{
    /// <summary>
    /// 获取所有好友编号
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<int>> GetFriendIdsAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取所有好友
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<UserDto>> GetFriendsAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 判断是否为好友
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsFriendAsync(int userId, int friendId, CancellationToken cancellationToken = default);
}