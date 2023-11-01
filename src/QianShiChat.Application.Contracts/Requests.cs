namespace QianShiChat.Application.Contracts;

public class AliasRequest
{
    [MaxLength(32)]
    public string? Alias { get; set; }
}

public class NameRequest
{
    [Required, StringLength(32, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}

public class LocaleRequest
{
    [Required(ErrorMessage = "name_can_not_be_empty")]
    public string Name { get; set; } = string.Empty;
}

public class FileBaseInfo
{
    [Required, MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(128)]
    public string ContentType { get; set; } = string.Empty;
    [Required, Range(1, int.MaxValue)]
    public int Size { get; set; }
}

public class QueryUserAvatarRequest
{
    [Required, Range(1, 100)]
    public int Count { get; set; }
    public int? BeforeId { get; set; }
}

public class CreateGroupRequest
{
    [Required]
    public List<int> FriendIds { get; set; } = new List<int>();
    [MaxLength(32)]
    public string? Name { get; set; }
    public int? AvatarId { get; set; }
}

public class JoinGroupRequest
{
    [Required, MaxLength(255)]
    public string Remark { get; set; } = string.Empty;
}

public class BasePagedRequest
{
    [Required, Range(1, int.MaxValue)]
    public int Page { get; set; }

    [Range(1, 100)]
    public virtual int Size { get; set; } = 30;
}

public class GroupMemberQueryRequest : BasePagedRequest
{

}

public class GroupSearchRequest : BasePagedRequest
{
    [MaxLength(32)]
    public string? Search { get; set; }
}

public class UserSearchRequest : BasePagedRequest
{
    [MaxLength(32)]
    public string? Search { get; set; }
}

public class QueryMessageRequest : BasePagedRequest
{

}

public class UploadAttachmentRequest
{
    [Required]
    public IFormFile File { get; set; } = default!;
}

public class CreateFriendApplyRequest
{
    [Required, Range(1, int.MaxValue)]
    public int UserId { get; set; }
    [MaxLength(50)]
    public string? Remark { get; set; }
}

public class QueryFriendApplyPendingRequest
{
    [Required, Range(1, 100)]
    public int Size { get; set; }
    [Range(0, long.MaxValue)]
    public long? BeforeLastTime { get; set; }
}


public class QueryGroupApplyPendingRequest
{
    [Required, Range(1, 100)]
    public int Size { get; set; }
    [Range(0, long.MaxValue)]
    public long? BeforeLastTime { get; set; }
}

public class GroupJoiningApprovalRequest
{
    [Required, ListNotEmpty]
    public List<int> ApplyIds { get; set; } = new List<int>();

    [Required, EnumDataType(typeof(ApprovalState))]
    public ApprovalState State { get; set; }
}

public class UserAuthRequest
{
    [Required, MaxLength(32)]
    public string Account { get; set; } = string.Empty;

    [Required, MaxLength(32)]
    public string Password { get; set; } = string.Empty;
}

public class GroupApplyRequest
{
    [MaxLength(250)]
    public string? Remark { get; set; }
}