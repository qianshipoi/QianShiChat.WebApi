namespace QianShiChat.Application.Contracts;

public class UpdateCursorRequest
{
    [Required]
    public long Position { get; set; }
}

public class QueryUserAvatar
{
    public int? BeforeId { get; set; }

    [Required, Range(1, 100)]
    public int Count { get; set; }
}

public class AvatarDto
{
    public long Id { get; set; }

    public long CreateTime { get; set; }

    public string Path { get; set; } = default!;

    public ulong Size { get; set; }
}

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
    public IFormFile File { get; set; } = default!;
}

public record CreateGroupRequest([property: Required] List<int> FriendIds, [property: MaxLength(32)] string? Name);

public record JoinGroupRequest([property: Required, MaxLength(32)] string Remark);