using QianShiChat.Application.Services.IServices;

namespace QianShiChat.WebApi.Endpoints;

public class UserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/user")
            .WithGroupName("endpoint")
            .RequireAuthorization();
        var friendsGroup = group.MapGroup("friends");
        friendsGroup.MapGet(string.Empty, GetMyFriends);
        friendsGroup.MapGet("{friendId:int}", GetFriend);
        friendsGroup.MapPut("{friendId:int}", AddFriend);
        friendsGroup.MapDelete("{friendId:int}", DeleteFriend);

        var groupGroup = group.MapGroup("group");
        groupGroup.MapGet(string.Empty, GetMyGroups);
        groupGroup.MapPost(string.Empty, CreateGroup);
        groupGroup.MapGet("{groupId:int}", GetMyGroup);
        groupGroup.MapDelete("{groupId:int}", DeleteGroup);
        groupGroup.MapPut("{groupId:int}/joining", JoinGroup);
        groupGroup.MapDelete("{groupId:int}/joining", LeaveGroup);
    }

    public static async Task<Results<NoContent, NotFound, UnauthorizedHttpResult>> LeaveGroup(
        [FromRoute] int groupId,
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin) return TypedResults.Unauthorized();

        var groupExists = await groupService.GroupExistsAsync(groupId, cancellationToken);
        if (!groupExists) return TypedResults.NotFound();

        await groupService.LeaveAsync(userManager.CurrentUserId, groupId, cancellationToken);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound, UnauthorizedHttpResult>> JoinGroup(
        [FromRoute] int groupId,
        [FromBody] GroupApplyRequest request,
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin) return TypedResults.Unauthorized();

        var groupExists = await groupService.GroupExistsAsync(groupId, cancellationToken);
        if (!groupExists) return TypedResults.NotFound();

        await groupService.ApplyAsync(groupId, userManager.CurrentUserId, request, cancellationToken);
        return TypedResults.NoContent();
    }

    public static async Task<Results<NoContent, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> DeleteGroup(
        int groupId,
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin) return TypedResults.Unauthorized();

        var group = await groupService.FindByIdAsync(groupId, cancellationToken);
        if (group is null) return TypedResults.NotFound();

        if (group.UserId != userManager.CurrentUserId) return TypedResults.Forbid();

        await groupService.DeleteAsync(userManager.CurrentUserId, groupId, cancellationToken);

        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<GroupDto>, UnauthorizedHttpResult, NotFound>> GetMyGroup(
        int groupId,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await groupService.FindByIdAsync(groupId, cancellationToken));
    }

    public static async Task<Results<Ok<GroupDto>, UnauthorizedHttpResult>> CreateGroup(
        [FromBody] CreateGroupRequest request,
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin)
        {
            return TypedResults.Unauthorized();
        }
        var dto = await groupService.CreateByFriendAsync(userManager.CurrentUserId, request, cancellationToken);
        return TypedResults.Ok(dto);
    }

    public static async Task<Results<Ok<List<GroupDto>>, UnauthorizedHttpResult>> GetMyGroups(
        IUserManager userManager,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin)
        {
            return TypedResults.Unauthorized();
        }
        return TypedResults.Ok(await groupService.GetAllByUserIdAsync(userManager.CurrentUserId, cancellationToken));
    }

    public static async Task<Results<Ok<List<UserDto>>, UnauthorizedHttpResult>> GetMyFriends(
        IUserManager userManager,
        IFriendService friendService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin)
        {
            return TypedResults.Unauthorized();
        }
        return TypedResults.Ok(await friendService.GetFriendsAsync(userManager.CurrentUserId, cancellationToken));
    }

    public static async Task<Results<Ok<UserDto>, UnauthorizedHttpResult>> GetFriend(
        int friendId,
        IUserService userService,
        CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await userService.GetUserByIdAsync(friendId, cancellationToken));
    }

    public static async Task<Results<NoContent, UnauthorizedHttpResult>> DeleteFriend(
        int friendId,
        IUserManager userManager,
        IFriendService friendService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin)
        {
            return TypedResults.Unauthorized();
        }
        await friendService.DeleteAsync(userManager.CurrentUserId, friendId, cancellationToken);
        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<FriendApplyDto>, NoContent, UnauthorizedHttpResult>> AddFriend(
        [FromBody] CreateFriendApplyRequest request,
        IUserManager userManager,
        IFriendApplyService friendApplyService,
        IFriendService friendService,
        CancellationToken cancellationToken = default)
    {
        if (!userManager.IsLogin)
        {
            return TypedResults.Unauthorized();
        }

        if (await friendService.IsFriendAsync(userManager.CurrentUserId, request.UserId, cancellationToken))
        {
            return TypedResults.NoContent();
        }

        var dto = await friendApplyService.ApplyAsync(userManager.CurrentUserId, request, cancellationToken);
        return TypedResults.Ok(dto);
    }
}
