using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BFFAPI.Services.Autenticacao;
using Microsoft.IdentityModel.Tokens;

namespace BFFAPI.Application.Services.Autenticacao
{
    public class JwtAuthService : IJwtAuthService
    {
        private readonly string _secretKey;

        public JwtAuthService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GenerateToken(string userId, string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId", userId),
                new Claim(ClaimTypes.Name, userName)                
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(authToken, validationParameters, out var token);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
