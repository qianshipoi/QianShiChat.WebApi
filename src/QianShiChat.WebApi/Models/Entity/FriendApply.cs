using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models
{
    /// <summary>
    /// 好友申请
    /// </summary>
    [Table(nameof(FriendApply))]
    [Index(nameof(Id))]
    public class FriendApply : ApplyBase
    {
        /// <summary>
        /// 目标人
        /// </summary>
        [Required]
        public int FriendId { get; set; }
        /// <summary>
        /// 目标
        /// </summary>
        [ForeignKey(nameof(FriendId))]
        public UserInfo Friend { get; set; }
    }
}
