using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using QianShiChat.WebApi.Requests;
using QianShiChat.WebApi.Filters;

namespace QianShiChat.WebApi
{
    public static class MinimalatrExtensions
    {
        public static WebApplication MediateGet<TRequest>(this WebApplication app, string template)
            where TRequest : class, IHttpRequest
        {
            app.MapGet(template, async (IMediator mediator, [AsParameters] TRequest request)
                => await mediator.Send(request)).AddEndpointFilter<ValidatorFilter<TRequest>>();
            return app;
        }

        public static WebApplication MediatePost<TRequest>(this WebApplication app, string template)
          where TRequest : class, IHttpRequest
        {
            app.MapPost(template, async (IMediator mediator, TRequest request)
                => await mediator.Send(request)).AddEndpointFilter<ValidatorFilter<TRequest>>();
            return app;
        }
    }
}
