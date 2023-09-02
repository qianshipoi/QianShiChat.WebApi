
namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// attachment api
/// </summary>
[ApiController]
[Authorize]
public class AttachmentController : BaseController
{
    private IFileService _fileService;

    public AttachmentController(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// upload file(size limit is 30 MB).
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024 * 30)]
    public async Task<AttachmentDto> UploadAsync([FromForm] UploadAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        using var stream = request.File.OpenReadStream();
        return await _fileService.SaveFileAsync(stream, request.File.FileName, cancellationToken);
    }

    [HttpPut("bind-tus-file/{fileId}")]
    public async Task<AttachmentDto> UploadByTusFileAsync([FromRoute, Required, MaxLength(128)] string fileId, CancellationToken cancellationToken = default)
    {
        var diskStorePath = HttpContext.RequestServices.GetRequiredService<TusDiskStorageOptionHelper>().StorageDiskPath;
        var store = new TusDiskStore(diskStorePath);
        var file = await store.GetFileAsync(fileId, cancellationToken);
        if(file is null)
        {
            throw Oops.Bah("file not exists.");
        }

        var fileStream = await file.GetContentAsync(cancellationToken);
        var metadata = await file.GetMetadataAsync(cancellationToken);
        metadata.TryGetValue("filename", out var name);
        return await _fileService.SaveFileAsync(fileStream, name!.GetString(Encoding.UTF8), cancellationToken);
    }
}
