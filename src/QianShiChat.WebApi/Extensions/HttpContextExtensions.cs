namespace QianShiChat.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static async Task<bool> ExistsLegalTokenAsync(this HttpContext context)
    {
        if (!context.TryGetHeaderFirstValue(AppConsts.ClientType, out var clientType))
        {
            return false;
        }

        if (!context.TryGetAccessToken(out var accessToken) || string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }

        var jwtService = context.RequestServices.GetRequiredService<IJwtService>();

        if (!jwtService.Validate(accessToken))
        {
            return false;
        }

        var id = jwtService.GetClaims(accessToken!).FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var redisCachingProvider = context.RequestServices.GetRequiredService<IRedisCachingProvider>();

        var token = await redisCachingProvider.StringGetAsync(AppConsts.GetAuthorizeCacheKey(clientType!, id!));

        if (string.IsNullOrWhiteSpace(token) || string.Compare(token, accessToken, true) != 0)
        {
            return false;
        }

        // authorization success.
        return true;
    }

    public static Task<DefaultTusConfiguration> TusConfigurationFactory(HttpContext httpContext)
    {
        var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

        var enableAuthorize = true;

        var diskStorePath = httpContext.RequestServices.GetRequiredService<TusDiskStorageOptionHelper>().StorageDiskPath;

        var config = new DefaultTusConfiguration
        {
            Store = new TusDiskStore(diskStorePath),
            MetadataParsingStrategy = MetadataParsingStrategy.AllowEmptyValues,
            UsePipelinesIfAvailable = true,
            Events = new Events
            {
                OnAuthorizeAsync = ctx => {
                    // Note: This event is called even if RequireAuthorization is called on the endpoint.
                    // In that case this event is not required but can be used as fine-grained authorization control.
                    // This event can also be used as a "on request started" event to prefetch data or similar.

                    if (!enableAuthorize)
                        return Task.CompletedTask;

                    if (ctx.HttpContext.User.Identity?.IsAuthenticated != true)
                    {
                        ctx.HttpContext.Response.Headers.Add("WWW-Authenticate", new StringValues("Bearer"));
                        ctx.FailRequest(HttpStatusCode.Unauthorized);
                        return Task.CompletedTask;
                    }

                    // Do other verification on the user; claims, roles, etc.

                    // Verify different things depending on the intent of the request.
                    // E.g.:
                    //   Does the file about to be written belong to this user?
                    //   Is the current user allowed to create new files or have they reached their quota?
                    //   etc etc
                    switch (ctx.Intent)
                    {
                        case IntentType.CreateFile:
                            break;
                        case IntentType.ConcatenateFiles:
                            break;
                        case IntentType.WriteFile:
                            break;
                        case IntentType.DeleteFile:
                            break;
                        case IntentType.GetFileInfo:
                            break;
                        case IntentType.GetOptions:
                            break;
                        default:
                            break;
                    }

                    return Task.CompletedTask;
                },
                OnBeforeCreateAsync = ctx => {
                    // Partial files are not complete so we do not need to validate
                    // the metadata in our example.
                    if (ctx.FileConcatenation is FileConcatPartial)
                    {
                        return Task.CompletedTask;
                    }

                    if (!ctx.Metadata.ContainsKey("filename") || ctx.Metadata["filename"].HasEmptyValue)
                    {
                        ctx.FailRequest("name metadata must be specified. ");
                    }

                    if (!ctx.Metadata.ContainsKey("filetype") || ctx.Metadata["filetype"].HasEmptyValue)
                    {
                        ctx.FailRequest("contentType metadata must be specified. ");
                    }

                    return Task.CompletedTask;
                },
                OnCreateCompleteAsync = ctx => {
                    logger.LogInformation($"Created file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                    return Task.CompletedTask;
                },
                OnBeforeDeleteAsync = ctx => {
                    // Can the file be deleted? If not call ctx.FailRequest(<message>);
                    return Task.CompletedTask;
                },
                OnDeleteCompleteAsync = ctx => {
                    logger.LogInformation($"Deleted file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                    return Task.CompletedTask;
                },
                OnFileCompleteAsync = ctx => {
                    logger.LogInformation($"Upload of {ctx.FileId} completed using {ctx.Store.GetType().FullName}");
                    // If the store implements ITusReadableStore one could access the completed file here.
                    // The default TusDiskStore implements this interface:
                    //var file = await ctx.GetFileAsync();

                    // move to wwwroot dir.
                    //var file = await ctx.GetFileAsync();
                    //var fileStream = await file.GetContentAsync(ctx.CancellationToken);
                    //var metadata = await file.GetMetadataAsync(ctx.CancellationToken);
                    //metadata.TryGetValue("filename", out var name);
                    //var fileService = ctx.HttpContext.RequestServices.GetRequiredService<IFileService>();
                    //var dto = await fileService.SaveFileAsync(fileStream, name!.GetString(Encoding.UTF8), ctx.CancellationToken);
                    //ctx.HttpContext.Response.Headers.Add("AttachmentLocaltion", JsonSerializer.Serialize(dto));

                    return Task.CompletedTask;
                }
            },
            Expiration = new AbsoluteExpiration(TimeSpan.FromMinutes(5))
        };

        return Task.FromResult(config);
    }
}
