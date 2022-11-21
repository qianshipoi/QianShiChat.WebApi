using EasyCaching.Core;

using QianShiChat.WebApi.Models;

using Quartz;

using System.Text.Json;


namespace QianShiChat.WebApi.BackgroundHost
{
    [DisallowConcurrentExecution]
    public class SaveMessageCursorJob : IJob
    {
        private readonly ILogger<SaveChatMessageJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SaveMessageCursorJob(ILogger<SaveChatMessageJob> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Start saving chat message data.");

            using var scoped = _serviceProvider.CreateScope();
            var redisCachingProvider = scoped.ServiceProvider.GetRequiredService<IRedisCachingProvider>();
            var dbContext = scoped.ServiceProvider.GetRequiredService<ChatDbContext>();

            var cacheValues = await redisCachingProvider.HGetAllAsync(AppConsts.MessageCursorCacheKey);

            if (cacheValues is not null && cacheValues.Count() > 0)
            {
                foreach (var cacheValue in cacheValues)
                {
                    if (long.TryParse(cacheValue.Key, out var userId)
                        && long.TryParse(cacheValue.Value, out var position))
                    {
                        var cursor = await dbContext.MessageCursors.FindAsync(cacheValue);
                        if (cursor != null)
                        {
                            try
                            {
                                cursor.Postiton = position;
                                await dbContext.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "saving [{0}] error.", JsonSerializer.Serialize(cacheValue));
                            }
                        }
                    }

                    // delete key if value not changed.
                    await redisCachingProvider.EvalAsync(
                        LUA_SCRIPT_DELETE_KEY,
                        AppConsts.MessageCursorCacheKey,
                        new List<object>
                             {
                                 cacheValue.Key,
                                 cacheValue.Value
                             });
                }
            }

            _logger.LogInformation("Savied message cursor data.");
        }
    }
}
