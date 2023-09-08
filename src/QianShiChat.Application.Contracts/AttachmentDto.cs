namespace QianShiChat.Application.Contracts;

public class AttachmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string RawPath { get; set; } = default!;
    public string? PreviewPath { get; set; }
    public string Hash { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
}
