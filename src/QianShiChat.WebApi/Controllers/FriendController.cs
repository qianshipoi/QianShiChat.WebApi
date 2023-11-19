using System.Threading.Tasks;

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

    [HttpPut("{friendId:int}/alias")]
    public Task ReMyAliasAsync([FromRoute] int friendId, AliasRequest request, CancellationToken cancellationToken = default)
        => _friendService.SetAliasAsync(CurrentUserId, friendId, request.Alias, cancellationToken);

    [HttpGet("groups")]
    public async Task<List<FriendGroupDto>> GetFriendGroups(CancellationToken cancellationToken = default)
        => await _friendGroupService.GetGroupsAsync(CurrentUserId, cancellationToken);

    [HttpPost("groups")]
    public Task<FriendGroupDto> CreateGroupAsync([FromBody] NameRequest request, CancellationToken cancellationToken = default)
        => _friendGroupService.AddAsync(CurrentUserId, request.Name, cancellationToken);

    [HttpPut("groups/{groupId:int}/rename")]
    public Task<FriendGroupDto> RenameGroupNameAsync([FromRoute] int groupId, [FromBody] RenameRequest request, CancellationToken cancellationToken = default)
        => _friendGroupService.UpdateAsync(CurrentUserId, groupId, request.Name, cancellationToken);

    [HttpDelete("groups/{groupId:int}")]
    public Task DeleteGroupAsync([FromRoute] int groupId, CancellationToken cancellationToken = default)
        => _friendGroupService.DeleteAsync(CurrentUserId, groupId, cancellationToken);

    [HttpPut("groups/sort")]
    public Task SortGroupAsync([FromBody] SortRequest request, CancellationToken cancellationToken = default)
        => _friendGroupService.SortAsync(CurrentUserId, request, cancellationToken);

    [HttpPut("{friendId:int}/move")]
    public Task MoveToGroupAsync([FromRoute] int friendId, [FromBody] MoveToGroupRequest request, CancellationToken cancellationToken = default)
        => _friendGroupService.MoveToAsync(CurrentUserId, friendId, request, cancellationToken);
}