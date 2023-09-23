namespace QianShiChat.Application.Hubs;

/// <summary>
/// chat client.
/// </summary>
public interface IChatClient
{
    /// <summary>
    /// send message.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    Task ReceiveMessage(string user, string text);

    /// <summary>
    /// notification.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task Notification(NotificationMessage message);

    /// <summary>
    /// private chat.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task PrivateChat(ChatMessageDto message);

    Task GroupMessage(ChatMessageDto message);

    Task OwnOtherClientMessage(ChatMessageDto message);

    Task<bool> ConfirmOnlineFile(FileOnlineTransmission file);

    Task OnlineFileReceive(FileOnlineTransmission file);
}