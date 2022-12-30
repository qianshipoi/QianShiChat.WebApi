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
        [FromQuery] QueryUserAvatar query,
        CancellationToken cancellationToken = default)
        => _avatarService.GetDefaultAvatarsAsync(query, cancellationToken);

    [HttpGet]
    public Task<PagedList<AvatarDto>> GetUserAvatars(
        [FromQuery] QueryUserAvatar query,
        CancellationToken cancellationToken = default)
        => _avatarService.GetUserAvatarsAsync(CurrentUserId, query, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<string>> UpdateAvatar([Required] IFormFile file, CancellationToken cancellationToken = default)
    {
        var (success, msg) = await _avatarService.UploadAvatarAsync(CurrentUserId, file, cancellationToken);

        if (!success)
        {
            return BadRequest(msg);
        }

        return Ok(msg);
    }
}