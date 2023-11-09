using QianShiChat.WebApi.Attributes;

namespace QianShiChat.WebApi.Endpoints;

public class NameRequestValidator : AbstractValidator<NameRequest>
{
    public NameRequestValidator(IStringLocalizer<Language> stringLocalizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(stringLocalizer[Language.NameCanNotBeEmpty]);
    }
}

public class LocaliationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/locale")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithGroupName("endpoint")
            .MapPost("", GetIndex);
    }

    private static async Task<IResult> GetIndex([FromBody, Validate] NameRequest localeRequest,/* IValidator<NameRequest> validator, */[FromServices] IStringLocalizer<Language> stringLocalizer)
    {
        //var result = await validator.ValidateAsync(localeRequest);
        //if (!result.IsValid)
        //{
        //    return Results.ValidationProblem(result.ToDictionary());
        //}

        return Results.Ok(stringLocalizer[Language.NameCanNotBeEmpty]);
    }
}

public class GroupEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/groups")
            .WithGroupName("endpoint")
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
