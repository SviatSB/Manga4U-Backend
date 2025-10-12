using ENTITIES;
using ENTITIES.Interfaces;
using ENTITIES.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace SERVICES.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtConfig _jwtConfig;
        public JwtTokenGenerator(JwtConfig jwtConfig) { this._jwtConfig = jwtConfig; }

        public string GenerateToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpireMinutes),
                SigningCredentials = credentials,
                TokenType = "JWT"
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(descriptor);
        }
    }
}
