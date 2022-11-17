using QianShiChat.WebApi.Models;

using Quartz;

using EasyCaching.Core;
using QianShiChat.WebApi.Models.Entity;
using System.Text.Json;

namespace QianShiChat.WebApi.BackgroundHost
{
    [DisallowConcurrentExecution]
    public class SaveChatMessageJob : IJob
    {
        private readonly ILogger<SaveChatMessageJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SaveChatMessageJob(ILogger<SaveChatMessageJob> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Start saving chat message data.");

            using var scoped = _serviceProvider.CreateScope();
            var redisCachingProvider = scoped.ServiceProvider.GetRequiredService<IRedisCachingProvider>();
            var dbContext = scoped.ServiceProvider.GetRequiredService<ChatDbContext>();

            var cacheValues = await redisCachingProvider.HGetAllAsync(AppConsts.ChatMessageCacheKey);
            if (cacheValues is not null && cacheValues.Count() > 0)
            {
                foreach (var cacheValue in cacheValues)
                {
                    var message = JsonSerializer.Deserialize<ChatMessage>(cacheValue.Value);
                    if (message is not null)
                    {
                        try
                        {
                            await dbContext.AddAsync(message);
                            await dbContext.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "saving [{0}] error.", JsonSerializer.Serialize(message));
                        }
                    }
                    await redisCachingProvider.HDelAsync(AppConsts.ChatMessageCacheKey, new List<string> { cacheValue.Key });
                }
            }

            _logger.LogInformation("Savied chat message data.");
        }
    }
}
