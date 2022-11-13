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
        private readonly IFirendService _firendService;

        public FriendController(IFirendService firendService)
        {
            _firendService = firendService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllFriends([FromQuery, Required, Range(1, int.MaxValue)] int userId, CancellationToken cancellationToken = default)
        {
            if (userId != CurrentUserId)
            {
                return Forbid();
            }

            return await _firendService.GetFirendsAsync(userId, cancellationToken);
        }
    }
}
