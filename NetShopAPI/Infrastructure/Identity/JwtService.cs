using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using NetShopAPI.Models;
using NetShopAPI.Features.Authorized.Services;

namespace NetShopAPI.Infrastructure.Identity
{
    public class JwtService : IJwtService
    {

        private readonly JwtOptions _options;
        private readonly SymmetricSecurityKey _key;

        public JwtService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        }


        public string CreateJwt(User user)
        {
            var claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [JwtRegisteredClaimNames.Email] = user.Email,
                [ClaimTypes.Role] = user.Role
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Claims = claims,
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(descriptor);
        }

    }
}
