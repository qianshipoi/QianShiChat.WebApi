namespace QianShiChat.WebApi.Controllers;

/// <summary>
/// auth controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly IRedisCachingProvider _redisCachingProvider;

    /// <summary>
    /// auth controller
    /// </summary>
    public AuthController(
        IMapper mapper,
        IJwtService jwtService,
        IUserService userService,
        IRedisCachingProvider redisCachingProvider)
    {
        _mapper = mapper;
        _jwtService = jwtService;
        _userService = userService;
        _redisCachingProvider = redisCachingProvider;
    }

    /// <summary>
    /// auth.
    /// </summary>
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

    /// <summary>
    /// check auth.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserDto>> CheckAuth(CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserByIdAsync(CurrentUserId, cancellationToken);

        var token = _jwtService.CreateToken(new List<Claim>
        {
            new Claim(ClaimTypes.Name , user.NickName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        });

        Response.Headers.Add("X-Access-Token", token);
        return Ok(user);
    }

    /// <summary>
    /// generate qr key.
    /// </summary>
    /// <returns></returns>
    [HttpGet("qr/key")]
    public async Task<ActionResult<QrKeyResponse>> QrKey()
    {
        var uuid = YitIdHelper.NextId();

        // cache 5 minutes.
        var qrKeyCacheKey = GetQrKeyCacheKey(uuid.ToString());
        var cacheValue = JsonSerializer.Serialize(new QrCheckResponse()
        {
            Code = 801,
            Message = "等待扫码"
        });
        await _redisCachingProvider.StringSetAsync(qrKeyCacheKey, cacheValue, TimeSpan.FromMinutes(5));

        return Ok(new QrKeyResponse
        {
            Code = 200,
            Key = uuid.ToString()
        });
    }

    /// <summary>
    /// create qrcode.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("qr/create")]
    public ActionResult<QrCreateResponse> QrCreate(QrCreateRequset request, CancellationToken cancellationToken = default)
    {
        var response = new QrCreateResponse();
        response.Url = $"{Request.GetBaseUrl()}/api/auth/qr/auth?key={request.Key.Trim()}";

        if (request.Qrimg)
        {
            using var qrCodeGenerator = new QRCodeGenerator();
            using var qrCodeData = qrCodeGenerator.CreateQrCode(response.Url, QRCodeGenerator.ECCLevel.M, true);
            using var qrCode = new PngByteQRCode(qrCodeData);
            response.Image = "data:image/png;base64," + Convert.ToBase64String(qrCode.GetGraphic(20));
        }

        return Ok(response);
    }

    /// <summary>
    /// check auth status.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpGet("qr/check")]
    public async Task<ActionResult<QrCheckResponse>> QrCheck([FromQuery, Required, MaxLength(32)] string key)
    {
        var qrKeyCacheKey = GetQrKeyCacheKey(key);
        var cacheValue = await _redisCachingProvider.StringGetAsync(qrKeyCacheKey);

        if (string.IsNullOrWhiteSpace(cacheValue))
        {
            return Ok(new QrCheckResponse
            {
                Code = 800,
                Message = "二维码不存在或已过期"
            });
        }

        var response = JsonSerializer.Deserialize<QrCheckResponse>(cacheValue) ?? new QrCheckResponse()
        {
            Code = 800,
            Message = "二维码不存在或已过期"
        };

        return Ok(response);
    }

    /// <summary>
    /// qrcode login preauth
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("qr/preauth")]
    [Authorize]
    public async Task<ActionResult<QrAuthResponse>> QrPreAuth([FromQuery, Required, MaxLength(32)] string key, CancellationToken cancellationToken = default)
    {
        var qrKeyCacheKey = GetQrKeyCacheKey(key);

        var cacheValue = await _redisCachingProvider.StringGetAsync(qrKeyCacheKey);

        if (string.IsNullOrWhiteSpace(cacheValue))
        {
            return Ok(new QrAuthResponse
            {
                Code = 800,
                Message = "二维码已过期"
            });
        }

        var qrCheck = JsonSerializer.Deserialize<QrCheckResponse>(cacheValue);

        if (qrCheck == null)
        {
            return Ok(new QrAuthResponse
            {
                Code = 800,
                Message = "二维码已过期"
            });
        }

        // get user info.
        var userDto = await _userService.GetUserByIdAsync(CurrentUserId, cancellationToken);

        qrCheck.Code = 802;
        qrCheck.Message = "授权中";
        qrCheck.User = userDto;

        // save to redis.
        var ttl = await _redisCachingProvider.TTLAsync(qrKeyCacheKey);
        if (ttl < 60) ttl = 60;

        await _redisCachingProvider.StringSetAsync(qrKeyCacheKey, JsonSerializer.Serialize(qrCheck), TimeSpan.FromSeconds(ttl));

        return Ok(new QrAuthResponse
        {
            Code = 200,
            Message = "授权成功"
        });
    }

    /// <summary>
    /// qrcode login auth.
    /// </summary>
    /// <returns></returns>
    [HttpPost("qr/auth")]
    [Authorize]
    public async Task<ActionResult<QrAuthResponse>> QrAuth([FromQuery, Required, MaxLength(32)] string key, CancellationToken cancellationToken = default)
    {
        var qrKeyCacheKey = GetQrKeyCacheKey(key);

        var cacheValue = await _redisCachingProvider.StringGetAsync(qrKeyCacheKey);

        if (string.IsNullOrWhiteSpace(cacheValue))
        {
            return Ok(new QrAuthResponse
            {
                Code = 800,
                Message = "二维码已过期"
            });
        }

        var qrCheck = JsonSerializer.Deserialize<QrCheckResponse>(cacheValue);

        if (qrCheck == null)
        {
            return Ok(new QrAuthResponse
            {
                Code = 800,
                Message = "二维码已过期"
            });
        }

        if (qrCheck.Code != 802 || qrCheck.User == null)
        {
            return Ok(new QrAuthResponse
            {
                Code = 801,
                Message = "二维码未预授权"
            });
        }

        var token = _jwtService.CreateToken(new List<Claim>
        {
            new Claim(ClaimTypes.Name , qrCheck.User.NickName),
            new Claim(ClaimTypes.NameIdentifier, qrCheck.User.Id.ToString()),
        });

        qrCheck.Code = 803;
        qrCheck.Message = "授权成功";
        qrCheck.AccessToken = token;

        // save to redis.
        var ttl = await _redisCachingProvider.TTLAsync(qrKeyCacheKey);
        if (ttl < 120) ttl = 120;

        await _redisCachingProvider.StringSetAsync(qrKeyCacheKey, JsonSerializer.Serialize(qrCheck), TimeSpan.FromSeconds(ttl));

        return Ok(new QrAuthResponse
        {
            Code = 200,
            Message = "授权成功"
        });
    }

    /// <summary>
    /// get qrcode key cache key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string GetQrKeyCacheKey(string key) => $"qr-key:{key}";
}