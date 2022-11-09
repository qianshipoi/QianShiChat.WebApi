using Furion.DatabaseAccessor;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace QianShiChat.Core
{
    public abstract class DEntityBase : DEntityBase<long, MasterDbContextLocator>
    {
        public DEntityBase()
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId();
        }
    }

    public abstract class DEntityBase<TKey, TDbContextLocator1> : PrivateDEntityBase<TKey>
       where TDbContextLocator1 : class, IDbContextLocator
    {
    }

    public abstract class PrivateDEntityBase<TKey> : IPrivateEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Id主键")]
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        public virtual DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Comment("更新时间")]
        public virtual DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Comment("创建者Id")]
        public virtual long? CreatedUserId { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [JsonIgnore]
        [Comment("软删除标记")]
        public virtual bool IsDeleted { get; set; } = false;
    }
}
