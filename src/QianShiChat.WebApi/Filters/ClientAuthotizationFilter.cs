namespace QianShiChat.WebApi.Filters;

public class ClientAuthotizationFilter : IAsyncAuthorizationFilter, IOrderedFilter
{
    public int Order => 0;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(x => x is IAllowAnonymous))
        {
            return;
        }

        var isAuthorization = await context.HttpContext.ExistsLegalTokenAsync();

        if (!isAuthorization)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
