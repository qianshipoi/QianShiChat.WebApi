using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.ComponentModel.DataAnnotations;

namespace QianShiChat.Application.Services;

public class FriendGroupService : IFriendGroupService, ITransient
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<FriendGroupService> _logger;
    private readonly IMapper _mapper;

    public FriendGroupService(
        IApplicationDbContext context,
        ILogger<FriendGroupService> logger,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<FriendGroupDto>> GetGroupsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var groups = await _context.FriendGroups.AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Sort)
            .ThenBy(x => x.Id)
            .ProjectTo<FriendGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (!groups.Any(x => x.IsDefault))
        {
            groups.Insert(0, new FriendGroupDto()
            {
                Id = 0,
                IsDefault = true,
                CreateTime = Timestamp.Now,
                Name = "好友",
                Sort = 0
            });
        }

        return groups;
    }

    public async Task<FriendGroupDto> AddAsync(int userId, string? name, CancellationToken cancellationToken = default)
    {
        var group = new FriendGroup
        {
            UserId = userId,
            Name = name,
            Sort = 255,
            IsDefault = false,
            CreateTime = Timestamp.Now,
        };

        _context.FriendGroups.Add(group);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FriendGroupDto>(group);
    }

    public async Task<FriendGroupDto> UpdateAsync(int userId, int groupId, string? name, CancellationToken cancellationToken = default)
    {
        FriendGroup? group;
        if(groupId == 0)
        {
            // update default group
            group = await GetOrCreateDefaultGroupAsync(userId, cancellationToken);
        }
        else
        {
            group = await _context.FriendGroups
            .FirstOrDefaultAsync(x => x.Id == groupId && x.UserId == userId, cancellationToken);
            if (group == null) throw Oops.Bah("").StatusCode(HttpStatusCode.Forbidden);
        }

        group.Name = name;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FriendGroupDto>(group);
    }

    public async Task SortAsync(int userId, SortRequest request, CancellationToken cancellationToken = default)
    {
        var groups = await _context.FriendGroups
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        int currentIndex = 0;
        foreach (var id in request.State)
        {
            var group = groups.FirstOrDefault(x => x.Id == id);
            if (group == null) continue;
            group.Sort = currentIndex++;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<FriendGroup> GetOrCreateDefaultGroupAsync(int userId, CancellationToken cancellationToken = default)
    {
        var defaultGroup = await _context.FriendGroups
            .FirstOrDefaultAsync(x => x.UserId == userId && x.IsDefault, cancellationToken);

        if (defaultGroup is null)
        {
            defaultGroup = new FriendGroup
            {
                CreateTime = Timestamp.Now,
                IsDefault = true,
                Name = "好友",
                Sort = 0,
                UserId = userId,
            };
            _context.FriendGroups.Add(defaultGroup);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return defaultGroup;
    }

    public async Task DeleteAsync(int userId, int groupId, CancellationToken cancellationToken = default)
    {
        var group = await _context.FriendGroups
            .FirstOrDefaultAsync(x => x.Id == groupId && x.UserId == userId, cancellationToken);
        if (group == null) throw Oops.Bah("").StatusCode(HttpStatusCode.Forbidden);
        if (group.IsDefault) throw Oops.Bah("cannot delete the default group");

        var friends = await _context.UserRealtions
            .Where(x => x.UserId == userId && x.FriendGroupId == groupId)
            .ToListAsync(cancellationToken);

        var defaultGroup = await GetOrCreateDefaultGroupAsync(userId, cancellationToken);

        foreach (var friend in friends)
        {
            friend.FriendGroupId = defaultGroup.Id;
        }

        _context.FriendGroups.Remove(group);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task MoveToAsync(int userId, int friendId, MoveToGroupRequest request, CancellationToken cancellationToken = default)
    {
        var friend = await _context.UserRealtions
            .FirstOrDefaultAsync(x => x.UserId == userId && x.FriendId == friendId, cancellationToken);

        if (friend == null)
        {
            throw Oops.Bah("friend not found");
        }

        var group = await _context.FriendGroups
            .FirstOrDefaultAsync(x => x.Id == request.GroupId && x.UserId == userId, cancellationToken);

        if (group == null)
        {
            throw Oops.Bah("group not found");
        }

        friend.FriendGroupId = group.Id;

        await _context.SaveChangesAsync(cancellationToken);
    }
}