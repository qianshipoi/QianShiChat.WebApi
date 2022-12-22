namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// notice api.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class NoticeController : BaseController
{
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public NoticeController(IHubContext<ChatHub, IChatClient> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task Send(string msg, int[]? userIds = null, NotificationType type = NotificationType.FriendApply)
    {
        if (userIds is not null && userIds.Length > 0)
        {
            await _hubContext.Clients.Users(userIds.Select(x => x.ToString()))
                       .Notification(new NotificationMessage(type, msg));
        }
        else
        {
            await _hubContext.Clients.All
                .Notification(new NotificationMessage(type, msg));
        }
    }
}