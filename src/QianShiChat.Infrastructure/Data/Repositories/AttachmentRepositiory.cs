using QianShiChat.Application.Common.IRepositories;

namespace QianShiChat.Infrastructure.Data.Repositories;

public class AttachmentRepositiory : IAttachmentRepository, IScoped
{
    private readonly IApplicationDbContext _chatDbContext;

    public AttachmentRepositiory(IApplicationDbContext chatDbContext)
    {
        _chatDbContext = chatDbContext;
    }

    public async Task<Attachment?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _chatDbContext.Attachments.FindAsync(new object[] { id }, cancellationToken);
    }
}
