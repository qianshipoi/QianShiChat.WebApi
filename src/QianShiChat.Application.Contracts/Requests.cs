namespace QianShiChat.Application.Contracts;

public record FileBaseInfo([property: Required, MaxLength(255)] string Name, [property: Required, MaxLength(128)] string ContentType, [property: Required, Range(1, int.MaxValue)] int Size);

public record QueryUserAvatarRequest([property: Required, Range(1, 100)] int Count, int? BeforeId);

public record CreateGroupRequest([property: Required] List<int> FriendIds, [property: MaxLength(32)] string? Name);

public record JoinGroupRequest([property: Required, MaxLength(32)] string Remark);

public record BasePagedRequest([property: Required][property: Range(1, int.MaxValue)] int Page, [property: Range(1, 100)] int Size = 30);

public record QueryMessagesRequest(int Page) : BasePagedRequest(Page);

public record UploadAttachmentRequest([property:Required] IFormFile File);

public record CreateFriendApplyRequest([property: Required, Range(1, int.MaxValue)] int UserId, [property: MaxLength(50)] string? Remark);

public record QueryFriendApplyPendingRequest([property: Required, Range(1, 100)] int Size, [property: Range(0, long.MaxValue)] long? BeforeLastTime);

public record UserAuthRequest([property: Required, MaxLength(32)] string Account, [property: Required, MaxLength(32)] string Password);
