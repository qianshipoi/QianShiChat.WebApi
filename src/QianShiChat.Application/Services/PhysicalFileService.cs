namespace QianShiChat.Application.Services;

public class PhysicalFileService : IFileService, IScoped
{
    private readonly ILogger<PhysicalFileService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private static HashSet<string> _compressibleExts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    { ".png", ".jpg", ".jpeg" };

    private const int MAX_WIDTH = 300;
    private const string FILES_DIRNAME = "files";
    private const string PREVIEW_DIRNAME = "previews";
    private const string STREAM_CONRENT_TYPE = "application/octet-stream";

    public PhysicalFileService(
        IWebHostEnvironment webHostEnvironment,
        ILogger<PhysicalFileService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        Init();
    }

    private void Init()
    {
        string[] dirNames = new string[] { FILES_DIRNAME, PREVIEW_DIRNAME };

        foreach (string dirName in dirNames)
        {
            var dir = Path.Combine(_webHostEnvironment.WebRootPath, dirName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }

    public string FormatPublicFile(string filePath)
    {
        return _httpContextAccessor.HttpContext!.Request.GetBaseUrl() + '/' + filePath.TrimStart('/').Replace("\\", "/");
    }

    public async Task<SaveFileResult> SaveFileAsync(Stream stream, string filename, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(stream);
        Guard.IsNotNullOrEmpty(filename);

        stream.Seek(0, SeekOrigin.Begin);

        var ext = Path.GetExtension(filename);

        new FileExtensionContentTypeProvider()
            .Mappings
            .TryGetValue(ext, out var contentType);

        if (string.IsNullOrEmpty(contentType))
        {
            contentType = STREAM_CONRENT_TYPE;
        }

        filename = $"{YitIdHelper.NextId()}{ext.ToLower()}";

        var absFilepath = Path.Combine(FILES_DIRNAME, filename);

        var filepath = Path.Combine(_webHostEnvironment.WebRootPath, absFilepath);

        using var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);
        await fs.CopyToAsync(stream, cancellationToken);
        await fs.FlushAsync(cancellationToken);
        await fs.DisposeAsync();

        stream.Seek(0, SeekOrigin.Begin);

        var previewPath = string.Empty;

        if (_compressibleExts.Contains(ext))
        {
            using var image = Image.Load(stream);

            if (Math.Max(image.Width, image.Height) > MAX_WIDTH)
            {
                var absPrevFilepath = Path.Combine(PREVIEW_DIRNAME, filename);
                int width, height;

                if (image.Width > image.Height)
                {
                    width = 300;
                    height = (int)Math.Floor(image.Width / 300d * image.Height);
                }
                else
                {

                    width = (int)Math.Floor(image.Height / 300d * image.Width);
                    height = 300;
                }

                image.Mutate(x => x.Resize(new Size(width, height)));
                var prevPath = Path.Combine(_webHostEnvironment.WebRootPath, absPrevFilepath);
                image.Save(prevPath);
                previewPath = FormatPublicFile(absPrevFilepath);
            }

            stream.Seek(0, SeekOrigin.Begin);
        }

        return new SaveFileResult(FormatPublicFile(absFilepath), contentType, previewPath);
    }
}
