using QianShiChat.Application.Services.IServices;

namespace QianShiChat.WebApi.Endpoints;

public class SearchEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/search")
            .WithGroupName("endpoint");
        group.MapGet("/groups", SearchGroups);
        group.MapGet("/users", SearchUsers);
    }

    /// <summary>
    /// search groups
    /// </summary>
    /// <param name="request"></param>
    /// <param name="groupService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Ok<PagedList<GroupDto>>> SearchGroups(
        [AsParameters] GroupSearchRequest request,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await groupService.SearchGroupAsync(request, cancellationToken));
    }

    /// <summary>
    /// search users
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userService"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Ok<PagedList<UserDto>>> SearchUsers(
        [AsParameters] UserSearchRequest request,
        IUserService userService,
        CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await userService.SearchUsersAsync(request, cancellationToken));
    }
}
