using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models.Entity
{
    [Table(nameof(MessageCursor))]
    [PrimaryKey(nameof(UserId), nameof(ToId), nameof(SendType))]
    public class MessageCursor
    {
        [Required]
        [Comment("sender user id")]
        public int UserId { get; set; }

        [Required]
        [Comment("receiver id (user or group)")]
        public int ToId { get; set; }

        [Required]
        [Comment("ToId type")]
        public ChatMessageSendType SendType { get; set; }

        [Required]
        public long LastUpdateTime { get; set; }

        [Required]
        [Comment("message start position")]
        [DefaultValue(0L)]
        public long StartPosition { get; set; }

        [Required]
        [Comment("message current position")]
        [DefaultValue(0L)]
        public long CurrentPosition { get; set; }
    }
}
