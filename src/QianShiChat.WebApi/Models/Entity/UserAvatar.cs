using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models
{
    public class UserAvatar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("avatar id.")]
        public long Id { get; set; }
        [Required]
        [Comment("user id.")]
        public int UserId { get; set; }
        [Required]
        [Comment("create time.")]
        public long CreateTime { get; set; }
        [Required]
        [MaxLength(255)]
        [Comment("file path.")]
        public string Path { get; set; }
        [Required]
        [Comment("file size.")]
        public ulong Size { get; set; }
        [Required]
        [DefaultValue(false)]
        [Comment("is deleted.")]
        public bool IsDeleted { get; set; }

        [Comment("delete time.")]
        public long DeleteTime { get; set; }

        public UserInfo User { get; set; }
    }
}