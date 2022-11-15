using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models
{
    /// <summary>
    /// 用户关系表
    /// </summary>
    [Table(nameof(UserRealtion))]
    [Index(nameof(Id))]
    public class UserRealtion
    {
        /// <summary>
        /// 编号
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Comment("用户关系表主键")]
        public int Id { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        [Comment("用户编号")]
        public int UserId { get; set; }
        /// <summary>
        /// 朋友编号
        /// </summary>
        [Required]
        [Comment("朋友编号")]
        public int FriendId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Comment("创建时间")]
        public long CreateTime { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserInfo User { get; set; }

        [ForeignKey(nameof(FriendId))]
        public UserInfo Friend { get; set; }
    }
}
