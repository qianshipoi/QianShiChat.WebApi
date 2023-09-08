namespace QianShiChat.Application.Filters;

public class ClientTypeAuthorize : IAsyncAuthorizationFilter
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ClientTypeAuthorize(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // get client type.
        string? accessToken = context.HttpContext.Request.Query["access_token"];
        var clientType = context.HttpContext.User.FindFirstValue(CustomClaim.ClientType);
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(clientType)
            || string.IsNullOrWhiteSpace(userId)
            || string.IsNullOrWhiteSpace(accessToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // check token.
        using var scope = _serviceScopeFactory.CreateScope();
        var redisCachingProvider = scope.ServiceProvider.GetRequiredService<IRedisCachingProvider>();
        var token = await redisCachingProvider.StringGetAsync(AppConsts.GetAuthorizeCacheKey(clientType, userId));
        if (string.IsNullOrWhiteSpace(token) || token != accessToken)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
