namespace QianShiChat.WebApi.Models.Requests;

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

    public string Path { get; set; }

    public ulong Size { get; set; }
}