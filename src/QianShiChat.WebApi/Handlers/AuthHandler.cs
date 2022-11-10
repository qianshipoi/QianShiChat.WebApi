using MediatR;

using Microsoft.EntityFrameworkCore;

using QianShiChat.WebApi.Models;
using QianShiChat.WebApi.Requests;
using QianShiChat.WebApi.Services;

using System.Security.Claims;

namespace QianShiChat.WebApi.Handlers
{
    public class AuthHandler : IRequestHandler<AuthRequest, IResult>
    {
        private readonly ChatDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthHandler(ChatDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<IResult> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await _context.UserInfos.FirstOrDefaultAsync(x => x.Account == request.Account, cancellationToken);
            if (userInfo == null)
            {
                return Results.BadRequest("未知用户");
            }

            if (string.Compare(userInfo.Password, request.Password, true) != 0)
            {
                return Results.BadRequest("密码不正确");
            }

            var token = _jwtService.CreateToken(new List<Claim>
            {
                new Claim(ClaimTypes.Name , userInfo.NickName),
                new Claim(ClaimTypes.NameIdentifier , userInfo.Id.ToString()),
            });

            return Results.Ok(token);
        }
    }
}
