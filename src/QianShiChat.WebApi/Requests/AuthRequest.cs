using FluentValidation;

namespace QianShiChat.WebApi.Requests
{
    public class AuthRequest : IHttpRequest
    {
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(x => x.Account)
                .NotEmpty()
                .WithMessage("账号不能为空")
                .Length(3, 11)
                .WithMessage("账号长度为3到11个字符");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("密码不能为空")
                .Length(32)
                .WithMessage("密码格式异常，请使用MD5格式化之后提交");
        }
    }
}
