namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// group api.
/// </summary>
[Route("/api/group")]
[ApiController]
[Authorize]
public class GroupController : BaseController
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<List<GroupDto>> GetMyGroupsAsync(CancellationToken cancellationToken = default)
    {
        return await _groupService.GetAllByUserIdAsync(CurrentUserId, cancellationToken);
    }

    [HttpPost]
    public async Task<GroupDto> CreateAsync(
        [FromBody] CreateGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _groupService.CreateByFriendAsync(CurrentUserId, request, cancellationToken);
    }

    [HttpPost("{id:int}/join")]
    public async Task JoinAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        [FromBody] JoinGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        await _groupService.ApplyJoin(CurrentUserId, id, request, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    public async Task DeleteAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        CancellationToken cancellationToken = default)
    {
        await _groupService.Delete(CurrentUserId, id, cancellationToken);
    }

    [HttpDelete("{id:int}/leave")]
    public async Task LeaveAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        CancellationToken cancellationToken = default)
    {
        await _groupService.Leave(CurrentUserId, id, cancellationToken);
    }
}
