namespace QianShiChat.WebApi.Filters;

public class ClientAuthotizationFilter : IAsyncAuthorizationFilter, IOrderedFilter
{
    private readonly IRedisCachingProvider _redisCachingProvider;
    private readonly ILogger<ClientAuthotizationFilter> _logger;
    private readonly IJwtService _jwtService;

    public int Order => 0;

    public ClientAuthotizationFilter(IRedisCachingProvider redisCachingProvider, ILogger<ClientAuthotizationFilter> logger, IJwtService jwtService)
    {
        _redisCachingProvider = redisCachingProvider;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.TryGetHeaderFirstValue(AppConsts.ClientType, out string? clientType))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptior)
        {
            var methodIsAuthorize = controllerActionDescriptior.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(false).FirstOrDefault();

            var methodIsAllowAnonymous = controllerActionDescriptior.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>(false).FirstOrDefault();

            if (methodIsAuthorize != null && methodIsAllowAnonymous == null)
            {
                // 校验身份
                goto Authorize;
            }
            else
            {
                // 方法上没有 Authorize 特性
                var controllerIsAuthorize = controllerActionDescriptior.ControllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(false).FirstOrDefault();

                var controllerIsAllowAonoymous = controllerActionDescriptior.ControllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>(false).FirstOrDefault();
                if (controllerIsAuthorize != null && controllerIsAllowAonoymous == null)
                {
                    goto Authorize;
                }
            }
        }

        return;

    Authorize:
        if (!context.HttpContext.TryGetAccessToken(out string? accessToken)
            || !_jwtService.Validate(accessToken!))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var id = _jwtService.GetClaims(accessToken!).FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var token = await _redisCachingProvider.StringGetAsync(AppConsts.GetAuthorizeCacheKey(clientType!, id!));

        if (string.IsNullOrWhiteSpace(token) || string.Compare(token, accessToken, true) != 0)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // authorization success.
    }
}
