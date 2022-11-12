using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using QianShiChat.WebApi;
using QianShiChat.WebApi.Models;

using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service Extension
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 添加JWT认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(JwtOptions.OptionsKey);

            var jwtOptions = new JwtOptions();
            section.Bind(jwtOptions);
            services.AddOptions();
            services.Configure<JwtOptions>(section);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
                {
                    var secretByte = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        ClockSkew = TimeSpan.FromSeconds(5),
                        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                    };
                });
            services.AddAuthorization();

            return services;
        }

        /// <summary>
        /// 添加ChatDbContext
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddChatDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("Default"), ServerVersion.Parse("8.0.31"));
            });
            return services;
        }
    }
}
