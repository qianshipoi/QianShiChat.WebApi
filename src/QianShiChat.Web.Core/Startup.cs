using Furion;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using QianShiChat.Web.Core.ServiceExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using QianShiChat.Application;

namespace QianShiChat.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsAccessor();
            services.AddControllersWithViews()
               .AddNewtonsoftJson(options =>
               {
                   // 首字母小写(驼峰样式)
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   // 时间格式化
                   options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                   // 忽略循环引用
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   // 忽略空值
                   // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
               }).AddInjectWithUnifyResult();

            services.AddSignalR();
            services.AddSnowflakeId(); // 雪花Id
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            app.UseUnifyResultStatusCodes();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject("swagger");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/hubs/chathub");
            });
        }
    }
}