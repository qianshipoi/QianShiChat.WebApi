namespace QianShiChat.Application.Services.IServices;

public interface IFileService
{
    /// <summary>
    /// format public file url.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    string FormatPublicFile(string filePath);

    /// <summary>
    /// get default group avatar.
    /// </summary>
    /// <returns></returns>
    string GetDefaultGroupAvatar();

    /// <summary>
    /// save file.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="stream"></param>
    /// <param name="filename"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentDto> SaveFileAsync(int userId, Stream stream, string filename, CancellationToken cancellationToken = default);
}

public static class IFileServiceExtensions
{
    public static void FormatAvatar(this IFileService fileService, UserDto user)
    {
        if (!string.IsNullOrWhiteSpace(user.Avatar))
            user.Avatar = fileService.FormatPublicFile(user.Avatar);
    }

    public static void FormatAvatar(this IFileService fileService, GroupDto group)
    {
        if (!string.IsNullOrWhiteSpace(group.Avatar))
            group.Avatar = fileService.FormatPublicFile(group.Avatar);
        else
        {
            group.Avatar = fileService.GetDefaultGroupAvatar();
        }
    }
}