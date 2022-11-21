using QianShiChat.Models;

using System.ComponentModel.DataAnnotations;

namespace QianShiChat.WebApi.Models.Requests
{
    public class ShedFileRequest
    {
        [Required]
        public int ToId { get; set; }

        [Required]
        public ChatMessageSendType SendType { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
