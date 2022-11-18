using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Models;

public class UpdateCursorRequest
{
    [Required, Range(1, int.MaxValue)]
    public int ToId { get; set; }

    [Required]
    public ChatMessageSendType Type { get; set; }

    [Required]
    public long Position { get; set; }
}
