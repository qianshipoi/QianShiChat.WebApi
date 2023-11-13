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
    private readonly IFriendGroupService _friendGroupService;

    /// <summary>
    /// friend api.
    /// </summary>
    public FriendController(
        IFriendService friendService, IFriendGroupService friendGroupService)
    {
        _friendService = friendService;
        _friendGroupService = friendGroupService;
    }

    /// <summary>
    /// get all friends.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<FriendDto>>> GetAllFriends(CancellationToken cancellationToken
        = default)
    {
        var result = await _friendService.GetFriendsAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("groups")]
    public async Task<ActionResult<List<FriendGroupDto>>> GetFriendGroups(CancellationToken cancellationToken
               = default)
    {
        var result = await _friendGroupService.GetGroupsAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{friendId:int}/alias")]
    public Task ReMyAliasAsync([FromRoute] int friendId, AliasRequest request, CancellationToken cancellationToken = default)
        => _friendService.SetAliasAsync(CurrentUserId, friendId, request.Alias, cancellationToken);
}