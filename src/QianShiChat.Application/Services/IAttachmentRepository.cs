namespace QianShiChat.Application.Services;

public interface IAttachmentRepository
{
    Task<Attachment?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
}
