namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// avatar controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AvatarController : BaseController
{
    private readonly IAvatarService _avatarService;

    public AvatarController(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }

    [HttpGet("defaults")]
    [AllowAnonymous]
    public Task<PagedList<AvatarDto>> GetDefaultAvatars(
        [FromQuery] QueryUserAvatarRequest query,
        CancellationToken cancellationToken = default)
        => _avatarService.GetDefaultAvatarsAsync(query, cancellationToken);

    [HttpGet]
    public Task<PagedList<AvatarDto>> GetUserAvatars(
        [FromQuery] QueryUserAvatarRequest query,
        CancellationToken cancellationToken = default)
        => _avatarService.GetUserAvatarsAsync(CurrentUserId, query, cancellationToken);
}