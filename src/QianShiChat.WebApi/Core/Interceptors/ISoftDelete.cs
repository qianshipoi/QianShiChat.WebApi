namespace QianShiChat.WebApi.Core.Interceptors;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public long DeleteTime { get; set; }
}
