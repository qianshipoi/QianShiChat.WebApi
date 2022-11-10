using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using QianShiChat.WebApi.Models;

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

        /// <summary>
        /// user controller
        /// </summary>
        public UserController(ChatDbContext context, IMapper mapper, ILogger<UserController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get(int id, CancellationToken cancellationToken = default)
        {
            var info = await _context.UserInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (info == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(info));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var existsAccount = await _context.UserInfos
                 .AsNoTracking()
                 .AnyAsync(x => x.Account.Equals(dto.Account), cancellationToken);

            if (existsAccount)
            {
                return BadRequest("The account already exists.");
            }

            var user = _mapper.Map<UserInfo>(dto);

            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var newDto = _mapper.Map<UserDto>(user);

            return Ok(newDto);
        }
    }
}
