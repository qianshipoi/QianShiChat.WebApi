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
    public Task<List<GroupDto>> GetMyGroupsAsync(CancellationToken cancellationToken = default)
        => _groupService.GetAllByUserIdAsync(CurrentUserId, cancellationToken);

    [HttpPost]
    public Task<GroupDto> CreateAsync(
        [FromBody] CreateGroupRequest request,
        CancellationToken cancellationToken = default)
         => _groupService.CreateByFriendAsync(CurrentUserId, request, cancellationToken);

    [HttpPost("{id:int}/join")]
    public Task JoinAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        [FromBody] JoinGroupRequest request,
        CancellationToken cancellationToken = default)
        => _groupService.ApplyJoin(CurrentUserId, id, request, cancellationToken);

    [HttpDelete("{id:int}")]
    public Task DeleteAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        CancellationToken cancellationToken = default)
        => _groupService.Delete(CurrentUserId, id, cancellationToken);

    [HttpDelete("{id:int}/leave")]
    public Task LeaveAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        CancellationToken cancellationToken = default)
        => _groupService.Leave(CurrentUserId, id, cancellationToken);
}
