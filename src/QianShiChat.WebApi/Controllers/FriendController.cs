using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using QianShiChat.Models;
using QianShiChat.WebApi.Services;

namespace QianShiChat.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : BaseController
    {
        private readonly IFriendService _friendService;
        private readonly IChatMessageService _chatMessageService;
        private readonly IMapper _mapper;

        public FriendController(IFriendService friendService, IChatMessageService chatMessageService, IMapper mapper)
        {
            _friendService = friendService;
            _chatMessageService = chatMessageService;
            _mapper = mapper;
        }

        [HttpGet("unread")]
        public async Task<ActionResult<List<UserWithMessage>>> GetAllUnreadMessageFriends(CancellationToken cancellationToken = default)
        {
            var friends = await _friendService
                .GetNewMessageFriendsAsync(CurrentUserId, cancellationToken);
            return Ok(friends);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllFriends(CancellationToken cancellationToken
            = default)
        {
            var result = await _friendService.GetFriendsAsync(CurrentUserId, cancellationToken);
            return Ok(result);
        }
    }
}
