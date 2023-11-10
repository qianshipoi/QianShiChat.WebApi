namespace QianShiChat.WebApi.Endpoints;

public class LocaliationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/locale")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithGroupName("endpoint")
            .MapPost("", GetIndex);
    }

    private static IResult GetIndex([FromBody, Validate] NameRequest localeRequest, [FromServices] IStringLocalizer<Language> stringLocalizer)
    {
        return Results.Ok(stringLocalizer[Language.NameCanNotBeEmpty]);
    }
}
