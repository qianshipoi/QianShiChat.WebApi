namespace QianShiChat.Application.Services;

public class OnlineDataTransmission : ISingleton
{
    private readonly static Dictionary<string, FileOnlineTransmission> _channels =
        new Dictionary<string, FileOnlineTransmission>();

    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public OnlineDataTransmission(IHubContext<ChatHub, IChatClient> hubContext)
    {
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
