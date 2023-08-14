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
    public async Task<SaveFileResult> UploadAsync([FromForm] UploadAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        using var stream = request.File.OpenReadStream();
        return await _fileService.SaveFileAsync(stream, request.File.FileName, cancellationToken);
    }
}
