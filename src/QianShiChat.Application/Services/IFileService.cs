namespace QianShiChat.Application.Services;

public interface IFileService
{
    /// <summary>
    /// format public file url.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    string FormatPublicFile(string filePath);
    string GetDefaultGroupAvatar();

    /// <summary>
    /// save file.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="filename"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentDto> SaveFileAsync(Stream stream, string filename, CancellationToken cancellationToken = default);
}