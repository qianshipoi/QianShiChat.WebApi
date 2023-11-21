namespace QianShiChat.Application.Services.IServices;

/// <summary>
/// friend apply service.
/// </summary>
public interface IFriendApplyService
{
    /// <summary>
    /// 是否已申请
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="friendId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsApplyAsync(int userId, int friendId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 是否已审批
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsApprovalAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 申请
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FriendApplyDto> ApplyAsync(int userId, CreateFriendApplyRequest dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新申请时间
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FriendApplyDto> UpdateApplyAsync(int userId, CreateFriendApplyRequest dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取申请列表数量
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetPendingListCountByUserAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取申请列表
    /// </summary>
    /// <param name="size"></param>
    /// <param name="userId"></param>
    /// <param name="beforeTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<FriendApplyDto>> GetPendingListByUserAsync(int size, int userId, long beforeTime = 0, CancellationToken cancellationToken = default);
    /// <summary>
    /// 审批
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FriendApplyDto> ApprovalAsync(int userId, ApprovalRequest request, CancellationToken cancellationToken = default);
    Task ClearApplyAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    Task RemoveByIdAsync(int id, CancellationToken cancellationToken = default);
    Task ClearAllApplyAsync(CancellationToken cancellationToken = default);
}