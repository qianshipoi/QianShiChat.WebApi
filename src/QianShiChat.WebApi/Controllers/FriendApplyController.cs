using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using QianShiChat.Common.Models;
using QianShiChat.Models;
using QianShiChat.WebApi.Hubs;
using QianShiChat.WebApi.Models;

using System.Text.Json;

namespace QianShiChat.WebApi.Controllers
{
    [Route("api/User/{userId:int}/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendApplyController : BaseController
    {
        private readonly ChatDbContext _context;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;
        private readonly IMapper _mapper;
        public FriendApplyController(ChatDbContext context, IHubContext<ChatHub, IChatClient> hubContext, IMapper mapper)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Apply([FromRoute] int userId, [FromBody] CreateFriendApplyDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.UserInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            var friendApply = await _context.FriendApplies
                .AsNoTracking()
                .Where(x => x.Status == ApplyStatus.Applied)
                .FirstOrDefaultAsync(x => x.UserId == CurrentUserId && x.FriendId == userId, cancellationToken);

            if (friendApply == null)
            {
                friendApply = new FriendApply()
                {
                    CreateTime = DateTime.Now,
                    UserId = CurrentUserId,
                    FriendId = userId,
                    LaseUpdateTime = DateTime.Now,
                    Remark = dto.Remark,
                    Status = ApplyStatus.Applied,
                    User = user
                };

                await _context.AddAsync(friendApply, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            await _hubContext.Clients.User(userId.ToString()).Notification(new NotificationMessage
            {
                Type = NotificationType.FirendApply,
                Message = JsonSerializer.Serialize(_mapper.Map<FriendApplyDto>(friendApply))
            });

            return Ok(friendApply);
        }

        /// <summary>
        /// 待处理
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Pending")]
        public async Task<ActionResult<List<FriendApply>>> Pending([FromRoute] int userId, CancellationToken cancellationToken = default)
        {
            if (userId != CurrentUserId)
            {
                return Forbid();
            }

            var user = await _context.UserInfos
                 .Include(x => x.FriendApplyFriends.Where(x => x.Status == ApplyStatus.Applied))
                 .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user.FriendApplyFriends);
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/Approval/{status}")]
        public async Task<IActionResult> Approval([FromRoute] int userId, [FromRoute] int id, [FromRoute] ApplyStatus status, CancellationToken cancellationToken = default)
        {
            if (userId != CurrentUserId)
            {
                return Forbid();
            }

            var user = await _context.UserInfos
                .FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return NotFound();
            }

            var apply = await _context.FriendApplies
                .Where(x => x.Id == id)
                .Include(x => x.Friend)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (apply is null)
            {
                return NotFound();
            }
            if (apply.Status != ApplyStatus.Applied)
            {
                return BadRequest("该申请已处理");
            }

            var now = DateTime.Now;

            apply.Status = status;
            apply.LaseUpdateTime = now;
            if (status == ApplyStatus.Passed)
            {
                // 创建关系
                user.Realtions.Add(new UserRealtion
                {
                    UserId = apply.FriendId,
                    FriendId = apply.UserId,
                    CreateTime = now
                });

                user.Friends.Add(new UserRealtion
                {
                    UserId = apply.UserId,
                    FriendId = apply.FriendId,
                    CreateTime = now
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            apply.User = user;

            await _hubContext.Clients.Users(apply.UserId.ToString(), apply.FriendId.ToString()).Notification(new NotificationMessage
            {
                Type = NotificationType.NewFirend,
                Message = JsonSerializer.Serialize(_mapper.Map<FriendApply>(apply))
            });

            return Ok("处理成功");
        }
    }
}
