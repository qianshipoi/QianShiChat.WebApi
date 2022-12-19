namespace QianShiChat.WebApi.Core.Interceptors;

public interface IAuditable
{
    public long CreateTime { get; set; }

    public long UpdateTime { get; set; }
}


