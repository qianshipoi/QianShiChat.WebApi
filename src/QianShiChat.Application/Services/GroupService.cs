using QianShiChat.Application.Services.IServices;

namespace QianShiChat.Application.Services;

public class GroupService : IGroupService, ITransient
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GroupService> _logger;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public GroupService(
        IApplicationDbContext context,
        ILogger<GroupService> logger,
        IMapper mapper,
        IFileService fileService,
        IHubContext<ChatHub, IChatClient> hubContext,
        IGroupRepository groupRepository,
        IUserRepository userRepository)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _fileService = fileService;
        _hubContext = hubContext;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// 创建群聊
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GroupDto> CreateAsync(int userId, string name, CancellationToken cancellationToken = default)
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

        await _context.Groups.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<GroupDto>(group);
        FormatGroupAvatar(dto);

        await _hubContext.Clients.User(userId.ToString()).Notification(new NotificationMessage(NotificationType.NewGroup, dto));
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
            var isFriend = await _userRepository.IsFriendAsync(userId, friendId, cancellationToken);

            if (!isFriend)
            {
                throw Oops.Bah("no access.").StatusCode(HttpStatusCode.Forbidden);
            }
        }

        var currentUserName = await _userRepository.GetNicknameAsync(userId, cancellationToken);
        var friendName = await _userRepository.GetNicknameAsync(request.FriendIds.First(), cancellationToken);

        var now = Timestamp.Now;
        var group = new Group()
        {
            Name = $"{currentUserName}、{friendName}",
            UserId = userId,
            TotalUser = 1 + request.FriendIds.Count,
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

        using var trans = await _context.BeginTransactionAsync();

        try
        {
            await _context.Groups.AddAsync(group, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var userIds = request.FriendIds.Select(x => x).ToList();
            userIds.Add(userId);

            foreach (var id in userIds)
            {
                var roomId = AppConsts.GetGroupChatRoomId(id, group.Id);
                var room = new Room
                {
                    Id = roomId,
                    CreateTime = now,
                    FromId = userId,
                    MessagePosition = YitIdHelper.NextId(),
                    ToId = group.Id,
                    Type = ChatMessageSendType.Group,
                    UpdateTime = now,
                };

                _context.Rooms.Add(room);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await _context.CommitAsync(trans!);
        }
        catch (Exception ex)
        {
            _context.RollbackTransaction();
            _logger.LogError(ex, "create group error - userId: {userId}, request: {request}", userId, JsonSerializer.Serialize(request));
            throw Oops.Oh("create group error.");
        }

        var dto = _mapper.Map<GroupDto>(group);
        FormatGroupAvatar(dto);
        await _hubContext.Clients.Users(new List<string>(request.FriendIds.Select(x => x.ToString())) { userId.ToString() }).Notification(new NotificationMessage(NotificationType.NewGroup, dto));

        return dto;
    }

    /// <summary>
    /// 离开群聊
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task LeaveAsync(int userId, int id, CancellationToken cancellationToken = default)
    {
        var group = await _context.UserGroupRealtions.FindAsync(new object[] { userId, id }, cancellationToken);

        if (group is null)
        {
            throw Oops.Bah("未找到群聊");
        }

        _context.UserGroupRealtions.Remove(group);
        await _groupRepository.TotalUserDecrementAsync(group.GroupId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// dissolve group.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int userId, int id, CancellationToken cancellationToken = default)
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
            _context.Groups.Remove(group);
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
             .Where(x => x.UserId == userId && !x.Group!.IsDeleted)
             .Select(x => x.Group)
             .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<GroupDto>>(groups);

        dtos.ForEach(FormatGroupAvatar);
        return dtos;
    }

    public async Task<GroupDto?> FindByIdAsync(int groupId, CancellationToken cancellationToken = default)
    {
        var dto = await _context.Groups
            .AsNoTracking()
            .Where(x => x.Id == groupId)
            .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null) return null;
        FormatGroupAvatar(dto);
        return dto;
    }

    public async Task ApplyAsync(int id, int userId, GroupApplyRequest request, CancellationToken cancellationToken = default)
    {
        var userInfo = await _userRepository.FindByIdAsync(userId, cancellationToken);
        if (userInfo == null) throw Oops.Bah("user not found.");

        var group = await _groupRepository.FindByIdAsync(id, cancellationToken);
        if (group is null) throw Oops.Bah("group not found.");

        var entity = await _context.GroupApplies
            .Where(x => x.GroupId == id && x.UserId == userId && x.Status == ApplyStatus.Applied && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        var now = Timestamp.Now;

        if (entity is null)
        {
            entity = new GroupApply
            {
                CreateTime = now,
                GroupId = id,
                Status = ApplyStatus.Applied,
                UpdateTime = now,
                UserId = userId,
            };
            _context.GroupApplies.Add(entity);
        }
        entity.Remark = request.Remark;

        // save
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "join group apply error.");
            throw Oops.Oh();
        }

        // send group apply message.
        entity.User = userInfo;
        entity.Group = group;
        var dto = _mapper.Map<GroupApplyDto>(entity);
        _ = SendApplyNotify(dto);
    }

    public async Task<List<GroupApplyDto>> ApprovalAsync(int userId, GroupJoiningApprovalRequest request, CancellationToken cancellationToken = default)
    {
        var result = new List<GroupApplyDto>();
        using var tran = await _context.BeginTransactionAsync();
        try
        {
            var status = request.State switch
            {
                ApprovalState.Approved => ApplyStatus.Passed,
                ApprovalState.Rejected => ApplyStatus.Rejected,
                ApprovalState.Ignored => ApplyStatus.Ignored,
                _ => throw new NotSupportedException()
            };
            foreach (var applyId in request.ApplyIds)
            {
                result.Add(await ApprovalAsync(applyId, userId, status, cancellationToken));
            }
            await _context.CommitAsync(tran);
        }
        catch (BusinessException)
        {
            _context.RollbackTransaction();
            throw;
        }
        catch (Exception ex)
        {
            _context.RollbackTransaction();
            _logger.LogError(ex, "appoval join group error.");
            throw Oops.Oh();
        }

        return result;
    }

    public async Task<GroupApplyDto> ApprovalAsync(int applyId, int userId, ApplyStatus status, CancellationToken cancellationToken = default)
    {
        var apply = await _context.GroupApplies
            .Include(x => x.Group)
            .Where(x => x.Id == applyId && x.Group!.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (apply is null) throw Oops.Bah("apply not exists.");
        var now = Timestamp.Now;

        if (apply.Status == status) return _mapper.Map<GroupApplyDto>(apply);

        apply.Status = status;

        if (status == ApplyStatus.Passed)
        {
            apply.Group!.UserGroupRealtions.Add(new UserGroupRealtion
            {
                UserId = apply.UserId,
                GroupId = apply.GroupId,
                CreateTime = now,
            });

            await _groupRepository.TotalUserIncrementAsync(apply.Group.Id, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (status == ApplyStatus.Passed)
        {
            _ = SendNewGroup(apply.Group!, apply.UserId);
        }

        return _mapper.Map<GroupApplyDto>(apply);
    }

    public async Task<PagedList<GroupApplyDto>> GetApplyPendingAsync(int userId, QueryGroupApplyPendingRequest request, CancellationToken cancellationToken = default)
    {
        if (!request.BeforeLastTime.HasValue) request.BeforeLastTime = 0L;

        var query = _context.GroupApplies
          .AsNoTracking()
          .Include(x => x.User)
          .Include(x => x.Group)
          .Where(x => x.Group!.UserId == userId)
          .Where(x => x.CreateTime > request.BeforeLastTime);

        var data = await query
          .OrderByDescending(x => x.UpdateTime)
          .ThenByDescending(x => x.Id)
          .Take(request.Size)
          .ProjectTo<GroupApplyDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);

        var total = await query.CountAsync(cancellationToken);

        data.ForEach(item => {
            if (item.User != null)
                item.User.Avatar = _fileService.FormatPublicFile(item.User.Avatar);
            if (item.Group != null)
                item.Group.Avatar = _fileService.FormatPublicFile(item.Group.Avatar);
        });

        return PagedList.Create(data, total, request.Size);
    }

    public async Task<bool> GroupExistsAsync(int groupId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups.Where(x => !x.IsDeleted && x.Id == groupId).AnyAsync(cancellationToken);
    }

    public Task<bool> IsJoinedAsync(int userId, int groupId, CancellationToken cancellationToken = default)
        => _context.GroupApplies.Where(x => x.GroupId == groupId && x.UserId == userId && !x.IsDeleted).AnyAsync(cancellationToken);

    public async Task<PagedList<GroupDto>> SearchGroupAsync(GroupSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.Groups.Where(x => !x.IsDeleted);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            if (int.TryParse(request.Search, out var id))
            {
                query = query.Where(x => x.Id == id || x.Name.Contains(request.Search));
            }
            else
            {
                query = query.Where(x => x.Name.Contains(request.Search));
            }
        }
        var total = await query.CountAsync(cancellationToken);
        var list = await query.OrderByDescending(x => x.CreateTime)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        list.ForEach(FormatGroupAvatar);
        return PagedList.Create(list, total, request.Size);
    }

    public async Task<PagedList<UserDto>> GetMembersByGroupAsync(int groupId, GroupMemberQueryRequest request, CancellationToken cancellationToken = default)
    {
        var query = _context.UserGroupRealtions
              .AsNoTracking()
              .Include(x => x.User)
              .Where(x => x.GroupId == groupId)
              .OrderBy(x => x.CreateTime)
              .Select(x => new UserDto
              {
                  Account = x.User!.Account,
                  Avatar = x.User.Avatar!,
                  CreateTime = x.User.CreateTime,
                  Id = x.User.Id,
                  IsOnline = false,
                  NickName = x.User.NickName,
                  Alias = x.Alias,
              });

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        items.ForEach(FormatUserAvatar);

        return PagedList.Create(items, total, request.Size);
    }

    public async Task RenameAsync(int userId, int groupId, string name, CancellationToken cancellationToken = default)
    {
        var isCreator = await _groupRepository.IsCreatorAsync(userId, groupId, cancellationToken);
        if (!isCreator) throw Oops.Bah("not permission").StatusCode(HttpStatusCode.Forbidden);

        await _groupRepository.UpdateGroupNameAsync(userId, name, cancellationToken);
    }

    public async Task SetAliasAsync(int userId, int groupId, string? name, CancellationToken cancellationToken = default)
    {
        var isJoined = await _groupRepository.IsJoinGroupAsync(userId, groupId, cancellationToken);
        if (!isJoined) throw Oops.Bah("").StatusCode(HttpStatusCode.Forbidden);
        await _groupRepository.SetAliasAsync(userId, groupId, name, cancellationToken);
    }

    private void FormatUserAvatar(UserDto user)
    {
        if (!string.IsNullOrWhiteSpace(user.Avatar))
        {
            user.Avatar = _fileService.FormatPublicFile(user.Avatar);
        }
    }

    private async Task SendNewGroup(Group group, int userId)
    {
        var groupDto = _mapper.Map<GroupDto>(group);
        FormatGroupAvatar(groupDto);
        await _hubContext.Clients.User(userId.ToString())
            .Notification(new NotificationMessage(NotificationType.NewGroup, groupDto));
    }

    private async Task SendApplyNotify(GroupApplyDto apply)
    {
        await _hubContext.Clients.User(apply.UserId.ToString()).Notification(new NotificationMessage(NotificationType.GroupApply, apply));
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
