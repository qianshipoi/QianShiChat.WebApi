using MediatR;

using QianShiChat.WebApi.Requests;

namespace QianShiChat.WebApi.Handlers
{
    public class ExampleHandler : IRequestHandler<ExampleRequest, IResult>
    {
        async Task<IResult> IRequestHandler<ExampleRequest, IResult>.Handle(ExampleRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            return Results.Ok($"Hello {request.Name}");
        }
    }
}
