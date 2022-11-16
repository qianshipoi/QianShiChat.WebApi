using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using QianShiChat.Models;
using QianShiChat.WebApi.Hubs;

namespace QianShiChat.WebApi.Controllers
{
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
                           .Notification(new NotificationMessage(default, default)
                           {
                               Type = type,
                               Message = msg
                           });
            }
            else
            {
                await _hubContext.Clients.All.Notification(new NotificationMessage(default, default)
                {
                    Type = type,
                    Message = msg
                });
            }
        }
    }
}
