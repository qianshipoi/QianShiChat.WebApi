namespace QianShiChat.Application.Services;

public class OnlineManager : IOnlineManager, ISingleton
{
    private readonly static ConnectionMapping<int> _connections =
        new ConnectionMapping<int>();

    public void Add(int id, string connectionId) => _connections.Add(id, connectionId);

    public void Remove(int id, string connectionId) => _connections.Remove(id, connectionId);

    public bool CheckOnline(int id) => _connections.ContainsKey(id);

    public IEnumerable<string> GetConnections(int id) => _connections.GetConnections(id);
}
