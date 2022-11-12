namespace QianShiChat.WebApi.Services
{
    public interface IFirendService
    {
        /// <summary>
        /// 获取所有好友编号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<int>> GetFirendIdsAsync(int userId, CancellationToken cancellationToken = default);
    }
}
