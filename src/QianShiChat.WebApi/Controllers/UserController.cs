using AutoMapper;

using EasyCaching.Core;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Services;

using System.Text.Json;

namespace QianShiChat.WebApi.Controllers
{
    /// <summary>
    /// user controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly ChatDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRedisCachingProvider _redisCachingProvider;

        /// <summary>
        /// user controller
        /// </summary>
        public UserController(ChatDbContext context, IMapper mapper, ILogger<UserController> logger, IUserService userService, IRedisCachingProvider redisCachingProvider)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _redisCachingProvider = redisCachingProvider;
        }

        /// <summary>
        /// get all user.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserDto>> GetUsers(CancellationToken cancellationToken = default)
        {
            var users = await _context.UserInfos.ToListAsync(cancellationToken);
            return _mapper.Map<List<UserDto>>(users);
        }

        /// <summary>
        /// get user info.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int id, CancellationToken cancellationToken = default)
        {
            var cacheKey = nameof(GetUser) + id.ToString();

            var cacheValue = await _redisCachingProvider.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cacheValue))
            {
                return Ok(JsonSerializer.Deserialize<UserDto>(cacheValue));
            }

            var info = await _userService.GetUserByIdAsync(id, cancellationToken);

            if (info == null)
            {
                return NotFound();
            }

            await _redisCachingProvider.StringSetAsync(cacheKey, JsonSerializer.Serialize(info), TimeSpan.FromSeconds(60));

            return Ok(info);
        }

        /// <summary>
        /// create user
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            if (await _userService.AccountExistsAsync(dto.Account, cancellationToken))
            {
                return BadRequest("The account already exists.");
            }

            await _userService.AddAsync(dto, cancellationToken);

            return Ok();
        }
    }
}
