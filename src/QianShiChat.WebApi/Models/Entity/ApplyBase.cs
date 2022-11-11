using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models
{
    public class ApplyBase
    {
        /// <summary>
        /// 申请编号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("申请编号")]
        public int Id { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 申请状态
        /// </summary>
        [Required]
        public ApplyStatus Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public string Remark { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Required]
        public DateTime LaseUpdateTime { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public UserInfo User { get; set; }
    }
}
