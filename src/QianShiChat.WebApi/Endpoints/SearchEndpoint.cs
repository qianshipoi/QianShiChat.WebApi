namespace QianShiChat.WebApi.Endpoints;

public class SearchEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/search");
        group.MapGet("/groups", SearchGroups);
        group.MapGet("/users", SearchUsers);
    }

    public static async Task<Ok<PagedList<GroupDto>>> SearchGroups(
        [AsParameters] GroupSearchRequest request,
        IGroupService groupService,
        CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await groupService.SearchGroupAsync(request, cancellationToken));
    }

    public static async Task<Ok<PagedList<UserDto>>> SearchUsers(
        [AsParameters] UserSearchRequest request,
        IUserService userService,
        CancellationToken cancellationToken = default)
    {
        return TypedResults.Ok(await userService.SearchUsersAsync(request, cancellationToken));
    }
}
