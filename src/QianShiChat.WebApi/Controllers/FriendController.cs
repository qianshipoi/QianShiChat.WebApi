using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using QianShiChat.Models;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Services;

using System.ComponentModel.DataAnnotations;

namespace QianShiChat.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : BaseController
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllFriends(CancellationToken cancellationToken = default)
        {
            return await _friendService.GetFriendsAsync(CurrentUserId, cancellationToken);
        }
    }
}
