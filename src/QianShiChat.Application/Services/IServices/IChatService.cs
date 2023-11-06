
namespace QianShiChat.Application.Services.IServices;

public interface IChatService
{
    Task SendGroupApplyNotifyAsync(int applyId);
    Task SendNewGroupNotifyAsync(int groupId);
    Task SendNewGroupNotifyByUsersAsync(int groupId, IEnumerable<int> userIds);
}
