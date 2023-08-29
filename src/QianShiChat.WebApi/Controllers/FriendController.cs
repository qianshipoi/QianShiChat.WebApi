namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// friend api.
/// </summary>
[Route("api/[Controller]")]
[ApiController]
[Authorize]
public class FriendController : BaseController
{
    private readonly IFriendService _friendService;

    /// <summary>
    /// friend api.
    /// </summary>
    public FriendController(
        IFriendService friendService)
    {
        _friendService = friendService;
    }

    /// <summary>
    /// unread message friends.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("unread")]
    public async Task<ActionResult<List<UserWithMessage>>> GetAllUnreadMessageFriends(CancellationToken cancellationToken = default)
    {
        var friends = await _friendService
            .GetNewMessageFriendsAsync(CurrentUserId, cancellationToken);
        return Ok(friends);
    }

    /// <summary>
    /// get all friends.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllFriends(CancellationToken cancellationToken
        = default)
    {
        var result = await _friendService.GetFriendsAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }
}