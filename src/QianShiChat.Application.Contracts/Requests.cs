namespace QianShiChat.Application.Contracts;

public record UpdateCursorRequest([property: Required] long Position);

public record FileBaseInfo(string Name, string ContentType, int Size);

public record QueryUserAvatarRequest(int? BeforeId, [property: Required, Range(1, 100)] int Count);

public record CreateGroupRequest([property: Required] List<int> FriendIds, [property: MaxLength(32)] string? Name);

public record JoinGroupRequest([property: Required, MaxLength(32)] string Remark);

public record BasePagedRequest([property: Required][property: Range(1, int.MaxValue)] int Page)
{
    [Range(1, 100)]
    public int Size { get; set; } = 30;
}

public record QueryMessagesRequest(int Page) : BasePagedRequest(Page);

public record UploadAttachmentRequest([Required] IFormFile File);
