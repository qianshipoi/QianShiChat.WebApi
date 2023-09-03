namespace QianShiChat.Application.Services;

public interface IOnlineManager
{
    void Add(int id, string connectionId);
    bool CheckOnline(int id);
    IEnumerable<string> GetConnections(int id);
    void Remove(int id, string connectionId);
}
