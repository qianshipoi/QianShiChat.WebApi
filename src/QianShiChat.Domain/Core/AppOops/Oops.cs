namespace QianShiChat.Domain.Core.AppOops;

public static class Oops
{
    public static BusinessException Bah(string errorMessage, params object[] args)
    {
        var friendlyException = Oh(errorMessage, typeof(ValidationException), args).StatusCode(StatusCodes.Status400BadRequest);
        friendlyException.ValidationException = true;
        return friendlyException;
    }

    public static BusinessException Oh(string errorMessage, params object[] args)
    {
        var friendlyException = new BusinessException(string.Format(errorMessage, args));

        return friendlyException;
    }

    public static BusinessException Oh<TException>(string errorMessage, params object[] args)
       where TException : class
    {
        return Oh(errorMessage, typeof(TException), args);
    }
}

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
        ErrorMessage = message;
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorMessage = message;
    }

    /// <summary>
    /// 错误消息（支持 Object 对象）
    /// </summary>
    public object ErrorMessage { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;

    /// <summary>
    /// 是否是数据验证异常
    /// </summary>
    public bool ValidationException { get; set; } = false;
}

public static class ExceptionExtentions
{
    public static BusinessException StatusCode(this BusinessException exception, int status)
    {
        exception.StatusCode = status;
        return exception;
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class NonUnifyAttribute : Attribute
{
}