using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

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

    public static IServiceCollection AddUnifyResult(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add<BusinessExceptionFilter>();
        });

        return services;
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class NonUnifyAttribute : Attribute
{
}

public class BusinessExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled) return;
        // 获取控制器信息
        if (context.Exception is BusinessException friendlyException 
            && context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            context.Result = new JsonResult(friendlyException.ErrorMessage)
            {
                StatusCode = friendlyException.StatusCode,
            };
        }
    }
}
