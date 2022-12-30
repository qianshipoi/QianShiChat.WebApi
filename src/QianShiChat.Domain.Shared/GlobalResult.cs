namespace QianShiChat.Domain.Shared;

public class GlobalResult<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 执行成功
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public object? Errors { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }

    public static GlobalResult<T> SuccessResult(T? data)
    {
        return new GlobalResult<T> { Data = data, StatusCode = 200, Succeeded = true };
    }

    public static GlobalResult<T> FailResult(string? msg = null)
    {
        return new GlobalResult<T> { Errors = msg, StatusCode = 400 };
    }

    public static GlobalResult<T> ErrorResult(object? errors = null)
    {
        return new GlobalResult<T> { Errors = errors, StatusCode = 500 };
    }
}
