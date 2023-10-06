namespace QianShiChat.Application.Managers;

public interface INotificationManager
{
}
public class NotificationManager : INotificationManager, ITransient
{
    private readonly IHubContext<ChatHub,IChatClient> _context;

    public NotificationManager(IHubContext<ChatHub, IChatClient> context)
    {
        _context = context;
    }
}

