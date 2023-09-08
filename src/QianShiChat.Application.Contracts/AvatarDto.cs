namespace QianShiChat.Application.Contracts;

public class AvatarDto
{
    public long Id { get; set; }

    public long CreateTime { get; set; }

    public string Path { get; set; } = default!;

    public ulong Size { get; set; }
}
