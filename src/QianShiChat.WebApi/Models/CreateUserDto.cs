using FluentValidation;

using System.ComponentModel.DataAnnotations;

namespace QianShiChat.WebApi.Models
{
    public class CreateUserDto : IValidatableObject
    {
        [Required, MaxLength(32)]
        public string Account { get; set; } = null!;
        [Required, StringLength(32)]
        public string Password { get; set; } = null!;

        [Required, StringLength(32)]
        public string ConfirmPassword { get; set; } = null!;

        [Required, StringLength(10, MinimumLength = 2)]
        public string NickName { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("The password is different from the confirm password", new string[] { nameof(Password), nameof(ConfirmPassword) });
            }
        }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Account)
                .NotNull().NotEmpty().WithMessage("账号不能为空")
                .Length(5, 11).WithMessage("账号应在5到11个字符");

            RuleFor(x => x.Password)
                .NotNull().NotEmpty().WithMessage("密码不能为空")
                .Length(32).WithMessage("密码应为MD5加密后传入");

            RuleFor(x => x.ConfirmPassword)
                .Must((x, p) => x.Password == p).WithMessage("两次密码不一致");

            RuleFor(x => x.NickName)
                .Length(2, 10);
        }
    }
}
