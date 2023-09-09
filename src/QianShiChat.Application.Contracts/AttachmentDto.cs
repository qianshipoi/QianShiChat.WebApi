namespace QianShiChat.Application.Contracts;

public class AttachmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RawPath { get; set; } = string.Empty;
    public string? PreviewPath { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}
