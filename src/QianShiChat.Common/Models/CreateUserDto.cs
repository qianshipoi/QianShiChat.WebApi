using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Models
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
}
