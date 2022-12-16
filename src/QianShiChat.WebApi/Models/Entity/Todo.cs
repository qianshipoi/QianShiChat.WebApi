using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace QianShiChat.WebApi.Models.Entity
{
    [Index(nameof(Id))]
    public class Todo : ISafeDelete
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid GroupId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(255)]
        public string Content { get; set; }

        [Required, Comment("代办状态")]
        public TodoStatus Status { get; set; }

        [Required, Comment("代办颜色")]
        public string Color { get; set; }
        [Required]
        public long CreateTime { get; set; }
        [Required, Comment("是否以删除")]
        public bool IsDeleted { get; set; }
        [Required, Comment("删除时间，默认0")]
        public long DeleteTime { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserInfo User { get; set; }
        [ForeignKey(nameof(GroupId))]
        public TodoGroup Group { get; set; }
    }

    public enum TodoStatus
    {
        None,
        Completed
    }

    [Index(nameof(Id))]
    public class TodoGroup : ISafeDelete
    {
        [Key]
        public Guid Id { get; set; }

        [Required, Comment("用户编号")]
        public int UserId { get; set; }

        [Required, MaxLength(64), Comment("组名称")]
        public string Name { get; set; }

        [Required, Comment("创建时间")]
        public long CreateTime { get; set; }

        [Required, Comment("是否以删除")]
        public bool IsDeleted { get; set; }

        [Required, Comment("删除时间，默认0")]
        public long DeleteTime { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserInfo User { get; set; }


        [InverseProperty(nameof(Todo.Group))]
        public virtual ICollection<Todo> Todos { get; set; }
    }
}
