namespace QianShiChat.Domain.Shared;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
}