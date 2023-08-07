namespace QianShiChat.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly HashSet<int> _httpStatusCodeMapper = new HashSet<int>() { 401, 403 };

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

            var result = new GlobalResult<object>
            {
                Data = businessException.Data,
                Errors = businessException.ErrorMessage,
                StatusCode = businessException.StatusCode,
            };


            switch (businessException.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    context.Result = new UnauthorizedResult();
                    break;
                case (int)HttpStatusCode.Forbidden:
                    context.Result = new ForbidResult();
                    break;
                default:
                    context.Result = new ObjectResult(result);
                    break;
            }
        }
        else
        {
            context.Result = new ObjectResult(GlobalResult<object>.ErrorResult("服务器异常"));
        }
        context.ExceptionHandled = true;
    }
}