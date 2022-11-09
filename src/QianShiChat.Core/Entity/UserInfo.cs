using Furion.DatabaseAccessor;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.Core
{
    [Table("userinfo")]
    [Comment("用户表")]
    public class UserInfo : EntityBase<long>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Comment("账号")]
        [Required, MaxLength(50)]
        public string Account { get; set; }

        /// <summary>
        /// 密码（默认MD5加密）
        /// </summary>
        [Comment("密码")]
        [Required, MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Comment("昵称")]
        [MaxLength(20)]
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Comment("姓名")]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Comment("头像")]
        public string Avatar { get; set; }
    }
}
