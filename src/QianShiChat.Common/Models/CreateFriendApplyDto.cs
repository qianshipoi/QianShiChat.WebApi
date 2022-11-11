using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Models
{
    public class CreateFriendApplyDto
    {
        [MaxLength(50)]
        public string Remark { get; set; }
    }
}
