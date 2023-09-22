namespace QianShiChat.WebApi.Endpoints;

public class GroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/groups")
            .RequireAuthorization();

        group.MapGet("{groupId:int}/pending_joining", GetPendingJoining);
        group.MapPost("{groupId:int}/pending_joining", ApprovalPendingJoning);
    }

    public async Task<Results<UnauthorizedHttpResult, NotFound, ForbidHttpResult, Ok<List<GroupApplyDto>>>> ApprovalPendingJoning(
        int groupId,
        [AsParameters] GroupJoiningApprovalRequest request,
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin) return TypedResults.Unauthorized();
        var group = await groupService.FindByIdAsync(groupId, cancellationToken);
        if (group is null) return TypedResults.NotFound();
        if (group.UserId != userManager.CurrentUserId) return TypedResults.Forbid();

        return TypedResults.Ok(await groupService.ApprovalAsync(userManager.CurrentUserId, request, cancellationToken));
    }

    public async Task<Results<Ok<PagedList<GroupApplyDto>>, NotFound, UnauthorizedHttpResult, NoContent>> GetPendingJoining(
        int groupId,
        [AsParameters] QueryGroupApplyPendingRequest request,
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin) return TypedResults.Unauthorized();

        var groupExists = await groupService.GroupExistsAsync(groupId, cancellationToken);
        if (!groupExists) return TypedResults.NotFound();

        var isJoined = await groupService.IsJoinedAsync(userManager.CurrentUserId, groupId, cancellationToken);
        if (isJoined) return TypedResults.NoContent();

        return TypedResults.Ok(await groupService.GetApplyPendingAsync(userManager.CurrentUserId, request, cancellationToken));
    }
}
