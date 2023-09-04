using System.Text.Json.Serialization;
using System.Threading.Channels;

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

public enum TransmissionStatus
{
    Confirm,
    Passed,
    Reject,
    Cancel,
    Transmitting,
}

public class FileOnlineTransmission
{
    public string Id { get; init; }
    public int FromId { get; init; }
    public int ToId { get; init; }
    public FileBaseInfo FileInfo { get; init; }
    public TransmissionStatus Status { get; private set; }
    [JsonIgnore]
    public Channel<string>? FileChannel { get; private set; }
    public string? ClientType { get; private set; }

    public FileOnlineTransmission(string id, int fromId, int toId, FileBaseInfo fileInfo)
    {
        Id = id;
        FromId = fromId;
        ToId = toId;
        FileInfo = fileInfo;
    }

    public void PassedClient(string clientType)
    {
        ClientType = clientType;
        FileChannel = Channel.CreateUnbounded<string>();
    }

    public void Cancel()
    {
        Status = TransmissionStatus.Cancel;
        FileChannel?.Writer.Complete(new Exception("user cancel."));
    }
}

public record FileBaseInfo(string Name, string ContentType, int Size);

public class OnlineDataTransmission : ISingleton
{
    private readonly static Dictionary<string, FileOnlineTransmission> _channels =
        new Dictionary<string, FileOnlineTransmission>();

    private readonly IOnlineManager _onlineManager;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public OnlineDataTransmission(IOnlineManager onlineManager, IHubContext<ChatHub, IChatClient> hubContext)
    {
        _onlineManager = onlineManager;
        _hubContext = hubContext;
    }

    public async Task<FileOnlineTransmission> CreateChannel(int fromId, int toId, FileBaseInfo fileInfo)
    {
        var id = Guid.NewGuid().ToString();
        var channel = new FileOnlineTransmission(id, fromId, toId, fileInfo);
        _channels[id] = channel;
        // send confirm.
        await _hubContext.Clients.User(toId.ToString())
              .Notification(new NotificationMessage(NotificationType.OnlineTransmissionConfirm, channel));
        return channel;
    }

    public async Task<bool> Passed(string id, int userId, string clientType)
    {
        var channel = _channels[id];
        if (channel.ToId != userId)
        {
            return false;
        }
        channel.PassedClient(clientType);

        // send passed.
        await _hubContext.Clients.Users(channel.FromId.ToString(), channel.ToId.ToString())
            .Notification(new NotificationMessage(NotificationType.OnlineTransmissionPassed, channel));

        return true;
    }

    public async Task<bool> Cancel(string id, int userId)
    {
        var channel = _channels[id];
        if (channel.ToId != userId)
        {
            return false;
        }

        if(channel.FromId != userId && channel.ToId != userId)
        {
            return false;
        }

        channel.Cancel();
        await _hubContext.Clients.Users(channel.FromId.ToString(), channel.ToId.ToString())
            .Notification(new NotificationMessage(NotificationType.OnlineTransmissionCancel, channel));
        return true;
    }

    public ChannelWriter<string>? GetChannelWriter(string id, int userId)
    {
        var channel = _channels[id];
        if (channel.FromId != userId || channel.FileChannel is null)
        {
            return null;
        }
        return channel.FileChannel.Writer;
    }

    public ChannelReader<string>? GetChannelReader(string id, int userId)
    {
        var channel = _channels[id];
        if (channel.ToId != userId || channel.FileChannel is null)
        {
            return null;
        }
        return channel.FileChannel.Reader;
    }
}
