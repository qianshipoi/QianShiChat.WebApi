namespace QianShiChat.WebApi.Validators;

public class GroupJoiningApprovalRequestValidator : AbstractValidator<GroupJoiningApprovalRequest>
{
    public GroupJoiningApprovalRequestValidator(IStringLocalizer<Language> stringLocalizer)
    {
        RuleFor(x => x.ApplyIds)
            .NotNull()
            .WithMessage(stringLocalizer[Language.PropertyCanNotBeNull])
            .NotEmpty()
            .WithMessage(stringLocalizer[Language.PropertyCanNotBeEmpty]);
        RuleFor(x => x.State)
            .IsInEnum();
    }
}
