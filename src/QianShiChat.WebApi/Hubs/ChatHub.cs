using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace QianShiChat.WebApi.Hubs;

public class ChatHub : Hub
{

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
      => await Clients.All.SendAsync("ReceiveMessage", user, message);
}