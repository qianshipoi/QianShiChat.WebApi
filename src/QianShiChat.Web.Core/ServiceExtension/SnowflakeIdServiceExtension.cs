using Furion;

using Microsoft.Extensions.DependencyInjection;

using Yitter.IdGenerator;

namespace QianShiChat.Web.Core.ServiceExtension
{
    internal static class SnowflakeIdServiceExtension
    {
        public static void AddSnowflakeId(this IServiceCollection services)
        {
            // 设置雪花Id的workerId，确保每个实例workerId都应不同
            var workerId = ushort.Parse(App.Configuration["SnowId:WorkerId"] ?? "1");
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions { WorkerId = workerId });
        }
    }
}
