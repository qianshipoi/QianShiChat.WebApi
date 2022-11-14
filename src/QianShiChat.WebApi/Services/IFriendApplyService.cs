using QianShiChat.Models;

namespace QianShiChat.WebApi.Services
{
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
        Task<FriendApplyDto> ApplyAsync(int userId, CreateFriendApplyDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新申请时间
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FriendApplyDto> UpdateApplyAsync(int userId, CreateFriendApplyDto dto, CancellationToken cancellationToken = default);

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
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<FriendApplyDto>> GetPendingListByUserAsync(int page, int size, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FriendApplyDto> ApprovalAsync(int userId, int id, ApplyStatus status, CancellationToken cancellationToken = default);
    }
}
