using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ClientesAPI.Application.Services.Autenticacao
{
    public class JwtAuthService : IJwtAuthService
    {
        private readonly string _secretKey;

        public JwtAuthService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public bool ValidateToken(string authToken)
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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
