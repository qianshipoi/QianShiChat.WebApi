namespace QianShiChat.Application.Services;

public class OnlineManager : IOnlineManager, ISingleton
{
    private readonly static ConnectionMapping<int> _connections =
        new ConnectionMapping<int>();

    private readonly IRedisCachingProvider _redisCachingProvider;

    public OnlineManager(IRedisCachingProvider redisCachingProvider)
    {
        _redisCachingProvider = redisCachingProvider;
    }

    private static string FormatOnlineUserKey(string userId, string clientType) => $"{userId}_{clientType}";

    public async Task ConnectedAsync(int id, string connectionId, string clientType)
    {
        var cacheKey = FormatOnlineUserKey(id.ToString(), clientType);
        await _redisCachingProvider.HSetAsync(AppConsts.OnlineCacheKey, cacheKey, connectionId);
        Add(id, connectionId);
    }

    public async Task DisconnectedAsync(int id, string connectionId, string clientType)
    {
        var cacheKey = FormatOnlineUserKey(id.ToString(), clientType);
        await _redisCachingProvider.HSetAsync(AppConsts.OnlineCacheKey, cacheKey, connectionId);
        Remove(id, connectionId);
    }

    public async Task<bool> UserClientIsOnlineAsync(int id, string clientType)
    {
        var cacheKey = FormatOnlineUserKey(id.ToString(), clientType);
        return await _redisCachingProvider.HExistsAsync(AppConsts.OnlineCacheKey, cacheKey);
    }

    public async Task<string> GetUserClientConnectionIdAsync(int id, string clientType)
    {
        var cacheKey = FormatOnlineUserKey(id.ToString(), clientType);
        return await _redisCachingProvider.HGetAsync(AppConsts.OnlineCacheKey, cacheKey);
    }

    public void Add(int id, string connectionId) => _connections.Add(id, connectionId);

    public void Remove(int id, string connectionId) => _connections.Remove(id, connectionId);

    public bool CheckOnline(int id) => _connections.ContainsKey(id);

    public IEnumerable<string> GetConnections(int id) => _connections.GetConnections(id);
}
