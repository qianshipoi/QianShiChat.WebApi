﻿namespace QianShiChat.WebApi.Filters;

public class ResultWrapperFilter : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var controllerAcrtionDescriptior = context.ActionDescriptor as ControllerActionDescriptor;
        var actionWrapper = controllerAcrtionDescriptior?.MethodInfo.GetCustomAttributes(typeof(NonUnifyAttribute), false).FirstOrDefault();
        var controllerWrapper = controllerAcrtionDescriptior?.ControllerTypeInfo.GetCustomAttributes(typeof(NonUnifyAttribute), false).FirstOrDefault();

        if (actionWrapper != null || controllerWrapper != null)
        {
            return;
        }

        switch (context.Result)
        {
            case BadRequestObjectResult:
                return;
            case ObjectResult objectResult:
                if (objectResult.StatusCode == 401)
                {
                    return;
                }

                if (objectResult.Value == null)
                {
                    context.Result = new ObjectResult(GlobalResult<object>.SuccessResult(null));
                }
                else
                {
                    if ((objectResult.DeclaredType?.IsGenericType ?? false) && objectResult.DeclaredType?.GetGenericTypeDefinition() == typeof(GlobalResult<>))
                    {
                        return;
                    }

                    context.Result = new ObjectResult(GlobalResult<object>.SuccessResult(objectResult.Value));
                }
                return;
            case EmptyResult:
                {
                    context.Result = new ObjectResult(GlobalResult<object>.SuccessResult(null));
                }
                return;
            default:
                break;
        }

        base.OnResultExecuting(context);
    }
}