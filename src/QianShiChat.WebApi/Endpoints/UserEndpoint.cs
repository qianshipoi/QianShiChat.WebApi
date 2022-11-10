using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Endpoints
{
    public static class UserEndpoint
    {
        public static RouteGroupBuilder MapUserApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetUsersAsync)
                .Produces<List<UserInfo>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized);
            //group.MapGet("/{id}", GetUser);
            //group.MapPost("/", CreateUser);
            //group.MapPut("/{id}", UpdateUser);
            //group.MapDelete("/{id}", DeleteUser);

            return group;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="size">每页数量</param>
        /// <param name="context">数据库</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public static async Task<IResult> GetUsersAsync(int page, int size, [FromServices] ChatDbContext context, CancellationToken cancellationToken = default)
        {
            return Results.Ok(await context.UserInfos.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken));
        }

    }
}
