namespace QianShiChat.Domain.Shared;

public interface IAuditable
{
    public long CreateTime { get; set; }

    public long UpdateTime { get; set; }
}