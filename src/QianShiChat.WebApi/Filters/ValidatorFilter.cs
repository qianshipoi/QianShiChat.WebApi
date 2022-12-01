namespace QianShiChat.WebApi.Filters;

public class ValidatorFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidatorFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validatable = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;
        if (validatable is null)
        {
            return Results.BadRequest();
        }

        var validatioResult = await _validator.ValidateAsync(validatable);
        if (!validatioResult.IsValid)
        {
            return Results.BadRequest(validatioResult.Errors.ToResponse());
        }

        return await next(context);
    }
}

public static class ValidationFailureMapper
{
    public static ValidationFailureResponse ToResponse(this IEnumerable<FluentValidation.Results.ValidationFailure> failures)
    {
        return new ValidationFailureResponse
        {
            Error = failures.Select(x => x.ErrorMessage)
        };
    }
}

public class ValidationFailureResponse
{
    public IEnumerable<string> Error { get; set; }
}