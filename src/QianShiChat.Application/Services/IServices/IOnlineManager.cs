namespace QianShiChat.Application.Services.IServices;

public interface IOnlineManager
{
    void Add(int id, string connectionId);
    bool CheckOnline(int id);
    Task ConnectedAsync(int id, string connectionId, string clientType);
    Task DisconnectedAsync(int id, string connectionId, string clientType);
    IEnumerable<string> GetConnections(int id);
    Task<string> GetUserClientConnectionIdAsync(int id, string clientType);
    void Remove(int id, string connectionId);
    Task<bool> UserClientIsOnlineAsync(int id, string clientType);
}
