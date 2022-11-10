using MediatR;

using QianShiChat.WebApi.Requests;
using QianShiChat.WebApi.Filters;

namespace QianShiChat.WebApi
{
    public static class MinimalatrExtensions
    {
        public static RouteHandlerBuilder MediateGet<TRequest>(this WebApplication app, string template)
            where TRequest : class, IHttpRequest
        {
            return app.MapGet(template, async (IMediator mediator, [AsParameters] TRequest request)
                => await mediator.Send(request)).AddEndpointFilter<ValidatorFilter<TRequest>>();
        }

        public static RouteHandlerBuilder MediatePost<TRequest>(this WebApplication app, string template)
          where TRequest : class, IHttpRequest
        {
            return app.MapPost(template, async (IMediator mediator, TRequest request)
                => await mediator.Send(request)).AddEndpointFilter<ValidatorFilter<TRequest>>();
        }
    }
}
