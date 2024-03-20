using System.Security.Claims;

namespace BFFAPI.Services.Autenticacao
{
    public interface IJwtAuthService
    {
        string GenerateToken(string userId, string userName);
        ClaimsPrincipal ValidateToken(string authToken);
    }
}
