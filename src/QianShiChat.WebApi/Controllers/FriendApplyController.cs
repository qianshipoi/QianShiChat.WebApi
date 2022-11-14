using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using QianShiChat.Common.Models;
using QianShiChat.Models;
using QianShiChat.WebApi.Hubs;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Services;

using System.Text.Json;

namespace QianShiChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendApplyController : BaseController
    {
        private readonly ChatDbContext _context;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFriendApplyService _friendApplyService;
        private readonly IFriendService _friendService;

        public FriendApplyController(ChatDbContext context, IHubContext<ChatHub, IChatClient> hubContext, IMapper mapper, IUserService userService, IFriendApplyService friendApplyService, IFriendService friendService)
        {
            _context = context;
            _hubContext = hubContext;
            _mapper = mapper;
            _userService = userService;
            _friendApplyService = friendApplyService;
            _friendService = friendService;
        }

        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Apply([FromBody] CreateFriendApplyDto dto, CancellationToken cancellationToken = default)
        {
            var friend = await _userService.GetUserByIdAsync(dto.UserId, cancellationToken);
            var user = await _userService.GetUserByIdAsync(CurrentUserId, cancellationToken);

            if (friend is null || user is null)
            {
                return NotFound();
            }

            var isFriend = await _friendService.IsFriendAsync(CurrentUserId, dto.UserId, cancellationToken);

            if (isFriend)
            {
                return BadRequest("已经是好友，请勿继续申请");
            }

            var isApply = await _friendApplyService.IsApplyAsync(CurrentUserId, dto.UserId, cancellationToken);
            FriendApplyDto applyDto;
            if (!isApply)
            {
                // 提交申请
                applyDto = await _friendApplyService.ApplyAsync(CurrentUserId, dto, cancellationToken);
            }
            else
            {
                // 更新申请时间
                applyDto = await _friendApplyService.UpdateApplyAsync(CurrentUserId, dto, cancellationToken);
            }

            applyDto.User = user;
            applyDto.Firend = friend;

            await _hubContext.Clients.User(applyDto.FirendId.ToString())
                .Notification(new NotificationMessage
                {
                    Type = NotificationType.FirendApply,
                    Message = JsonSerializer.Serialize(applyDto)
                });

            return Ok(applyDto);
        }

        /// <summary>
        /// 待处理
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("Pending")]
        public async Task<PagedList<FriendApplyDto>> Pending(int page, int size, CancellationToken cancellationToken = default)
        {
            var items = await _friendApplyService.GetPendingListByUserAsync(page, size, CurrentUserId, cancellationToken);

            var total = await _friendApplyService.GetPendingListCountByUserAsync(CurrentUserId, default);

            return PagedList.Create(items, total, page, size);
        }

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/Approval/{status}")]
        public async Task<IActionResult> Approval([FromRoute] int id, [FromRoute] ApplyStatus status, CancellationToken cancellationToken = default)
        {
            var user = await _context.UserInfos
                .FindAsync(new object[] { CurrentUserId }, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            if (await _friendApplyService.IsApprovalAsync(id, cancellationToken))
            {
                return BadRequest("该申请已处理");
            }

            var dto = await _friendApplyService.ApprovalAsync(CurrentUserId, id, status, cancellationToken);

            await _hubContext.Clients.Users(dto.UserId.ToString(), dto.FirendId.ToString()).Notification(new NotificationMessage
            {
                Type = NotificationType.NewFirend,
                Message = JsonSerializer.Serialize(dto)
            });

            return Ok("处理成功");
        }
    }
}
