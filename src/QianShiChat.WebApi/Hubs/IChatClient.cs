using QianShiChat.Models;

namespace QianShiChat.WebApi.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string text);

        Task Notification(NotificationMessage message);
    }
}
