namespace QianShiChat.Application.Contracts;

public record UploadAttachmentRequest(IFormFile File);

public record SaveFileResult(string RawPath, string ContentType, string? PreviewPath = null);