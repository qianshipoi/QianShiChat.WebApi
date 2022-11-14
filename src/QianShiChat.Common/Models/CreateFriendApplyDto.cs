using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Models
{
    public class CreateFriendApplyDto
    {
        [MaxLength(50)]
        public string Remark { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
