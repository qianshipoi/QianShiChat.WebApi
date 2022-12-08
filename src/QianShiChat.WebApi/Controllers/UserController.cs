﻿namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// user controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    readonly ILogger<UserController> _logger;
    readonly IMapper _mapper;
    readonly IUserService _userService;
    readonly IRedisCachingProvider _redisCachingProvider;
    readonly IAvatarService _avatarService;

    /// <summary>
    /// user controller
    /// </summary>
    public UserController(
        IAvatarService avatarService,
        IMapper mapper,
        ILogger<UserController> logger,
        IUserService userService,
        IRedisCachingProvider redisCachingProvider)
    {
        _mapper = mapper;
        _logger = logger;
        _userService = userService;
        _redisCachingProvider = redisCachingProvider;
        _avatarService = avatarService;
    }

    /// <summary>
    /// search users.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="nickName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{page:int}/{size:int}")]
    public async Task<PagedList<UserDto>> Search(
        [FromRoute] int page,
        [FromRoute] int size,
        [FromQuery, Required] string nickName,
        CancellationToken cancellationToken = default)
    {
        var users = await _userService.GetUserByNickNameAsync(page, size, nickName, cancellationToken);
        var totalCount = await _userService.GetUserCountByNickNameAsync(nickName, cancellationToken);

        return PagedList.Create(users, totalCount, page, size);
    }

    /// <summary>
    /// get user info.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id, CancellationToken cancellationToken = default)
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
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var avatarPath = await _avatarService.GetDefaultAvatarByIdAsync(dto.DefaultAvatarId, cancellationToken);
        if(string.IsNullOrWhiteSpace(avatarPath))
        {
            return BadRequest("default avatar not found.");
        }

        if (await _userService.AccountExistsAsync(dto.Account, cancellationToken))
        {
            return BadRequest("The account already exists.");
        }

        var user = await _userService.AddAsync(dto, avatarPath, cancellationToken);

        return CreatedAtAction(nameof(GetUser), new { Id = user.Id }, user);
    }
}
