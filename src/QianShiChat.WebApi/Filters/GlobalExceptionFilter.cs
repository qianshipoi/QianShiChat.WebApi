namespace QianShiChat.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled) return;
        _logger.LogError(context.Exception, context.Exception.Message);

        var controllerAcrtionDescriptior = context.ActionDescriptor as ControllerActionDescriptor;
        var actionWrapper = controllerAcrtionDescriptior?.MethodInfo.GetCustomAttributes(typeof(NonUnifyAttribute), false).FirstOrDefault();
        var controllerWrapper = controllerAcrtionDescriptior?.ControllerTypeInfo.GetCustomAttributes(typeof(NonUnifyAttribute), false).FirstOrDefault();

        if (actionWrapper != null || controllerWrapper != null)
        {
            return;
        }

        if (context.Exception is BusinessException businessException)
        {
            // 业务异常
            context.Result = new ObjectResult(new GlobalResult<object>
            {
                Data = businessException.Data,
                Errors = businessException.ErrorMessage,
                StatusCode = businessException.StatusCode,
            });
        }
        else
        {
            context.Result = new ObjectResult(GlobalResult<object>.ErrorResult(context.Exception.Message));
        }
        context.ExceptionHandled = true;
    }
}