namespace QianShiChat.WebApi.Filters;

public class GlobalExceptionEndpointFilter : IEndpointFilter
{
    private readonly ILogger<GlobalExceptionEndpointFilter> _logger;

    public GlobalExceptionEndpointFilter(ILogger<GlobalExceptionEndpointFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (BadHttpRequestException ex)
        {
            _logger.LogError(ex, "BadHttpRequestException");
            return Results.BadRequest();
        }
    }
}

public class GlobalExeptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExeptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await Results.Problem()
                 .ExecuteAsync(context);
    }
}
