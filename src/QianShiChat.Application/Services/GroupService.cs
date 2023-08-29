using System.Net;

namespace QianShiChat.Application.Services;

public class GroupService : IGroupService, ITransient
{
    private readonly ChatDbContext _context;
    private readonly ILogger<GroupService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IFileService _fileService;

    public GroupService(ChatDbContext context, ILogger<GroupService> logger, IMapper mapper, IUserService userService, IFileService fileService)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
        _fileService = fileService;
    }

    /// <summary>
    /// 创建群聊
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GroupDto> Create(int userId, string name, CancellationToken cancellationToken = default)
    {
        var now = Timestamp.Now;
        var group = new Group()
        {
            Name = name,
            UserId = userId,
            TotalUser = 1,
            CreateTime = now
        };
        group.UserGroupRealtions.Add(new UserGroupRealtion()
        {
            CreateTime = now,
            UserId = userId
        });

        await _context.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<GroupDto>(group);
        FormatGroupAvatar(dto);
        return dto;
    }

    public async Task<GroupDto> CreateByFriendAsync(int userId, CreateGroupRequest request, CancellationToken cancellationToken = default)
    {
        if (!request.FriendIds.Any())
        {
            throw Oops.Bah("choose at least one friend.");
        }

        foreach (var friendId in request.FriendIds)
        {
            var isFriend = await _userService.IsFriendAsync(userId, friendId, cancellationToken);

            if (!isFriend)
            {
                throw Oops.Bah("no access.").StatusCode(HttpStatusCode.Forbidden);
            }
        }

        var currentUserName = await _userService.GetNickNameByIdAsync(userId, cancellationToken);
        var friendName = await _userService.GetNickNameByIdAsync(request.FriendIds.First(), cancellationToken);

        var now = Timestamp.Now;
        var group = new Group()
        {
            Name = $"{currentUserName}、{friendName}",
            UserId = userId,
            TotalUser = 1,
            CreateTime = now,
        };
        group.UserGroupRealtions.Add(new UserGroupRealtion()
        {
            CreateTime = now,
            UserId = userId,
        });

        foreach (var friendId in request.FriendIds)
        {
            group.UserGroupRealtions.Add(new UserGroupRealtion
            {
                CreateTime = now,
                UserId = friendId
            });
        }

        await _context.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        var dto = _mapper.Map<GroupDto>(group);
        FormatGroupAvatar(dto);
        return dto;
    }

    /// <summary>
    /// 申请加入
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ApplyJoin(int userId, int id, JoinGroupRequest request, CancellationToken cancellationToken = default)
    {
        var apply = await _context.GroupApplies
            .FirstOrDefaultAsync(x => x.GroupId == id && x.UserId == userId && x.Status == ApplyStatus.Applied, cancellationToken);

        if (apply is null)
        {
            apply = new GroupApply()
            {
                GroupId = id,
                UserId = userId,
                Remark = request.Remark,
                Status = ApplyStatus.Applied,
            };

            await _context.AddAsync(apply, cancellationToken);
        }
        else
        {
            apply.Remark = request.Remark;
            apply.UpdateTime = Timestamp.Now;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 离开群聊
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Leave(int userId, int id, CancellationToken cancellationToken = default)
    {
        var group = await _context.UserGroupRealtions.FindAsync(new object[] { userId, id }, cancellationToken);

        if (group is null)
        {
            throw Oops.Bah("未找到群聊");
        }

        _context.UserGroupRealtions.Remove(group);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// dissolve group.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Delete(int userId, int id, CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups.FindAsync(new object[] { id }, cancellationToken);

        if (group is null)
        {
            throw Oops.Bah("group not exists.");
        }

        if (group.UserId != userId)
        {
            throw Oops.Bah("no access.").StatusCode(HttpStatusCode.Forbidden);
        }

        try
        {
            _context.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "delete group error");
            throw Oops.Oh();
        }
    }

    public async Task<List<GroupDto>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var groups = await _context.UserGroupRealtions
             .AsNoTracking()
             .Include(x => x.Group)
             .Where(x => x.UserId == userId && !x.Group.IsDeleted)
             .Select(x => x.Group)
             .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GroupDto>>(groups);

        dtos.ForEach(FormatGroupAvatar);
        return dtos;
    }

    private void FormatGroupAvatar(GroupDto item)
    {
        if (string.IsNullOrWhiteSpace(item.Avatar))
        {
            item.Avatar = _fileService.GetDefaultGroupAvatar();
        }
        else
        {
            item.Avatar = _fileService.FormatPublicFile(item.Avatar);
        }
    }
}
