using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QianShiChat.WebApi.Services
{
    public class JwtService : IJwtService
    {
        private JwtOptions _jwtOptions;

        public JwtService(IOptionsMonitor<JwtOptions> jwtOptionsMonitor)
        {
            _jwtOptions = jwtOptionsMonitor.CurrentValue;

            jwtOptionsMonitor.OnChange(options =>
            {
                _jwtOptions = options;
            });
        }

        public string CreateToken(IEnumerable<Claim> claims)
        {
            var secretByte = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                   issuer: _jwtOptions.Issuer,        //发布者
                   audience: _jwtOptions.Audience,    //接收者
                   claims: claims,                                         //存放的用户信息
                   notBefore: DateTime.UtcNow,                             //发布时间
                   expires: DateTime.UtcNow.AddDays(1),                      //有效期设置为1天
                   signingCredentials                                      //数字签名
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<Claim> GetClaims(string token)
        {
            var jwtHander = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHander.ReadJwtToken(token);
            return jwtSecurityToken.Claims;
        }

        public bool Validate(string token)
        {
            var jwtHander = new JwtSecurityTokenHandler();
            var secretByte = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                ClockSkew = TimeSpan.FromSeconds(5),
                IssuerSigningKey = new SymmetricSecurityKey(secretByte)
            };

            try
            {
                var result = jwtHander.ValidateToken(token, parameters, out _);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
