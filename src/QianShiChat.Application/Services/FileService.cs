namespace QianShiChat.Application.Services;

public class FileService : IFileService, IScoped
{
    private readonly ILogger<FileService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileService(
        IWebHostEnvironment webHostEnvironment,
        ILogger<FileService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public string FormatWwwRootFile(string filePath)
    {
        return _httpContextAccessor.HttpContext!.Request.GetBaseUrl() + '/' + filePath.TrimStart('/').Replace("\\", "/");
    }
}
