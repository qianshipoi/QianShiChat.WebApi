using Microsoft.EntityFrameworkCore;

using QianShiChat.Common.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QianShiChat.WebApi.Models.Entity
{
    [Table(nameof(ChatMessage))]
    [Index(nameof(Id))]
    public class ChatMessage
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public int FormId { get; set; }

        [Required]
        public int ToId { get; set; }

        [Required]
        public ChatMessageSendType SendType { get; set; }

        [Required]
        public ChatMessageType MessageType { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public long CreateTime { get; set; }

        [Required]
        public long LastUpdateTime { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
