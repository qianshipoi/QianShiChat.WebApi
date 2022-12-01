using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models.Entity
{
    [Table(nameof(MessageCursor))]
    [PrimaryKey(nameof(UserId))]
    public class MessageCursor
    {
        [Required]
        [Comment("user id")]
        public int UserId { get; set; }

        [Required]
        [Comment("message position")]
        [DefaultValue(0L)]
        public long Postiton { get; set; }

        [Required]
        public long LastUpdateTime { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserInfo User { get; set; }
    }
}