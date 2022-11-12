using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;
using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Services;

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

        /// <summary>
        /// user controller
        /// </summary>
        public UserController(ChatDbContext context, IMapper mapper, ILogger<UserController> logger, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
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
            var info = _userService.GetUserByIdAsync(id, cancellationToken);

            if (info == null)
            {
                return NotFound();
            }

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
