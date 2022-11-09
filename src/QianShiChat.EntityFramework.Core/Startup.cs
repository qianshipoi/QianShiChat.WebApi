using Furion;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using QianShiChat.EntityFramework.Core.DbContexts;
using Furion.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Furion.DatabaseAccessor;

namespace QianShiChat.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabaseAccessor(options =>
            {
                options.AddDbPool<DefaultDbContext>(DbProvider.Sqlite, optionBuilder: (services, opt) =>
                {
                    opt.UseBatchEF_Sqlite(); // EF批量组件 --- SQlite数据库包
                });
            }, "QianShiChat.Database.Migrations");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //// 自动迁移数据库（update-database命令）
            //if (env.IsDevelopment())
            //{
            //    Scoped.Create((_, scope) =>
            //    {
            //        var context = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
            //        context.Database.Migrate();
            //    });
            //}
        }
    }
}