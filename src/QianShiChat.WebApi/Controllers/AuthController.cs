using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using QianShiChat.Models;
using QianShiChat.WebApi.Services;

using System.Security.Claims;

namespace QianShiChat.WebApi.Controllers
{
    /// <summary>
    /// auth controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, IJwtService jwtService, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Auth([FromBody] UserAuthDto dto, CancellationToken cancellationToken = default)
        {
            var userInfo = await _userService.GetUserByAccountAsync(dto.Account, cancellationToken);

            if (userInfo == null)
            {
                return BadRequest("Unknown user.");
            }

            if (string.Compare(userInfo.Password, dto.Password, true) != 0)
            {
                return BadRequest("The password is incorrect.");
            }

            var token = _jwtService.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.Name , userInfo.NickName),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
            });

            var userDto = _mapper.Map<UserDto>(userInfo);
            Response.Headers.Add("X-Access-Token", token);
            return Ok(userDto);
        }
    }
}
