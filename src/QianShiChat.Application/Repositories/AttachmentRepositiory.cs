namespace QianShiChat.Application.Repositories;

public class AttachmentRepositiory : IAttachmentRepository, IScoped
{
    private readonly ChatDbContext _chatDbContext;

    public AttachmentRepositiory(ChatDbContext chatDbContext)
    {
        _chatDbContext = chatDbContext;
    }

    public async Task<Attachment?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _chatDbContext.Attachments.FindAsync(new object[] { id }, cancellationToken);
    }
}
