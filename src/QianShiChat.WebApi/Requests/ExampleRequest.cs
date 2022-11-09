using FluentValidation;

namespace QianShiChat.WebApi.Requests;

public class ExampleRequest : IHttpRequest
{
    public string Name { get; set; }
}

public class ExampleRequestValidator : AbstractValidator<ExampleRequest>
{
    public ExampleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("name can not ne empty")
            .Length(3, 10)
            .WithMessage("name length in 3 and 10");
    }
}

