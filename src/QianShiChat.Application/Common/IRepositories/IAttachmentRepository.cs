namespace QianShiChat.Application.Common.IRepositories;

public interface IAttachmentRepository
{
    Task<Attachment?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
}
