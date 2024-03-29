﻿namespace QianShiChat.Application.Services;

public class PhysicalFileService : IFileService, ISingleton
{
    private const int MAX_WIDTH = 300;
    private const string FILES_DIRNAME = "files";
    private const string PREVIEW_DIRNAME = "previews";
    private const string STREAM_CONRENT_TYPE = "application/octet-stream";
    private const string AVATAR_DIRNAME = "avatar";
    private const string GROUP_AVATAR_DIRNAME = "group_avatar";

    private readonly static HashSet<string> _compressibleExts = new(StringComparer.OrdinalIgnoreCase)
    { ".png", ".jpg", ".jpeg" };

    private readonly ILogger<PhysicalFileService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMapper _mapper;
    private AppSettingsOptions _appSettings;

    public PhysicalFileService(
        IWebHostEnvironment webHostEnvironment,
        ILogger<PhysicalFileService> logger,
        IMapper mapper,
        IOptionsMonitor<AppSettingsOptions> appSettings,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
        _appSettings = appSettings.CurrentValue;
        appSettings.OnChange(x => _appSettings = x);
        _serviceScopeFactory = serviceScopeFactory;
        Init();
    }

    private void Init()
    {
        var dirNames = new string[] { FILES_DIRNAME, PREVIEW_DIRNAME, AVATAR_DIRNAME, GROUP_AVATAR_DIRNAME };

        foreach (string dirName in dirNames)
        {
            var dir = Path.Combine(_webHostEnvironment.WebRootPath, dirName);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }
    }

    public string FormatPublicFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return string.Empty;
        var apiUrl = _appSettings.ApiUrl.TrimEnd(AppConsts.TrimChars);
        return apiUrl + '/' + filePath.TrimStart(AppConsts.TrimChars);
    }

    public string GetDefaultGroupAvatar() => FormatPublicFile("Raw/DefaultAvatar/1.jpg");

    public async Task<AttachmentDto> SaveFileAsync(int userId, Stream stream, string filename, CancellationToken cancellationToken = default)
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

        var newFilename = $"{YitIdHelper.NextId()}{ext.ToLower()}";

        var absFilepath = Path.Combine(FILES_DIRNAME, newFilename);

        var filepath = Path.Combine(_webHostEnvironment.WebRootPath, absFilepath);

        using var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write);
        await stream.CopyToAsync(fs, cancellationToken);
        await fs.FlushAsync(cancellationToken);
        await fs.DisposeAsync();

        stream.Seek(0, SeekOrigin.Begin);

        var previewPath = string.Empty;
        var absPrevFilepath = string.Empty;
        var prevPath = string.Empty;

        if (_compressibleExts.Contains(ext))
        {
            absPrevFilepath = Path.Combine(PREVIEW_DIRNAME, newFilename);
            prevPath = Path.Combine(_webHostEnvironment.WebRootPath, absPrevFilepath);
            using var image = Image.Load(stream);

            if (Math.Max(image.Width, image.Height) > MAX_WIDTH)
            {
                int width, height;

                if (image.Width > image.Height)
                {
                    width = MAX_WIDTH;
                    height = (int)Math.Floor((decimal)MAX_WIDTH / image.Width * image.Height);
                }
                else
                {

                    width = (int)Math.Floor((decimal)MAX_WIDTH / image.Height * image.Width);
                    height = MAX_WIDTH;
                }

                image.Mutate(x => x.Resize(new Size(width, height)));
                image.Save(prevPath);
                previewPath = FormatPublicFile(absPrevFilepath);
            }
            else
            {
                absPrevFilepath = absFilepath;
            }

            stream.Seek(0, SeekOrigin.Begin);
        }

        // calc md5 hash.
        var hash = stream.ToMd5();

        // save to database.
        var attachment = new Attachment
        {
            Hash = hash,
            ContentType = contentType,
            Name = filename,
            PreviewPath = string.IsNullOrWhiteSpace(absPrevFilepath) ? null : absPrevFilepath.Replace('\\', '/'),
            RawPath = absFilepath.Replace('\\', '/'),
            Size = stream.Length,
            UserId = userId,
        };
        using var scope = _serviceScopeFactory.CreateScope();
        var chatDbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        try
        {
            await chatDbContext.Attachments.AddAsync(attachment, cancellationToken);
            await chatDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "save file error.");

            // clear file.
            if (File.Exists(filepath)) File.Delete(filepath);
            if (File.Exists(prevPath)) File.Delete(prevPath);
            throw Oops.Oh();
        }

        var dto = _mapper.Map<AttachmentDto>(attachment);

        dto.RawPath = FormatPublicFile(dto.RawPath);

        if (!string.IsNullOrWhiteSpace(dto.PreviewPath))
        {
            dto.PreviewPath = FormatPublicFile(dto.PreviewPath);
        }

        return dto;
    }
}
