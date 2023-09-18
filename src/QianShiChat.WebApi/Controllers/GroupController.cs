﻿namespace QianShiChat.WebApi.Controllers;

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
        [FromBody] GroupApplyRequest request,
        CancellationToken cancellationToken = default)
        => _groupService.ApplyAsync(id, CurrentUserId, request, cancellationToken);

    [HttpDelete("{id:int}")]
    public Task DeleteAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        CancellationToken cancellationToken = default)
        => _groupService.DeleteAsync(CurrentUserId, id, cancellationToken);

    [HttpDelete("{id:int}/leave")]
    public Task LeaveAsync(
        [FromRoute, Range(1, int.MaxValue)] int id,
        CancellationToken cancellationToken = default)
        => _groupService.LeaveAsync(CurrentUserId, id, cancellationToken);

    [HttpGet("apply/pending")]
    public Task<PagedList<GroupApplyDto>> GetApplyPendingAsync([FromQuery] QueryGroupApplyPendingRequest request, CancellationToken cancellationToken = default)
        => _groupService.GetApplyPendingAsync(CurrentUserId, request, cancellationToken);

    [HttpPut("approval/{applyId:int}/pass")]
    public Task ApprovalPassAync(
        [FromRoute, Range(1, int.MaxValue)] int applyId, 
        CancellationToken cancellationToken = default)
        => _groupService.ApprovalAync(applyId, CurrentUserId, ApplyStatus.Passed, cancellationToken);

    [HttpPut("approval/{applyId:int}/reject")]
    public Task ApprovalRejectAync(
        [FromRoute, Range(1, int.MaxValue)] int applyId,
        CancellationToken cancellationToken = default)
        => _groupService.ApprovalAync(applyId, CurrentUserId, ApplyStatus.Rejected, cancellationToken);

    [HttpPut("approval/{applyId:int}/ignore")]
    public Task ApprovalIgnoreAync(
        [FromRoute, Range(1, int.MaxValue)] int applyId, 
        CancellationToken cancellationToken = default)
        => _groupService.ApprovalAync(applyId, CurrentUserId, ApplyStatus.Ignored, cancellationToken);
}
