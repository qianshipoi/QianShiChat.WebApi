using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Models;

public class UpdateCursorRequest
{
    [Required]
    public long Position { get; set; }
}
