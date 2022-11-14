using Microsoft.AspNetCore.Mvc;

using QianShiChat.Models;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Services;

using System.ComponentModel.DataAnnotations;

namespace QianShiChat.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FriendController : BaseController
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService firendService)
        {
            _friendService = firendService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllFriends([FromQuery, Required, Range(1, int.MaxValue)] int userId, CancellationToken cancellationToken = default)
        {
            if (userId != CurrentUserId)
            {
                return Forbid();
            }

            return await _friendService.GetFriendsAsync(userId, cancellationToken);
        }
    }
}
