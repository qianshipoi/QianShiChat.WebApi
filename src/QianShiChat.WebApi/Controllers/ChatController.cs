using EasyCaching.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QianShiChat.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IRedisCachingProvider _redisCachingProvider;
        public ChatController(IRedisCachingProvider redisCachingProvider)
        {
            _redisCachingProvider = redisCachingProvider;
        }

        private const string LUA_SCRIPT_DELETE_KEY = "local current = redis.call('hget', KEYS[1], ARGV[1]);" +
           "if current == false then " +
           "    return nil;" +
           "end " +
           "if current == ARGV[2] then " +
           "    return redis.call('hdel', KEYS[1], ARGV[1]);" +
           "else " +
           "    return 0;" +
           "end";

        [HttpGet]
        public async Task<IActionResult> Get(string field, string val)
        {
            var result = await _redisCachingProvider.EvalAsync(LUA_SCRIPT_DELETE_KEY, "1",
                  new List<object> { field, val });

            return Ok(result);
        }


    }
}
