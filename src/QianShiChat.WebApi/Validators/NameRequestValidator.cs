namespace QianShiChat.WebApi.Validators;

public class NameRequestValidator : AbstractValidator<NameRequest>
{
    public NameRequestValidator(IStringLocalizer<Language> stringLocalizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(stringLocalizer[Language.NameCanNotBeEmpty])
            .MaximumLength(32)
            .WithMessage(stringLocalizer[Language.LengthCanNotBeGreaterThan]);
    }
}
