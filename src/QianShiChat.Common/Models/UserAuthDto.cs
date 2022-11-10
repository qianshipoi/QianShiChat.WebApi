using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Models
{
    public class UserAuthDto
    {
        [Required,MaxLength(32)]
        public string Account { get; set; } = null!;

        [Required, MaxLength(32)]
        public string Password { get; set; } = null!;
    }
}
