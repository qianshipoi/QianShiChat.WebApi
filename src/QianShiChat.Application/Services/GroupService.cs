namespace QianShiChat.Application.Services;

public class GroupService : IGroupService, ITransient
{
    private readonly ChatDbContext _context;
    private readonly ILogger<GroupService> _logger;
    private readonly IMapper _mapper;

    public GroupService(ChatDbContext context, ILogger<GroupService> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
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

        return _mapper.Map<GroupDto>(group);
    }

    /// <summary>
    /// 申请加入
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="id"></param>
    /// <param name="remark"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ApplyJoin(int userId, int id, string remark, CancellationToken cancellationToken = default)
    {
        var apply = await _context.GroupApplies.FirstOrDefaultAsync(x => x.GroupId == id && x.UserId == userId && x.Status == ApplyStatus.Applied, cancellationToken);

        if (apply is null)
        {
            apply = new GroupApply()
            {
                GroupId = id,
                UserId = userId,
                Remark = remark,
                Status = ApplyStatus.Applied,
            };

            await _context.AddAsync(apply, cancellationToken);
        }
        else
        {
            apply.Remark = remark;
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
    public async Task Exit(int userId, int id, CancellationToken cancellationToken = default)
    {
        var group = await _context.UserGroupRealtions.FindAsync(new object[] { userId, id }, cancellationToken);

        if (group is null)
        {
            throw Oops.Bah("未找到群聊");
        }

        _context.UserGroupRealtions.Remove(group);
        await _context.SaveChangesAsync(cancellationToken);
    }

}


public interface IGroupService
{
}