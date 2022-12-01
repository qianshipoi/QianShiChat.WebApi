namespace QianShiChat.WebApi.Models.Requests;

/// <summary>
/// send file request.
/// </summary>
public class SendFileRequest
{
    /// <summary>
    /// send id.
    /// </summary>
    [Required]
    public int ToId { get; set; }

    /// <summary>
    /// send type.
    /// </summary>
    [Required]
    public ChatMessageSendType SendType { get; set; }

    /// <summary>
    /// file.
    /// </summary>
    [Required]
    public IFormFile File { get; set; }
}