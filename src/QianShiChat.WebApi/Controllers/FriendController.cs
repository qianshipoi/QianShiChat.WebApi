using QianShiChat.Application.Services.IServices;

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

    [HttpPut("{friendId:int}/alias")]
    public Task ReMyAliasAsync([FromRoute] int friendId, AliasRequest request, CancellationToken cancellationToken = default)
        => _friendService.SetAliasAsync(CurrentUserId, friendId, request.Alias, cancellationToken);
}