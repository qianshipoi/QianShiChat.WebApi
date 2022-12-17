namespace QianShiChat.WebApi.Models;

public interface ISafeDelete
{
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
}
